using System;
using TechTalk.SpecFlow;

namespace Log.SpecFlow
{
    [Binding]
    public class QueryLogServicesApiSteps
    {
        [Given]
        public void Given_I_have_the_proper_access_to_the_log_Api()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When]
        public void When_I_call_the_api_endpoint_url_P0(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When]
        public void When_I_call_the_api_endpoint_url_P0_providing_the_P1_lapse(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When]
        public void When_I_call_the_api_endpoint_url_P0_providing_the_count_value(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_a_summary_of_the_entire_log_is_returned_grouped_by_event_level()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_a_filtered_summary_of_the_log_is_returned_grouped_by_event_level()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_the_entire_log_entries_are_returned_ordered_by_the_latest_time_stamps_first()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_a_filtered_list_of_log_entries_is_returned_ordered_by_the_latest_time_stamps_first()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_only_top_count_of_error_log_entries_are_returned_ordered_by_the_latest_time_stamps_first()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_only_top_count_of_critical_log_entries_are_returned_ordered_by_the_latest_time_stamps_first()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_only_top_count_of_information_log_entries_are_returned_ordered_by_the_latest_time_stamps_first()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_only_top_count_of_warning_log_entries_are_returned_ordered_by_the_latest_time_stamps_first()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_only_top_count_of_any_log_entries_are_returned_ordered_by_the_latest_time_stamps_first()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
