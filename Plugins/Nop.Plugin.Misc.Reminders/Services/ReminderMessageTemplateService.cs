using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Data;
using Nop.Plugin.Misc.Reminders.Domain;
using StackExchange.Profiling.Internal;

namespace Nop.Plugin.Misc.Reminders.Services
{
    internal class ReminderMessageTemplateService : IReminderMessageTemplateService
    {
        protected readonly CacheKey _remindersAllKey = new("Nop.remindersMessageTemplates.all-{0}", TEMPLATE_PATTERN_KEY);
        protected const string TEMPLATE_PATTERN_KEY = "Nop.remindersMessageTemplates.";


        private readonly IRepository<ReminderMessageTemplate> _reminderMessageTemplateRepository;
        private readonly IStaticCacheManager _staticCacheManager;

        public ReminderMessageTemplateService(IRepository<ReminderMessageTemplate> reminderMessageTemplateRepository,
           IStaticCacheManager staticCacheManager)
        {
            _reminderMessageTemplateRepository = reminderMessageTemplateRepository;
            _staticCacheManager = staticCacheManager;
        }

        public Task<IPagedList<ReminderMessageTemplate>> GetAllReminderMessageTemplates(int reminderId = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            throw new NotImplementedException();

        }

        public async Task<ReminderMessageTemplate> GetReminderMessageTemplateByIdAsync(int templateId)
        {
            return await _reminderMessageTemplateRepository.GetByIdAsync(templateId);

        }

        public async Task InsertReminderMessageTemplateAsync(ReminderMessageTemplate template)
        {
            await _reminderMessageTemplateRepository.InsertAsync(template, false);
            await _staticCacheManager.RemoveByPrefixAsync(TEMPLATE_PATTERN_KEY);
        }

        public async Task UpdateReminderMessageTemplateByIdAsync(ReminderMessageTemplate template)
        {
            await _reminderMessageTemplateRepository.UpdateAsync(template, false);
            await _staticCacheManager.RemoveByPrefixAsync(TEMPLATE_PATTERN_KEY);
        }

        public async Task DeleteReminderMessageTemplateByIdAsync(ReminderMessageTemplate template)
        {
            await _reminderMessageTemplateRepository.DeleteAsync(template, false);
            await _staticCacheManager.RemoveByPrefixAsync(TEMPLATE_PATTERN_KEY);
        }

    }
}
