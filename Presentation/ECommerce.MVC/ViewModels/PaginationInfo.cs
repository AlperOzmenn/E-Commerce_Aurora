namespace ECommerce.MVC.ViewModels
{
    public class PaginationInfo
    {
        public int Count { get; set; }
        public int PageSize { get; set; } = 30;
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)Count / PageSize);
    }
}