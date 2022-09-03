namespace MinimalDatatablesEditor.Models
{
    public class JoinSelfModel
    {
        public class users
        {
            public string first_name { get; set; }

            public string last_name { get; set; }

            public int manager { get; set; }
        }

        public class manager
        {
            public string first_name { get; set; }

            public string last_name { get; set; }
        }
    }
}