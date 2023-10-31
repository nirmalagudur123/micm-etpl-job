using micm_etpl_job.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace micm_etpl_job.Service
{
    public class ApplicationService
    {
        public readonly IMessageQueueHandler _handler;

        public ApplicationService(IMessageQueueHandler handler)
        {
            _handler = handler;
        }

        public async Task Run(string[] args)
        {
            await _handler.HandleMessagesAsync(10);
        }
    }
}
