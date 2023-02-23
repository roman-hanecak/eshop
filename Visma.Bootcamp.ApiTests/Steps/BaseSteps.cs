using System.Net;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using RestSharp;

namespace Visma.Bootcamp.ApiTests.Steps
{
    public class BaseSteps
    {
        protected BaseSteps()
        {
        }

        protected void VerifyResponse(RestResponse response, HttpStatusCode statusCode, 
            [CallerLineNumber] int callerLineNumber = 0, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "")
        {
            Assert.That(response.StatusCode, Is.EqualTo(statusCode),
                $"Status code: {response.StatusCode}, expected: {statusCode} " +
                $"Message: [{response.Content}] " +
                $"called from file {callerFilePath}: {callerMemberName} line {callerLineNumber}");
        }
    }
}