
using System;
using System.IO;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using RestSharp;
using TechTalk.SpecFlow;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;

namespace SpecFlowProject.Steps
{
    [Binding]
    [Scope(Tag = "ReliableRoute")]
    public class ReliableRouteSteps
    {
        private RestResponse? _response;  
        private string _apiEndpoint;   
        private string _payload;   

        public ReliableRouteSteps()
        {
            _payload = string.Empty;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _apiEndpoint = configuration["ReliableRouteEndpoint"] ?? throw new InvalidOperationException("API endpoint is not configured in appsettings.json.");
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

        [Given(@"The request data from the '(.*)' file stored in the 'Payloads' folder")]
        public void GivenTheRequestDataFromTheFileStoredInTheFolder(string fileName)
        {
            _payload = ReadPayloadFromFile(fileName, "Payloads");
        }

        [Given(@"The API endpoint is loaded from ""(.*)""")]
        public void GivenTheApiEndpointIsLoadedFrom(string fileName)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(fileName)
                .Build();

            _apiEndpoint = configuration["ReliableRouteEndpoint"] ?? throw new InvalidOperationException("API endpoint is not configured in " + fileName);
            Console.WriteLine($"Loaded API endpoint: {_apiEndpoint}");
        }

        [When(@"A POST request to the API with the data")]
        public void WhenISendAPostRequestToTheApiWithTheData()
        {
            var client = new RestClient();
            var request = new RestRequest(_apiEndpoint, Method.Post);

            request.AddJsonBody(_payload ?? string.Empty);  // Use a fallback to an empty string if _payload is null

            _response = client.Execute(request); // Assign response to nullable field

            Console.WriteLine("Status Code: " + _response?.StatusCode);
            Console.WriteLine("Response Content: " + _response?.Content);
        }

        [Then(@"A response with status code (.*)")]
        public void ThenIShouldReceiveAResponseWithStatusCode(int expectedStatusCode = 200)
        {
            Assert.That(_response, Is.Not.Null, "The response should not be null.");
            Assert.That((int)(_response?.StatusCode ?? 0), Is.EqualTo(expectedStatusCode), "Expected status code did not match.");
        }

        [Then(@"The response schema is valid according to the '(.*)' schema in the 'Schema' folder")]
        public void ThenTheResponseSchemaIsValidAccordingToTheSchemaInTheFolder(string schemaFileName)
        {
            var schemaFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Schema", schemaFileName);
            if (!File.Exists(schemaFilePath))
            {
                throw new FileNotFoundException($"Schema file {schemaFileName} not found in the 'Schema' folder.");
            }

            var schemaJson = File.ReadAllText(schemaFilePath);
            JSchema schema = JSchema.Parse(schemaJson);

            var responseJson = JObject.Parse(_response?.Content ?? "{}");

            bool isValid = responseJson.IsValid(schema);
            Assert.That(isValid, Is.True, "The response JSON does not match the schema.");
        }
    }
}
