using FluentResults;
using Hangfire;
using ipbase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NatechAssignmentCommon.Configuration;
using NatechAssignmentCommon.DTO;
using NatechAssignmentCommon.Enumerator;
using NatechAssignmentCommon.Exception;
using NatechAssignmentCommon.Extension;
using NatechAssignmentCommon.Interface;
using NatechAssignmentDataLayer;
using NatechAssignmentDataLayer.Models;
using System;
using System.Linq;
using System.Net;

namespace NatechAssignmentBusinessObject;

public class NatechBusinessObject : INatechBusinessObject
{
    private readonly IConfiguration _configuration;
    private readonly LocateDbContext _dbContext;
    private readonly ILogger<NatechBusinessObject> _logger;
    private readonly Ipbase _ipbase;

    public NatechBusinessObject(
        IConfiguration configuration,
        LocateDbContext dbContext,
        ILogger<NatechBusinessObject> logger)
    {
        _configuration = configuration;
        _dbContext = dbContext;
        _logger = logger;

        var section = _configuration.GetSection(Section.Ipbase);
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

    public Result<GetLocationForIPResponse> GetLocationForIp(string ip)
    {
        try
        {
            var info = _ipbase.Info(ip);
            return GetLocationForIpResponseExtension.FormIpbaseInfoResponse(info);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error");
            return Result.Fail(e.Message);
        }
    }

    public Result<Guid> GetLocationForListOfIp(string[] ipAddresses)
    {
        try
        {
            var newKey = Guid.NewGuid();
            _logger.LogDebug("New Header ID '{newKey}'", newKey);
            var header = new LocateHeader(newKey, DateTime.Now);

            _dbContext.Headers.Add(header);

            foreach (var ipAddress in ipAddresses)
            {
                var details = new LocateDetails
                {
                    Header = header,
                    Id = Guid.NewGuid(),
                    Status = LocateStatus.Pending,
                    IP = ipAddress,
                    Created = header.Created
                };
                _logger.LogDebug("New Details ID '{details.Id}'", details.Id);

                if (!IPAddress.TryParse(ipAddress, out _))
                {
                    _logger.LogDebug("Could not parse IP address '{ipAddress}'", ipAddress);
                    details.Status = LocateStatus.Error;
                    details.ResultMessage = "Failed to parse IP address";
                }

                _dbContext.Details.Add(details);
            }

            _dbContext.SaveChanges();

            RecurringJob.TriggerJob(Jobs.ProcessJob);

            _logger.LogDebug("Job {Jobs.ProcessJob} is triggered", Jobs.ProcessJob);

            return newKey;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error");
            return Result.Fail(e.Message);
        }
    }

    public Result<GetLocationForListOfIpStatusResponse> GetLocationForListOfIpStatus(Guid guid)
    {
        try
        {
            var header = _dbContext
                .Headers
                .Include(x => x.Details)
                .FirstOrDefault(x => x.Id == guid);

            if (header is null)
            {
                return Result.Fail($"Process not found by '{guid}' id");
            }

            var total = header.Details.Count;
            var processed = header.Details.Count(x => x.Status != LocateStatus.Pending);
            var remaining = total - processed;
            var processDuration = DateTime.Now - header.Created;
            var processDurationPerItem = processDuration.Divide(processed);
            var estimated = processDurationPerItem.Multiply(remaining);

            return new GetLocationForListOfIpStatusResponse
            {
                TotalIpCount = total,
                ProcessedIpCount = processed,
                EstimatedTime = estimated
            };

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error");
            return Result.Fail(e.Message);
        }
    }
}