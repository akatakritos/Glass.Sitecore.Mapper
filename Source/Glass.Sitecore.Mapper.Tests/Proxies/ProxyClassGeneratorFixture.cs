﻿/*
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
using Glass.Sitecore.Mapper.Configuration;
using Glass.Sitecore.Mapper.Data;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Sitecore.Data.Items;
using Glass.Sitecore.Mapper.Proxies;
using System.Reflection;

namespace Glass.Sitecore.Mapper.Tests.Proxies
{
    [TestFixture]
    public class ProxyClassGeneratorFixture
    {
        Context _context;
        Database _db;
        Guid _itemId;
        ISitecoreService _service;

        [SetUp]
        public void Setup()
        {


            _context = new Context(
                new AttributeConfigurationLoader("Glass.Sitecore.Mapper.Tests.Proxies.ProxyClassGeneratorFixtureNS, Glass.Sitecore.Mapper.Tests"));

            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");

            _itemId = new Guid("{8A317CBA-81D4-4F9E-9953-64C4084AECCA}");
            _service = new SitecoreService(_db);
        }

        [Test]
        public void Create_CreatesProxyClassAndCorrectlyReplaces()
        {
            //Assign 
            Item item = _db.GetItem(new ID(_itemId));
            
            InstanceContext instContext = Context.GetContext();

            SitecoreClassConfig config = instContext.GetSitecoreClass(typeof(ProxyClassGeneratorFixtureNS.SubClass));
            
            Context.CreateConstructorDelegates(config);
            
            //Act
            var result = ProxyGenerator.CreateProxy(config, _service, item, false) as ProxyClassGeneratorFixtureNS.SubClass;
            result.CallMe = "something";

            //Assert 


            Assert.AreEqual("something",result.CallMe);
            


        }
    }

    namespace ProxyClassGeneratorFixtureNS
    {
        [SitecoreClass]
        public class TestClass {
            public SubClass Class { get; set; }
        }
        [SitecoreClass]
        public class SubClass {
            private string _callMe;
            public virtual string CallMe
            {
                get
                {
                    return _callMe;
                }
                set
                {
                    _callMe = value;
                }
            }
        }
    }
}
