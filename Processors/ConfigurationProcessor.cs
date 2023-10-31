using micm_etpl_job.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace micm_etpl_job.Processors
{
    public class ConfigurationProcessor
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string _apiUrl = "https://webapi.mitalent.org/MITC/api/TrainingDetails?$expand=Configurations";
        public ConfigurationProcessor()
        {
        }

        private async Task<Configurations[]> GetConfiguration()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _apiUrl);
            HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(request);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<ApiConfigurationResponse>(jsonResponse);
                return responseData!.Configuration;
            }
            else
            {
                throw new HttpRequestException("Error: " + httpResponseMessage.StatusCode);
            }
        }
    }
}
