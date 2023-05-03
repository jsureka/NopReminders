using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Misc.Reminders.Models;
using Nop.Plugin.Misc.Reminders.Service;
using Nop.Plugin.Misc.Reminders.Services;
using Nop.Services.Localization;
using Nop.Services.Stores;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.Reminders.Factories
{
    public class RemindersModelFactory : IRemindersModelFactory
    {
        #region Fields

        protected readonly IRemindersService _remindersService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IStoreService _storeService;
        private readonly IReminderMessageTemplateService _reminderMessageTemplateService;

        #endregion

        #region Ctor

        public RemindersModelFactory(IRemindersService remindersService,
            ILocalizationService localizationService, IStoreService storeService,
            IReminderMessageTemplateService reminderMessageTemplateService)
        {
            _remindersService = remindersService;
            _localizationService = localizationService;
            _storeService = storeService;
            _reminderMessageTemplateService = reminderMessageTemplateService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare reminder list model
        /// </summary>
        /// <param name="searchModel">reminder search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the reminder list model
        /// </returns>
        public async Task<ReminderListModel> PrepareReminderListModelAsync(RemindersSearchModel searchModel)
        {
            var reminders = await _remindersService.GetAllRemindersAsync(pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);
           
            var model = await new ReminderListModel().PrepareToGridAsync(searchModel, reminders, () =>
            {
                return reminders.SelectAwait(async reminder =>
                {
                    var reminderMessageTemplate = await _reminderMessageTemplateService.GetReminderMessageTemplateByIdAsync(reminder.ReminderMessageTemplateId);
                    var messageTemplate = new MessageTemplateModel
                    {
                        MessageTemplateName = reminderMessageTemplate.MessageTemplateName,
                        BccEmailAddresses = reminderMessageTemplate.BccEmailAddresses,
                        Body = reminderMessageTemplate.Body,
                        EmailAccountId = reminderMessageTemplate.EmailAccountId,
                        Id = reminderMessageTemplate.Id,
                        Subject = reminderMessageTemplate.Subject,
                        
                    };
                    return new RemindersModel( messageTemplate)
                    {
                        Id = reminder.Id,
                        Name = reminder.Name,
                        NumberOfMessagesPerCustomer = reminder.NumberOfMessagesPerCustomer,
                        ConditionMetEarlierThan = reminder.ConditionMetEarlierThan,
                        ConditionMetLaterThan = reminder.ConditionMetLaterThan,
                        Enabled = reminder.Enabled,
                        IntervalBetweenMessages = reminder.IntervalBetweenMessages,
                        StoreId = reminder.StoreId.ToString(),
                        ReminderRuleId = reminder.ReminderRuleId.ToString(),
                        MessageTemplateName = reminderMessageTemplate.MessageTemplateName,
                    };
                });
            });

            return model;
        }

        ///// <summary>
        ///// Prepare store pickup point search model
        ///// </summary>
        ///// <param name="searchModel">Store pickup point search model</param>
        ///// <returns>
        ///// A task that represents the asynchronous operation
        ///// The task result contains the store pickup point search model
        ///// </returns>
        //public Task<StorePickupPointSearchModel> PrepareStorePickupPointSearchModelAsync(StorePickupPointSearchModel searchModel)
        //{
        //    if (searchModel == null)
        //        throw new ArgumentNullException(nameof(searchModel));

        //    //prepare page parameters
        //    searchModel.SetGridPageSize();

        //    return Task.FromResult(searchModel);
        //}

        #endregion
    }
}
