using ipbase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NatechAssignmentCommon.Configuration;
using NatechAssignmentCommon.Enumerator;
using NatechAssignmentCommon.Exception;
using NatechAssignmentCommon.Extension;
using NatechAssignmentCommon.Ipbase.Status;
using NatechAssignmentDataLayer;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;

namespace NatechAssignmentBusinessObject;

public class NatechBackgroundJob
{
    private readonly LocateDbContext _dbContext;
    private readonly ILogger<NatechBackgroundJob> _logger;
    private readonly Ipbase _ipbase;
    private static readonly object _lockObj = new();

    public NatechBackgroundJob(
        IConfiguration configuration,
        LocateDbContext dbContext,
        ILogger<NatechBackgroundJob> logger)
    {
        _dbContext = dbContext;
        _logger = logger;

        var section = configuration.GetSection(Section.Ipbase);
        if (section is null)
        {
            _logger.LogError("There is no '{Section.Ipbase}' section in configuration", Section.Ipbase);
            throw new NatechException($"There is no '{Section.Ipbase}' section in configuration");
        }

        var value = section.GetSection(Value.IpbaseApiKey);
        if (value is null)
        {
            _logger.LogError("There is no '{Value.Ipbase_API_KEY}' section in configuration", Value.IpbaseApiKey);
            throw new NatechException($"There is no '{Value.IpbaseApiKey}' section in configuration");
        }

        if (value is null)
        {
            _logger.LogError("The '{Value.Ipbase_API_KEY}' section has no value", Value.IpbaseApiKey);
            throw new NatechException($"The '{Value.IpbaseApiKey}' section has no value");
        }
        var apiKey = value.Value!;

        _ipbase = new Ipbase(apiKey);
    }

    public void Process()
    {
        _logger.LogDebug("Job is started");
        var lockTaken = false;
        try
        {
            Monitor.TryEnter(_lockObj, 1, ref lockTaken);
            if (!lockTaken)
            {
                _logger.LogDebug("An other thread is execution");
                return;
            }

            var status = _ipbase.Status();
            var statusResponse = JsonConvert.DeserializeObject<StatusResponse>(status);

            if (statusResponse is null)
            {
                _logger.LogError("Could not get Status of the Ipbase");
                return;
            }

            var remaining = statusResponse.quotas.month.remaining;
            _logger.LogDebug("Remaining try are {remaining}", remaining);

            while (remaining > 0)
            {
                var toBeProcessed = _dbContext
                    .Details
                    .Where(x => x.Status == LocateStatus.Pending)
                    .OrderBy(x => x.Created)
                    .FirstOrDefault();
                if (toBeProcessed is null)
                {
                    _logger.LogDebug("No more items detected to be processed");
                    return;
                }

                _logger.LogDebug("Processing IP '{toBeProcessed.IP}'", toBeProcessed.IP);
                try
                {
                    var info = _ipbase.Info(toBeProcessed.IP);
                    var result = GetLocationForIpResponseExtension.FormIpbaseInfoResponse(info);
                    if (result.IsSuccess)
                    {
                        remaining--;
                        toBeProcessed.Status = LocateStatus.Success;
                        toBeProcessed.CountryName = result.Value.CountryName;
                        toBeProcessed.Latitude = result.Value.Latitude;
                        toBeProcessed.Longitude = result.Value.Longitude;
                        toBeProcessed.TimeZone = result.Value.TimeZone;
                        toBeProcessed.CountryCode = result.Value.CountryCode;
                    }
                    else
                    {
                        toBeProcessed.Status = LocateStatus.Error;
                        toBeProcessed.ResultMessage = result.Errors.FirstOrDefault()?.Message ?? "Unknown error";
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error");
                    toBeProcessed.Status = LocateStatus.Error;
                    toBeProcessed.ResultMessage = e.Message;
                }

                toBeProcessed.Updated = DateTime.Now;

                _logger.LogDebug("Result of IP '{toBeProcessed.IP}' is {toBeProcessed.Status}", toBeProcessed.IP,
                    toBeProcessed.Status);

                _dbContext.SaveChanges();
            }

            _logger.LogWarning("There are no remaining try, will need to wait for till 'end of month'");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error");
        }
        finally
        {
            if (lockTaken)
            {
                Monitor.Exit(_lockObj);
            }
        }
    }
}