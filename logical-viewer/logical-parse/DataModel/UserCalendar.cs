namespace LogicalParse.DataModel
{
    public class UserCalendar
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string ID { get; set; }

        // Just a guess for the 1/0 0/1 1/1
        // I'm guessing these are sync directions
        // TODO - clarify w/ Kent
        public bool SyncRx { get; set; }
        public bool SyncTx { get; set; }
        public int Count { get; set; }
        public string Details { get; set; }
    }
}
