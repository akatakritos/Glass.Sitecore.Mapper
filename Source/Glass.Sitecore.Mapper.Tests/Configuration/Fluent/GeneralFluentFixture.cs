/*
   Copyright 2011 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sitecore.Data;
using Glass.Sitecore.Mapper.Data;
using Glass.Sitecore.Mapper.FieldTypes;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Data.Fields;
using Sitecore.Resources.Media;
using Sitecore.SecurityModel;
using Glass.Sitecore.Mapper.Configuration;
using Glass.Sitecore.Mapper.Configuration.Fluent;
using Glass.Sitecore.Mapper.Tests.Configuration.Fluent.GeneralFluentFixtureNS;

namespace Glass.Sitecore.Mapper.Tests.Configuration.Fluent
{
    [TestFixture]
    public class GeneralFluentFixture
    {


        SitecoreService _sitecore;
        Context _context;
        Database _db;
        Item _query1;
        Item _query2;
        Item _test3;
        Item _check1;
        Item _check2;
        Item _multiList1;
        Item _multiList2;
        Item _treeList2;
        Item _treeList1;
        Item _link1;
        Item _link2;
        Item _link3;


        [SetUp]
        public void Setup()
        {

            var basic = new SitecoreClass<GeneralFluentFixtureNS.BasicTemplate>()
            .Fields(x =>
                {
                    x.Field(y => y.Checkbox);
                    x.Field(y => y.Date);
                })
            .Infos(x =>
                {
                    x.Info(y => y.ContentPath).InfoType(SitecoreInfoType.ContentPath);
                    x.Info(y => y.DisplayName).InfoType(SitecoreInfoType.DisplayName);
                })
            .Queries(x =>
                {
                    x.Query(y => y.Query).Query("/sitecore/content/Configuration/Fluent/GeneralFluent/Query/*[@@TemplateName='BasicTemplate']");

                });

            
            basic.Id(x => x.Id);

            //basic.Field(x => x.Checkbox);
            //basic.Field(x => x.Date);
            basic.Field(x => x.DateTime);
            basic.Field(x => x.File);
            basic.Field(x => x.Image);
            basic.Field(x => x.Integer);
            basic.Field(x => x.Float);
            basic.Field(x => x.Double);
            basic.Field(x => x.Decimal);
            basic.Field(x => x.MultiLineText);
            basic.Field(x => x.Number);
            basic.Field(x => x.Password);
            basic.Field(x => x.RichText).Setting(SitecoreFieldSettings.RichTextRaw);
            basic.Field(x => x.SingleLineText);

            basic.Field(x => x.CheckList);
            basic.Field(x => x.DropList);
            basic.Field(x => x.GroupedDropLink);
            basic.Field(x => x.GroupedDropList);
            basic.Field(x => x.MultiList);
            basic.Field(x => x.Treelist);
            basic.Field(x => x.TreeListEx);

            basic.Field(x => x.DropLink);
            basic.Field(x => x.DropTree);
            basic.Field(x => x.GeneralLink);

            basic.Field(x => x.Icon);
            basic.Field(x => x.TriState);

            basic.Field(x => x.Attachment);

            
            basic.Info(x => x.FullPath).InfoType(SitecoreInfoType.FullPath);
            basic.Info(x => x.Key).InfoType(SitecoreInfoType.Key);
            basic.Info(x => x.MediaUrl).InfoType(SitecoreInfoType.MediaUrl);
            basic.Info(x => x.Path).InfoType(SitecoreInfoType.Path);
            basic.Info(x => x.TemplateId).InfoType(SitecoreInfoType.TemplateId);
            basic.Info(x => x.TemplateName).InfoType(SitecoreInfoType.TemplateName);
            basic.Info(x => x.Url).InfoType(SitecoreInfoType.Url);
            basic.Info(x => x.Version).InfoType(SitecoreInfoType.Version);

            basic.Children(x => x.Children);

            basic.Parent(x => x.Parent);


            var subClass = new SitecoreClass<GeneralFluentFixtureNS.SubClass>();
            subClass.Id(x => x.Id);

            FluentConfigurationLoader loader = new FluentConfigurationLoader(
              basic, subClass                  
                             );

            _context = new Context(loader, new AbstractSitecoreDataHandler[] {});
            global::Sitecore.Context.Site = global::Sitecore.Configuration.Factory.GetSite("website");

            _sitecore = new SitecoreService("master");
            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");
            using (new SecurityDisabler())
            {
                _query1 = _db.GetItem("/sitecore/content/Configuration/Fluent/GeneralFluent/Query/Item1");
                _query2 = _db.GetItem("/sitecore/content/Configuration/Fluent/GeneralFluent/Query/Item2");
                _test3 = _db.GetItem("/sitecore/content/Glass/Test1/Test3");

                _check1 = _db.GetItem("/sitecore/content/Lookups/CheckList/Check1");
                _check2 = _db.GetItem("/sitecore/content/Lookups/CheckList/Check2");

                _multiList1 = _db.GetItem("/sitecore/content/Lookups/Multilist/MultiList1");
                _multiList2 = _db.GetItem("/sitecore/content/Lookups/Multilist/MultiList2");

                _treeList1 = _db.GetItem("/sitecore/content/Lookups/TreeList/TreeList1");
                _treeList2 = _db.GetItem("/sitecore/content/Lookups/TreeList/TreeList2");

                _link1 = _db.GetItem("/sitecore/content/Lookups/Links/Link1");
                _link2 = _db.GetItem("/sitecore/content/Lookups/Links/Link2");
                _link3 = _db.GetItem("/sitecore/content/Lookups/Links/Link3");
            }
        }

        #region Item Test1
        // Item: /sitecore/content/Glass/Test1
        [Test]
        public void GetItem_Test1()
        {

            //Assign 
            GeneralFluentFixtureNS.BasicTemplate test = null;

            //Act
            using (new SecurityDisabler())
            {
                test = _sitecore.GetItem<GeneralFluentFixtureNS.BasicTemplate>("/sitecore/content/Configuration/Fluent/GeneralFluent/GetItemTest1");
            }
            var getTestItem1 = _db.GetItem("/sitecore/content/Configuration/Fluent/GeneralFluent/GetItemTest1");
            //Assert

            #region SitecoreId

            Assert.AreEqual(getTestItem1.ID.Guid, test.Id);

            #endregion

            #region Fields
            #region SimpleTypes

            Assert.IsTrue(test.Checkbox);

            Assert.AreEqual(new DateTime(2011, 05, 01), test.Date);

            Assert.AreEqual(new DateTime(2011, 06, 01, 06, 30, 00), test.DateTime);


            var file=new File(){
                    Id = new Guid("{368A358E-5835-458B-AFE6-BA5F80334F5A}"),
                    Src ="~/media/Files/SimpleTextFile.ashx"
                };
            Assert.AreEqual(file.Id, test.File.Id);
            Assert.AreEqual(file.Src, test.File.Src);

            var image = new Image()
            {
                Alt = "Alternate Text  Test",
                Border = "",
                Class = "",
                Height = 540,
                HSpace = 50,
                MediaId = new Guid("{223EEAE5-DF4C-4E30-95AC-17BE2F00E2CD}"),
                Src = "~/media/Files/Kitten1.ashx",
                VSpace = 60,
                Width = 720,
            };
            Assert.AreEqual(image.Alt, test.Image.Alt);
            Assert.AreEqual(image.Border, test.Image.Border);
            Assert.AreEqual(image.Class, test.Image.Class);
            Assert.AreEqual(image.Height, test.Image.Height);
            Assert.AreEqual(image.HSpace, test.Image.HSpace);
            Assert.AreEqual(image.MediaId, test.Image.MediaId);
            Assert.AreEqual(image.Src, test.Image.Src);
            Assert.AreEqual(image.VSpace, test.Image.VSpace);
            Assert.AreEqual(image.Width, test.Image.Width);

            Assert.AreEqual(456, test.Integer);

            Assert.AreEqual(456f, test.Float);

            Assert.AreEqual(456d, test.Double);

            Assert.AreEqual(456m, test.Decimal);

            Assert.AreEqual("Multi Line Text Test", test.MultiLineText);

            Assert.AreEqual(789, test.Number);

            Assert.AreEqual("Password Test", test.Password);

            Assert.AreEqual("<p>Rich Text Test</p>", test.RichText.Trim());

            Assert.AreEqual("Single Line Text Test", test.SingleLineText);

            #endregion

            #region List Types

            Assert.AreEqual(2, test.CheckList.Count());
            Assert.AreEqual(_check1.ID.Guid, test.CheckList.First().Id);
            Assert.AreEqual(_check2.ID.Guid, test.CheckList.Last().Id);

            Assert.AreEqual(TestEnum.Test1, test.DropList);

            Assert.AreEqual(_test3.ID.Guid, test.GroupedDropLink.Id);

            Assert.AreEqual(TestEnum.Test3, test.GroupedDropList);

            Assert.AreEqual(_multiList1.ID.Guid, test.MultiList.First().Id);

            Assert.AreEqual(_treeList1.ID.Guid, test.Treelist.First().Id);

            Assert.AreEqual(_treeList2.ID.Guid, test.TreeListEx.First().Id);

            #endregion
            #region Link Types

            Assert.AreEqual(_link1.ID.Guid, test.DropLink.Id);

            Assert.AreEqual(_link2.ID.Guid, test.DropTree.Id);

            var link = new Link(){
                Anchor = "",
                Class = "Style Class Test",
                Target = "_blank",
                TargetId = Guid.Empty,
                Text = "Link Description Test",
                Title = "Alternate Text Test",
                Url = "http://www.google.com"
            };
            Assert.AreEqual(link.Anchor, test.GeneralLink.Anchor);
            Assert.AreEqual(link.Class, test.GeneralLink.Class);
            Assert.AreEqual(link.Target, test.GeneralLink.Target);
            Assert.AreEqual(link.TargetId, test.GeneralLink.TargetId);
            Assert.AreEqual(link.Text, test.GeneralLink.Text);
            Assert.AreEqual(link.Title, test.GeneralLink.Title);
            Assert.AreEqual(link.Url, test.GeneralLink.Url);


            #endregion

            #region Developer Types

            Assert.AreEqual("/sitecore/shell/Themes/Standard/Applications/48x48/about.png", test.Icon);

            Assert.AreEqual(TriState.No, test.TriState);

            #endregion

            #region System Types

            Console.WriteLine("Attachment not tested");

            #endregion
            #endregion

            #region SitecoreInfo

            Assert.AreEqual(getTestItem1.Paths.ContentPath, test.ContentPath);

            Assert.AreEqual(getTestItem1.DisplayName, test.DisplayName);

            Assert.AreEqual(getTestItem1.Paths.FullPath, test.FullPath);

            Assert.AreEqual(getTestItem1.Key, test.Key);

            Console.WriteLine("Not test media URL");

            Assert.AreEqual(getTestItem1.Paths.Path, test.Path);

            Assert.AreEqual(getTestItem1.TemplateID.Guid, test.TemplateId);

            Assert.AreEqual(getTestItem1.TemplateName, test.TemplateName);

            Assert.AreEqual(LinkManager.GetItemUrl(getTestItem1), test.Url);

            Assert.AreEqual(getTestItem1.Version.Number, test.Version);

            #endregion

            #region SitecoreChildren

            Assert.AreEqual(getTestItem1.Children.Count, test.Children.Count());
            Assert.AreEqual(getTestItem1.Children.First().ID.Guid, test.Children.First().Id);

            #endregion

            #region SitecoreParent

            Assert.AreEqual(getTestItem1.ParentID.Guid, test.Parent.Id);

            #endregion

            #region SitecoreQuery

            //we have to use the security disabler because we are outside of an ASP.NET context
            //if you do this without the disabler the role manager throws an exception
            using (new SecurityDisabler())
            {
                Assert.AreEqual(3, test.Query.Count());
                Assert.AreEqual(_query1.ID.Guid, test.Query.First().Id);
                Assert.AreEqual(_query2.ID.Guid, test.Query.Skip(1).First().Id);
            }
            #endregion

        }

        #endregion

        #region Item Test 
        [Test]
        public void SetItem_Test2()
        {
            //Assign 
            //clear all fields
            var clearItem = _db.GetItem("/sitecore/content/Configuration/Fluent/GeneralFluent/SetItemTest2");

            using (new SecurityDisabler())
            {
                clearItem.BeginEdit();
                foreach (Field field in clearItem.Fields)
                {
                    field.Value = "";
                }
                clearItem["GroupedDropList"] = "Test2";
                clearItem["DropList"] = "Test2";
                clearItem.EndEdit();
            }

            GeneralFluentFixtureNS.BasicTemplate test;
            using (new SecurityDisabler())
            {
               test =
                    _sitecore.GetItem<GeneralFluentFixtureNS.BasicTemplate>("/sitecore/content/Configuration/Fluent/GeneralFluent/SetItemTest2");

            }
            //Simple Types

            test.Checkbox = true;
            test.Date = new DateTime(2011, 02, 28);
            test.DateTime = new DateTime(2011, 03, 04, 15, 23, 12);
            test.File = new File() { Id = new Guid("{B89EA3C6-C947-44AF-9AEF-7EF89CEB0A4B}") };
            test.Image = new Image()
            {
                Alt="Test Alt",
                Border = "Test Border",
                Class="Test Class",
                Height=487,
                HSpace=52,
                MediaId = new Guid("{0CF0A6D0-8A2B-479B-AD8F-14938135174A}"),
                VSpace= 32,
                Width = 26
            };
            test.Integer = 659;
            test.Float = 458.7f;
            test.Double = 789.5d;
            test.Decimal = 986.4m;
            test.MultiLineText = "Test MultiLineText";
            test.Number = 986;
            test.Password = "test password";
            test.RichText = "test Rich Text";
            test.SingleLineText = "test single line text";

            //List Types
            test.CheckList = new GeneralFluentFixtureNS.SubClass[]{
                new GeneralFluentFixtureNS.SubClass(){Id = _check1.ID.Guid},
                new GeneralFluentFixtureNS.SubClass(){Id = _check2.ID.Guid},
            };
            test.DropList = GeneralFluentFixtureNS.TestEnum.Test3;
            test.GroupedDropLink = new GeneralFluentFixtureNS.SubClass() { Id = _test3.ID.Guid };
            test.GroupedDropList = GeneralFluentFixtureNS.TestEnum.Test3;
            test.MultiList = new GeneralFluentFixtureNS.SubClass[]{
                new GeneralFluentFixtureNS.SubClass(){Id = _multiList1.ID.Guid},
                new GeneralFluentFixtureNS.SubClass(){Id = _multiList2.ID.Guid},
            };
            test.Treelist = new GeneralFluentFixtureNS.SubClass[]{
                new GeneralFluentFixtureNS.SubClass(){Id = _treeList1.ID.Guid},
                new GeneralFluentFixtureNS.SubClass(){Id = _treeList2.ID.Guid},
            };
            test.TreeListEx = new GeneralFluentFixtureNS.SubClass[]{
                new GeneralFluentFixtureNS.SubClass(){Id = _treeList1.ID.Guid},
            };

            //Link Types 
            test.DropLink = new GeneralFluentFixtureNS.SubClass() { Id = _test3.ID.Guid };
            test.DropTree = new GeneralFluentFixtureNS.SubClass() { Id = _test3.ID.Guid };
            test.GeneralLink = new Link(){
                Type = LinkType.External,
                Anchor="test anchor",
                Class="test class",
                Target="test target",
                Text="test text",
                Title="test title",
                Url="test url"
            };

            //Developer Types
            test.Icon = "test icon";
            test.TriState = TriState.Yes;


            //Act
            using (new SecurityDisabler())
            {
                _sitecore.Save<GeneralFluentFixtureNS.BasicTemplate>(test);
            }
            
            //Assert

            //Simple Types
            Item result = _db.GetItem(clearItem.ID);
            Assert.AreEqual("1", result["Checkbox"]);
            Assert.AreEqual("20110228T000000", result["Date"]);
            Assert.AreEqual("20110304T152312", result["DateTime"]);
          
            var file = new FileField(result.Fields["File"]);
            Assert.AreEqual(new Guid("{B89EA3C6-C947-44AF-9AEF-7EF89CEB0A4B}"), file.MediaID.Guid);
            Assert.AreEqual("/~/media/Files/SimpleTextFile2.ashx", file.Src);
           
            var image = new ImageField(result.Fields["Image"]);
            Assert.AreEqual("Test Alt", image.Alt);
            Assert.AreEqual("Test Border", image.Border);
            Assert.AreEqual("Test Class", image.Class);
            Assert.AreEqual("487", image.Height);
            Assert.AreEqual("52", image.HSpace);
            Assert.AreEqual(new Guid("{0CF0A6D0-8A2B-479B-AD8F-14938135174A}"), image.MediaID.Guid);
            Assert.AreEqual("/~/media/Files/Kitten2.ashx", MediaManager.GetMediaUrl(image.MediaItem));
            Assert.AreEqual("32", image.VSpace);
            Assert.AreEqual("26", image.Width);

            Assert.AreEqual("659", result["Integer"]);
            Assert.AreEqual("458.7", result["Float"]);
            Assert.AreEqual("789.5", result["Double"]);
            Assert.AreEqual("986.4", result["Decimal"]);
            Assert.AreEqual("Test MultiLineText", result["MultiLineText"]);
            Assert.AreEqual("986", result["Number"]);
            Assert.AreEqual("test password", result["Password"]);
            Assert.AreEqual("test Rich Text", result["RichText"]);
            Assert.AreEqual("test single line text", result["SingleLineText"]);
            
            //List Types

            Assert.AreEqual("{68B87391-CFF7-4FF2-848E-90C8FD8336A2}|{154CABCB-9B04-449A-9F6F-8FD2AD0783D9}", result["CheckList"].ToUpper());
            Assert.AreEqual("Test3", result["DropList"]);
            Assert.AreEqual(_test3.ID.Guid.ToString("B").ToUpper(), result["GroupedDropLink"].ToUpper());
            Assert.AreEqual("Test3", result["GroupedDropList"]);
            Assert.AreEqual("{1BE4FAC5-90C2-4A5E-952C-071ECD7DA957}|{6127B3E3-B2EB-4B96-AE6D-51DF0212C0EA}", result["MultiList"].ToUpper());
            Assert.AreEqual("{35E0CE21-2651-46CD-B87D-0B34216D3137}|{F52A1B3B-356E-45A4-B06C-DFAE28B6D78A}", result["Treelist"].ToUpper());
            Assert.AreEqual("{35E0CE21-2651-46CD-B87D-0B34216D3137}", result["TreeListEx"].ToUpper());

            //Linked Types
            Assert.AreEqual(_test3.ID.Guid.ToString("B").ToUpper(), result["DropLink"].ToUpper());
            Assert.AreEqual(_test3.ID.Guid.ToString("B").ToUpper(), result["DropTree"].ToUpper());
            LinkField link = new LinkField(result.Fields["GeneralLink"]);
            Assert.AreEqual("test class", link.Class);
            Assert.AreEqual("test target", link.Target);
            Assert.AreEqual("test text", link.Text);
            Assert.AreEqual("test title", link.Title);
            Assert.AreEqual("test url", link.Url);

            //Developer Type

            Assert.AreEqual("test icon", result["Icon"]);
            Assert.AreEqual("1", result["TriState"]);

        }
        #endregion
    }

    namespace GeneralFluentFixtureNS
    {
        public class BasicTemplate
        {
            #region SitecoreId

            public virtual Guid Id { get; set; }

            #endregion

            #region Fields
            #region Simple Types

            public virtual bool Checkbox { get; set; }
            public virtual DateTime Date { get; set; }
            public virtual DateTime DateTime { get; set; }
            public virtual File File { get; set; }
            public virtual Image Image { get; set; }
            public virtual int Integer { get; set; }
            public virtual float Float { get; set; }
            public virtual double Double { get; set; }
            public virtual decimal Decimal { get; set; }
            public virtual string MultiLineText { get; set; }
            public virtual int Number { get; set; }
            public virtual string Password { get; set; }
            public virtual string RichText { get; set; }
            public virtual string SingleLineText { get; set; }

            #endregion

            #region List Types

            public virtual IEnumerable<SubClass> CheckList { get; set; }
            public virtual TestEnum DropList { get; set; }
            public virtual SubClass GroupedDropLink { get; set; }
            public virtual TestEnum GroupedDropList { get; set; }
            public virtual IEnumerable<SubClass> MultiList { get; set; }
            public virtual IEnumerable<SubClass> Treelist { get; set; }
            public virtual IEnumerable<SubClass> TreeListEx { get; set; }

            #endregion

            #region Link Types

            public virtual SubClass DropLink { get; set; }
            public virtual SubClass DropTree { get; set; }
            public virtual Link GeneralLink { get; set; }

            #endregion

            #region Developer Types

            public virtual string Icon { get; set; }
            public virtual TriState TriState { get; set; }

            #endregion

            #region SystemType

            public virtual System.IO.Stream Attachment { get; set; }

            #endregion

            #endregion

            #region SitecoreInfo

            public virtual string ContentPath { get; set; }
            public virtual string DisplayName { get; set; }
            public virtual string FullPath { get; set; }
            public virtual string Key { get; set; }
            public virtual string MediaUrl { get; set; }
            public virtual string Path { get; set; }
            public virtual Guid TemplateId { get; set; }
            public virtual string TemplateName { get; set; }
            public virtual string Url { get; set; }
            public virtual int Version { get; set; }

            #endregion

            #region SitecoreChildren
            
            public virtual IEnumerable<SubClass> Children { get; set; }

            #endregion
            
            #region SitecoreParent

            public virtual SubClass Parent { get; set; }

            #endregion

            #region SitecoreQuery

            public virtual IEnumerable<SubClass> Query { get; set; }

            #endregion

        }

        public class SubClass{
            
            public virtual Guid Id{get;set;}

        }

        public enum TestEnum
        {
            Test1,
            Test2,
            Test3
        }
    }
}
