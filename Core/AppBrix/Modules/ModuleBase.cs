// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Lifecycle;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Modules
{
    /// <summary>
    /// Base class for application modules.
    /// This class will set <see cref="App"/> before calling the overriden methods.
    /// </summary>
    public abstract class ModuleBase : IModule
    {
        #region Properties
        #nullable disable
        /// <summary>
        /// Gets the current module's app.
        /// </summary>
        protected IApp App { get; private set; }
        #nullable restore

        /// <summary>
        /// Gets the types of the modules which are direct dependencies for the current module.
        /// This is used to determine the order in which the modules are loaded.
        /// </summary>
        public virtual IEnumerable<Type> Dependencies => this.GetType().Assembly
            .GetReferencedAssemblies(recursive: false)
            .Skip(1)
            .SelectMany(assembly => assembly.ExportedTypes)
            .Where(t => t.IsClass && !t.IsAbstract && typeof(IModule).IsAssignableFrom(t));
        #endregion

        #region Public and overriden methods
        void IInstallable.Install(IInstallContext context)
        {
            this.App = context.App;
            this.Install(context);
            this.App = null;
        }

        void IInstallable.Upgrade(IUpgradeContext context)
        {
            this.App = context.App;
            this.Upgrade(context);
            this.App = null;
        }


        void IInstallable.Uninstall(IUninstallContext context)
        {
            this.App = context.App;
            this.Uninstall(context);
            this.App = null;
        }

        void IApplicationLifecycle.Initialize(IInitializeContext context)
        {
            this.App = context.App;
            this.Initialize(context);
        }

        void IApplicationLifecycle.Uninitialize()
        {
            this.Uninitialize();
            this.App = null;
        }
        #endregion

        #region Protected methods
        /// <summary>
        /// Installs the module by making any permanent changes required for it to be initialized in the future.
        /// Automatically called by <see cref="ModuleBase.Install"/>
        /// There is no need to call the base method when overriding.
        /// </summary>
        /// <param name="context">The install context.</param>
        protected virtual void Install(IInstallContext context) { }

        /// <summary>
        /// Upgrades the module by making any permanent changes required for it to be initialized in the future.
        /// Automatically called by <see cref="ModuleBase.Upgrade"/>
        /// There is no need to call the base method when overriding.
        /// </summary>
        /// <param name="context">The upgrade context.</param>
        protected virtual void Upgrade(IUpgradeContext context) { }

        /// <summary>
        /// Uninstalls the module by reverting any changes from <see cref="ModuleBase.Install"/> or <see cref="ModuleBase.Upgrade"/>.
        /// Automatically called by <see cref="ModuleBase.Uninstall"/>.
        /// There is no need to call the base method when overriding.
        /// </summary>
        /// <param name="context">The uninstall context.</param>
        protected virtual void Uninstall(IUninstallContext context) { }

        /// <summary>
        /// Initializes the module.
        /// Automatically called by <see cref="ModuleBase.Initialize"/>
        /// There is no need to call the base method when overriding.
        /// </summary>
        /// <param name="context">The initialization context.</param>
        protected virtual void Initialize(IInitializeContext context) { }

        /// <summary>
        /// Uninitializes the module.
        /// Automatically called by <see cref="ModuleBase.Uninitialize"/>
        /// There is no need to call the base method when overriding.
        /// </summary>
        protected virtual void Uninitialize() { }
        #endregion
    }
}
