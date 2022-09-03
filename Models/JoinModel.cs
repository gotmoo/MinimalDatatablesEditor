namespace MinimalDatatablesEditor.Models
{
    public class JoinModelUsers
    {
        public string first_name { get; set; }

        public string last_name { get; set; }

        public string phone { get; set; }

        public int site { get; set; }

        public int manager { get; set; }
    }

    public class JoinModelSites
    {
        public string name { get; set; }
    }
}