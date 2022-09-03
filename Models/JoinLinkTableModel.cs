namespace MinimalDatatablesEditor.Models
{
    public class JoinLinkTableModel
    {
        public class users
        {
            public string first_name { get; set; }

            public string last_name { get; set; }

            public int site { get; set; }
        }

        public class sites
        {
            public string name { get; set; }
        }

        public class user_dept
        {
            public int dept_id { get; set; }
        }

        public class dept
        {
            public string name { get; set; }
        }
    }
}