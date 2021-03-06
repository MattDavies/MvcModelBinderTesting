﻿using System.Collections.Specialized;
using System.Web.Mvc;
using ModelBinderTesting.Controllers.ViewModels;
using NUnit.Framework;

namespace ModelBinderTesting.Tests.Controllers.ModelBinders
{
    [TestFixture]
    public class DefaultModelBinderTests : ModelBinderTestBase<DefaultModelBinder, ExampleViewModel>
    {
        [Test]
        public void Have_int_set_to_input()
        {
            SetFormValues(new NameValueCollection { { "TestInteger", "3" } });

            var vm = BindModel();

            Assert.That(vm.TestInteger, Is.EqualTo(3));
        }
        
        [Test]
        public void Not_bind_if_no_integer_submitted()
        {
            var vm = BindModel();

            Assert.That(vm.TestInteger, Is.Null);
            AssertModelError("TestInteger", "The TestInteger field is required.");
        }
    }
}