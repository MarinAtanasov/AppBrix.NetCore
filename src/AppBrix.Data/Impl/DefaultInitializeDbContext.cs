﻿// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using System;
using System.Linq;

namespace AppBrix.Data.Impl
{
    internal class DefaultInitializeDbContext : IInitializeDbContext
    {
        #region Construction
        public DefaultInitializeDbContext(IApp app, string migrationsAssembly)
        {
            this.App = app;
            this.MigrationsAssembly = migrationsAssembly;
        }
        #endregion

        #region Propreties
        public IApp App { get; }

        public string MigrationsAssembly { get; }
        #endregion
    }
}
