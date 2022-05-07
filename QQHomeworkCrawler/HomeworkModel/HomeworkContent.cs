using System.Collections.Generic;

namespace QQHomeworkCrawler
{
    public class HomeworkContent
    {
        public string comment { get; set; }

        public string examination { get; set; }

        public IList<HomeworkContentMain> main { get; set; } = new List<HomeworkContentMain>();
    }
}