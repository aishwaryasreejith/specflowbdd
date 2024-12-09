using System;
using System.IO;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using RestSharp;
using TechTalk.SpecFlow;

namespace SpecFlowProject.Steps
{
    [Binding]
    [Scope(Tag = "ReturnLabel")]
    public class ReturnLabelSteps
    {
        private RestResponse? _response;
        private string _apiEndpoint;
        private string _payload;

        public ReturnLabelSteps()
        {
            // Load the API endpoint from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _apiEndpoint = configuration["ReturnLabelApiEndpoint"] ?? throw new InvalidOperationException("Return Label API endpoint is not configured in appsettings.json.");
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

        [When(@"A POST request to the API with the data")]
        public void WhenISendAPostRequestToTheApiWithTheData()
        {
            var client = new RestClient();
            var request = new RestRequest(_apiEndpoint, Method.Post);
            request.AddJsonBody(_payload);

            _response = client.Execute(request);

            Console.WriteLine("Status Code: " + _response?.StatusCode);
            Console.WriteLine("Response Content: " + _response?.Content);
        }

       /* [Then(@"A response with status code (.*)")]
        public void ThenIShouldReceiveAResponseWithStatusCode(int expectedStatusCode = 201)
        {
            // Correct usage of Assert.IsNotNull
            Assert.IsNotNull(_response, "The response should not be null.");
            Assert.That((int)(_response?.StatusCode ?? 0), Is.EqualTo(expectedStatusCode), "Expected status code did not match.");
        }*/
    }
}
