using LogicalParse.DataModel;
using System.Text.RegularExpressions;

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
                else if (sc_CurrentCalendarSetRegex.IsMatch(line))
                {// TODO matching twice
                    parseState = ParseState.CalendarSet;

                    var match = sc_CurrentCalendarSetRegex.Match(line);
                    ret.CurrentCalendarSet = new()
                    {
                        Name = match.Groups[1].Value,
                        ID = match.Groups[2].Value
                    };

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
                            for (var i = 0; i < split.Length; ++i)
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
                    case ParseState.Calendars:
                        {
                            var match = sc_CalendarRegex.Match(line);
                            if (!match.Success)
                                break;

                            ret.UserCalendars.Add(new()
                            {
                                Name = match.Groups[1].Value,
                                Source = match.Groups[2].Value,
                                ID = match.Groups[3].Value,
                                SyncRx = match.Groups[4].Value == "1",
                                SyncTx = match.Groups[5].Value == "1",
                                Count = int.Parse(match.Groups[6].Value)
                            });
                            line = reader.ReadLine();
                            continue;
                        }
                    case ParseState.CalendarSet:
                        {
                            var match = sc_CalendarSetRegex.Match(line);
                            if (!match.Success)
                                break;

                            ret.CurrentCalendarSet.Entries.Add(new()
                            {
                                Name = match.Groups[1].Value,
                                ID = match.Groups[2].Value
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

        private static readonly Regex sc_CalendarRegex = new Regex(@"^.* \t(.+), (.+), (.+), (0|1)\/(0|1) \(count: (\d+)\)\s*$");
        private static readonly Regex sc_CurrentCalendarSetRegex = new Regex(@"^.* Current calendar set: (.+), (.+)\s*$");
        private static readonly Regex sc_CalendarSetRegex = new Regex(@"^.* \t(.+), (.+)\s*$");
    }
}
