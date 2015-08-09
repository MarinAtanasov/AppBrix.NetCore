// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Lifecycle;
using AppBrix.Modules;
using System;
using System.Linq;

namespace AppBrix.Logging
{
    /// <summary>
    /// Modules used for registering a logs manager.
    /// </summary>
    public sealed class LoggingModule : ModuleBase
    {
        #region Public and overriden methods
        protected override void InitializeModule(IInitializeContext context)
        {
            this.App.GetResolver().Register(this);
            var logHub = this.logHub.Value;
            logHub.Initialize(context);
            this.App.GetResolver().Register(logHub);
        }

        protected override void UninitializeModule()
        {
            this.logHub.Value.Uninitialize();
        }
        #endregion

        #region Private fields and constants
        private Lazy<DefaultLogHub> logHub = new Lazy<DefaultLogHub>();
        #endregion
    }
}
