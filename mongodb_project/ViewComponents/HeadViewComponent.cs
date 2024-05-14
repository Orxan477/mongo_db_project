using Microsoft.AspNetCore.Mvc;

namespace mongodb_project.ViewComponents
{
    public class HeadViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
