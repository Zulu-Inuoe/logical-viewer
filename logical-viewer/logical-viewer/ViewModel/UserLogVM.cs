using LogicalParse.DataModel;
using LogicalViewer.Util;

namespace LogicalViewer.ViewModel
{
    public class UserLogVM : ViewModelBase
    {
        public string Name { get; set; }
        public UserLog UserLog { get; set; }
    }
}
