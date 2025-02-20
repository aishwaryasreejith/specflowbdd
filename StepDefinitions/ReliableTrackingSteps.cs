
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TechTalk.SpecFlow;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace MyNamespace
{
    [Binding]
    public class ReliableTrackingSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private HttpResponseMessage? _response;
        private string? _apiEndpoint;
        private static readonly HttpClient _httpClient = new HttpClient();

        public ReliableTrackingSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"The request data from the '(.*)' file stored in the '(.*)' folder")]
        public void GivenTheRequestDataFromTheFileStoredInTheFolder_Reliable(string fileName, string folderPath)
        {
            var payloadPath = Path.Combine(folderPath, fileName);
            if (!File.Exists(payloadPath))
            {
                throw new FileNotFoundException($"Payload file not found: {payloadPath}");
            }
            string requestData = File.ReadAllText(payloadPath);
            _scenarioContext["RequestData"] = requestData;
        }

        [Given(@"The ReliableapiEndpoint is loaded from ""(.*)""")]
        public void GivenTheReliableapiEndpointIsLoadedFrom_Reliable(string fileName)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(fileName, optional: false, reloadOnChange: true)
                .Build();

            _apiEndpoint = configuration["ReliableapiEndpoint"] ?? throw new InvalidOperationException("ReliableapiEndpoint is not configured in appsettings.json");
            _scenarioContext["ReliableapiEndpoint"] = _apiEndpoint;
            Console.WriteLine($"Loaded ReliableapiEndpoint: {_apiEndpoint}");
        }

        [When(@"A POST request to the API with the data")]
        public async Task WhenAPOSTRequestToTheAPIWithTheData_Reliable()
        {
            var requestData = _scenarioContext["RequestData"] as string;
            var apiEndpoint = _scenarioContext["ReliableapiEndpoint"] as string;

            if (string.IsNullOrEmpty(requestData) || string.IsNullOrEmpty(apiEndpoint))
            {
                throw new InvalidOperationException("Request data or API endpoint is not set.");
            }

            var content = new StringContent(requestData, Encoding.UTF8, "application/json");
            _response = await _httpClient.PostAsync(apiEndpoint, content);
        }

        [When(@"the Username is set to '(.*)'")]
        public void WhenTheUsernameIsSetTo_Reliable(string username)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            username = configuration["ReliableUsername"] ?? throw new InvalidOperationException("ReliableUsername is not configured in appsettings.json");
            _scenarioContext["Username"] = username;
        }

        [When(@"Password is set to '(.*)'")]
        public void WhenPasswordIsSetTo_Reliable(string password)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            password = configuration["ReliablePassword"] ?? throw new InvalidOperationException("ReliablePassword is not configured in appsettings.json");
            _scenarioContext["Password"] = password;
        }

        [Then(@"A response with status code (.*)")]
        public void ThenAResponseWithStatusCode_Reliable(int expectedStatusCode)
        {
            Assert.That(_response, Is.Not.Null, "The response should not be null.");
            Assert.That((int)(_response?.StatusCode ?? 0), Is.EqualTo(expectedStatusCode), "Expected status code did not match.");
        }
    }
}
