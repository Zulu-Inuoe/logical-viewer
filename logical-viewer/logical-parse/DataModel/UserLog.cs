namespace LogicalParse.DataModel
{
    public class UserLog
    {
        public List<UserAccount> UserAccounts { get; set; } = new();
        public List<UserCalendar> UserCalendars { get; set; } = new();
        public List<UserSyncQueue> UserSyncQueues { get; set; } = new();
        public UserCalendarSet CurrentCalendarSet { get; set; }
        public string? UnresolvedErrors { get; set; }
    }
}
