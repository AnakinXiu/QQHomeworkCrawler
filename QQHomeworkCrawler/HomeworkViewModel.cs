using System.Collections.Generic;

namespace QQHomeworkCrawler
{
    public class HomeworkViewModel
    {
        public string StudentName { get; set; }

        public List<string> ImageUrl { get; set; }

        public HomeworkViewModel(string studentName, params string[] imageUrls)
        {
            StudentName = studentName;

            ImageUrl = new List<string>(imageUrls);
        }

        public HomeworkViewModel(string studentName, IEnumerable<string> imageUrls)
        {
            StudentName = studentName;

            ImageUrl = new List<string>(imageUrls);
        }
    }
}