using Humanizer;
using LogicalParse.DataModel;
using LogicalViewer.Util;
using System;

namespace LogicalViewer.ViewModel
{
    public class UserSyncQueueVM : ViewModelBase
    {
        public UserSyncQueue UserSyncQueue { get; set; }

        public string SourceKind => UserSyncQueue.SourceKind;
        public string SourceID => UserSyncQueue.SourceID;
        public string? SourceUser => UserSyncQueue.SourceUser;
        public DateTime LastSync => UserSyncQueue.LastSync;
        public string LastSyncAgo => LastSync.Humanize();
        public string QueueKind => UserSyncQueue.QueueKind;
        public bool Active => UserSyncQueue.Active;
    }
}
