// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Data.InMemory;
using AppBrix.Data.InMemory.Configuration;
using AppBrix.Data.Migration;
using AppBrix.Data.Migration.Configuration;
using AppBrix.Tests;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tests.Mocks;
using Xunit;

namespace AppBrix.Data.Tests
{
    public sealed class InMemoryDataTests : IDisposable
    {
        #region Setup and cleanup
        public InMemoryDataTests()
        {
            this.app = TestUtils.CreateTestApp(typeof(InMemoryDataModule), typeof(MigrationDataModule));
            this.app.GetConfig<InMemoryDataConfig>().ConnectionString = Guid.NewGuid().ToString();
            this.app.GetConfig<MigrationDataConfig>().EntryAssembly = this.GetType().Assembly.FullName;
            this.app.Start();
        }

        public void Dispose()
        {
            this.app.Stop();
        }
        #endregion

        #region Tests
        [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
        public void TestCrudOperations()
        {
            using (var context = this.app.GetDbContextService().Get<DataItemContextMock>())
            {
                context.Items.Add(new DataItemMock { Content = nameof(TestCrudOperations) });
                context.SaveChanges();
            }

            using (var context = this.app.GetDbContextService().Get<DataItemContextMock>())
            {
                var item = context.Items.Single();
                item.Id.Should().NotBe(Guid.Empty, "Id should be automatically generated");
                item.Content.Should().Be(nameof(TestCrudOperations), $"{nameof(item.Content)} should be saved");
                item.Content = nameof(DataItemContextMock);
                context.SaveChanges();
            }

            using (var context = this.app.GetDbContextService().Get<DataItemContextMock>())
            {
                var item = context.Items.Single();
                item.Content.Should().Be(nameof(DataItemContextMock), $"{nameof(item.Content)} should be updated");
                context.Items.Remove(item);
                context.SaveChanges();
            }

            using (var context = this.app.GetDbContextService().Get<DataItemContextMock>())
            {
                context.Items.Count().Should().Be(0, "the item should have been deleted");
            }
        }

        [Fact, Trait(TestCategories.Category, TestCategories.Performance)]
        public void TestPerformanceGetItem() => TestUtils.TestPerformance(this.TestPerformanceGetItemInternal);
        #endregion

        #region Private methods
        private void TestPerformanceGetItemInternal()
        {
            using (var context = this.app.GetDbContextService().Get<DataItemContextMock>())
            {
                context.Items.Add(new DataItemMock { Content = nameof(TestCrudOperations) });
                context.SaveChanges();
            }

            for (int i = 0; i < 150; i++)
            {
                using (var context = this.app.GetDbContextService().Get<DataItemContextMock>())
                {
                    context.Items.Single();
                } 
            }

            using (var context = this.app.GetDbContextService().Get<DataItemContextMock>())
            {
                context.Items.Remove(context.Items.Single());
                context.SaveChanges();
            }
        }
        #endregion

        #region Private fields and constants
        private readonly IApp app;
        #endregion
    }
}
