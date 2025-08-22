namespace ECommerce.MVC.ViewModels
{
    public class ListViewModel<T>
    {
        public List<T> Items { get; set; }
        public string SearchQuery { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }

}
