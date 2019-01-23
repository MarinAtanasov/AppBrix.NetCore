﻿// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Linq;

namespace AppBrix.Data.Migration.Impl
{
    internal sealed class DefaultInitializeDbContext : IInitializeDbContext
    {
        #region Construction
        public DefaultInitializeDbContext(IApp app, string migrationsAssembly, string migrationsHistoryTable)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            if (string.IsNullOrEmpty(migrationsAssembly))
                throw new ArgumentNullException(nameof(migrationsAssembly));
            if (string.IsNullOrEmpty(migrationsHistoryTable))
                throw new ArgumentNullException(nameof(migrationsHistoryTable));

            this.App = app;
            this.MigrationsAssembly = migrationsAssembly;
            this.MigrationsHistoryTable = migrationsHistoryTable;
        }
        #endregion

        #region Propreties
        public IApp App { get; }

        public string MigrationsAssembly { get; }

        public string MigrationsHistoryTable { get; }
        #endregion
    }
}
