using ECommerce.Application.DTOs.BasketsDTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;

namespace ECommerce.Persistence.Concrates
{
    public class BasketSessionService : IBasketSessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string BasketKey => $"basket_{_httpContextAccessor.HttpContext.User.Identity.Name}";


        public BasketSessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public BasketDTO GetBasket()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var basketJson = session.GetString(BasketKey);

            return basketJson == null
                ? new BasketDTO()
                : JsonSerializer.Deserialize<BasketDTO>(basketJson);
        }

        public void SaveBasket(BasketDTO basket)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var basketJson = JsonSerializer.Serialize(basket);
            session.SetString(BasketKey, basketJson);
        }

        public void ClearBasket()
        {
            _httpContextAccessor.HttpContext.Session.Remove(BasketKey);
        }
    }

}
