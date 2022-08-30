using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Stackjudge_Identity_Server.Controllers;

public class HomeController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}