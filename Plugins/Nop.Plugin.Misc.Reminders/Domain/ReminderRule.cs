using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;

namespace Nop.Plugin.Misc.Reminders.Domain
{
    public class ReminderRule : BaseEntity
    {
        public string Name { get; set; }
    }
}
