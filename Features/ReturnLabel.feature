@ReturnLabel
Feature: Return Label for Multiple Carriers

  Scenario: ReturnLabel for LSO
    Given The request data from the 'LSOPayload.json' file stored in the 'Payloads' folder
    And The API endpoint is loaded from "appsettings.json"
    When A POST request to the API with the data
    Then A response with status code 200
    And The response schema is valid according to the 'LSOSchema.json' schema in the 'Schema' folder

 Scenario: ReturnLabel for GLS
    Given The request data from the 'GLSPayload.json' file stored in the 'Payloads' folder
    And The API endpoint is loaded from "appsettings.json"
    When A POST request to the API with the data
    Then A response with status code 200
    And The response schema is valid according to the 'GLSSchema.json' schema in the 'Schema' folder

     Scenario: ReturnLabel for UPSMI
    Given The request data from the 'UPSMIPayload.json' file stored in the 'Payloads' folder
    And The API endpoint is loaded from "appsettings.json"
    When A POST request to the API with the data
    Then A response with status code 200
    And The response schema is valid according to the 'UPSMISchema.json' schema in the 'Schema' folder

      Scenario: ReturnLabel for Landmark
    Given The request data from the 'LandmarkPayload.json' file stored in the 'Payloads' folder
    And The API endpoint is loaded from "appsettings.json"
    When A POST request to the API with the data
    Then A response with status code 200
    And The response schema is valid according to the 'LandmarkSchema.json' schema in the 'Schema' folder

    Scenario: ReturnLabel for Veho
    Given The request data from the 'VehoPayload.json' file stored in the 'Payloads' folder
    And The API endpoint is loaded from "appsettings.json"
    When A POST request to the API with the data
    Then A response with status code 200
    And The response schema is valid according to the 'VehoSchema.json' schema in the 'Schema' folder