using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ExtraDry.Server {

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ExceptionStatusCodeAttribute : ExceptionFilterAttribute {

        public ExceptionStatusCodeAttribute(Type exceptionType, HttpStatusCode code)
        {
            matchingExceptionType = exceptionType;
            returnCode = code;
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            return base.OnExceptionAsync(context);
        }

        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            if(context.Exception.GetType().Name == matchingExceptionType.Name) {
                var response = context.HttpContext.Response;
                ExceptionResponse.RewriteResponse(response, returnCode, returnCode.ToString(), context.Exception.Message);
                context.ExceptionHandled = true;
            }
        }

        private readonly HttpStatusCode returnCode;

        private readonly Type matchingExceptionType;

    }

}
