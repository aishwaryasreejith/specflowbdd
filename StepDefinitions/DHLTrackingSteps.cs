using System;
using System.Net.Http;
using System.Text;
using NUnit.Framework;
using TechTalk.SpecFlow;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace MyNamespace
{
    [Binding]
    public class StepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private HttpResponseMessage? _response; // Marked as nullable
        private string? _apiEndpoint;  // Make it nullable

        public StepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        // Given The request data from the 'DHLPayload.json' file stored in the 'Payloads' folder
        [Given(@"The request data from the '(.*)' file stored in the '(.*)' folder")]
        public void GivenTheRequestDataFromTheFileStoredInTheFolder(string fileName, string folderPath)
        {
            var payloadPath = Path.Combine(folderPath, fileName);
            string requestData = File.ReadAllText(payloadPath);
            _scenarioContext["RequestData"] = requestData; // Store in ScenarioContext
        }

        // Given The DHL API endpoint is loaded from "appsettings.json"
        [Given(@"The DHLapiEndpoint is loaded from ""(.*)""")]
        public void GivenTheDHLapiEndpointIsLoadedFrom(string fileName)
        {
            // Load the configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Make sure it's looking in the right folder
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Retrieve the API endpoint from the configuration
            _apiEndpoint = configuration["DHLapiEndpoint"] ?? throw new InvalidOperationException("DHLapiEndpoint is not configured in appsettings.json");

            // Store the API endpoint in ScenarioContext
            _scenarioContext["DHLapiEndpoint"] = _apiEndpoint;

            Console.WriteLine($"Loaded DHLapiEndpoint: {_apiEndpoint}");
        }

        // When A POST request to the API with the data
        [When(@"A POST request to the API with the data")]
        public void WhenAPOSTRequestToTheAPIWithTheData()
        {
            var requestData = _scenarioContext["RequestData"] as string;
            var apiEndpoint = _scenarioContext["DHLapiEndpoint"] as string; // Retrieve the endpoint from ScenarioContext

            if (requestData == null || apiEndpoint == null)
            {
                throw new InvalidOperationException("Request data or API endpoint is not set.");
            }

            var httpClient = new HttpClient();
            var content = new StringContent(requestData, Encoding.UTF8, "application/json");

            _response = httpClient.PostAsync(apiEndpoint, content).Result;
        }

        // When the Username is set to 'DHLeComm-pp'
        [When(@"the Username is set to '(.*)'")]
        public void WhenTheUsernameIsSetTo(string username)
        {
            _scenarioContext["Username"] = username;
        }

        // When Password is set to '1kZ1znys8rjyF1c'
        [When(@"Password is set to '(.*)'")]
        public void WhenPasswordIsSetTo(string password)
        {
            _scenarioContext["Password"] = password;
        }

        // Then A response with status code 201
        [Then(@"A response with status code (.*)")]
        public void ThenAResponseWithStatusCode(int expectedStatusCode)
        {
            Assert.That(_response, Is.Not.Null, "The response should not be null.");
            Assert.That((int)(_response?.StatusCode ?? 0), Is.EqualTo(expectedStatusCode), "Expected status code did not match.");
        }
    }
}
