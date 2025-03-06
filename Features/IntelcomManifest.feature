@IntelcomManifest
Feature: Return Manifest for Intelcom

  Scenario: ReturnManifest for Intelcom
    Given The request data from the 'IntelcomManifestPayload.json' file stored in the 'Payloads' folder
    And The ReliableManifestEndpoint is loaded from "appsettings.json"
    When A POST request to the API with the data
    Then A response with status code 200
