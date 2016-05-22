Feature: Query Log Services Api
	In order to be able to quickly review services logs and its execution behavior
	As a consumer of the log Api
	I want to query the log using several criteria searches

@logSummary
Scenario: Retrieve entire log summary
	Given I have the proper access to the log Api
	When I call the api endpoint url 'api/log/summary'
	Then a summary of the entire log is returned grouped by event level

@logSummaryForTimeSpan
Scenario: Retrieve log summary for a given time span
	Given I have the proper access to the log Api
	When I call the api endpoint url 'api/log/summary/{timeSpan}' providing the 'timeSpan' lapse
	Then a filtered summary of the log is returned grouped by event level

@allLogEntries
Scenario: Retrieve entire log entries
	Given I have the proper access to the log Api
	When I call the api endpoint url 'api/log/items'
	Then the entire log entries are returned ordered by the latest time stamps first 

@logEntriesForTimeSpan
Scenario: Retrieve log entries for a given time span
	Given I have the proper access to the log Api
	When I call the api endpoint url 'api/log/items/{timeSpan}' providing the 'timeSpan' lapse
	Then a filtered list of log entries is returned ordered by the latest time stamps first 

@topErrorEntries
Scenario: Retrieve top n error log entries
	Given I have the proper access to the log Api
	When I call the api endpoint url 'api/log/top/errors/{int:count}' providing the count value
	Then only top count of error log entries are returned ordered by the latest time stamps first 

@topCriticalErrorEntries
Scenario: Retrieve top n critical error log entries
	Given I have the proper access to the log Api
	When I call the api endpoint url 'api/log/top/critical/{int:count}' providing the count value
	Then only top count of critical log entries are returned ordered by the latest time stamps first 

@topInformationEntries
Scenario: Retrieve top n information log entries
	Given I have the proper access to the log Api
	When I call the api endpoint url 'api/log/top/information/{int:count}' providing the count value
	Then only top count of information log entries are returned ordered by the latest time stamps first 

@topWarningEntries
Scenario: Retrieve top n warning log entries
	Given I have the proper access to the log Api
	When I call the api endpoint url 'api/log/top/warning/{int:count}' providing the count value
	Then only top count of warning log entries are returned ordered by the latest time stamps first 

@topEntries
Scenario: Retrieve top n log entries no matter what kind of log level they are
	Given I have the proper access to the log Api
	When I call the api endpoint url 'api/log/top/latest/{int:count}' providing the count value
	Then only top count of any log entries are returned ordered by the latest time stamps first 
