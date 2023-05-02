using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;

namespace Nop.Plugin.Misc.Reminders.Domain
{
    public class Reminder : BaseEntity
    {
        public Reminder() { 
            ReminderMessageTemplate = new ReminderMessageTemplate();
        }
        /// <summary>
        /// Gets or sets enabled status
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets reminder rule id
        /// </summary>
        public int ReminderRuleId{ get; set; }

        /// <summary>
        /// Gets or sets number of messages per customer
        /// </summary>
        public int NumberOfMessagesPerCustomer { get; set; }

        /// <summary>
        /// Gets or sets the time for conditions met later than
        /// </summary>
        public int ConditionMetLaterThan { get; set; }
        
        /// <summary>
        /// Gets or sets the time for conditions met later than
        /// </summary>
        public int ConditionMetEarlierThan { get; set; }

        ///// <summary>
        ///// Gets or sets a display order
        ///// </summary>
        //public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets a store identifier
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// Gets or sets interval between messages
        /// </summary>
        public int? IntervalBetweenMessages { get; set; }

        public int? ReminderMessageTemplateId { get; set; }

        public ReminderMessageTemplate ReminderMessageTemplate { get; set; }

    }
}
