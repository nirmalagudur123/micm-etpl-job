using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace micm_etpl_job.Models
{
    public class Provider
    {
        public string? ProviderName { get; set; }
        public string? County { get; set; }
        public string? InstCode { get; set; }
        public int ProvType { get; set; }
        public string? ProvDesc { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? ZipExtn { get; set; }
        public string? TelePhn { get; set; }
        public string? TelePhnExt { get; set; }
        public string? Fax { get; set; }
        public string? WebURL { get; set; }
        public string? Contact { get; set; }
        public string? ContactTitle { get; set; }
    }

    public class ApiResponse
    {
        public Provider[]? Providers { get; set; }
    }
}
