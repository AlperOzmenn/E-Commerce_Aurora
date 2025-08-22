using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.BasketsDTOs
{
    public class BasketDTO
    {
        public List<BasketItemDTO> Items { get; set; } = new List<BasketItemDTO>();

        public decimal TotalPrice => Items.Sum(x => x.UnitPrice * x.Quantity);
    }
}
