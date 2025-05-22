namespace MVCApplication.Models
{
    public class FilterModel
    {
        public List<string> Models { get; set; }
        public string Model { get; set; }
        public List<string> Makes { get; set; }
        public string Make { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public List<string> Transmitions { get; set; }
        public string Transmition { get; set; }
        public List<string> Fuels { get; set; }
        public string Fuel { get; set; }
        public List<string> Colors { get; set; }
        public string Color { get; set; }
        public int MinMilliage { get; set; }
        public int MaxMilliage { get; set; }
        public int MinPower { get; set; }
        public int MaxPower { get; set; }
        public bool isNull { get; set; }
        public List<string> SortTypes { get; set; }
        public string SortType { get; set; }
        public string ModelsJson { get; set; }
        public string MakesJson { get; set; }
        public string TransmitionsJson { get; set; }
        public string ColorsJson { get; set; }
        public string FuelsJson { get; set; }
        public int CurrentPageIndex { get; set; }
        public int CurrentPage { get { return CurrentPageIndex + 1; } set { CurrentPage = value; } }
        public List<int> ViewPages { get; set; }
        public int ListingsPerPage { get; set; }

        public FilterModel()
        {
            Models = new List<string>();
            Makes = new List<string>();
            Transmitions = new List<string>();
            Fuels = new List<string>();
            Colors = new List<string>();

            MinPrice = 0;
            MaxPrice = 1000000;
            
            MinMilliage = 0;
            MaxMilliage = 500000;

            MinPower = 0;
            MaxPower = 600;

            isNull = true;

            SortTypes = new List<string>();
            SortTypes.Add("Newest - First");
            SortTypes.Add("Oldest - First");
            SortTypes.Add("Price - Ascending");
            SortTypes.Add("Price - Descening");

            SortType = SortTypes[0];

            CurrentPageIndex = 0;

            ViewPages = new List<int>();

            ListingsPerPage = 20;
        }
    }
}
