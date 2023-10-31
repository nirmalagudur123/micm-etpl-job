using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace micm_etpl_job.Models
{
    public class Programs
    {
        public int ProgramCode { get; set; }
        public string? InstCode { get; set; }
        public string? CIPCode { get; set; }
        public string? ProgCert { get; set; }
        public string? ProgName { get; set; }
        public string? ProgDesc { get; set; }
        public int ProgLengthWks { get; set; }
        public int ProgLengthHrs { get; set; }
        public string? ProgCost { get; set; }
        public string? ContName { get; set; }
        public string? ContPhn { get; set; }
        public string? ProgURL { get; set; }
        public string? OthrCreds { get; set; }
        public int Prerequisites { get; set; }
        public int ProgFormat { get; set; }
        public int AssoSOC1 { get; set; }
        public int AssoSOC2 { get; set; }
        public int AssoSOC3 { get; set; }
        public int Ita { get; set; }
        public int PellGrnt { get; set; }
        public int OthrGrnt { get; set; }
        public string? OthrGrntDet { get; set; }
        public string? WaitTime { get; set; }
        public string? WaitPeriod { get; set; }
        public string? Tuition { get; set; }
        public int LicenseFee { get; set; }
        public string? MandFee { get; set; }
        public string? AddCostAmt { get; set; }
        public int WIOAITAeligty { get; set; }
        public string? FirstAddedDate { get; set; }
        public string? StartDate { get; set; }
        public string? ExpDate { get; set; }
    }

    public class ApiProgramResponse
    {
        public Programs[]? ProgramsArray { get; set; }
    }
}
