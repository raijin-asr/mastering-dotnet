namespace PracticeSandBox.Models
{
    public class ProductListViewModel
    {
        //Instead of passing products separately from pagination and sorting information, we put them all into one object.
        public List<Product> Products { get; set; } = new();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalProducts { get; set; }

        public string? SearchString { get; set; }
        public string? SortOrder { get; set; }

        public string NameSortParam { get; set; } = "";
        public string PriceSortParam { get; set; } = "";
    }
}
