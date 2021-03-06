﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Dtos
{

    public class SmsDto
    {
        public string MobilePhone { get; set; }

        public string SmsCode { get; set; }

        public DateTime SendTime { get; set; }
    }

    public class RegisterDto
    {
        public string MobilePhone { get; set; }

        public string PassWord { get; set; }

        public string VerifyCode { get; set; }

        public string InvitationCode { get; set; }
    }

    public class LoginUserDto : DtoBase<int>
    {
        public string MobilePhone { get; set; }

        public string PassWord { get; set; }
    }


    public class BaseAreaDto : DtoBase<int>
    {
        public int? Pid { get; set; }

        public int Type { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public string Code { get; set; }

        public string PCode { get; set; }

        public string Area { get; set; }
    }




    public class ArticleDto
    {
        public Guid ArticleUID { get; set; }

        public string Code { get; set; }

        public int Type { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string FrontPic { get; set; }

        public int ClicksCount { get; set; }

        public int CommentsCount { get; set; }

        public decimal Score { get; set; }
        //打分人数
        public int ScoreCount { get; set; }
    }

    public class NavigationDto
    {
        public int Type { get; set; }

        public string Desc { get; set; }

        public string PicUrl { get; set; }

        public string Target { get; set; }

        public int Order { get; set; }

        public string ArticleUID { get; set; }
    }

    public class ItemInformationDto
    {
        public Guid Uid { get; set; }

        public string ArticleUid { get; set; }

        public string Type { get; set; }

        public string Target { get; set; }

        public string Title { get; set; }

        public string FrontPic { get; set; }

        public string Author { get; set; }

        public string Summary { get; set; }

        public int ClicksCount { get; set; }

        public int CommentsCount { get; set; }

        public decimal Score { get; set; }

        public DateTime CreateTime { get; set; }

        public string Location { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }


    }

    public class CommentDto
    {
        public string Type { get; set; }

        public Guid SubjectKey { get; set; }

        public string Content { get; set; }
    }

    public class CommentListDto
    {
        public Guid AuthID { get; set; }

        public string UserName { get; set; }

        public string AvatarUrl { get; set; }

        public string Content { get; set; }

        public string CreateTime { get; set; }
    }


    public class AttentionDto
    {
        public string Type { get; set; }

        public Guid Uid { get; set; }


        public string Title { get; set; }

        public string PicUrl { get; set; }

        public DateTime CreateTime { get; set; }
    }


    public class VideoInfoDto
    {
        public Guid VideoUID { get; set; }

        public string Title { get; set; }

        public string Presenter { get; set; }

        public string Introduction { get; set; }

        public string File { get; set; }

        public string Source { get; set; }

        public string Snapshot { get; set; }

        public double? Length { get; set; }

        public int Order { get; set; }

    }
}
