using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCNBlog.Infrastructure.Mappers;
using MVCNBlog.ViewModels;
using MVCNBlog.ViewModels.Roles;

namespace MVCNBlog.Infrastructure.ModelBinders
{
    public class AccountModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            if (propertyDescriptor.Name == nameof(AccountViewModel.CreationDate))
            {
                propertyDescriptor.SetValue(bindingContext.Model, DateTime.Today);
                return;
            }

            if (propertyDescriptor.Name == nameof(AccountViewModel.Role))
            {
                int roleId = FromPostedData<int>(bindingContext, "roleId");
                var role = roleId.ToMvcRole();

                propertyDescriptor.SetValue(bindingContext.Model, role);
                return;
            }

            if (propertyDescriptor.Name == nameof(AccountViewModel.PayedRole))
            {
                string isVipStr = request.Form.Get("isVip");
                bool isVip = isVipStr == "on";
                if (isVip)
                {
                    IPayedRole payedRole = new VipUserRole();

                    propertyDescriptor.SetValue(bindingContext.Model, payedRole);
                }

                return;
            }

            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }

        private T FromPostedData<T>(ModelBindingContext bindingContext, string prefix)
        {
            var valueProvider = bindingContext.ValueProvider;
            var valueProviderResult = valueProvider.GetValue(prefix);
            if(valueProviderResult != null)
                return (T)valueProviderResult.ConvertTo(typeof(T));
            return default(T);
        }
    }
}