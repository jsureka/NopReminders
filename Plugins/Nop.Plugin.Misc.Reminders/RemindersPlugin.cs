using ExCSS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Nop.Core.Domain.Media;
using Nop.Data;
using Nop.Plugin.Misc.Reminders.Data;
using Nop.Plugin.Misc.Reminders.Domain;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.ScheduleTasks;
using Nop.Web.Framework;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Misc.Reminders
{
    /// <summary>
    /// Represents Zettle plugin
    /// </summary>
    public class RemindersPlugin : BasePlugin, IAdminMenuPlugin, IMiscPlugin, IPlugin
    {
        #region Fields

        protected readonly IActionContextAccessor _actionContextAccessor;
        protected readonly ILocalizationService _localizationService;
        protected readonly IScheduleTaskService _scheduleTaskService;
        protected readonly ISettingService _settingService;
        protected readonly IUrlHelperFactory _urlHelperFactory;
        protected readonly MediaSettings _mediaSettings;

        #endregion

        #region Ctor

        public RemindersPlugin(IActionContextAccessor actionContextAccessor,
            ILocalizationService localizationService,
            IScheduleTaskService scheduleTaskService,
            ISettingService settingService,
            IUrlHelperFactory urlHelperFactory,
            MediaSettings mediaSettings)
        {
            _actionContextAccessor = actionContextAccessor;
            _localizationService = localizationService;
            _scheduleTaskService = scheduleTaskService;
            _settingService = settingService;
            _urlHelperFactory = urlHelperFactory;
            _mediaSettings = mediaSettings;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext).RouteUrl(RemindersDefault.ConfigurationRouteName);
        }

        /// <summary>
        /// Manage sitemap. You can use "SystemName" of menu items to manage existing sitemap or add a new menu item.
        /// </summary>
        /// <param name="rootNode">Root node of the sitemap.</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            var configurationItem = rootNode.ChildNodes.FirstOrDefault(node => node.SystemName.Equals("Configuration"));
            if (configurationItem is null)
                return;

            var shippingItem = configurationItem.ChildNodes.FirstOrDefault(node => node.SystemName.Equals("Shipping"));
            var widgetsItem = configurationItem.ChildNodes.FirstOrDefault(node => node.SystemName.Equals("Widgets"));
            if (shippingItem is null && widgetsItem is null)
                return;

            var index = shippingItem is not null ? configurationItem.ChildNodes.IndexOf(shippingItem) : -1;
            if (index < 0)
                index = widgetsItem is not null ? configurationItem.ChildNodes.IndexOf(widgetsItem) : -1;
            if (index < 0)
                return;

            configurationItem.ChildNodes.Insert(index + 1, new SiteMapNode
            {
                Visible = true,
                SystemName = "Reminders",
                Title = "Reminders",
                IconClass = "far fa-dot-circle",
                ChildNodes = new List<SiteMapNode>
                {
                    new()
                    {
                        Visible = true,
                        SystemName = PluginDescriptor.SystemName,
                        Title = PluginDescriptor.FriendlyName,
                        ControllerName = "RemindersAdmin",
                        ActionName = "Configure",
                        IconClass = "far fa-circle",
                        RouteValues = new RouteValueDictionary { { "area", AreaNames.Admin } }
                    },
                     new()
                    {
                        Visible = true,
                        SystemName = PluginDescriptor.SystemName,
                        Title = "Reminders Report",
                        ControllerName = "RemindersAdmin",
                        ActionName = "Configure",
                        IconClass = "far fa-circle",
                        RouteValues = new RouteValueDictionary { { "area", AreaNames.Admin } }
                    }
                }
            });
        }

        /// <summary>
        /// Install the plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task InstallAsync()
        {
           
            //ensure MediaSettings.UseAbsoluteImagePath is enabled (used for images uploading)
            await _settingService.SetSettingAsync($"{nameof(MediaSettings)}.{nameof(MediaSettings.UseAbsoluteImagePath)}", true, clearCache: false);

            await _settingService.SaveSettingAsync(new RemindersSetting
            {
                //SyncEnabled = true,
                //DefaultTaxEnabled = true,
                //AutoSyncEnabled = false,
                //AutoSyncPeriod = ZettleDefaults.SynchronizationTask.Period / 60,
                //RequestTimeout = ZettleDefaults.RequestTimeout,
                //ImportProductsNumber = ZettleDefaults.ImportProductsNumber,
                //AutoAddRecordsEnabled = true,
                //LogSyncMessages = true,
                //CategorySyncEnabled = true,
                //ClearRecordsOnChangeCredentials = true
            });

            //if (await _scheduleTaskService.GetTaskByTypeAsync(ZettleDefaults.SynchronizationTask.Type) is null)
            //{
            //    await _scheduleTaskService.InsertTaskAsync(new()
            //    {
            //        Enabled = false,
            //        StopOnError = false,
            //        LastEnabledUtc = DateTime.UtcNow,
            //        Name = ZettleDefaults.SynchronizationTask.Name,
            //        Type = ZettleDefaults.SynchronizationTask.Type,
            //        Seconds = ZettleDefaults.SynchronizationTask.Period
            //    });
            //}

            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Misc.Reminders.Menu.Title"] = "Reminders",
                ["Plugins.Misc.Reminders.Fields.Name"] = "Name",
                ["Plugins.Misc.Reminders.Fields.MessageTemplateName"] = "Message Template Name",
                ["Plugins.Misc.Reminders.Fields.ReminderRule"] = "Reminder Rule",
                ["Plugins.Misc.Reminders.Fields.Store"] = "Store",
                ["Plugins.Misc.Reminders.Fields.Enabled"] = "Enabled",
                ["Plugins.Misc.Reminders.Fields.NumberOfMessagesPerCustomer"] = "Number Of Messages Per Customer",
                ["Plugins.Misc.Reminders.Fields.ConditionMetLaterThan"] = "Condition Met Later Than",
                ["Plugins.Misc.Reminders.Fields.ConditionMetEarlierThan"] = "Condition Met Earlier Than",
                ["Plugins.Misc.Reminders.Fields.IntervalBetweenMessages"] = "Interval Between Messages",
            });

            await base.InstallAsync();
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task UninstallAsync()
        {
            var remindersSetting = await _settingService.LoadSettingAsync<RemindersSetting>();
          

            await _settingService.DeleteSettingAsync<RemindersSetting>();
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Misc.Reminders");

            //var scheduleTask = await _scheduleTaskService.GetTaskByTypeAsync(ZettleDefaults.SynchronizationTask.Type);
            //if (scheduleTask is not null)
            //    await _scheduleTaskService.DeleteTaskAsync(scheduleTask);

            await base.UninstallAsync();
        }

        #endregion
    }
}