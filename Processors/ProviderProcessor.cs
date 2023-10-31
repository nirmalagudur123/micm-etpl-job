using micm_etpl_job.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace micm_etpl_job.Processors
{
    public class ProviderProcessor
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string _apiUrl = "https://webapi.mitalent.org/MITC/api/TrainingDetails?$expand=Providers";
        public ProviderProcessor() {
        }

        private async Task<Provider[]> GetProviders()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _apiUrl);
            HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(request);
           
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<ApiResponse>(jsonResponse);
                return responseData!.Providers;
            }
            else
            {
                throw new HttpRequestException("Error: " + httpResponseMessage.StatusCode);
            }
        }

    }
}
