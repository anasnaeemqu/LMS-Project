using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;

namespace mvcLab.ErrorHandler
{
    public class AddHeaderResultFilter:ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add("TKey","this is a key from filter");
            base.OnResultExecuting(context);
        }
    }
}
