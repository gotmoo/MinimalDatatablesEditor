namespace MinimalDatatablesEditor.Models
{
    public class UploadManyModel
    {
        public class users
        {
            public string first_name { get; set; }

            public string last_name { get; set; }

            public string phone { get; set; }

            public int site { get; set; }
        }

        public class sites
        {
            public string name { get; set; }
        }
    }
}