namespace LogicalParse.DataModel
{
    public class UserSyncQueue
    {
        public string SourceKind { get; set; }
        public string SourceID { get; set; }
        public string? SourceUser { get; set; }
        public DateTime LastSync { get; set; }
        // TODO - Only seen 'FBSyncQueue' but maybe others?
        public string QueueKind { get; set; }
        public bool Active { get; set; }

        // TODO - Pull additional property list
    }
}
