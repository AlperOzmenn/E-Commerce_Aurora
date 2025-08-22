using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalOrders { get; set; }
        public int NewProducts { get; set; }
        public int SellerApplications { get; set; }
        public int TodayVisitors { get; set; }
    }
}
