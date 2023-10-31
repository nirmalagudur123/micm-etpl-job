using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace micm_etpl_job.Interfaces
{
    public interface IMessageQueueHandler
    {
        Task HandleMessagesAsync(int? count);
    }
}
