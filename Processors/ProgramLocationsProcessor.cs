using micm_etpl_job.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace micm_etpl_job.Processors
{
    public class ProgramLocationsProcessor
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string _apiUrl = "https://webapi.mitalent.org/MITC/api/TrainingDetails?$expand=ProgramLocations";
        public ProgramLocationsProcessor()
        {
        }

        private async Task<ProgramLocations[]> GetProgramLocations()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _apiUrl);
            HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(request);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<ApiProgramLocationsResponse>(jsonResponse);
                return responseData!.ProgramLocation;
            }
            else
            {
                throw new HttpRequestException("Error: " + httpResponseMessage.StatusCode);
            }
        }
    }
}
