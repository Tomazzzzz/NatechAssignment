using FluentResults;
using NatechAssignmentCommon.DTO;
using System;

namespace NatechAssignmentCommon.Interface;

public interface INatechBusinessObject
{
    /// <summary>
    /// Return geolocation info of the IP
    /// </summary>
    /// <param name="ip">IP address that will be used to get geolocation</param>
    /// <returns>Result object, if success then the value will contain geolocation information
    /// On error there will be Error message</returns>
    Result<GetLocationForIPResponse> GetLocationForIp(string ip);

    /// <summary>
    /// Gets a array of IP addresses and starts process
    /// </summary>
    /// <param name="ipAddresses">Array of IP addresses that will be processed</param>
    /// <returns>Result object, will return a GUID that will represent the process that may be used to get process status.
    /// On error there will b Error message</returns>
    Result<Guid> GetLocationForListOfIp(string[] ipAddresses);

    /// <summary>
    /// Returns the status of the process by GUID
    /// </summary>
    /// <param name="guid">The process key, this is use to distinguish processes</param>
    /// <returns>Result object, if success then the value will contain status of the process by GUID
    /// On error there will be Error message</returns>
    Result<GetLocationForListOfIpStatusResponse> GetLocationForListOfIpStatus(Guid guid);
}