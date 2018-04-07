using Common.Domain;
using Common.Services.Dtos;
using Common.Services.ViewModels;
using Common.Util;
using Common.Util.Extesions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                var data = db.Set<Navigation>().Where(t => t.IsValid == true && t.Type == type).OrderBy(t => t.Order).ToList();

                

                var res = data.MapToList<NavigationDto>();

                //var res = new List<NavigationDto>();
                //foreach (var item in data)
                //{
                //    res.Add(new NavigationDto
                //    {
                //        //id = item.Id,
                //        //desc = item.Desc,
                //        //picurl = GetPicUrl(item.Pic),
                //        //target = item.Target,
                //        //articleid = item.ArticleID
                //    });
                //}

                return res;
            }
        }


        public dynamic GetArticles(string queryJson)
        {
            string cacheKey = "GetArticles" + queryJson;
            try
            {
                var res = CacheHelper.Get<List<ItemInformationDto>>(cacheKey);

                if (res != null)
                {
                    return res;
                }
            }
            catch (Exception)
            {
                throw;
            }

            using (var db = base.NewDB())
            {
                IEnumerable<ItemInformationDto> dblist = null;

                var expression = LinqExtensions.True<Article>();
                var queryParam = queryJson.ToJObject();

                expression = expression.And(t => (t.IsDeleted ?? false) == false && (t.IsValid ?? true) == true);

                if (!queryParam["Type"].IsEmpty())
                {
                    int keyword = queryParam["Type"].ToInt();

                    expression = expression.And(t => t.Type == keyword);
                }
                else
                {
                    expression = expression.And(t => t.Type == 0);
                }


                var StaticPicUrlHost = Function.GetStaticPicUrlHost();

                var query = db.Set<Article>().Where(expression).OrderByDescending(t => t.Order).ThenByDescending(t => t.CreateTime).Select(t => new ItemInformationDto
                {
                    Author = t.Author,
                    Title = t.Title,
                    ClicksCount = t.ClicksCount,
                    CommentsCount = t.CommentsCount,
                    FrontPic = string.IsNullOrEmpty(t.FrontPic)? null : StaticPicUrlHost + t.FrontPic,
                    Score = t.Score,
                    Uid = t.ArticleUID,
                    Type = "Article",
                    CreateTime = t.CreateTime
                });


                var list = Function.GetPageData(query, queryParam);

                var res = list.ToList();

                CacheHelper.Set(cacheKey, res, DateTime.Now.AddMinutes(_cacheabsoluteminutes));

                return res;
            }
        }

        public ArticleDto GetArticle(Guid uid)
        {
            using (var db = base.NewDB())
            {
                var dbitem = db.Set<Article>().SingleOrDefault(t => t.ArticleUID == uid);

                if (dbitem == null)
                {
                    dbitem = db.Set<Article>().SingleOrDefault(t => t.Type == -1 && t.Title == "404");
                }

                //var content = dbitem == null ? string.Empty : dbitem.Content;
                //content = content.Replace("{host}", Function.GetHostAndApp());

                if (dbitem == null)
                {
                    return null;
                }
                else
                {
                    dbitem.ClicksCount++;
                    db.SaveChanges();
                }

                    //ArticleDto res = new ArticleDto
                    //{
                    //    Author = dbitem.Author,
                    //    title = dbitem.Title,
                    //    createtime = dbitem.CreateTime,
                    //    content = dbitem.Content.Replace("{host}", Function.GetHostAndApp())
                    //};

                var res = dbitem.MapTo<ArticleDto>();

                return res;

                //return new ArticleDto { content = content};
            }
        }

    }
}
