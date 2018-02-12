using Common.Domain;
using Common.Services.Dtos;
using Common.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public class CommonService : ServiceBase
    {

        public dynamic GetAreaTree()
        {
            using (var db = base.NewDB())
            {
                var res = new List<ViewTreeItem>();
                var data = db.Set<BaseArea>().Where(t=>t.Level==2).OrderBy(t=>t.Id);

                foreach (var item in data)
                {
                    var node = new ViewTreeItem
                    {
                        key = item.Id.ToString(),
                        value = item.Name
                    };

                    //下级第一个
                    //node.SubItem.Add(new ViewTreeItem
                    //{
                    //    key = item.Id.ToString(),
                    //    value = "全部"
                    //});

                    var subdata= db.Set<BaseArea>().Where(t => t.Pid == item.Id).OrderBy(t => t.Id);

                    foreach (var sub in subdata)
                    {
                        node.SubItem.Add(new ViewTreeItem
                        {
                            key = sub.Id.ToString(),
                            value = sub.Name
                        });
                    }

                    res.Add(node);
                }
                return res;
            }
        }

        public List<NavigationDto> GetNavigations(int type)
        {
            using (var db = base.NewDB())
            {
                var dblist = db.Set<Navigation>().Where(t => t.IsValid == true && t.Type == type).OrderBy(t => t.Order).ToList();

                var res = new List<NavigationDto>();
                foreach (var item in dblist)
                {
                    res.Add(new NavigationDto
                    {
                        //id = item.Id,
                        //desc = item.Desc,
                        //picurl = GetPicUrl(item.Pic),
                        //target = item.Target,
                        //articleid = item.ArticleID
                    });
                }

                return res;
            }
        }
    }
}
