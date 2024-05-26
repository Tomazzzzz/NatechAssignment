using Microsoft.AspNetCore.Mvc;
using NatechAssignmentCommon.DTO;
using NatechAssignmentCommon.Exception;
using NatechAssignmentCommon.Interface;
using System.Net;

namespace NatechAssignmentWebService.Controllers
{
    [ApiController]
    [Route("[action]")]
    public class IpGeolocationController : ControllerBase
    {
        private readonly INatechBusinessObject _businessObject;
        private readonly ILogger<IpGeolocationController> _logger;

        public IpGeolocationController(
            INatechBusinessObject businessObject,
            ILogger<IpGeolocationController> logger)
        {
            _businessObject = businessObject;
            _logger = logger;
        }

        [HttpGet(Name = nameof(GetLocationForIp))]
        [ProducesResponseType<GetLocationForIPResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetLocationForIp(string ipAddress)
        {
            _logger.LogDebug("Got new request with IP address {0}", ipAddress);
            try
            {
                if (string.IsNullOrWhiteSpace(ipAddress))
                {
                    return BadRequest("IP address was not provided");
                }

                if (!IPAddress.TryParse(ipAddress, out _))
                {
                    return BadRequest("Not a valid IP address provided");
                }

                var result = _businessObject.GetLocationForIp(ipAddress);
                if (result.IsFailed)
                {
                    return Problem(result.Errors.FirstOrDefault()?.Message);
                }

                return Ok(result);

            }
            catch (NatechException e)
            {
                _logger.LogError(e, "Error");
                return Problem(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error");
                return Problem(e.Message);
            }
        }

        [HttpPost(Name = nameof(GetLocationForListOfIp))]
        [ProducesResponseType<GetLocationForIPResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetLocationForListOfIp(string[] ipAddresses)
        {
            _logger.LogDebug("Got new request with list of IP address count: {0}", ipAddresses.Length);
            try
            {
                if (ipAddresses.Length == 0)
                {
                    return BadRequest("No IP addresses was provided");
                }

                var result = _businessObject.GetLocationForListOfIp(ipAddresses);
                if (result.IsFailed)
                {
                    return Problem(result.Errors.FirstOrDefault()?.Message);
                }

                // Construct the URL containing the GUID
                var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                var urlWithGuid = $"{baseUrl}/{nameof(GetLocationForListOfIpStatus)}/{result.Value}";

                // Return the URL in the response
                return Ok(urlWithGuid);
            }
            catch (NatechException e)
            {
                _logger.LogError(e, "Error");
                return Problem(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error");
                return Problem(e.Message);
            }
        }


        [HttpGet("{guid}", Name = nameof(GetLocationForListOfIpStatus))]
        [ProducesResponseType<GetLocationForIPResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetLocationForListOfIpStatus([FromRoute] string? guid)
        {
            _logger.LogDebug("Got new request with GUID: {0}", guid);
            try
            {
                if (string.IsNullOrWhiteSpace(guid))
                {
                    return BadRequest("GUID was not provided");
                }

                if (!Guid.TryParse(guid, out var requestGuid))
                {
                    return BadRequest("GUID was not recognized as a valid");
                }

                var result = _businessObject.GetLocationForListOfIpStatus(requestGuid);
                if (result.IsFailed)
                {
                    return Problem(result.Errors.FirstOrDefault()?.Message);
                }

                return Ok(result.Value);
            }
            catch (NatechException e)
            {
                _logger.LogError(e, "Error");
                return Problem(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error");
                return Problem(e.Message);
            }
        }
    }
}
