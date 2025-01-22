@EasyPostTracking
Feature: EasyPost Tracking API

  Scenario: Testing EasyPost Tracking API
    Given The request data from the 'easypostpayload.json' file stored in the 'Payloads' folder
    And The EasyPost tracking api Endpoint is loaded from "appsettings.json"
    And The X-Webhook-User-Id is set to "user_15ac5eb88f2a480080f2f23b3649db1d"
    And The X-Hmac-Signature is set to "hmac-sha256-hex=448ad4ac7f2b236d4126f3663409eca8576d1fe7fdbfa531f563cbe53dd9c12b"
    And The User-Agent is set to "EasyPost WebHook Agent 1.0"
    And The Content-Type is set to "application/json"
    And The Connection is set to "close"
    When A POST request to the API with the data
    Then A response with status code 200
    And The response body matches the schema in 'EasypostSchema.json' file stored in the 'Schema' folder
