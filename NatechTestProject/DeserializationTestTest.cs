using NatechAssignmentCommon.Extension;
using NatechAssignmentCommon.Ipbase.Status;
using Newtonsoft.Json;

namespace NatechTestProject;

[TestClass]
public class DeserializationTestTest
{
    [TestMethod]
    public void TestSample()
    {
        var response = File.ReadAllText(@".\Assets\ipbase_sample_response.json");
        var result = GetLocationForIpResponseExtension.FormIpbaseInfoResponse(response);
        Assert.IsTrue(result.IsSuccess);

        Assert.AreEqual("1.1.1.1", result.Value.IP);
    }

    [TestMethod]
    public void TestSuccessFullResponse()
    {
        var response = File.ReadAllText(@".\Assets\ipbase_sucess_response.json");
        var result = GetLocationForIpResponseExtension.FormIpbaseInfoResponse(response);
        Assert.IsTrue(result.IsSuccess);

        Assert.AreEqual("8.8.8.8", result.Value.IP);
    }

    [TestMethod]
    public void TestErrorResponse()
    {
        var response = File.ReadAllText(@".\Assets\ipbase_error_response.json");
        var result = GetLocationForIpResponseExtension.FormIpbaseInfoResponse(response);
        Assert.IsTrue(result.IsFailed);
        Assert.IsTrue(result.Errors.Count > 0);
        Assert.AreEqual("Validation error\r\nThe ip must be a valid IP address.\r\n", result.Errors[0].Message);
    }

    [TestMethod]
    public void TestStatusResponse()
    {
        var response = File.ReadAllText(@".\Assets\ipbase_status_response.json");
        var statusResponse = JsonConvert.DeserializeObject<StatusResponse>(response);
        Assert.IsNotNull(statusResponse);
        Assert.AreEqual(150, statusResponse.quotas.month.total);
    }
}