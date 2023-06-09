﻿using Nop.Core;

namespace Nop.Plugin.Misc.Reminders
{
    /// <summary>
    /// Represents plugin constants
    /// </summary>
    public class RemindersDefault
    {
        /// <summary>
        /// Gets the plugin system name
        /// </summary>
        public static string SystemName => "Misc.Reminders";

        /// <summary>
        /// Gets the user agent used to request third-party services
        /// </summary>
        public static string UserAgent => $"nopCommerce-{NopVersion.CURRENT_VERSION}";

        /// <summary>
        /// Gets the application name
        /// </summary>
        public static string ApplicationName => "nopCommerce-integration";

        /// <summary>
        /// Gets the partner identifier
        /// </summary>
        public static string PartnerIdentifier => "nopCommerce";

        /// <summary>
        /// Gets the partner affiliation header used for each request to APIs
        /// </summary>
        public static (string Name, string Value) PartnerHeader => ("X-reminders-Application-Id", "f4954821-e7e4-4fca-854e-e36060b5748d");

        /// <summary>
        /// Gets the webhook request signature header
        /// </summary>
        public static string SignatureHeader => "X-reminders-Signature";

        /// <summary>
        /// Gets a default period (in seconds) before the request times out
        /// </summary>
        public static int RequestTimeout => 15;

        /// <summary>
        /// Gets a default number of products to import in one request
        /// </summary>
        public static int ImportProductsNumber => 500;

        /// <summary>
        /// Gets webhook event names to subscribe
        /// </summary>
        public static List<string> WebhookEventNames => new()
        {
            "ProductCreated",
            "InventoryBalanceChanged",
            "InventoryTrackingStopped",
            "ApplicationConnectionRemoved"
        };

        /// <summary>
        /// Gets the configuration route name
        /// </summary>
        public static string ConfigurationRouteName => "Plugin.Misc.Reminders.Configure";

        /// <summary>
        /// Gets the webhook route name
        /// </summary>
        public static string WebhookRouteName => "Plugin.Misc.Reminders.Webhook";

        /// <summary>
        /// Gets a name, type and period (in seconds) of the auto synchronization task
        /// </summary>
        public static (string Name, string Type, int Period) ReminderTask =>
            ("Reminder Task", "Nop.Plugin.Misc.Reminders.Services.ReminderTask", 3600);
    }
}