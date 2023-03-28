namespace LogicalParse.DataModel
{
    public class UserAccount
    {
        public string Name { get; set; }
        public string ProviderID { get; set; }
        public bool Enabled { get; set; }
        // caldav, zoom, exchange, addressbook, todoist, weather, flexibits
        public string SourceKind { get; set; }

        // TODO - May be better to split these into subclasses if we can get enumerations of all of the source kinds
        public string? SourceUser { get; set; }
        public string? SourceProvider { get; set; }

        // TODO - Pull additional property list
    }
}
