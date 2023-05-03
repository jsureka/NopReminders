using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Logging;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Stores;
using Nop.Core.Domain.Tax;
using Nop.Data;
using Nop.Plugin.Misc.Reminders.Data;
using Nop.Plugin.Misc.Reminders.Domain;
using Nop.Plugin.Misc.Reminders.Factories;
using Nop.Plugin.Misc.Reminders.Models;
using Nop.Plugin.Misc.Reminders.Service;
using Nop.Plugin.Misc.Reminders.Services;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.ScheduleTasks;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Models.Extensions;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.Reminders.Controllers
{
    [Area(AreaNames.Admin)]
    [AuthorizeAdmin]
    [AutoValidateAntiforgeryToken]
    public class RemindersAdminController : BasePluginController
    {
        #region Fields

        protected readonly CurrencySettings _currencySettings;
        protected readonly IBaseAdminModelFactory _baseAdminModelFactory;
        protected readonly IDateTimeHelper _dateTimeHelper;
        protected readonly ILocalizationService _localizationService;
        protected readonly INotificationService _notificationService;
        protected readonly IPermissionService _permissionService;
        protected readonly IProductService _productService;
        protected readonly IScheduleTaskService _scheduleTaskService;
        protected readonly ISettingService _settingService;
        protected readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IRemindersService _remindersService;
        private readonly IRemindersModelFactory _remindersModelFactory;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly IReminderMessageTemplateService _reminderMessageTemplateService;

        #endregion

        #region Ctor

        public RemindersAdminController(CurrencySettings currencySettings,
            IBaseAdminModelFactory baseAdminModelFactory,
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            IProductService productService,
            IScheduleTaskService scheduleTaskService,
            ISettingService settingService,
            IStoreContext storeContext,
            IStoreService storeService,
            IRemindersService remindersService,
            IRemindersModelFactory remindersModelFactory,
            IEmailAccountService emailAccountService,
            IMessageTokenProvider messageTokenProvider,
            IReminderMessageTemplateService reminderMessageTemplateService
            )
        {
            _currencySettings = currencySettings;
            _baseAdminModelFactory = baseAdminModelFactory;
            _dateTimeHelper = dateTimeHelper;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _productService = productService;
            _scheduleTaskService = scheduleTaskService;
            _settingService = settingService;
            _storeContext = storeContext;
            _storeService = storeService;
            _remindersService = remindersService;
            _remindersModelFactory = remindersModelFactory;
            _emailAccountService = emailAccountService;
            _messageTokenProvider = messageTokenProvider;
            _reminderMessageTemplateService = reminderMessageTemplateService;
        }

        #endregion

        #region Methods

        #region Configuration

        [ActionName("Configure")]
        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();
            var model = new RemindersSearchModel();
            

            return View("~/Plugins/Misc.Reminders/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> List(RemindersSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAppSettings))
                return await AccessDeniedDataTablesJson();

            var model = await _remindersModelFactory.PrepareReminderListModelAsync(searchModel);

            return Json(model);
        }

        public async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            var model = new RemindersModel();
            model.MessageTemplateModel = new MessageTemplateModel();
            var emails = await _emailAccountService.GetAllEmailAccountsAsync();
            model.MessageTemplateModel.AllowedTokens = "";
            model.MessageTemplateModel.AvailableEmailAccounts = emails;
            var allowedTokens = await _messageTokenProvider.GetListOfAllowedTokensAsync();
            foreach(var token in allowedTokens)
            {
                model.MessageTemplateModel.AllowedTokens += token;
            }
            var activeStores = await _storeService.GetAllStoresAsync();
            if (activeStores?.Any() is true)
            {
                model.Store = activeStores.Select(store => new SelectListItem
                {
                    Text = store.DefaultTitle,
                    Value = store.Id.ToString(),
                }).ToList();
            }
            var reminderRules = SeedDataReminderRule.Entities;
            var currentStore = await _storeContext.GetCurrentStoreAsync();
            model.StoreId = currentStore.Id.ToString();
            model.ReminderRuleId = reminderRules.FirstOrDefault().Id.ToString();
            model.ReminderRule = reminderRules.Select(rule => new SelectListItem
            {
                Text = rule.Name,
                Value = rule.Id.ToString(),
            }).ToList();
            return View("~/Plugins/Misc.Reminders/Views/Create.cshtml", model);
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Create(RemindersModel remindersModel)
        {
            if(!ModelState.IsValid)
            {
                return await Create();
            }
            var template = new ReminderMessageTemplate
            {
                BccEmailAddresses = remindersModel.MessageTemplateModel.BccEmailAddresses,
                EmailAccountId = remindersModel.MessageTemplateModel.EmailAccountId,
                Body = remindersModel.MessageTemplateModel.Body,
                Subject = remindersModel.MessageTemplateModel.Subject,
                MessageTemplateName = remindersModel.MessageTemplateModel.MessageTemplateName
            };
            await _reminderMessageTemplateService.InsertReminderMessageTemplateAsync(template);
            var reminder = new Reminder
            {
                Enabled = remindersModel.Enabled,
                Name = remindersModel.Name,
                NumberOfMessagesPerCustomer = remindersModel.NumberOfMessagesPerCustomer,
                ConditionMetEarlierThan = remindersModel.ConditionMetEarlierThan,
                ConditionMetLaterThan = remindersModel.ConditionMetLaterThan,
                IntervalBetweenMessages = remindersModel.IntervalBetweenMessages,
                StoreId = int.Parse(remindersModel.StoreId),
                ReminderRuleId = int.Parse(remindersModel.ReminderRuleId),
                ReminderMessageTemplateId = template.Id
            };
            await _remindersService.InsertReminderAsync(reminder);
            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAppSettings))
                return AccessDeniedView();

            var reminder = await _remindersService.GetReminderByIdAsync(id);
            if (reminder == null)
                return RedirectToAction("Configure");

            var reminderRules = SeedDataReminderRule.Entities;
            var model = new RemindersModel
            {
                Id = reminder.Id,
                Name = reminder.Name,
                ConditionMetEarlierThan = reminder.ConditionMetEarlierThan,
                ConditionMetLaterThan = reminder.ConditionMetLaterThan,
                Enabled = reminder.Enabled,
                ReminderRuleId = reminder.ReminderRuleId.ToString(),
                IntervalBetweenMessages = reminder.IntervalBetweenMessages,
                StoreId = reminder.StoreId.ToString(),
                NumberOfMessagesPerCustomer = reminder.NumberOfMessagesPerCustomer,
            };
            var messageTemplate = await _reminderMessageTemplateService.GetReminderMessageTemplateByIdAsync(reminder.ReminderMessageTemplateId);
            model.MessageTemplateModel = new MessageTemplateModel
            {
                Subject = messageTemplate.Subject,
                BccEmailAddresses = messageTemplate.BccEmailAddresses,
                Body = messageTemplate.Body,
                EmailAccountId = messageTemplate.EmailAccountId,
                MessageTemplateName = messageTemplate.MessageTemplateName,
                Id = messageTemplate.Id,
            };
            var emails = await _emailAccountService.GetAllEmailAccountsAsync();
            model.MessageTemplateModel.AllowedTokens = "";
            model.MessageTemplateModel.AvailableEmailAccounts = emails;
            var allowedTokens = await _messageTokenProvider.GetListOfAllowedTokensAsync();
            foreach (var token in allowedTokens)
            {
                model.MessageTemplateModel.AllowedTokens += token;
            }
            var activeStores = await _storeService.GetAllStoresAsync();
            if (activeStores?.Any() is true)
            {
                model.Store = activeStores.Select(store => new SelectListItem
                {
                    Text = store.DefaultTitle,
                    Value = store.Id.ToString(),
                }).ToList();
            }
            model.ReminderRule = reminderRules.Select(rule => new SelectListItem
            {
                Text = rule.Name,
                Value = rule.Id.ToString(),
            }).ToList();

            return View("~/Plugins/Misc.Reminders/Views/Edit.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RemindersModel remindersModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAppSettings))
                return AccessDeniedView();
            if (!ModelState.IsValid)
                return await Edit(remindersModel.Id);
            var reminder = await _remindersService.GetReminderByIdAsync(remindersModel.Id);
            var messageTemplate = new ReminderMessageTemplate()
            {
                Id = remindersModel.MessageTemplateModel.Id,
                BccEmailAddresses = remindersModel.MessageTemplateModel.BccEmailAddresses,
                EmailAccountId = remindersModel.MessageTemplateModel.EmailAccountId,
                Body = remindersModel.MessageTemplateModel.Body,
                Subject = remindersModel.MessageTemplateModel.Subject,
                MessageTemplateName = remindersModel.MessageTemplateModel.MessageTemplateName
            };
            if (reminder == null)
                return RedirectToAction("Configure");
            await _reminderMessageTemplateService.UpdateReminderMessageTemplateByIdAsync(messageTemplate);
            reminder.Enabled = remindersModel.Enabled;
            reminder.Name = remindersModel.Name;
            reminder.NumberOfMessagesPerCustomer = remindersModel.NumberOfMessagesPerCustomer;
            reminder.ConditionMetEarlierThan = remindersModel.ConditionMetEarlierThan;
            reminder.ConditionMetLaterThan = remindersModel.ConditionMetLaterThan;
            reminder.IntervalBetweenMessages = remindersModel.IntervalBetweenMessages;
            reminder.StoreId = int.Parse(remindersModel.StoreId);
            reminder.ReminderRuleId = int.Parse(remindersModel.ReminderRuleId);
            await _remindersService.UpdateReminderByIdAsync(reminder);

            ViewBag.RefreshPage = true;

            return await Configure();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAppSettings))
                return AccessDeniedView();

            var reminder = await _remindersService.GetReminderByIdAsync(id);
            if (reminder == null)
                return RedirectToAction("Configure");
            var messageTemplate = await _reminderMessageTemplateService.GetReminderMessageTemplateByIdAsync(reminder.ReminderMessageTemplateId);
            await _reminderMessageTemplateService.DeleteReminderMessageTemplateByIdAsync(messageTemplate);
            await _remindersService.DeleteReminderByIdAsync(reminder);

            return new NullJsonResult();
        }
     
        #endregion
        #endregion

    }
}