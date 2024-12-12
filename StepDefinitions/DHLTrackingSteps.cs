using System;
using System.IO;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using RestSharp;
using TechTalk.SpecFlow;

namespace SpecFlowProject.Steps
{
    [Binding]
    [Scope(Tag = "DHLTracking")]
    public class DHLTrackingSteps
    {
        private RestResponse? _response;  // Nullable _response field
        private string _apiEndpoint;      // Non-nullable _apiEndpoint field
        private string _payload;          // Non-nullable _payload field

        public DHLTrackingSteps()
        {
            // Initialize _payload to an empty string by default
            _payload = string.Empty;

            // Load API endpoint from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _apiEndpoint = configuration["DHLApiEndpoint"] ?? throw new InvalidOperationException("DHL API endpoint is not configured in appsettings.json.");
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

        // Given: The request data from the 'DHLPayload.json' file stored in the 'Payloads' folder
        [Given(@"The request data from the '(.*)' file stored in the 'Payloads' folder")]
        public void GivenTheRequestDataFromTheFileStoredInTheFolder(string fileName)
        {
            _payload = ReadPayloadFromFile(fileName, "Payloads");
        }

        // Given: The DHL API endpoint is loaded from "appsettings.json"
        [Given(@"The DHL API endpoint is loaded from ""(.*)""")]
        public void GivenTheDHLApiEndpointIsLoadedFrom(string fileName)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(fileName)
                .Build();

            _apiEndpoint = configuration["DHLApiEndpoint"] ?? throw new InvalidOperationException("DHL API endpoint is not configured in " + fileName);
            Console.WriteLine($"Loaded DHL API endpoint: {_apiEndpoint}");
        }

        // When: A POST request to the API with the data
        [When(@"A POST request to the API with the data")]
        public void WhenISendAPostRequestToTheApiWithTheData()
        {
            var client = new RestClient();
            var request = new RestRequest(_apiEndpoint, Method.Post);

            // Ensure _payload is not null when adding to the request
            request.AddJsonBody(_payload ?? string.Empty);  // Use a fallback to an empty string if _payload is null

            _response = client.Execute(request); // Assign response to nullable field

            Console.WriteLine("Status Code: " + _response?.StatusCode);
            Console.WriteLine("Response Content: " + _response?.Content);
        }

        // Then: A response with status code 201
        [Then(@"A response with status code (.*)")]
        public void ThenIShouldReceiveAResponseWithStatusCode(int expectedStatusCode = 201)
        {
            Assert.That(_response, Iz.Not.Null, "The response should not be null.");
            Assert.That((int)(_response?.StatusCode ?? 0), Is.EqualTo(expectedStatusCode), "Expected status code did not match.");
        }
    }
}