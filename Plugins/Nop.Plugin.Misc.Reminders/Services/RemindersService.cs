using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Shipping;
using Nop.Data;
using Nop.Plugin.Misc.Reminders.Domain;
using Nop.Plugin.Misc.Reminders.Service;

namespace Nop.Plugin.Misc.Reminders.Services
{
    internal class RemindersService : IRemindersService
    {
        #region Constants

        /// <summary>
        /// Cache key for reminders
        /// </summary>
        /// <remarks>
        /// {0} : current store ID
        /// </remarks>
        protected readonly CacheKey _remindersAllKey = new("Nop.reminders.all-{0}", REMINDERS_PATTERN_KEY);
        protected const string REMINDERS_PATTERN_KEY = "Nop.reminders.";

        #endregion

        #region Fields
        private readonly IRepository<Reminder> _remindersRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="_remindersRepository">Store pickup point repository</param>
        /// <param name="staticCacheManager">Cache manager</param>
        public RemindersService(IRepository<Reminder> remindersRepository,
            IStaticCacheManager staticCacheManager)
        {
            _remindersRepository = remindersRepository;
            _staticCacheManager = staticCacheManager;
        }

        #endregion

        #region methods
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
        public virtual async Task<IPagedList<Reminder>> GetAllRemindersAsync(int storeId = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var rez = await _remindersRepository.GetAllAsync(query =>
            {
                if (storeId > 0)
                    query = query.Where(point => point.StoreId == storeId || point.StoreId == 0);
                query = query.OrderBy(point => point.Name);

                return query;
            }, cache => cache.PrepareKeyForShortTermCache(_remindersAllKey, storeId));

            return new PagedList<Reminder>(rez, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets a reminder
        /// </summary>
        /// <param name="reminderId">Reminder identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the reminder
        /// </returns>
        public virtual async Task<Reminder> GetReminderByIdAsync(int reminderId)
        {
            return await _remindersRepository.GetByIdAsync(reminderId);
        }

        /// <summary>
        /// Inserts a reminder
        /// </summary>
        /// <param name="reminder">Reminder</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertReminderAsync(Reminder reminder)
        {
            await _remindersRepository.InsertAsync(reminder, false);
            await _staticCacheManager.RemoveByPrefixAsync(REMINDERS_PATTERN_KEY);
        }

        /// <summary>
        /// Updates a reminder
        /// </summary>
        /// <param name="reminder">Reminder</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateReminderByIdAsync(Reminder reminder)
        {
            await _remindersRepository.UpdateAsync(reminder, false);
            await _staticCacheManager.RemoveByPrefixAsync(REMINDERS_PATTERN_KEY);
        }

        /// <summary>
        /// Deletes a reminder
        /// </summary>
        /// <param name="reminder">Reminder</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteReminderByIdAsync(Reminder reminder)
        {
            await _remindersRepository.DeleteAsync(reminder, false);
            await _staticCacheManager.RemoveByPrefixAsync(REMINDERS_PATTERN_KEY);
        }
        #endregion
    }
}
