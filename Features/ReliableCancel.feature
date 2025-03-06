@ReliableCancel
Feature: Return Cancel

  Scenario: ReturnCancel for Reliable
    Given The request data from the 'ReliableCancelPayload.json' file stored in the 'Payloads' folder
    And The ReliableCancelEndpoint is loaded from "appsettings.json"
    When A POST request to the API with the data
    Then A response with status code 200
