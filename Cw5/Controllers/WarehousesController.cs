using Cw5.Models;
using Cw5.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehousesController(IWarehouseService _warehouseService)
        {
            this._warehouseService = _warehouseService;
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateProductInWarehouse([FromBody] RequestModel requestModel)
        {
            Response response = _warehouseService.RegisterProductInWarehouse(requestModel);
            if (!response.Code)
                return NotFound(response.Message);
            return Ok(response.Message);
        }
    }
}
