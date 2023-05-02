﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.Reminders.Domain;

namespace Nop.Plugin.Misc.Reminders.Data
{
    [NopMigration("2023/04/25 09:09:17:6455442", "Misc.Reminders base schema", MigrationProcessType.Installation)]
    public class SchemaMigration : AutoReversingMigration
    {
        #region Methods

        /// <summary>
        /// Collect the UP migration expressions
        /// </summary>
        public override void Up()
        {
            Create.TableFor<Reminder>();
            Create.TableFor<ReminderMessageTemplate>();
        }
    }
        #endregion
}
