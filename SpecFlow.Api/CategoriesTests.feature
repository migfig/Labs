#Following code could be completely generated from API endpoint code by using reflection or from Postman collection
Feature: CategoriesEndpointTests
	In order to ensure the coding quality of the Categories endpoint 
	As a endpoint consumer 
	I want to be able to run the following test case scenarios

#Token generation could use app.config or some other config file instead of providing following config tables
Background: Prepare run tests with an authentication token
	Given I provide run settings as table
	| Program                              | User                 | Password  | ApiKey | TargetUrl             |
	| C:\Program Files\Git\usr\bin\curl.exe | test.user@domain.com | test1234@ |        | http://localhost:3033 |
	And I call the authentication endpoint with values 
	| Url                                                   | Method | Headers                       | Payload | Property |
	| http://localhost:3033/api/authentication/authenticate | POST   | Content-Type:application/json | {"User":"$user$", "Password":"$password$", "ApiKey":"$apikey$", "TargetUrl":"$targeturl$"} | Code     |
	And then call the token endpoint with values
	| Url                                                 | Method | Headers                       | Payload                              | Property |
	| http://localhost:3033/api/authentication/token/code | POST   | Content-Type:application/json | {"Code":"$code$", "TargetUrl":"$targeturl$"} | Token         |

@ListAllCategories
Scenario: List all categories available
	Given I have been granted with a valid access token 'Token'
	When I call the endpoint with values
	| Url                                  | Method | Headers                                             | Property                   |
	| http://localhost:3033/api/categories | GET    | Content-Type:application/json;Authorization:$token$ | ListAllCategories[Id,Name] |
	Then result items count should be 2 and values match the table for property 'ListAllCategories'
	| Id | Name        |
	| 1  | Electronics |
	| 2  | Outdoors    |

@AddCategory
Scenario: Add a new category
	Given I have been granted with a valid access token 'Token'
	When I call the endpoint with values
	| Url                                      | Method | Headers                                             | Property | Payload |
	| http://localhost:3033/api/categories/add | POST   | Content-Type:application/json;Authorization:$token$ | AddCategory(Id,Name)  | {"Name": "Kitchen"} |
	Then result items count should be 1 and values match the table for property 'AddCategory'
	| Id | Name    |
	| 3  | Kitchen |

@GetCategory
Scenario: Get an specific category
	Given I have been granted with a valid access token 'Token'
	When I call the endpoint with values
	| Url                                    | Method | Headers                                             | Property             |
	| http://localhost:3033/api/categories/3 | GET    | Content-Type:application/json;Authorization:$token$ | GetCategory(Id,Name) |
	Then result items count should be 1 and values match the table for property 'GetCategory'
	| Id | Name    |
	| 3  | Kitchen |

@UpdateCategory
Scenario: Update a category
	Given I have been granted with a valid access token 'Token'
	When I call the endpoint with values
	| Url                                      | Method | Headers                                             | Property | Payload |
	| http://localhost:3033/api/categories/update/3 | PUT   | Content-Type:application/json;Authorization:$token$ | UpdateCategory(Id,Name)  | {"Name":"Gourmet Kitchen"} |
	Then result items count should be 1 and values match the table for property 'UpdateCategory'
	| Id | Name            |
	| 3  | Gourmet Kitchen |

@DeleteCategory
Scenario: Delete an specific category
	Given I have been granted with a valid access token 'Token'
	When I call the endpoint with values
	| Url                                    | Method | Headers                                             | Property             |
	| http://localhost:3033/api/categories/3 | DELETE    | Content-Type:application/json;Authorization:$token$ | DeleteCategory(Id,Name) |
	Then result items count should be 1 and values match the table for property 'DeleteCategory'
	| Id | Name            |
	| 3  | Gourmet Kitchen |
