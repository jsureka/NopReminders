using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Misc.Reminders.Models;

namespace Nop.Plugin.Misc.Reminders.Factories
{
    public interface IRemindersModelFactory
    {
        public Task<ReminderListModel> PrepareReminderListModelAsync(RemindersSearchModel searchModel);

    }
}
