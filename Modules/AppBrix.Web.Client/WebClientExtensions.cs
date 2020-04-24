﻿// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Configuration;
using AppBrix.Factory;
using AppBrix.Web.Client;
using AppBrix.Web.Client.Configuration;

namespace AppBrix
{
    /// <summary>
    /// Extension methods for the <see cref="WebClientModule"/>.
    /// </summary>
    public static class WebClientExtensions
    {
        /// <summary>
        /// Gets the <see cref="WebClientConfig"/> from <see cref="IConfigService"/>.
        /// </summary>
        /// <param name="service">The configuration service.</param>
        /// <returns>The <see cref="WebClientConfig"/>.</returns>
        public static WebClientConfig GetWebClientConfig(this IConfigService service) => (WebClientConfig) service.Get(typeof(WebClientConfig));

        /// <summary>
        /// Returns an object of type <see cref="IHttpRequest"/>.
        /// </summary>
        /// <param name="factory">The factory service.</param>
        /// <returns>An instance of type <see cref="IHttpRequest"/>.</returns>
        public static IHttpRequest GetHttpRequest(this IFactoryService factory) => (IHttpRequest) factory.Get(typeof(IHttpRequest));
    }
}