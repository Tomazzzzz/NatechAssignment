using FluentResults;
using NatechAssignmentCommon.DTO;
using NatechAssignmentCommon.Ipbase.Error;
using NatechAssignmentCommon.Ipbase.Info;
using Newtonsoft.Json;
using System.Text;

namespace NatechAssignmentCommon.Extension;

public static class GetLocationForIpResponseExtension
{
    public static Result<GetLocationForIPResponse> FormIpbaseInfoResponse(string ipbaseInfoResponse)
    {
        var result = JsonConvert.DeserializeObject<SuccessResponse>(ipbaseInfoResponse);
        if (result?.data is not null)
        {
            return new GetLocationForIPResponse
            {
                CountryCode = result.data.location.country.alpha2,
                CountryName = result.data.location.country.name,
                IP = result.data.ip,
                Latitude = result.data.location.latitude,
                Longitude = result.data.location.longitude,
                TimeZone = result.data.timezone.id
            };
        }

        var error = JsonConvert.DeserializeObject<ErrorResponse>(ipbaseInfoResponse);
        if (error is null)
        {
            return Result.Fail("Could not deserialize response");
        }

        var errorMessage = new StringBuilder(error.message).AppendLine();
        foreach (var errorError in error.errors.ip)
        {
            errorMessage.AppendLine(errorError);
        }
        return Result.Fail(errorMessage.ToString());
    }
}