// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Configuration;
using System;
using System.Linq;
using System.Threading;

namespace AppBrix.Web.Client.Configuration
{
    public sealed class WebClientConfig : IConfig
    {
        #region Construction
        /// <summary>
        /// Creates a new instance of <see cref="WebClientConfig"/> with default values for the properties.
        /// </summary>
        public WebClientConfig()
        {
            this.RequestTimeout = Timeout.InfiniteTimeSpan;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the timeout used when making HTTP requests.
        /// </summary>
        public TimeSpan RequestTimeout { get; set; }
        #endregion
    }
}
