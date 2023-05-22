using Microsoft.AspNetCore.Mvc;

namespace StockExchangeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockExchangeController : ControllerBase
    {
        public StockExchangeController() { }

        [HttpGet]
        [Route("companies/{databaseId}")]
        public async Task<IActionResult> GetCompanies([FromRoute] string databaseId, [FromServices] IConfiguration configuration)
        {
            string url = $"https://data.nasdaq.com/api/v3/datasets/?database_code={databaseId}&api_key={configuration.GetValue<string>("ApiKey")}";
            var result = await SendRequest(url);

            return Ok(result);
        }

        [HttpGet]
        [Route("company-data/{databaseId}/{companyId}")]
        public async Task<IActionResult> GetCompanyData([FromRoute] string databaseId, [FromRoute] string companyId, [FromServices] IConfiguration configuration)
        {
            string url = $"https://data.nasdaq.com/api/v3/datasets/{databaseId}/{companyId}/data.json?column_index=1&column_index=4&api_key={configuration.GetValue<string>("ApiKey")}";
            var result = await SendRequest(url);

            return Ok(result);
        }

        private async Task<string> SendRequest(string url)
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Add("Accept", "application/json");

            using var httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            Response.Headers.Add("Content-Type", "application/json");

            return content;
        }
    }
}