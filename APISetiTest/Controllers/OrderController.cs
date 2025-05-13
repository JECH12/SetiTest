using APISetiTest.Models;
using APISetiTest.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APISetiTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService services) 
        { 
            _orderService = services;
        }

        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] JsonRequest request)
        {
            string xml = _orderService.ConvertToXml(request);
            string soapResponse = await _orderService.SendSoapRequest(xml);
            JsonResponse jsonResponse = _orderService.TransformXmlToJson(soapResponse);
            return Ok(jsonResponse);
        }
    }
}
