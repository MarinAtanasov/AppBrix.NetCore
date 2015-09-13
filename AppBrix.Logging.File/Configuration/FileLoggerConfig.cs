// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Configuration;
using System;
using System.Linq;

namespace AppBrix.Logging.File.Configuration
{
    public sealed class FileLoggerConfig : IConfig
    {
        #region Construction
        /// <summary>
        /// Creates a default instance of <see cref="FileLoggerConfig"/> with default property values.
        /// </summary>
        public FileLoggerConfig()
        {
            this.Path = "Log.log";
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the path to the file where to keep the log.
        /// </summary>
        public string Path { get; set; }
        #endregion
    }
}
