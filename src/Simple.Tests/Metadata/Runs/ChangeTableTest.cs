﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Migrator1 = Simple.Migrator.DbMigrator;
using Simple.Metadata;
using Simple.Migrator.Framework;
using Simple.Migrator.Fluent;
using System.Data;

namespace Simple.Tests.Metadata.Runs
{
    public class ChangeTableTest : BaseTest
    {
        public ChangeTableTest(DatabasesXml.Entry entry) : base(entry) { }


        [Migration(1)]
        public class Migration1 : FluentMigration
        {
            public override void Up()
            {
                Schema.AddTable("t_simple_table", t =>
                {
                    t.AddString("string1").WithSize(123);
                    t.AddInt32("int1");
                });
            }

            public override void Down()
            {
                Schema.RemoveTable("t_simple_table");
            }
        }

        [Migration(2)]
        public class Migration2 : FluentMigration
        {
            public override void Up()
            {
                Schema.ChangeTable("t_simple_table", t =>
                {
                    t.RemoveColumn("string1");
                    t.RemoveColumn("int1");
                    t.AddString("whatever").WithSize(1000);
                });
            }

            public override void Down()
            {
                Schema.ChangeTable("t_simple_table", t =>
                {
                    t.AddString("string1").WithSize(123);
                    t.AddInt32("int1");
                    t.RemoveColumn("whatever");
                });
            }
        }

        public override IEnumerable<Type> GetMigrations()
        {
            yield return typeof(Migration1);
            yield return typeof(Migration2);
        }

        public override IEnumerable<TableAddAction> GetTableDefinitions()
        {
            yield return TableDef("t_simple_table", t =>
            {
                t.AddInt32("id").PrimaryKey();
                t.AddString("whatever").WithSize(1000);
            });
        }
    }
}