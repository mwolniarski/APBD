using Cw5.Models;

namespace Cw5.Repositories
{
    public interface IWarehouseRepository
    {
        bool ProductExistById(int id);
        bool WarehouseExistById(int id);
        int OrderExistByProductId(int id);
        bool ProductWarehouseExistByOrderId(int id);
        void UpdateFullFilledDateOnOrder(int id);
        int CreateProductWearhouses(RequestModel request, int orderId, decimal price);
        Product GetProductbyId(int id);
    }
}
