using micm_etpl_job.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using micm_etpl_job.Models;

namespace micm_etpl_job.Processors
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly IMessageProcessingLogger _logger;
        private const int MI_CHANNELID = 11211;
        // TODO: move this to environment variable??
        private const string seekerMatchingApiUrl = "https://internal-services.lexus.monster.com/seeker/api/v1/seeker-matching";
        private readonly HttpClient client = new HttpClient();

        #region Offsets and lengths
        private const int REFID_LENGTH = 9;
        private const int MWA_LENGTH = 2;
        private const int YEAR_LENGTH = 4;
        private const int DAY_MONTH_LENGTH = 2;
        private const int FIRSTNAME_LENGTH = 25;
        private const int LASTNAME_LENGTH = 30;
        private const int CLAIMANTSCORE_LENGTH = 15;
        private const int EMAILADDRESS_LENGTH = 60;
        private const int REFID_OFFSET = 0;
        private const int MWA_OFFSET = 9;
        private const int DOB_OFFSET = 11;
        private const int FIRSTNAME_OFFSET = 19;
        private const int LASTNAME_OFFSET = 44;
        private const int LETTERSENTDATE_OFFSET = 74;
        private const int BENEFITYEARENDDATE_OFFSET = 82;
        private const int CLAIMANTSCORE_OFFSET = 90;
        private const int EMAILADDRESS_OFFSET = 105;
        #endregion

        public MessageProcessor(IMessageProcessingLogger logger)
        {
            _logger = logger;
        }
        public ProcessingStatus Process(string message)
        {
            return ProcessingStatus.Success;
        }

        //public ProcessingStatus Process(string message)
        //{
        //    Guid trackingGuid = Guid.NewGuid();
        //    try
        //    {
        //        // Deserialize to Resea1on1Event object
        //        var resea1on1Event = new Resea1on1Event
        //        {
        //            ReferenceId = message.Substring(REFID_OFFSET, REFID_LENGTH),
        //            Mwa = message.Substring(MWA_OFFSET, MWA_LENGTH),
        //            DateOfBirth = new DateTime(Int32.Parse(message.Substring(DOB_OFFSET, YEAR_LENGTH)), Int32.Parse(message.Substring(DOB_OFFSET + YEAR_LENGTH, DAY_MONTH_LENGTH)), Int32.Parse(message.Substring(DOB_OFFSET + YEAR_LENGTH + DAY_MONTH_LENGTH, DAY_MONTH_LENGTH))),
        //            FirstName = message.Substring(FIRSTNAME_OFFSET, FIRSTNAME_LENGTH).Trim(),
        //            LastName = message.Substring(LASTNAME_OFFSET, LASTNAME_LENGTH).Trim(),
        //            LetterSentDate = new DateTime(Int32.Parse(message.Substring(LETTERSENTDATE_OFFSET, YEAR_LENGTH)), int.Parse(message.Substring(LETTERSENTDATE_OFFSET + YEAR_LENGTH, DAY_MONTH_LENGTH)), int.Parse(message.Substring(LETTERSENTDATE_OFFSET + YEAR_LENGTH + DAY_MONTH_LENGTH, DAY_MONTH_LENGTH))),
        //            BenefitYearEndDate = new DateTime(Int32.Parse(message.Substring(BENEFITYEARENDDATE_OFFSET, YEAR_LENGTH)), int.Parse(message.Substring(BENEFITYEARENDDATE_OFFSET + YEAR_LENGTH, DAY_MONTH_LENGTH)), int.Parse(message.Substring(BENEFITYEARENDDATE_OFFSET + YEAR_LENGTH + DAY_MONTH_LENGTH, DAY_MONTH_LENGTH))),
        //            ClaimantScore = double.Parse(message.Substring(CLAIMANTSCORE_OFFSET, CLAIMANTSCORE_LENGTH)),
        //            EmailAddress = message.Substring(EMAILADDRESS_OFFSET, EMAILADDRESS_LENGTH).Trim()
        //        };
        //        _logger.LogProcessStep($"{trackingGuid} - MWA: {resea1on1Event.Mwa}; EmailAddress: {resea1on1Event.EmailAddress}; DoB (MM/DD): {resea1on1Event.DateOfBirth.Month}/{resea1on1Event.DateOfBirth.Day}; Score: {resea1on1Event.ClaimantScore}");
        //        var userIds = CallSeekerMatching(resea1on1Event.ReferenceId, resea1on1Event.DateOfBirth, trackingGuid);
        //        if (userIds.Count() > 1)
        //        {
        //            _logger.LogProcessStep($"{trackingGuid} - Multiple matches found {userIds.Count}: {userIds.Select(x => x.ToString())}");
        //        }
        //        else
        //        {
        //            if (userIds.Count() == 1)
        //            {
        //                _logger.LogProcessStep($"{trackingGuid} - Matched seeker: {userIds[0]}");
        //                // TODO: Call API to Update first/last name and date of birth
        //            }
        //            else
        //            {
        //                _logger.LogProcessStep($"{trackingGuid} - No matching seeker found.");
        //                // TODO: Call API to Create Seeker
        //            }
        //            // TODO: Call API to save claimant score and BYE date
        //            // TODO: Call API to create RESEA 1-1 event
        //        }
        //        return ProcessingStatus.Success;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogProcessStep($"{trackingGuid} - Unable to process message. {ex.Message}");
        //        // TODO: How to log exception?? What details to avoid PII??
        //        return ProcessingStatus.Unprocessable;
        //    }
        //}

        private IList<int> CallSeekerMatching(string refId, DateTime dateOfBirth, Guid trackingGuid)
        {
            // Call seeker matching with ssn and dob
            var token = GetBearerToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("ContentType", "application/json");
            client.DefaultRequestHeaders.Add("channel_id", MI_CHANNELID.ToString());
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(seekerMatchingApiUrl));
            request.Content = JsonContent.Create(new
            {
                ReferenceId = refId,
                DateOfBirth = dateOfBirth,
                EmailAddress = "" // email address field must be passed as empty
            });
            HttpResponseMessage response = client.SendAsync(request).Result;
            _logger.LogProcessStep($"{trackingGuid} - SeekerMatching response: {response.StatusCode}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // return userIds;
                dynamic stuff = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                var userIds = new List<int>(stuff.userIds.Count);
                foreach (int userId in stuff.userIds)
                {
                    userIds.Add(userId);
                }
                return userIds;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.Conflict)
            {
                // 200 ok
                // 401 no match found
                // 409 conflict
            }
            return new List<int>();
        }

        /// <summary>
        /// Move to separate application!!
        /// </summary>
        /// <param name="reseaData"></param>
        //private void CallSeekerCreate(Resea1on1Event reseaData)
        //{
        //    using (HttpClient httpClient = new HttpClient())
        //    {
        //        string authTokenUrl = $"https://internal-services.lexus.monster.com/seeker/api/v1/seekers/0/profile-info";
        //        HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, new Uri(authTokenUrl));
        //        httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
        //        httpClient.DefaultRequestHeaders.Add("channel_id", "11211");
        //        msg.Content = JsonContent.Create(new
        //        {
        //            emailAddress = reseaData.EmailAddress,
        //            firstName = reseaData.FirstName,
        //            lastName = reseaData.LastName,
        //            socialSecurityNumber = reseaData.ReferenceId,
        //            dateOfBirth = reseaData.DateOfBirth
        //        });

        //        HttpResponseMessage response = httpClient.SendAsync(msg).Result;
        //        var results = response.Content.ReadAsStringAsync().Result;
        //        dynamic stuff = JObject.Parse(results);
        //    }
        //}

        /// <summary>
        /// Move to separate application!!
        /// </summary>
        /// <returns></returns>
        private string GetBearerToken()
        {
            bool usingDev = false; // set based on dev vs qa token
            using (HttpClient httpClient = new HttpClient())
            {
                string authTokenUrl = $"https://ocs-caseworker-mm-{(usingDev ? "dev" : "qa")}.us.auth0.com/oauth/token";
                HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, new Uri(authTokenUrl));
                httpClient.DefaultRequestHeaders.Add("ContentType", "application/x-www-form-urlencoded");
                if (usingDev)
                {
                    msg.Content = JsonContent.Create(new
                    {
                        grant_type = "client_credentials",
                        client_id = "YJIal7nu2XYF1wtTHOOfTjNQZs6M1gXC",
                        client_secret = "vUBHMByCX3-Uxbf19XfEaYveT_krr3iAlSoBfzUqsWRlqSs3t5Ex079pAG4ZsXGD",
                        audience = "https://ocs-monster.com/System/"
                    });
                }
                else
                {
                    msg.Content = JsonContent.Create(new
                    {
                        grant_type = "client_credentials",
                        client_id = "5CU1XuH824xhUSHWlKQ6rwwVqGPvCvvj",
                        client_secret = "F7rvHmy-90lJlQmEu8Knha9H0VGM_xy0Iu4Mzd3RLsoS1GghOCjW7uNsROvLQxVv",
                        audience = "https://ocs-monster.com/System/"
                    });
                }

                HttpResponseMessage response = httpClient.SendAsync(msg).Result;
                var results = response.Content.ReadAsStringAsync().Result;
                dynamic stuff = JObject.Parse(results);
                return stuff.access_token.Value;
            }
        }
    }
}
