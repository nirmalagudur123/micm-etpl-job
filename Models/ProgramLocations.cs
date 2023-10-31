using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace micm_etpl_job.Models
{
    public class ProgramLocations
    {
        public int ProgLocID { get; set; }
        public int ProgramCode { get; set; }
        public int LocationCode { get; set; }
    }
    public class ApiProgramLocationsResponse
    {
        public ProgramLocations[] ProgramLocation { get; set; }
    }
}
