﻿// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Lifecycle;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Factory
{
    /// <summary>
    /// Default factory which will execute the default constructor
    /// unless a different method has been registered.
    /// </summary>
    internal sealed class DefaultFactory : IFactory, IApplicationLifecycle
    {
        #region IApplicationLifecycle implementation
        public void Initialize(IInitializeContext context)
        {
        }

        public void Uninitialize()
        {
            this.factories.Clear();
        }
        #endregion

        #region IFactory implementation
        public void Register(Func<object> factoryMethod, Type type)
        {
            if (factoryMethod == null)
                throw new ArgumentNullException(nameof(factoryMethod));
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var baseType = type;
            while (baseType != null && baseType != typeof(object))
            {
                this.factories[baseType] = factoryMethod;
                baseType = baseType.BaseType;
            }

            foreach (var @interface in type.GetInterfaces())
            {
                this.factories[@interface] = factoryMethod;
            }
        }
        
        public object Get(Type type)
        {
            try
            {
                return this.factories.TryGetValue(type, out var factory) ? factory() : type.CreateObject();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    string.Concat(
                        $"Unable to create instance of type {type.GetAssemblyQualifiedName()}. ",
                        "Make sure that you have registered a factory method first."
                    ), ex);
            }
        }
        #endregion

        #region Private fields and constants
        private readonly IDictionary<Type, Func<object>> factories = new Dictionary<Type, Func<object>>();
        #endregion
    }
}
