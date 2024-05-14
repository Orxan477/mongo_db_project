using Microsoft.AspNetCore.Mvc;

namespace mongodb_project.ViewComponents
{
    public class AsideViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
