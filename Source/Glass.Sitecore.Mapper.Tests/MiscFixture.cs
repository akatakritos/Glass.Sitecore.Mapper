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
using System.Text;
using NUnit.Framework;
using Sitecore.Data;
using Glass.Sitecore.Mapper.Data;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Glass.Sitecore.Mapper.FieldTypes;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Data.Fields;
using Sitecore.Resources.Media;
using Sitecore.SecurityModel;
using Glass.Sitecore.Mapper.Configuration;
using Sitecore.Data.Managers;
using Sitecore.Globalization;

namespace Glass.Sitecore.Mapper.Tests
{
    [TestFixture]
    public class MiscFixture
    {
        

        SitecoreService _sitecore;
        Context _context;
        Database _db;
        Item _test1;
        Item _test2;
        Item _test3;
        Item _demo;

        [SetUp]
        public void Setup()
        {
            AttributeConfigurationLoader loader = new AttributeConfigurationLoader(
               new string[] { "Glass.Sitecore.Mapper.Tests.MiscFixtureNS, Glass.Sitecore.Mapper.Tests" }
               );

            _context = new Context(loader, new AbstractSitecoreDataHandler[] {});
            global::Sitecore.Context.Site = global::Sitecore.Configuration.Factory.GetSite("website");

            _sitecore = new SitecoreService("master");
            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");

            _test1 = _db.GetItem("/sitecore/content/Glass/Test1");
            _test2 = _db.GetItem("/sitecore/content/Glass/Test2");
            _test3 = _db.GetItem("/sitecore/content/Glass/Test1/Test3");
            _demo = _db.GetItem("/sitecore/content/Glass/Demo");
        }

        #region Item Test 
        [Test]
        public void SetItem_Test2()
        {
            //Assign 
            //clear all fields
            MiscFixtureNS.BasicTemplate test = null;
            using (new SecurityDisabler())
            {
                _test2.BeginEdit();
                foreach (Field field in _test2.Fields)
                {
                    field.Value = "";
                }
                _test2["GroupedDropList"] = "Test2";
                _test2["DropList"] = "Test2";
                _test2.EndEdit();


                test =
                    _sitecore.GetItem<MiscFixtureNS.BasicTemplate>("/sitecore/content/Glass/Test2");
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
            test.CheckList = new MiscFixtureNS.SubClass[]{
                new MiscFixtureNS.SubClass(){Id = _test1.ID.Guid},
                new MiscFixtureNS.SubClass(){Id = _test2.ID.Guid},
            };
            test.DropList = MiscFixtureNS.TestEnum.Test3;
            test.GroupedDropLink = new MiscFixtureNS.SubClass() { Id = _test3.ID.Guid };
            test.GroupedDropList = MiscFixtureNS.TestEnum.Test3;
            test.MultiList = new MiscFixtureNS.SubClass[]{
                new MiscFixtureNS.SubClass(){Id = _test1.ID.Guid},
                new MiscFixtureNS.SubClass(){Id = _test2.ID.Guid},
            };
            test.Treelist = new MiscFixtureNS.SubClass[]{
                new MiscFixtureNS.SubClass(){Id = _test1.ID.Guid},
                new MiscFixtureNS.SubClass(){Id = _test2.ID.Guid},
            };
            test.TreeListEx = new MiscFixtureNS.SubClass[]{
                new MiscFixtureNS.SubClass(){Id = _test1.ID.Guid},
                new MiscFixtureNS.SubClass(){Id = _test2.ID.Guid},
            };

            //Link Types 
            test.DropLink = new MiscFixtureNS.SubClass() { Id = _test3.ID.Guid };
            test.DropTree = new MiscFixtureNS.SubClass() { Id = _test3.ID.Guid };
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
                _sitecore.Save<MiscFixtureNS.BasicTemplate>(test);
            }
            
            //Assert

            //Simple Types
            Item result = _db.GetItem(_test2.ID);
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

            Assert.AreEqual("{BD193B3A-D3CA-49B4-BF7A-2A61ED77F19D}|{8A317CBA-81D4-4F9E-9953-64C4084AECCA}", result["CheckList"].ToUpper());
            Assert.AreEqual("Test3", result["DropList"]);
            Assert.AreEqual(_test3.ID.Guid.ToString("B").ToUpper(), result["GroupedDropLink"].ToUpper());
            Assert.AreEqual("Test3", result["GroupedDropList"]);
            Assert.AreEqual("{BD193B3A-D3CA-49B4-BF7A-2A61ED77F19D}|{8A317CBA-81D4-4F9E-9953-64C4084AECCA}", result["MultiList"].ToUpper());
            Assert.AreEqual("{BD193B3A-D3CA-49B4-BF7A-2A61ED77F19D}|{8A317CBA-81D4-4F9E-9953-64C4084AECCA}", result["Treelist"].ToUpper());
            Assert.AreEqual("{BD193B3A-D3CA-49B4-BF7A-2A61ED77F19D}|{8A317CBA-81D4-4F9E-9953-64C4084AECCA}", result["TreeListEx"].ToUpper());

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


        

        /// <summary>
        /// Ensure that the data is the link manager
        /// </summary>
        [Test]
        public void LinkManager_DropLink_UpdatesReferences(){

            using (new SecurityDisabler())
            {
                var parent = _sitecore.GetItem<MiscFixtureNS.SubClass>(_test1.ID.Guid);
                var newItem = _sitecore.Create<MiscFixtureNS.LinkTest, MiscFixtureNS.SubClass>(parent, "DroplinkLinkTest");
                
                Assert.IsNotNull(newItem);

                newItem.Droplink = parent;
                _sitecore.Save<MiscFixtureNS.LinkTest>(newItem);

                string path = newItem.Path;
                Item item = _db.GetItem(path);

                LinkDatabase linkDb = global::Sitecore.Configuration.Factory.GetLinkDatabase();
                int count = linkDb.GetReferenceCount(item);
                Assert.AreEqual(2, count); //the additional one is for the template reference

                try
                {
                    item.Delete();
                }
                catch (NullReferenceException ex)
                {
                    //we need to catch a null reference exception raised by the Sitecore.Tasks.ItemEventHandler.OnItemDeleted
                }
            }
        }

        /// <summary>
        /// Ensure that the data is the link manager
        /// </summary>
        [Test]
        public void LinkManager_Multilist_UpdatesReferences()
        {

            using (new SecurityDisabler())
            {
                var test1Object  = _sitecore.GetItem<MiscFixtureNS.SubClass>(_test1.ID.Guid);
                var test2Object = _sitecore.GetItem<MiscFixtureNS.SubClass>(_demo.ID.Guid);

                var newItem = _sitecore.Create<MiscFixtureNS.LinkTest, MiscFixtureNS.SubClass>(test1Object, "MultilistLinkTest");

                Assert.IsNotNull(newItem);

                newItem.Multilist = new[] { test1Object, test2Object };
                _sitecore.Save<MiscFixtureNS.LinkTest>(newItem);

                string path = newItem.Path;
                Item item = _db.GetItem(path);

                LinkDatabase linkDb = global::Sitecore.Configuration.Factory.GetLinkDatabase();
                int count = linkDb.GetReferenceCount(item);
                Assert.AreEqual(3, count); //the additional one is the link to the template

                try
                {
                    item.Delete();
                }
                catch (NullReferenceException ex)
                {
                    //we need to catch a null reference exception raised by the Sitecore.Tasks.ItemEventHandler.OnItemDeleted
                }
            }
        }

        [Test]
        public void IList_SetListOfIntsAndClasses()
        {
            //Assign
            MiscFixtureNS.IListTest target = _sitecore.GetItem<MiscFixtureNS.IListTest>("/sitecore/content/Glass/IListTest");


            Assert.AreEqual(0, target.ListOfClasses.Count);
            Assert.AreEqual(0, target.ListOfInts.Count);


            MiscFixtureNS.LinkTest link1 = _sitecore.GetItem<MiscFixtureNS.LinkTest>("/sitecore/content/Glass/Test1");
            MiscFixtureNS.LinkTest link2 = _sitecore.GetItem<MiscFixtureNS.LinkTest>("/sitecore/content/Glass/Test2");

            //Act

            target.ListOfInts.Add(45);
            target.ListOfInts.Add(67);
            string intString = "45|67";
            
            target.ListOfClasses.Add(link1);
            target.ListOfClasses.Add(link2);
            string classString = "{BD193B3A-D3CA-49B4-BF7A-2A61ED77F19D}|{8A317CBA-81D4-4F9E-9953-64C4084AECCA}";
            
            using (new SecurityDisabler())
            {
             
                _sitecore.Save<MiscFixtureNS.IListTest>(target);

                //Assert
                Item result = _db.GetItem("/sitecore/content/Glass/IListTest");
                Assert.AreEqual(classString, result["ListOfClasses"]);
                Assert.AreEqual(intString, result["ListOfInts"]);

                result.Editing.BeginEdit();
                result["ListOfClasses"] = "";
                result["ListOfInts"] = "";
                result.Editing.EndEdit();
            }



        }

        [Test]
        public void AddingAVersionToAParticularLanguage()
        {
                       //Assign
            MiscFixtureNS.LanguageTest target = _sitecore.GetItem<MiscFixtureNS.LanguageTest>(
                "/sitecore/content/Glass/Language", LanguageManager.GetLanguage("af-ZA"));

            //Act
            using (new SecurityDisabler())
            {
                MiscFixtureNS.LanguageTest newVersion = _sitecore.AddVersion<MiscFixtureNS.LanguageTest>(target);

                //Assert
                Assert.AreEqual(target.Version + 1, newVersion.Version);
            }
        }

        #region AccessModifiersTest

        [Test]
        public void AccessModifiers_AllPropertiesFilled()
        {
            //Assign

            MiscFixtureNS.AccessModifiersTest target = null;
            Guid id = new Guid("{6FEE384F-3A05-4520-A80C-F80A6A454608}");

            //Act
            target = _sitecore.GetItem<MiscFixtureNS.AccessModifiersTest>(id);

            //Assert
            Assert.AreEqual("Some test content", target.PublicSingleLineText);
            Assert.AreEqual("Some test content", target.GetProtectedSingleLineText);            
            Assert.AreEqual("Some test content", target.GetPrivateSingleLineText);

        }

        [Test]
        public void AccessModifiers_PrivatePropertiesWriteToField()
        {
            //Assign

            MiscFixtureNS.AccessModifiersPrivate target = null;
            Guid id = new Guid("{6FEE384F-3A05-4520-A80C-F80A6A454608}");

            Item item = _db.GetItem(new ID(id));

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                item["SingleLineText"] = string.Empty;

                target = new MiscFixtureNS.AccessModifiersPrivate();
                target.GetPrivateSingleLineText = "private test";

                _sitecore.WriteToItem(target, item);

                Assert.AreEqual("private test", item["SingleLineText"]);

                item.Editing.CancelEdit();
            }
        }

        [Test]
        public void AccessModifiers_ProtectedPropertiesWriteToField()
        {
            //Assign

            MiscFixtureNS.AccessModifiersProtected target = null;
            Guid id = new Guid("{6FEE384F-3A05-4520-A80C-F80A6A454608}");

            Item item = _db.GetItem(new ID(id));

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                item["SingleLineText"] = string.Empty;

                target = new MiscFixtureNS.AccessModifiersProtected();
                target.GetProtectedSingleLineText = "Protected test";

                _sitecore.WriteToItem(target, item);

                Assert.AreEqual("Protected test", item["SingleLineText"]);

                item.Editing.CancelEdit();
            }
        }

        [Test]
        public void AccessModifiers_PublicPrivatePropertiesWriteAndReadsField()
        {
            //Assign

            MiscFixtureNS.AccessModifiersPublicPrivate target = null;
            Guid id = new Guid("{6FEE384F-3A05-4520-A80C-F80A6A454608}");

            Item item = _db.GetItem(new ID(id));

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                item["SingleLineText"] = "pre value";

                target = _sitecore.CreateClass<MiscFixtureNS.AccessModifiersPublicPrivate>(false, false, item);

                Assert.AreEqual("pre value", target.SingleLineText);

                target.GetPrivateSingleLineText = "Protected test";

                _sitecore.WriteToItem(target, item);

                Assert.AreEqual("Protected test", item["SingleLineText"]);

                item.Editing.CancelEdit();
            }
        }

        [Test]
        public void AccessModifiers_PublicPrivatePropertiesWriteAndReadsFieldOnInheritedMember()
        {
            //Assign

            MiscFixtureNS.AccessModifiersPublicPrivateInherited target = null;
            Guid id = new Guid("{6FEE384F-3A05-4520-A80C-F80A6A454608}");

            Item item = _db.GetItem(new ID(id));

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                item["SingleLineText"] = "pre value";

                target = _sitecore.CreateClass<MiscFixtureNS.AccessModifiersPublicPrivateInherited>(false, false, item);

                Assert.AreEqual("pre value", target.SingleLineText);

                target.GetPrivateSingleLineText = "Protected test";

                _sitecore.WriteToItem(target, item);

                Assert.AreEqual("Protected test", item["SingleLineText"]);

                item.Editing.CancelEdit();
            }
        }

        #endregion

    }

    namespace MiscFixtureNS
    {

        [SitecoreClass]
        public class AccessModifiersPublicPrivateInherited : AccessModifiersPublicPrivate
        {
        }
       
        [SitecoreClass]
        public class AccessModifiersPublicPrivate
        {
            [SitecoreField("SingleLineText")]
            public string SingleLineText { get; private set; }


            public string GetPrivateSingleLineText { get { return SingleLineText; } set { SingleLineText = value; } }
        }


        [SitecoreClass]
        public class AccessModifiersPrivate
        {
            [SitecoreField("SingleLineText")]
            private string PrivateSingleLineText { get; set; }


            public string GetPrivateSingleLineText { get { return PrivateSingleLineText; } set { PrivateSingleLineText = value; } }
        }

        [SitecoreClass]
        public class AccessModifiersProtected
        {
            [SitecoreField("SingleLineText")]
            private string ProtectedSingleLineText { get; set; }


            public string GetProtectedSingleLineText { get { return ProtectedSingleLineText; } set { ProtectedSingleLineText = value; } }
        }

        [SitecoreClass]
        public class AccessModifiersTest
        {
            [SitecoreField("SingleLineText")]
            public virtual string PublicSingleLineText { get; set; }

            [SitecoreField("SingleLineText")]
            private string PrivateSingleLineText { get; set; }

            [SitecoreField("SingleLineText")]
            protected virtual string ProtectedSingleLineText { get; set; }

            public string GetPrivateSingleLineText { get { return PrivateSingleLineText; } }
            public string GetProtectedSingleLineText { get { return ProtectedSingleLineText; } }
        }

        [SitecoreClass]
        public class LanguageTest{
            
            [SitecoreId]
            public virtual Guid Id { get; set; }
            
            [SitecoreInfo(SitecoreInfoType.Version)]
            public virtual int Version { get; set; }

            [SitecoreInfo(SitecoreInfoType.Language)]
            public virtual Language Language { get; set; }
        }
        

        [SitecoreClass(TemplateId="{1D0EE1F5-21E0-4C5B-8095-EDE2AF3D3300}")]
        public class BasicTemplate
        {
            #region SitecoreId

            [SitecoreId]
            public virtual Guid Id { get; set; }

            #endregion

            #region Fields
            #region Simple Types

            [SitecoreField]
            public virtual bool Checkbox { get; set; }
            [SitecoreField]
            public virtual DateTime Date { get; set; }
            [SitecoreField]
            public virtual DateTime DateTime { get; set; }
            [SitecoreField]
            public virtual File File { get; set; }
            [SitecoreField]
            public virtual Image Image { get; set; }
            [SitecoreField]
            public virtual int Integer { get; set; }
            [SitecoreField]
            public virtual float Float { get; set; }
            [SitecoreField]
            public virtual double Double { get; set; }
            [SitecoreField]
            public virtual decimal Decimal { get; set; }

            [SitecoreField]
            public virtual string MultiLineText { get; set; }
            [SitecoreField]
            public virtual int Number { get; set; }
            [SitecoreField]
            public virtual string Password { get; set; }
            [SitecoreField(Setting=SitecoreFieldSettings.RichTextRaw)]
            public virtual string RichText { get; set; }
            [SitecoreField]
            public virtual string SingleLineText { get; set; }

            #endregion

            #region List Types

            [SitecoreField]
            public virtual IEnumerable<SubClass> CheckList { get; set; }
            [SitecoreField]
            public virtual TestEnum DropList { get; set; }
            [SitecoreField]
            public virtual SubClass GroupedDropLink { get; set; }
            [SitecoreField]
            public virtual TestEnum GroupedDropList { get; set; }
            [SitecoreField]
            public virtual IEnumerable<SubClass> MultiList { get; set; }
            [SitecoreField]
            public virtual IEnumerable<SubClass> Treelist { get; set; }
            [SitecoreField]
            public virtual IEnumerable<SubClass> TreeListEx { get; set; }

            #endregion

            #region Link Types

            [SitecoreField]
            public virtual SubClass DropLink { get; set; }
            [SitecoreField]
            public virtual SubClass DropTree { get; set; }
            [SitecoreField]
            public virtual Link GeneralLink { get; set; }

            #endregion

            #region Developer Types

            [SitecoreField]
            public virtual string Icon { get; set; }
            
            [SitecoreField]
            public virtual TriState TriState { get; set; }

            #endregion

            #region SystemType

            [SitecoreField]
            public virtual System.IO.Stream Attachment { get; set; }

            #endregion

            #endregion

            #region SitecoreInfo

            [SitecoreInfo(SitecoreInfoType.ContentPath)]
            public virtual string ContentPath { get; set; }
            [SitecoreInfo(SitecoreInfoType.DisplayName)]
            public virtual string DisplayName { get; set; }
            [SitecoreInfo(SitecoreInfoType.FullPath)]
            public virtual string FullPath { get; set; }
            [SitecoreInfo(SitecoreInfoType.Key)]
            public virtual string Key { get; set; }
            [SitecoreInfo(SitecoreInfoType.MediaUrl)]
            public virtual string MediaUrl { get; set; }
            [SitecoreInfo(SitecoreInfoType.Path)]
            public virtual string Path { get; set; }
            [SitecoreInfo(SitecoreInfoType.TemplateId)]
            public virtual Guid TemplateId { get; set; }
            [SitecoreInfo(SitecoreInfoType.TemplateName)]
            public virtual string TemplateName { get; set; }
            [SitecoreInfo(SitecoreInfoType.Url)]
            public virtual string Url { get; set; }
            [SitecoreInfo(SitecoreInfoType.Version)]
            public virtual int Version { get; set; }

            #endregion

            #region SitecoreChildren
            
            [SitecoreChildren]
            public virtual IEnumerable<SubClass> Children { get; set; }

            #endregion
            
            #region SitecoreParent

            [SitecoreParent]
            public virtual SubClass Parent { get; set; }

            #endregion

            #region SitecoreQuery

            [SitecoreQuery("/sitecore/content/Glass/*[@@TemplateName='BasicTemplate']")]
            public virtual IEnumerable<SubClass> Query { get; set; }

            #endregion

        }

        [SitecoreClass]
        public class SubClass{
            
            [SitecoreId]
            public virtual Guid Id{get;set;}

        }

        [SitecoreClass(TemplateId="{C867D02D-103C-404A-B008-4A3E6B8B6F51}")]
        public class LinkTest
        {
            [SitecoreId]
            public virtual Guid Id { get; set; }

            [SitecoreField]
            public virtual SubClass Droplink { get; set; }

            [SitecoreField]
            public virtual IEnumerable<SubClass> Multilist { get; set; }

            [SitecoreInfo(SitecoreInfoType.Path)]
            public virtual string Path { get; set; }

        }

        [SitecoreClass]
        public interface IListTest
        {
            [SitecoreId]
            Guid Id{get;set;}

            [SitecoreField]
            IList<int> ListOfInts { get; set; }

            [SitecoreField]
            IList<LinkTest> ListOfClasses { get; set; }
        }

        public enum TestEnum
        {
            Test1,
            Test2,
            Test3
        }

        
    
    
    }
}
