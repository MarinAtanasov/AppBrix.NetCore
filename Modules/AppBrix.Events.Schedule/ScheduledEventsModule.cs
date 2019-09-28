// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Events.Schedule.Impl;
using AppBrix.Lifecycle;
using AppBrix.Modules;
using AppBrix.Time;
using System;
using System.Collections.Generic;

namespace AppBrix.Events.Schedule
{
    /// <summary>
    /// Base module used for registering scheduled events.
    /// </summary>
    public sealed class ScheduledEventsModule : ModuleBase
    {
        #region Properties
        /// <summary>
        /// Gets the types of the modules which are direct dependencies for the current module.
        /// This is used to determine the order in which the modules are loaded.
        /// </summary>
        public override IEnumerable<Type> Dependencies => new[] { typeof(EventsModule), typeof(TimeModule) };
        #endregion

        #region Public and overriden methods
        /// <summary>
        /// Initializes the module.
        /// Automatically called by <see cref="ModuleBase.Initialize"/>
        /// </summary>
        /// <param name="context">The initialization context.</param>
        protected override void Initialize(IInitializeContext context)
        {
            this.App.Container.Register(this);
            this.eventHub.Initialize(context);
            this.App.Container.Register(this.eventHub);
        }

        /// <summary>
        /// Uninitializes the module.
        /// Automatically called by <see cref="ModuleBase.Uninitialize"/>
        /// </summary>
        protected override void Uninitialize()
        {
            this.eventHub.Uninitialize();
        }
        #endregion

        #region Private fields and constants
        private readonly DefaultScheduledEventHub eventHub = new DefaultScheduledEventHub();
        #endregion
    }
}
