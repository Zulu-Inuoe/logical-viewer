using LogicalParse.DataModel;

namespace LogicalParse
{
    public class Parser
    {
        public UserLog Parse(string str)
        {
            using (var reader = new StringReader(str))
            {
                return Parse(reader);
            }
        }

        public UserLog Parse(FileInfo f)
        {
            using (var stream = f.OpenRead())
            using (var reader = new StreamReader(stream))
            {
                return Parse(reader);
            }
        }

        public UserLog Parse(TextReader reader)
        {
            var ret = new UserLog();

            var parseState = ParseState.None;
            var calendarSet = default(UserCalendarSet);
            for (var line = reader.ReadLine(); line != null;)
            {
                if (line.Contains(" Accounts:"))
                {
                    parseState = ParseState.Accounts;
                    line = reader.ReadLine();
                }
                else if (line.Contains(" Calendars:"))
                {
                    parseState = ParseState.Calendars;
                    line = reader.ReadLine();
                }
                else if (line.Contains(" Current calendar set: "))
                {// TODO
                    parseState = ParseState.CalendarSet;
                    calendarSet = new UserCalendarSet();
                    line = reader.ReadLine();
                }
                else if (line.Contains(" Sync queues: "))
                {
                    parseState = ParseState.SyncQueues;
                }

                if (line is null)
                {
                    break;
                }

                switch (parseState)
                {
                    case ParseState.Accounts:
                        {// Look for a \t char to lessen chances of a false positive..
                            var tabPos = line.IndexOf('\t');
                            if (tabPos is -1 || tabPos == line.Length - 1)
                                break;

                            var split = line.Substring(tabPos + 1).Split(',');

                            // Name, ID, enabled|disabled, kind[, user[, provider]]
                            if (split.Length < 4 || 6 < split.Length)
                                break;
                            for(var i = 0; i < split.Length; ++i)
                            {
                                split[i] = split[i].Trim();
                            }
                            var enabledDisabled = split[2];
                            if (enabledDisabled != "enabled" && enabledDisabled != "disabled")
                                break;

                            ret.UserAccounts.Add(new()
                            {
                                Name = split[0],
                                ProviderID = split[1],
                                Enabled = enabledDisabled == "enabled",
                                SourceKind = split[3],
                                SourceUser = split.Length > 4 ? split[4] : null,
                                SourceProvider = split.Length > 5 ? split[5] : null
                            });

                            line = reader.ReadLine();
                            continue;
                        }
                    case ParseState.None:
                    default:
                        {
                            line = reader.ReadLine();
                        }
                        break;
                }

                // if we got here it means our parser state's no good
                parseState = ParseState.None;
            }

            return ret;
        }

        private enum ParseState
        {
            None,
            Accounts,
            Calendars,
            CalendarSet,
            SyncQueues
        }
    }
}
