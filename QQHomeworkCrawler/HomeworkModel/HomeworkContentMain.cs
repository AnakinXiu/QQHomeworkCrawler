namespace QQHomeworkCrawler
{
    public class HomeworkContentMain
    {
        public string id { get; set; }
        
        public long uin { get; set; }
        
        public int create_ts { get; set; }
        
        public int modify_ts { get; set; }
        
        public int good_fb { get; set; }

        public HomeworkText text { get; set; }
    }
}