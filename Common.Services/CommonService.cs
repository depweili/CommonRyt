﻿using Common.Domain;
using Common.Services.Dtos;
using Common.Services.ViewModels;
using Common.Util.Extesions;
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
            using (var db = base.NewDB())
            {
                IEnumerable<ItemInformationDto> dblist = null;

                var expression = LinqExtensions.True<Article>();
                var queryParam = queryJson.ToJObject();

                expression = expression.And(t => (t.IsDeleted ?? false) == false && (t.IsValid ?? true) == true);

                //if (!queryParam["areaid"].IsEmpty() && queryParam["areaid"].ToString() != "-1")
                //{
                //    string keyword = queryParam["areaid"].ToString();

                //    var inlist = Function.GetColumnListByTree<int>(db, keyword, "Ryt_BaseArea");
                //    expression = expression.And(t => inlist.Contains(t.MedicineDepartment.Hospital.AreaID ?? 0));
                //}

                var query = db.Set<Article>().Where(expression).OrderByDescending(t => t.Order).ThenByDescending(t=>t.CreateTime).Select(t => new ItemInformationDto
                {
                    Author = t.Author,
                    Title = t.Title,
                    ClicksCount = t.ClicksCount,
                    CommentsCount = t.CommentsCount,
                    FrontPic = t.FrontPic,
                    Score = t.Score,
                    Uid = t.ArticleUID,
                    Type = "Article",
                    CreateTime = t.CreateTime
                });


                var list = Function.GetPageData(query, queryParam);

                var res = list.ToList();

                return res;
            }
        }


    }
}
