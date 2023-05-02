using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Misc.Reminders.Domain;

namespace Nop.Plugin.Misc.Reminders.Data
{
    public static class SeedDataReminderRule
    {
        public static List<ReminderRule> Entities => new List<ReminderRule>
    {
        new ReminderRule { Id = 1,  Name = "Abandoned Shopping Cart" },
        new ReminderRule {Id = 2, Name = "Unpaid Orders" },
        new ReminderRule {Id = 3, Name = "Inactive Customers" },
        new ReminderRule {Id = 4, Name = "Birthday" },
        new ReminderRule {Id = 5, Name = "Completed Order" }
    };
    }
}
