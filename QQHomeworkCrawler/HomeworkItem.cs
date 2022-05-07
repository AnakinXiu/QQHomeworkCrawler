using System.Collections.Generic;
using System.Security.Permissions;

namespace QQHomeworkCrawler
{
    public class HomeworkItem
    {
        public  int comment_status { get; set; }

        public Content content { get; set; }

        public string feedback_ts { get; set; }

        /// <summary>
        /// Url for user head image
        /// </summary>
        public string head { get; set; }

        public string nick { get; set; }

        public int remind_ts { get; set; }
        public string review_ts { get; set; }
        public int status { get; set; }
        public string uin { get; set; }
    }

    public class Content
    {
        public string comment { get; set; }
        public string examination { get; set; }
        public IList<HomeworkMainItem> main { get; set; }

        public Content()
        {
            main = new List<HomeworkMainItem>();
        }
    }

    public class HomeworkMainItem
    {

    }
}