﻿// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Linq;

namespace AppBrix.Data.Impl
{
    internal sealed class DefaultInitializeDbContext : IInitializeDbContext
    {
        #region Construction
        public DefaultInitializeDbContext(IApp app, string migrationsAssembly = null, string migrationsHistoryTable = "__EFMigrationsHistory")
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

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
