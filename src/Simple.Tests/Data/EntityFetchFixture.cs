﻿using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NUnit.Framework;
using SharpTestsEx;
using Simple.Tests.Resources;
using System;

namespace Simple.Tests.Data
{
    public class EntityFetchFixture : BaseDataFixture
    {
        protected override bool OpenOwnTx
        {
            get
            {
                return false;
            }
        }

        [Test]
        public void CanLoadOneProductAndItsCategoryWithLazyLoad()
        {
            Product p = null;
            using (MySimply.EnterContext())
            {
                p = Product.Find(x => true);
            }
            Assert.Throws<LazyInitializationException>(() => p.Category.Name.ToString());
        }

        [Test]
        public void CanLoadOneProductAndItsCategoryWithLazyLoadFetchingCategory()
        {
            Product p = null;
            using (MySimply.EnterContext())
            {
                p = Product.Find(x => true, q => q.Fetch(x => x.Category));
            }
            p.Category.Name.Should().Be("Beverages");
        }

        [Test]
        public void CanLoadOneProductAndItsCategoryWithLazyLoadFetchingCategoryButNotSupplier()
        {
            Product p = null;
            using (MySimply.EnterContext())
            {
                p = Product.Find(x => true, q => q.Fetch(x => x.Category));
            }
            p.Category.Name.Should().Be("Beverages");
            Assert.Throws<LazyInitializationException>(() => p.Supplier.ContactName.ToString());
        }

        [Test]
        public void CanLoadOneProductAndItsCategoryWithLazyLoadFetchingCategoryAndSupplier()
        {
            Product p = null;
            using (MySimply.EnterContext())
            {
                p = Product.Find(x => true, q => q.Fetch(x => x.Category).Fetch(x => x.Supplier));
            }
            p.Category.Name.Should().Be("Beverages");
            p.Supplier.ContactName.Should().Be("Charlotte Cooper");

        }

        [Test]
        public void CanListAllProductsAndTheirCategoriesWithLazyLoadFetchingCategoryAndSupplier()
        {
            IList<Product> p = null;
            using (MySimply.EnterContext())
            {
                p = Product.ListAll(q => q.Fetch(x => x.Category).Fetch(x => x.Supplier));
            }
            CollectionAssert.AllItemsAreNotNull(p.Select(x => x.Category.Name).ToArray());
            CollectionAssert.AllItemsAreNotNull(p.Select(x => x.Supplier.ContactName).ToArray());
        }

        [Test]
        public void CanListAllBeverageProductsUsingFetch()
        {
            Category c;
            using (MySimply.EnterContext())
            {
                c = Category.Find(x => x.Name == "Beverages",
                    q => q.FetchMany(x => x.Products).ThenFetch(x => x.Supplier));
            }
            c.Products.Select(x => x.Category.Name)
                .Should().Have.SameSequenceAs(new[] { "Beverages" });
            
            c.Products.Select(x => x.Supplier.ContactName)
                .Should().Not.Contain(null);
        }

        [Test]
        public void CanFetchInTwoLevels()
        {
                EmployeeTerritory p;
                using (MySimply.EnterContext())
                {
                    p = EmployeeTerritory.Find(x => true, q => q.Fetch(x => x.Territory).ThenFetch(x => x.Region));
                }
                p.Territory.Region.Description.Should().Be("Eastern".PadRight(50, ' '));
           
        }


        [Test]
        public void CanLoadOneProductAndItsCategoryWithLazyLoadFetchingCategoryWhenCategoryIsNull()
        {

            using (MySimply.EnterContext())
            {
                using (var tx = MySimply.BeginTransaction())
                {
                    var p1 = Product.Find(x => true).Clone();
                    p1.Category = null;
                    p1 = p1.Save();

                    Product p = Product.Find(x => x.Id == p1.Id, q => q.Fetch(x => x.Category));

                    p.Category.Should().Be(null);
                }
            }
        }
    }
}
