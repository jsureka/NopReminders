using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.Reminders.Models
{
    public record RemindersModel : BaseNopEntityModel
    {
        public RemindersModel()
        {
            MessageTemplateModel = new MessageTemplateModel();
        }

        [NopResourceDisplayName("Plugins.Misc.Reminders.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Plugins.Misc.Reminders.Fields.MessageTemplateName")]
        public string MessageTemplateName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.Reminders.Fields.Enabled")]
        public bool Enabled { get; set; }

        [NopResourceDisplayName("Plugins.Misc.Reminders.Fields.ReminderRule")]
        public string ReminderRuleId { get; set; }
        public IList<SelectListItem> ReminderRule { get; set; }

        [NopResourceDisplayName("Plugins.Misc.Reminders.Fields.Store")]
        public string StoreId { get; set; }
        public List<SelectListItem> Store { get; set; }

        [NopResourceDisplayName("Plugins.Misc.Reminders.Fields.NumberOfMessagesPerCustomer")]
        public int NumberOfMessagesPerCustomer { get; set; }

        [NopResourceDisplayName("Plugins.Misc.Reminders.Fields.ConditionMetLaterThan")]
        public int ConditionMetLaterThan { get; set; }

        [NopResourceDisplayName("Plugins.Misc.Reminders.Fields.ConditionMetEarlierThan")]
        public int ConditionMetEarlierThan { get; set; }

        [NopResourceDisplayName("Plugins.Misc.Reminders.Fields.IntervalBetweenMessages")]
        public int? IntervalBetweenMessages { get; set; }

        public MessageTemplateModel MessageTemplateModel { get; set; }
   //     public IList<int>? ReminderExcludedCustomerIds { get; set; }

    }
}
