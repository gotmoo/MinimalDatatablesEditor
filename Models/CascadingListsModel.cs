namespace MinimalDatatablesEditor.Models
{
    public class CascadingListsModel
    {
        public class team
        {
            public string name { get; set; }

            public int continent { get; set; }

            public int country { get; set; }
        }

        public class continent
        {
            public string name { get; set; }
        }

        public class country
        {
            public string name { get; set; }
        }
    }
}
