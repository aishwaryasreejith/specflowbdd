@ReliableRoute
Feature: Return Route

  Scenario: Return Route for Reliable
    Given The request data from the 'ReliableRoutePayload.json' file stored in the 'Payloads' folder
    And The API endpoint is loaded from "appsettings.json"
    When A POST request to the API with the data
    Then A response with status code 200
    # And The response schema is valid according to the 'ReliableRouteSchema.json' schema in the 'Schema' folder