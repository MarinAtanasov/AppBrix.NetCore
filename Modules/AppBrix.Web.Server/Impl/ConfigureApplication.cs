﻿// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Web.Server.Events;
using Microsoft.AspNetCore.Builder;

namespace AppBrix.Web.Server.Impl
{
    internal sealed class ConfigureApplication : IConfigureApplication
    {
        #region Construction
        public ConfigureApplication(IApplicationBuilder builder)
        {
            this.Builder = builder;
        }
        #endregion

        #region Properties
        public IApplicationBuilder Builder { get; }
        #endregion
    }
}
