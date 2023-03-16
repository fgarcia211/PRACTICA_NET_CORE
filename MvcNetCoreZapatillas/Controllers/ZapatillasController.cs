using Microsoft.AspNetCore.Mvc;
using MvcNetCoreZapatillas.Models;
using MvcNetCoreZapatillas.Repositories;

namespace MvcNetCoreZapatillas.Controllers
{
    public class ZapatillasController : Controller
    {
        private RepositoryZapatillas repo;

        public ZapatillasController(RepositoryZapatillas repo)
        {
            this.repo = repo;
        }

        public IActionResult VistaZapatillas()
        {
            return View(this.repo.GetAllZapatillas());
        }

        public IActionResult DetallesZapatilla(int idproducto)
        {
            return View(this.repo.GetOneZapatilla(idproducto));
        }

        public async Task<IActionResult> _PaginadoPartial(int idproducto, int? posicion)
        {
            if (posicion == null || posicion < 1)
            {
                posicion = 1;
            }

            int numeroimagenes = this.repo.CountImagenesZapatilla(idproducto);

            if (posicion > numeroimagenes)
            {
                posicion = numeroimagenes;
            }

            ImagenZapatilla imagen = this.repo.GetImagenZapatilla(idproducto, posicion.Value);

            ViewData["REGISTROS"] = numeroimagenes;
            ViewData["POSICION"] = posicion;

            return PartialView("_PaginadoPartial", imagen);
        }
    }
}
