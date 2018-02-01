using Common.Domain;
using Common.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public class CommonService : ServiceBase
    {
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
