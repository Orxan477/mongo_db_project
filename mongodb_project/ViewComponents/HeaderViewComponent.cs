using Microsoft.AspNetCore.Mvc;

namespace mongodb_project.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
