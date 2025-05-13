using APISetiTest.Models;
using APISetiTest.Services.Interfaces;
using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace APISetiTest.Services
{
    public class OrderService : IOrderService
    {
        public string ConvertToXml(JsonRequest request)
        {
            XNamespace soapenv = XNamespace.Get("http://schemas.xmlsoap.org/soap/envelope/");
            XNamespace env = XNamespace.Get("http://WSDLs/EnvioPedidos/EnvioPedidosAcme");

            XDocument doc = new XDocument(
                new XElement(soapenv + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "env", env),
                    new XAttribute(XNamespace.Xmlns + "soapenv", soapenv),
                    new XElement(soapenv + "Header"),
                    new XElement(soapenv + "Body",
                        new XElement(env + "EnvioPedidoAcme",
                            new XElement("EnvioPedidoRequest",
                                new XElement("pedido", request.SendOrder.OrderNumber),
                                new XElement("Cantidad", request.SendOrder.OrderQuantity),
                                new XElement("EAN", request.SendOrder.EANCode),
                                new XElement("Producto", request.SendOrder.ProductName),
                                new XElement("Cedula", request.SendOrder.IdNumber),
                                new XElement("Direccion", request.SendOrder.Address)
                            )
                        )
                    )
                )
            );
            return doc.ToString();
        }

        public async Task<string> SendSoapRequest(string xml)
        {

            string url = "https://run.mocky.io/v3/19217075-6d4e-4818-98bc-416d1feb7b84";

            using HttpClient client = new HttpClient();

            StringContent content = new StringContent(xml, Encoding.UTF8, "text/xml");

            HttpResponseMessage response = await client.PostAsync(url, content);

            return await response.Content.ReadAsStringAsync();
        }

        public JsonResponse TransformXmlToJson(string xml)
        {
            var doc = XDocument.Parse(xml);
            string? code = doc.Descendants("Codigo").FirstOrDefault()?.Value;
            string? message = doc.Descendants("Mensaje").FirstOrDefault()?.Value;

            return new JsonResponse
            {
                SendOrderResponse = new SendOrderResponse
                {
                    codigoEnvio = code,
                    estado = message
                }
            };
        }
    }
}
