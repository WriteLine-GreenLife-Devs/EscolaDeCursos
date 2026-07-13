using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EscolaDeCursosWebApp.Modulos.ModuloADM.Apresentacao;

[Authorize(Roles = "ADM")]
[Route("ModuloADM/Apresentacao/[action]")]
public class ADMController : Controller
{
    [HttpGet]
    public ActionResult Index()
    {
        return View();
    }
}
