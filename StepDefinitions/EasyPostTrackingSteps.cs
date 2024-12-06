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
        private RestResponse? _response;  // Nullable _response field
        private string _apiEndpoint;      // Non-nullable _apiEndpoint field
        private string _payload;          // Non-nullable _payload field

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
            _payload = string.Empty;  // Ensure payload is not null
        }

        private string ReadPayloadFromFile(string fileName, string folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName, fileName);
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                throw new FileNotFoundException($"The file {fileName} was not found in the {folderName} folder.");
            }
        }

        // Given: The request data from the 'EasyPostTrackingPayload.json' file stored in the 'Payloads' folder
        [Given(@"The request data from the '(.*)' file stored in the 'Payloads' folder")]
        public void GivenTheRequestDataFromTheFileStoredInTheFolder(string fileName)
        {
            _payload = ReadPayloadFromFile(fileName, "Payloads");
        }

        // Given: The EasyPost tracking API endpoint is loaded from "appsettings.json"
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

        // Given: The X-Webhook-User-Id is set to a specific value
        [Given(@"The X-Webhook-User-Id is set to ""(.*)""")]
        public void GivenTheXWebhookUserIdIsSetTo(string webhookUserId)
        {
            _client.AddDefaultHeader("X-Webhook-User-Id", webhookUserId);
        }

        // Given: The X-Hmac-Signature is set to a specific value
        [Given(@"The X-Hmac-Signature is set to ""(.*)""")]
        public void GivenTheXHmacSignatureIsSetTo(string hmacSignature)
        {
            _client.AddDefaultHeader("X-Hmac-Signature", hmacSignature);
        }

        // Given: The User-Agent is set to "EasyPost WebHook Agent 1.0"
        [Given(@"The User-Agent is set to ""(.*)""")]
        public void GivenTheUserAgentIsSetTo(string userAgent)
        {
            _client.AddDefaultHeader("User-Agent", userAgent);
        }

        // Given: The Content-Type is set to "application/json"
        [Given(@"The Content-Type is set to ""(.*)""")]
        public void GivenTheContentTypeIsSetTo(string contentType)
        {
            _client.AddDefaultHeader("Content-Type", contentType);
        }

        // Given: The Connection is set to "close"
        [Given(@"The Connection is set to ""(.*)""")]
        public void GivenTheConnectionIsSetTo(string connection)
        {
            _client.AddDefaultHeader("Connection", connection);
        }

        // When: A POST request to the API with the data
        [When(@"A POST request to the API with the data")]
        public void WhenISendAPostRequestToTheApiWithTheData()
        {
            var request = new RestRequest(_apiEndpoint, Method.Post);

            // Ensure _payload is not null when adding to the request
            request.AddJsonBody(_payload ?? string.Empty);  // Fallback to an empty string if _payload is null

            _response = _client.Execute(request); // Assign response to nullable field

            Console.WriteLine("Status Code: " + _response?.StatusCode);
            Console.WriteLine("Response Content: " + _response?.Content);
        }

        // Then: A response with status code 200
        [Then(@"A response with status code (.*)")]
        public void ThenIShouldReceiveAResponseWithStatusCode(int expectedStatusCode = 200)
        {
            // Ensure _response is not null
            Assert.IsNotNull(_response, "The response should not be null.");
            Assert.That((int)(_response?.StatusCode ?? 0), Is.EqualTo(expectedStatusCode), "Expected status code did not match.");
        }
    }
}
