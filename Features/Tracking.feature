@DHLTracking
Feature: Tracking for DHL
    This feature covers the DHL tracking API

    Scenario: Tracking for DHL
        Given The request data from the 'DHLPayload.json' file stored in the 'Payloads' folder
        And The DHLapiEndpoint is loaded from "appsettings.json"
        When A POST request to the API with the data
        And the Username is set to 'DHLeComm-pp'
        And Password is set to '1kZ1znys8rjyF1c'
        Then A response with status code 201
