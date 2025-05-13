using APISetiTest.Models;

namespace APISetiTest.Services.Interfaces
{
    public interface IOrderService
    {
        string ConvertToXml(JsonRequest request);
        Task<string> SendSoapRequest(string xml);
        JsonResponse TransformXmlToJson(string xml);
    }
}
