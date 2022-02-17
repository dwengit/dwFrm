using Dw.Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dw.Framework.Web.Infrastructure.Common.Filters
{
    public class ValidateParameterFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;
            foreach (var item in context.ModelState.Values)
            {
                foreach (var error in item.Errors)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.StatusCode = (int)ResultCodeEnums.CODE400;
                    context.Result = new JsonResult(new Respbase(error.ErrorMessage, (int)ResultCodeEnums.CODE400));
                    break;
                }
            }
        }
    }
}
