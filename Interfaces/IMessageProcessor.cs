using micm_etpl_job.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace micm_etpl_job.Interfaces
{
    public interface IMessageProcessor
    {
        ProcessingStatus Process(string message);
    }
}
