﻿using System;
using NHibernate;
using NUnit.Framework;
using SharpTestsEx;
using Simple.Tests.Resources;

namespace Simple.Tests.Data
{
    [TestFixture]
    public class DataContextFixture : BaseDataFixture
    {
        protected override bool OpenOwnTx
        {
            get
            {
                return false;
            }
        }

        [Test]
        public void SimpleDataContextTest()
        {
            ISession s1, s2, s3, s4;
            using (var dx = MySimply.EnterContext())
            {
                s1 = dx.Session;
                s1.IsOpen.Should().Be.True();

                using (var dx2 = MySimply.EnterContext())
                {
                    s2 = dx2.Session;
                    s2.Should().Be(s1);

                    s3 = dx2.NewSession();
                    s3.IsOpen.Should().Be.True();

                    s4 = dx.NewSession();
                }

                s3.IsOpen.Should().Be.False();

                s2.IsOpen.Should().Be.True();
                s4.IsOpen.Should().Be.True();
            }

            s2.IsOpen.Should().Be.False();
            s4.IsOpen.Should().Be.False();
        }

        [Test]
        public void TestReopenDataContext()
        {
            try
            {
                using (var dx = MySimply.EnterContext())
                {
                    var s1 = dx.Session;
                    s1.Close();
                }
            }
            catch (InvalidOperationException) { }

            using (var dx = MySimply.EnterContext())
            {
                var s1 = dx.Session;
                dx.IsOpen.Should().Be.True();
            }
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void TryCloseMainSessionTest()
        {
            using (var dx = MySimply.EnterContext())
            {
                using (var dx2 = MySimply.EnterContext())
                {
                    dx2.Session.Close();
                }
            }
        }

        [Test]
        public void ParentingWillFollowCorrectOrder()
        {
            using (var dx = MySimply.EnterContext())
            {
                dx.Parent.Should().Be(null);
                using (var dx2 = MySimply.EnterContext())
                {
                    dx.Child.Should().Be(dx2);
                    dx2.Parent.Should().Be(dx);

                    using (var dx3 = MySimply.EnterContext())
                    {
                        dx2.Child.Should().Be(dx3);
                        dx3.Parent.Should().Be(dx2);
                        using (var dx4 = MySimply.EnterContext())
                        {
                            dx3.Child.Should().Be(dx4);
                            dx4.Parent.Should().Be(dx3);
                        }
                    }
                }
            }
        }


        [Test]
        public void ClosingParentContextAlsoClosesChildren()
        {
            using (var dx = MySimply.EnterContext())
            {
                var dx2 = MySimply.EnterContext();
                var dx3 = MySimply.EnterContext();
                var dx4 = MySimply.EnterContext();
                var dx5 = MySimply.EnterContext();

                dx3.Exit();
                dx3.IsOpen.Should().Be.False();
                dx4.IsOpen.Should().Be.False();
                dx5.IsOpen.Should().Be.False();

                dx2.IsOpen.Should().Be.True();

                MySimply.GetContext().Should().Be(dx2);
            }
        }



        [Test, ExpectedException(typeof(ObjectDisposedException))]
        public void NestedTransactions()
        {
            using (var dx = MySimply.EnterContext())
            {
                Category c = Category.Load(1);

                string saveOldName = c.Name;

                using (var tx1 = dx.Session.BeginTransaction())
                {
                    using (var tx2 = dx.Session.BeginTransaction())
                    {
                        c.Name = "NewName";
                        c.SaveOrUpdate();

                        c = Category.Load(1);
                        c.Name.Should().Be("NewName");

                        tx2.Commit();
                    }

                    c = Category.Load(1);
                    c.Name.Should().Be("NewName");

                    using (var tx2 = dx.Session.BeginTransaction())
                    {
                        c = Category.Load(1);
                        c.Name = saveOldName;
                        c.Update();

                        tx2.Commit();
                    }

                    tx1.Rollback();
                }

                c.Name.Should().Not.Be("NewName");

            }
        }
    }
}
