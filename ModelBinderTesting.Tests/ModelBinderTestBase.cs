using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using AutofacContrib.NSubstitute;
using NSubstitute;
using NUnit.Framework;
using Subtext.TestLibrary;

namespace ModelBinderTesting.Tests
{
    public abstract class ModelBinderTestBase<TBinder, TModel> where TBinder : DefaultModelBinder
    {
        #region Setup base + private methods

        private ControllerContext _context;
        private ModelBindingContext _bindingContext;
        protected AutoSubstitute AutoSubstitute;
        private NameValueCollection _formCollection;
        private TBinder _binder;

        [SetUp]
        protected void Setup()
        {
            AutoSubstitute = new AutoSubstitute();
            var httpContext = Substitute.For<HttpContextBase>();
            var request = Substitute.For<HttpRequestBase>();
            httpContext.Request.Returns(request);
            var controllerContext = Substitute.For<ControllerContext>();
            controllerContext.HttpContext = httpContext;
            AutoSubstitute.Provide(request);
            AutoSubstitute.Provide(httpContext);
            AutoSubstitute.Provide(controllerContext);

            _context = AutoSubstitute.Resolve<ControllerContext>();
            _binder = AutoSubstitute.Resolve<TBinder>();

            SetFormValues(new NameValueCollection());
        }

        private void SetupBindingContext()
        {
            _formCollection = _context.HttpContext.Request.Form;
            var valueProvider = new NameValueCollectionValueProvider(_formCollection, null);
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(TModel));
            _bindingContext = new ModelBindingContext
            {
                ModelName = typeof(TModel).Name,
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata,
                FallbackToEmptyPrefix = true
            };
        }

        #endregion

        [Test]
        public virtual void Bind_to_correct_model()
        {
            if (typeof(TBinder) == typeof(DefaultModelBinder))
                return;

            var attr = (ModelBinderTypeAttribute)Attribute.GetCustomAttribute(typeof(TBinder), typeof(ModelBinderTypeAttribute));
            Assert.That(attr, Is.Not.Null, "The ModelBinderType attribute is missing");
            Assert.That(attr.TargetTypes, Is.Not.Null, "No bindings are defined");
            Assert.That(attr.TargetTypes.ToList()[0], Is.EqualTo(typeof(TModel)), String.Format("The binding is incorrect; it should be {0}", typeof(TModel).FullName));
        }

        protected void SetFormValues(NameValueCollection formValues)
        {
            _context.HttpContext.Request.Form.Returns(formValues);
        }

        protected TModel BindModel()
        {
            SetupBindingContext();
            return (TModel) _binder.BindModel(_context, _bindingContext);
        }

        protected void AssertModelError(string key, string error)
        {
            Assert.That(_bindingContext.ModelState.ContainsKey(key), key + " not present in model state");
            Assert.That(_bindingContext.ModelState[key].Errors.Count, Is.EqualTo(1), "Expecting an error against " + key);
            Assert.That(_bindingContext.ModelState[key].Errors[0].ErrorMessage, Is.EqualTo(error), "Expecting different error message for model state against " + key);
        }
    }
}
