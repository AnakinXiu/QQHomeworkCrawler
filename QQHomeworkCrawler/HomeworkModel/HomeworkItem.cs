namespace QQHomeworkCrawler
{
    public class HomeworkItem
    {
        public int comment_status { get; set; }

        public HomeworkContent content { get; set; }

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
}