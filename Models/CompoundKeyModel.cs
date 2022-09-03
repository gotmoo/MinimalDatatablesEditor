namespace MinimalDatatablesEditor.Models
{
    public class CompoundKeyModel
    {
        public class users_visits
        {
            public int user_id { get; set; }
            public int site_id { get; set; }
            public string visit_date { get; set; }
        }

        public class users
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
        }

        public class sites
        {
            public string name { get; set; }
        }
    }
}