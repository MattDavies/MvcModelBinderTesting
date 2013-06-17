using System.Threading.Tasks;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using ModelBinderTesting.Controllers.ViewModels;

namespace ModelBinderTesting.Controllers.ModelBinders
{
    [ModelBinderType(typeof(ExampleViewModel))]
    public class ExampleModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var viewModel = new ExampleViewModel();

            var monkey = bindingContext.ValueProvider.GetValue("someNumber");
            if (monkey == null)
            {
                bindingContext.ModelState.AddModelError("TestInteger", "You didn't submit a number.");
            }
            else
            {
                viewModel.TestInteger = (int?) monkey.ConvertTo(typeof (int?));
            }

            bindingContext.ModelMetadata.Model = viewModel;
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}