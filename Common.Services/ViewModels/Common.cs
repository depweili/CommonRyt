using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.ViewModels
{
    public class ViewTreeItem
    {
        public ViewTreeItem()
        {
            SubItem = new List<ViewTreeItem>();
        }
        public string key { get; set; }

        public string value { get; set; }

        public List<ViewTreeItem> SubItem { get; set; }
    }
}
