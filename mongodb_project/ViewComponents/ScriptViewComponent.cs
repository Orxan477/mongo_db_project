using Microsoft.AspNetCore.Mvc;

namespace mongodb_project.ViewComponents
{
    public class ScriptViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
