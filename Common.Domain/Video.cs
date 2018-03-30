using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class VideoSeries : EntityBase<int>
    {

        public VideoSeries()
        {
            VideoSeriesUID = Guid.NewGuid();
        }

        public Guid VideoSeriesUID { get; set; }

        public string Title { get; set; }

        public string Cover { get; set; }

        public string Folder { get; set; }

        public string Introduction { get; set; }

        public int? Total { get; set; }

        public virtual ICollection<VideoInfo> VideoInfos { get; set; }

    }

    public class VideoInfo : EntityBase<int>
    {
        public VideoInfo()
        {
            VideoUID = Guid.NewGuid();
        }

        public Guid VideoUID { get; set; }

        public string Title { get; set; }

        public string Presenter { get; set; }

        public string Introduction { get; set; }

        public string File { get; set; }

        public string Source { get; set; }

        public string Snapshot { get; set; }

        public double? Length { get; set; }

        public int Order { get; set; }

        public int VideoSeriesID { get; set; }
        public virtual VideoSeries VideoSeries { get; set; }

    }
}
