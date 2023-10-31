using micm_etpl_job.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace micm_etpl_job.Processors
{
    public class ProgramProcessor
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string _apiUrl = "https://webapi.mitalent.org/MITC/api/TrainingDetails?$expand=Programs";

        private async Task<Programs[]> GetPrograms()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _apiUrl);
            HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(request);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<ApiProgramResponse>(jsonResponse);
                return responseData!.ProgramsArray;
            }
            else
            {
                throw new HttpRequestException("Error: " + httpResponseMessage.StatusCode);
            }
        }
    }
}
