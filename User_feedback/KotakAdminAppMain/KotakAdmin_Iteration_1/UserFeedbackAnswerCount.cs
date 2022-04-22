using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMBLNetNbanking
{
    public class UserFeedbackAnswerCount
    {
        public int like_total_count { get; set; }
        public int simplified_process_count { get; set; }
        public int easy_to_follow_instructions_count { get; set; }
        public int quick_login_count { get; set; }
        public int face_correctly_identified_count { get; set; }
        public int like_none_count { get; set; }
        public int not_like_total_count { get; set; }
        public int many_attempts_required_count { get; set; }
        public int instructions_are_confusing_count { get; set; }
        public int lengthy_process_count { get; set; }
        public int camera_not_working_count { get; set; }
        public int face_not_identified_correctly_count { get; set; }
        public int application_crashed_count { get; set; }
        public int not_like_none_count { get; set; }
    }
}