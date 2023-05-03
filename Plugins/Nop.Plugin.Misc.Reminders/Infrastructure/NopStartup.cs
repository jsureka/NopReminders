using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Plugin.Misc.Reminders.Data;
using Nop.Plugin.Misc.Reminders.Domain;
using Nop.Plugin.Misc.Reminders.Factories;
using Nop.Plugin.Misc.Reminders.Service;
using Nop.Plugin.Misc.Reminders.Services;

namespace Nop.Plugin.Misc.Reminders.Infrastructure
{
    internal class NopStartup : INopStartup
    {
        public int Order => 3000;

        public void Configure(IApplicationBuilder application)
        {
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRemindersService, RemindersService>();
            services.AddScoped<IRemindersModelFactory, RemindersModelFactory>();
            services.AddScoped<IReminderMessageTemplateService, ReminderMessageTemplateService>();
        }
    }
}
