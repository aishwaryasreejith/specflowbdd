@ReturnLabel
Feature: Return Label for Multiple Carriers

  Scenario: ReturnLabel for Better Trucks
    Given The request data from the 'BetterTrucksPayload.json' file stored in the 'Payloads' folder
    And The API endpoint is loaded from "appsettings.json"
    When A POST request to the API with the data
    Then A response with status code 200

  Scenario: ReturnLabel for GLS
    Given The request data from the 'GLSPayload.json' file stored in the 'Payloads' folder
    And The API endpoint is loaded from "appsettings.json"
    When A POST request to the API with the data
    Then A response with status code 200

  Scenario: ReturnLabel for LSO
    Given The request data from the 'LSOPayload.json' file stored in the 'Payloads' folder
    And The API endpoint is loaded from "appsettings.json"
    When A POST request to the API with the data
    Then A response with status code 200

  Scenario: ReturnLabel for UPSMI
    Given The request data from the 'UPSMIPayload.json' file stored in the 'Payloads' folder
    And The API endpoint is loaded from "appsettings.json"
    When A POST request to the API with the data
    Then A response with status code 200