namespace LogicalParse.DataModel
{
    public class UserCalendarSet
    {
        public string Name { get; set; }
        public string ID { get; set; }

        public List<UserCalendarSetEntry> Entries = new();

        public class UserCalendarSetEntry
        {
            public string Name { get; set; }
            public string ID { get; set; }
        }
    }
}
