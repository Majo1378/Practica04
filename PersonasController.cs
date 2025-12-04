
using Microsoft.AspNetCore.Mvc;
using EF_core_master.Models;
using System.Linq;

namespace EF_core_master.Controllers
{
    public class PersonasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Buscar(string nombre, int? edad)
        {
            var query = _context.Personas.AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(p => p.Nombre.Contains(nombre));

            if (edad.HasValue)
                query = query.Where(p => p.Edad == edad);

            return View(query.ToList());
        }

        public IActionResult Delete(int id)
        {
            var persona = _context.Personas.Find(id);
            if (persona == null) return NotFound();

            _context.Personas.Remove(persona);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var persona = _context.Personas.Find(id);
            if (persona == null) return NotFound();
            return View(persona);
        }

        [HttpPost]
        public IActionResult Edit(Persona persona)
        {
            if (ModelState.IsValid)
            {
                _context.Personas.Update(persona);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(persona);
        }

        public IActionResult OrdenadasPorNombre()
        {
            var lista = _context.Personas.OrderBy(p => p.Nombre).ToList();
            return View(lista);
        }

        public IActionResult PromedioEdad()
        {
            ViewBag.Promedio = _context.Personas.Average(p => p.Edad);
            return View();
        }

        public IActionResult BuscarPorId(int id)
        {
            var persona = _context.Personas.FirstOrDefault(p => p.Id == id);
            if (persona == null) return NotFound();
            return View(persona);
        }
    }
}
