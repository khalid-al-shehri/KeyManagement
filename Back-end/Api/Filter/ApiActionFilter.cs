using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Application.Common.Shared;

namespace Api.Filter;

public class ApiActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result == null)
            return;

        var resultName = context.Result.GetType().Name;
        if (resultName.Equals("FileContentResult") || resultName.Contains("StatusCodeResult"))
            return;

        if (resultName.Equals("FileStreamResult"))
            return;

        var objResult = ((ObjectResult)context.Result).Value;
        var objType = objResult.GetType();

        if (objType.FullName.Contains("Common.Shared.Result"))
        {
            var metaData = (MetaData)objType.GetProperty("MetaData").GetValue(objResult, null);    
            var status = metaData.StatusCode;

            if (status == HttpStatusCode.OK)
            {
                context.Result = new ObjectResult(objResult);
            }
            else if (status == HttpStatusCode.BadRequest)
            {
                context.Result = new BadRequestObjectResult(objResult);
            }
            else if (status == HttpStatusCode.NotFound)
            {
                context.Result = new NotFoundObjectResult(objResult);
            }
            else if (status == HttpStatusCode.InternalServerError)
            {
                context.Result = new ObjectResult(objResult);
            }
            else if (status == HttpStatusCode.Unauthorized)
            {
                context.Result = new UnauthorizedObjectResult(objResult);
            }
        }

    }
}
