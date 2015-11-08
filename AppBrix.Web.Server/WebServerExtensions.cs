﻿// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Linq;

namespace AppBrix
{
    /// <summary>
    /// Adds extension methods for using MVC and Web Api controllers.
    /// </summary>
    public static class WebServerExtensions
    {
        #region Public methods
        /// <summary>
        /// Adds the current application to be resolved in MVC and Web Api controllers.
        /// This method must be called from <see cref="ConfigureServices"/> method.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="app"></param>
        public static void AddApp(this IServiceCollection services, IApp app)
        {
            services.AddInstance(app);
        }
        #endregion
    }
}