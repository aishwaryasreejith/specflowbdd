using System;
using System.IO;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using RestSharp;
using TechTalk.SpecFlow;

namespace SpecFlowProject.Steps
{
    [Binding]
    [Scope(Tag = "EasyPostTracking")]
    public class EasyPostTrackingSteps
    {
        private readonly RestClient _client;
        private RestResponse? _response;
        private string _apiEndpoint;
        private string _payload;

        public EasyPostTrackingSteps()
        {
            // Initialize RestClient
            _client = new RestClient();

            // Load EasyPost tracking API endpoint from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _apiEndpoint = configuration["EasyPosttrackingEndpoint"] ?? throw new InvalidOperationException("EasyPost tracking API endpoint is not configured in appsettings.json.");
            _payload = string.Empty;
        }

        private string ReadPayloadFromFile(string fileName, string folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName, fileName);
            return File.ReadAllText(filePath);
        }

        [Given(@"The request data from the '(.*)' file stored in the 'Payloads' folder")]
        public void GivenTheRequestDataFromTheFileStoredInTheFolder(string fileName)
        {
            _payload = ReadPayloadFromFile(fileName, "Payloads");
        }

        [Given(@"The EasyPost tracking api Endpoint is loaded from ""(.*)""")]
        public void GivenTheEasyPostTrackingApiEndpointIsLoadedFrom(string fileName)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(fileName)
                .Build();

            _apiEndpoint = configuration["EasyPosttrackingEndpoint"] ?? throw new InvalidOperationException("EasyPost tracking API endpoint is not configured in " + fileName);
            Console.WriteLine($"Loaded EasyPost tracking API endpoint: {_apiEndpoint}");
        }

        [Given(@"The X-Webhook-User-Id is set to ""(.*)""")]
        public void GivenTheXWebhookUserIdIsSetTo(string webhookUserId)
        {
            _client.AddDefaultHeader("X-Webhook-User-Id", webhookUserId);
        }

        [Given(@"The X-Hmac-Signature is set to ""(.*)""")]
        public void GivenTheXHmacSignatureIsSetTo(string hmacSignature)
        {
            _client.AddDefaultHeader("X-Hmac-Signature", hmacSignature);
        }

        [Given(@"The User-Agent is set to ""(.*)""")]
        public void GivenTheUserAgentIsSetTo(string userAgent)
        {
            _client.AddDefaultHeader("User-Agent", userAgent);
        }

        [Given(@"The Content-Type is set to ""(.*)""")]
        public void GivenTheContentTypeIsSetTo(string contentType)
        {
            _client.AddDefaultHeader("Content-Type", contentType);
        }

        [Given(@"The Connection is set to ""(.*)""")]
        public void GivenTheConnectionIsSetTo(string connection)
        {
            _client.AddDefaultHeader("Connection", connection);
        }

        [When(@"A POST request to the API with the data")]
        public void WhenISendAPostRequestToTheApiWithTheData()
        {
            var request = new RestRequest(_apiEndpoint, Method.Post);
            request.AddJsonBody(_payload);

            _response = _client.Execute(request);

            Console.WriteLine("Status Code: " + _response?.StatusCode);
            Console.WriteLine("Response Content: " + _response?.Content);
        }

        [Then(@"A response with status code (.*)")]
        public void ThenIShouldReceiveAResponseWithStatusCode(int expectedStatusCode = 200)
        {
            Assert.IsNotNull(_response, "The response should not be null.");
            Assert.That((int)(_response?.StatusCode ?? 0), Is.EqualTo(expectedStatusCode), "Expected status code did not match.");
        }
    }
}
