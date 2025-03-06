@ReliableManifest
Feature: Return Manifest

  Scenario: ReturnManifest for Reliable
    Given The request data from the 'ReliableManifestPayload.json' file stored in the 'Payloads' folder
    And The ReliableManifestEndpoint is loaded from "appsettings.json"
    When A POST request to the API with the data
    Then A response with status code 200
