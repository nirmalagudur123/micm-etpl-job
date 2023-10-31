using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace micm_etpl_job.Models
{
    public class Locations
    {
        public int LocationCode { get; set; }
        public string? ProviderCode { get; set; }
        public string? LocationName { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? County { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? ZipExtn { get; set; }
        public string? TelePhn { get; set; }
        public string? TelePhnExt { get; set; }
        public string? Fax { get; set; }
        public string? ContactName { get; set; }
        public string? ContactTitle { get; set; }
        public string? PrimaryLocation { get; set; }
    }
    public class ApiLocationsResponse
    {
        public Locations[]? Location { get; set; }
    }
}
