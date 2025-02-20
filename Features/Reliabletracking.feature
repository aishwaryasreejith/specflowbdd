@ReliableTracking
Feature: Tracking for Reliable
    This feature covers the Reliable tracking API

    Scenario: Tracking for Reliable
        Given The request data from the 'ReliablePayload.json' file stored in the 'Payloads' folder
        And The ReliableapiEndpoint is loaded from "appsettings.json"
        When A POST request to the API with the data
        And the Username is set to 'reliable_cdt'
        And Password is set to 'D6Z3Yp8K1N4xB7R2'
        Then A response with status code 200
