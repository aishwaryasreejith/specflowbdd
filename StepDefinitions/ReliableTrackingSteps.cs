
using System.IO;
using Newtonsoft.Json.Linq;
using RestSharp;
using TechTalk.SpecFlow;
using NUnit.Framework;

[Binding]
public class TrackingForReliableSteps
{
    private readonly ScenarioContext _scenarioContext;
    private IRestResponse? _response;
    private object? expectedStatusCode;

    public TrackingForReliableSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"The request data from the 'ReliableTrackingPayload.json' file stored in the 'Payloads' folder")]
    public void GivenTheRequestDataFromTheReliableTrackingPayloadJsonFileStoredInThePayloadsFolder()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Payloads", "ReliableTrackingPayload.json");
        var jsonData = File.ReadAllText(filePath);
        _scenarioContext["RequestData"] = JObject.Parse(jsonData);
    }

    [Given(@"The ReliableTrackingEndpoint is loaded from ""(.*)""")]
    public void GivenTheReliableTrackingEndpointIsLoadedFrom(string appSettingsFile)
    {
        var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), appSettingsFile);
        var appSettingsJson = JObject.Parse(File.ReadAllText(appSettingsPath));
        _scenarioContext["Endpoint"] = appSettingsJson["ReliableTrackingEndpoint"].ToString();
    }

    [When(@"A POST request to the API with the data")]
    public void WhenAPostRequestToTheAPIWithTheData()
    {
        var client = new RestClient(_scenarioContext["Endpoint"].ToString());
        var request = new RestRequest(_scenarioContext["Endpoint"].ToString(), Method.Post);
        request.AddJsonBody(_scenarioContext["RequestData"]);
        _scenarioContext["Request"] = request; // Store the request in the scenario context
        _response = (IRestResponse?)client.Execute(request);
    }

    [When(@"the Username is set to 'ReliableUsername'")]
    public void WhenTheUsernameIsSetToReliableUsername()
    {
        var request = _scenarioContext["Request"] as RestRequest;
        request?.AddHeader("Username", "ReliableUsername");
    }

    [When(@"Password is set to 'ReliablePassword'")]
    public void WhenPasswordIsSetToReliablePassword()
    {
        var request = _scenarioContext["Request"] as RestRequest;
        request?.AddHeader("Password", "ReliablePassword");
    }

    [Then(@"A response with status code (.*)")]
    public void ThenAResponseWithStatusCode(int statusCode)
    {
Assert.That(_response, Is.Not.Null, "The response should not be null.");
            Assert.That((int)(_response?.StatusCode ?? 0), Is.EqualTo(expectedStatusCode), "Expected status code did not match.");    }
}

internal interface IRestResponse
{
    int StatusCode { get; }
}
