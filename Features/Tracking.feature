 @DHLTracking
Feature: Tracking for DHL
    This feature covers the DHL tracking API

    Scenario: Tracking for DHL
        Given The request data from the 'DHLPayload.json' file stored in the 'Payloads' folder
        And The DHL API endpoint is loaded from "appsettings.json"
        When A POST request to the API with the data
        Then A response with status code 201

     