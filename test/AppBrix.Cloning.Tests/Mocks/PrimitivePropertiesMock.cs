﻿// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBrix.Cloning.Tests.Mocks
{
    internal class PrimitivePropertiesMock : NumericPropertiesMock
    {
        #region Construction
        public PrimitivePropertiesMock()
            : base()
        {
        }

        public PrimitivePropertiesMock(bool b, char c, string s, DateTime d)
            : base(1, 2, 3, 4, 5.5f, 6.6, (decimal)7.7)
        {
            this.Bool = b;
            this.c = c;
            this.String = s;
            this.dateTime = d;
        }
        #endregion

        #region Properties
        public bool Bool { get; set; }

        public char Char { get { return this.c; } }

        public string String { get; private set; }

        public DateTime DateTime { get { return this.dateTime; } }
        #endregion

        #region Private fields and constants
        private readonly char c;
        private DateTime dateTime;
        #endregion
    }
}