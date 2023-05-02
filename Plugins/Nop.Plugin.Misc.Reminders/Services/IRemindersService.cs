using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Misc.Reminders.Domain;

namespace Nop.Plugin.Misc.Reminders.Service
{
    /// <summary>
    /// Store Reminders service interface
    /// </summary>
    public interface IRemindersService
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
        Task<IPagedList<Reminder>> GetAllRemindersAsync(int storeId = 0, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets a reminder
        /// </summary>
        /// <param name="reminderId">Reminder identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the reminder
        /// </returns>
        Task<Reminder> GetReminderByIdAsync(int reminderId);

        /// <summary>
        /// Inserts a reminder
        /// </summary>
        /// <param name="reminder">Reminder</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertReminderAsync(Reminder reminder);

        /// <summary>
        /// Updates a reminder
        /// </summary>
        /// <param name="reminder">Reminder</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateReminderByIdAsync(Reminder reminder);

        /// <summary>
        /// Deletes a reminder
        /// </summary>
        /// <param name="reminder">Reminder</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteReminderByIdAsync(Reminder reminder);
    }
}
