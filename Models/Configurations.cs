using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace micm_etpl_job.Models
{
    public class Configurations
    {
        public string? ConfigType { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
    }
    public class ApiConfigurationResponse
    {
        public Configurations[]? Configuration { get; set; }
    }
}
