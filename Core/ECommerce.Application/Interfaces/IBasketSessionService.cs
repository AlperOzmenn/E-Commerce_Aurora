using ECommerce.Application.DTOs.BasketsDTOs;

namespace ECommerce.Application.Interfaces
{
    public interface IBasketSessionService
    {
        BasketDTO GetBasket();
        void SaveBasket(BasketDTO basket);
        void ClearBasket();
    }
}
