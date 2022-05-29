using Cw5.Models;
using Cw5.Repositories;

namespace Cw5.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public WarehouseService(IWarehouseRepository _warehouseRepository)
        {
            this._warehouseRepository = _warehouseRepository;
        }

        public Response RegisterProductInWarehouse(RequestModel requestModel)
        {
            // czesc 1
            if(requestModel.Amount <= 0)
            {
                return new Response
                {
                    Code = false,
                    Message = "Błędna ilość produktu (musi być większa niż 0)"
                };
            }
            Product product = GetProductbyId(requestModel.IdProduct);
            if (product == null)
            {
                return new Response
                {
                    Code = false,
                    Message = "Brak produktu o podanym id"
                };
            }
            if (!WarehouseExistById(requestModel.IdWarehouse))
            {
                return new Response
                {
                    Code = false,
                    Message = "Brak magazynu o podanym id"
                };
            }
            // czesc 2
            int orderId = OrderExistByProductId(requestModel.IdProduct);
            if (orderId == -1)
            {
                return new Response
                {
                    Code = false,
                    Message = "Brak zamówienia dla produktu o podanym id"
                };
            }
            // czesc 3
            if (ProductWarehouseExistByOrderId(orderId))
            {
                return new Response
                {
                    Code = false,
                    Message = "Zamówienie zostało już zrealizowane"
                };
            }
            // czesc 4
            UpdateFullFilledDateOnOrder(orderId);
            // czesc 5
            int newRecordId = CreateProductWearhouses(requestModel, orderId, product.Price);
            // czesc 6
            return new Response
            {
                Code = true,
                Message = "" + newRecordId
            };
        }

        private bool ProductExistById(int id)
        {
            return _warehouseRepository.ProductExistById(id);
        }

        private int OrderExistByProductId(int id)
        {
            return _warehouseRepository.OrderExistByProductId(id);
        }

        private bool WarehouseExistById(int id)
        {
            return _warehouseRepository.WarehouseExistById(id);
        }

        private bool ProductWarehouseExistByOrderId(int id)
        {
            return _warehouseRepository.ProductWarehouseExistByOrderId(id);
        }

        private void UpdateFullFilledDateOnOrder(int id)
        {
            _warehouseRepository.UpdateFullFilledDateOnOrder(id);
        }
        private Product GetProductbyId(int id)
        {
            return _warehouseRepository.GetProductbyId(id);
        }
        private int CreateProductWearhouses(RequestModel request, int orderId, decimal price)
        {
            return _warehouseRepository.CreateProductWearhouses(request, orderId, request.Amount*price);
        }
    }
}
