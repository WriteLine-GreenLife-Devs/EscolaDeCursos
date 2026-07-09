using Microsoft.AspNetCore.Mvc;

namespace EscolaDeCursosWebApp.Compartilhado.Apresentacao;

public class HomeController : Controller
{
    [HttpGet]
    public ActionResult Index()
    {
        return View();
    }
}
