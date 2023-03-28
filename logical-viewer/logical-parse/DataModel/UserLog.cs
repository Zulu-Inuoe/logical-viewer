namespace LogicalParse.DataModel
{
    public class UserLog
    {
        public List<UserAccount> UserAccounts { get; set; }
        public List<UserCalendar> UserCalendars { get; set; }
        public List<UserSyncQueue> UserSyncQueues { get; set; }
        public UserCalendarSet CurrentCalendarSet { get; set; }
    }
}
