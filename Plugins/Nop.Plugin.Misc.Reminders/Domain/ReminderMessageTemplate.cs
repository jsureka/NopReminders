using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Messages;

namespace Nop.Plugin.Misc.Reminders.Domain
{
    public class ReminderMessageTemplate : BaseEntity
    {
        public string MessageTemplateName { get; set; }
        public string SystemName { get; set; }
        public string BccEmailAddresses { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EmailAccountId { get; set; }
    }
}
