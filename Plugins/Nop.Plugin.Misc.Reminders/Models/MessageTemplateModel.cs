using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Messages;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.Reminders.Models
{
    public record MessageTemplateModel : BaseNopEntityModel
    {
        public string MessageTemplateName { get; set; }
        public string SystemName { get; set; }
        public string BccEmailAddresses { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EmailAccountId { get; set; }
        public IList<EmailAccount> AvailableEmailAccounts { get; set; }
        public string AllowedTokens { get; set; }
    }
}
