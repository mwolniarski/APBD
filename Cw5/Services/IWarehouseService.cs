using Cw5.Models;

namespace Cw5.Services
{
    public interface IWarehouseService
    {

        Response RegisterProductInWarehouse(RequestModel requestModel);
    }
}
