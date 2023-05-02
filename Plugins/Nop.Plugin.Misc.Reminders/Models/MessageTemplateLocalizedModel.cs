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
    public record MessageTemplateLocalizedModel : ILocalizedModel
    {
            public int LanguageId { get; set; }
            public int MessageTemplateId { get; set; }
            public string BccEmailAddresses { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public IList<EmailAccount> AvailabelEmailAccounts { get; set; }
            public int EmailAccountId { get; set; }
    }
}
