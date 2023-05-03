using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Misc.Reminders.Domain;

namespace Nop.Plugin.Misc.Reminders.Services
{
    public interface IReminderMessageTemplateService 
    {
        /// <summary>
        /// Gets all reminders
        /// </summary>
        /// <param name="storeId">The store identifier; pass 0 to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the reminders
        /// </returns>
        Task<IPagedList<ReminderMessageTemplate>> GetAllReminderMessageTemplates (int storeId = 0, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets a reminder
        /// </summary>
        /// <param name="reminderId">Reminder identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the reminder
        /// </returns>
        Task<ReminderMessageTemplate> GetReminderMessageTemplateByIdAsync(int reminderId);

        /// <summary>
        /// Inserts a reminder
        /// </summary>
        /// <param name="reminder">Reminder</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertReminderMessageTemplateAsync(ReminderMessageTemplate template);

        /// <summary>
        /// Updates a reminder
        /// </summary>
        /// <param name="reminder">Reminder</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateReminderMessageTemplateByIdAsync(ReminderMessageTemplate template);

        /// <summary>
        /// Deletes a reminder
        /// </summary>
        /// <param name="reminder">Reminder</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteReminderMessageTemplateByIdAsync(ReminderMessageTemplate template);
    }
}
