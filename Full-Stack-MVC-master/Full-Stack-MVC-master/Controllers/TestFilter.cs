using Microsoft.AspNetCore.Mvc;
using mvcLab.ErrorHandler;

namespace mvcLab.Controllers
{
    public class TestFilter : Controller
    {
        //[CustomHandlerFilter]
        public IActionResult Index()
        {
            ContentResult res = new ContentResult();
            res.Content = "action to try custom filter";

            throw new Exception();
        }
    }
}
