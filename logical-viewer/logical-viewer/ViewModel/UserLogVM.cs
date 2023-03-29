using LogicalParse.DataModel;
using LogicalViewer.Util;
using System.Collections.Generic;

namespace LogicalViewer.ViewModel
{
    public class UserLogVM : ViewModelBase
    {
        public UserLog UserLog
        {
            get => m_UserLog;
            set
            {
                if (m_UserLog != value)
                {
                    m_UserLog = value;

                    UserSyncQueues.Clear();
                    if (m_UserLog != null)
                    {
                        foreach (var queue in m_UserLog.UserSyncQueues)
                        {
                            UserSyncQueues.Add(new()
                            {
                                UserSyncQueue = queue
                            });
                        }
                    }
                }
            }
        }

        public string Name { get; set; }
        public List<UserAccount> UserAccounts => UserLog.UserAccounts;
        public List<UserCalendar> UserCalendars => UserLog.UserCalendars;
        public List<UserSyncQueueVM> UserSyncQueues { get; set; } = new();
        public UserCalendarSet CurrentCalendarSet => UserLog.CurrentCalendarSet;

        private UserLog m_UserLog;
    }
}
