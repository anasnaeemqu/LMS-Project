using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using mvcLab.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace mvcLab.ErrorHandler
{
    public class DeptActionFilter : ActionFilterAttribute
    {
        Stopwatch sc = new Stopwatch();
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            sc.Start();
            var dept = context.ActionArguments["dept"] as Department;
            if (dept == null ||(dept.Location != "Fayoum" && dept.Location != "Smart"))
            {
                throw new ValidationException("Department Location must be Fayoum or Smart");
            }
            Console.WriteLine($"error in {context.ActionDescriptor.DisplayName} ");
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            sc.Stop();
            Console.WriteLine($"result to try filter {context.Result.ToString()},time {sc.ElapsedMilliseconds}ms ");
        }
    }
}
