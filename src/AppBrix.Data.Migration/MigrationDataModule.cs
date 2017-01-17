// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Data.Migration.Impl;
using AppBrix.Lifecycle;
using AppBrix.Modules;
using System;
using System.Linq;

namespace AppBrix.Data.Migration
{
    /// <summary>
    /// Module used for enabling database CodeFirst migration functionality.
    /// </summary>
    public sealed class MigrationDataModule : ModuleBase
    {
        #region Public and overriden methods
        protected override void InitializeModule(IInitializeContext context)
        {
            this.App.GetContainer().Register(this);
            this.contextLoader.Value.Initialize(context);
            this.App.GetContainer().Register(this.contextLoader.Value);
            this.App.GetFactory().Register(() => new MigrationContext(this.App));
        }

        protected override void UninitializeModule()
        {
            this.contextLoader.Value.Uninitialize();
        }
        #endregion

        #region Private fields and constants
        private readonly Lazy<DefaultMigrationDbContextLoader> contextLoader = new Lazy<DefaultMigrationDbContextLoader>();
        #endregion
    }
}