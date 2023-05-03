using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Services.ScheduleTasks;

namespace Nop.Plugin.Misc.Reminders.Services
{
    public class ReminderTask : IScheduleTask
    {
        public ReminderTask() { }

        public Task ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
