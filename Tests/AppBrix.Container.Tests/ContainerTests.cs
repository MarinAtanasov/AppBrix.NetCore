// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Container.Tests.Mocks;
using AppBrix.Tests;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace AppBrix.Container.Tests
{
    public sealed class ContainerTests : TestsBase
    {
        #region Setup and cleanup
        public ContainerTests() : base(TestUtils.CreateTestApp<ContainerModule>()) => this.app.Start();
        #endregion

        #region Tests
        [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
        public void TestGetContainer()
        {
            var container = this.GetContainer();
            container.Should().NotBeNull("unable to get the container");
            var container2 = this.GetContainer();
            container2.Should().NotBeNull("second call did not return a container");
            container2.Should().BeSameAs(container, "returned a different instance of the container");
        }

        [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
        public void TestResolveByInterface()
        {
            var container = this.GetContainer();
            var iContainer = container.Get<IContainer>();
            iContainer.Should().NotBeNull("unable to resolve the IContainer interface");
            iContainer.Should().BeSameAs(container, "returned IContainer is a different instance");
        }

        [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
        public void TestResolveByClass()
        {
            var container = this.GetContainer();
            var registered = new ChildMock();
            container.Register(registered);
            var resolved = container.Get<ChildMock>();
            resolved.Should().NotBeNull("unable to resolve the item by class");
            resolved.Should().BeSameAs(registered, "returned item is a different instance than the registered");
        }

        [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
        public void TestResolveByBaseClass()
        {
            var container = this.GetContainer();
            var original = new ChildMock();
            container.Register(original);
            var resolved = container.Get<ParentMock>();
            resolved.Should().NotBeNull("unable to resolve the Parent class");
            resolved.Should().BeSameAs(original, "returned Child is a different instance");
        }

        [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
        public void TestRegisterNull()
        {
            var container = this.GetContainer();
            Action action = () => container.Register(null);
            action.Should().Throw<ArgumentNullException>("passing a null object is not allowed");
        }

        [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
        public void TestObjectBaseTypeNotRegistered()
        {
            var container = this.GetContainer();
            container.Register(new ChildMock());
            Action action = () => container.Get<object>();
            action.Should().Throw<KeyNotFoundException>("items should not be registered as type of object");
        }

        [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
        public void TestDoubleRegistration()
        {
            var container = this.GetContainer();
            var resolved = new ChildMock();
            var resolved2 = new ChildMock();
            container.Register(resolved);
            container.Register(resolved2);
            container.Get<ChildMock>().Should().BeSameAs(resolved2, "object not replaced with second");
            container.Register(resolved);
            container.Get<ChildMock>().Should().BeSameAs(resolved, "object not replaced with original");
        }

        [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
        public void TestRegisterGenericObjectError()
        {
            var container = this.GetContainer();
            Action action = () => container.Register(new object());
            action.Should().Throw<ArgumentException>("registering an item as System.Object should not be allowed.");
        }

        [Fact, Trait(TestCategories.Category, TestCategories.Performance)]
        public void TestPerformanceContainer() => TestUtils.TestPerformance(this.TestPerformanceContainerInternal);
        #endregion

        #region Private methods
        private IContainer GetContainer() => this.app.Container;

        private void TestPerformanceContainerInternal()
        {
            var container = this.GetContainer();
            for (var i = 0; i < 1000; i++)
            {
                container.Register(new ChildMock());
            }
            for (var i = 0; i < 150000; i++)
            {
                container.Get(typeof(ChildMock));
                container.Get(typeof(ParentMock));
                container.Get(typeof(IContainer));
            }
            this.app.Reinitialize();
        }
        #endregion
    }
}
