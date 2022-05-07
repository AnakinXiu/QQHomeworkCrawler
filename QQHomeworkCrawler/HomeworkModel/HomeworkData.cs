using System.Collections.Generic;
using Newtonsoft.Json;

namespace QQHomeworkCrawler
{
    public class HomeworkData
    {
        public int gid { get; set; }

        public int team_id { get; set; }

        public int hw_type { get; set; }

        public long hw_publisher { get; set; }

        public int total { get; set; }

        public int unreview_num { get; set; }

        public int feedback_num { get; set; }

        [JsonProperty(PropertyName = "feedback")]
        public List<HomeworkItem> Feedback { get; set; } = new List<HomeworkItem>();

        public int comment_num { get; set; }

        public bool is_hw_exist { get; set; }

        public bool need_feedback { get; set; }

        public bool onekey_remind_limit { get; set; }

        public bool reviewed_num { get; set; }

        public bool single_remind_limit { get; set; }
    }
}