@ReliableTracking
Feature: Tracking for Reliable

    Scenario: Tracking for Reliable
        Given The request data from the 'ReliableTrackingPayload.json' file stored in the 'Payloads' folder
        And The ReliableTrackingEndpoint is loaded from "appsettings.json"
        When A POST request to the API with the data
        And the Username is set to 'ReliableUsername'
        And Password is set to 'ReliablePassword'
        Then A response with status code 200
