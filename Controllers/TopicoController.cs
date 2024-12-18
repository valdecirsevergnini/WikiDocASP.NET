using Microsoft.AspNetCore.Mvc;
using WikiSistemaASP.NET.Models;
using WikiSistemaASP.NET.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WikiSistemaASP.NET.Controllers
{
    public class TopicoController : Controller
    {
        private readonly WikiDbContext _context;

        public TopicoController(WikiDbContext context)
        {
            _context = context;
        }

        private void PopulateModulos()
        {
            ViewBag.ModuloId = new SelectList(_context.Modulos, "Id", "Nome");
            ViewBag.Modulos = _context.Modulos.ToList();
        }

        // GET: /Topico/
        public IActionResult Index()
        {
            PopulateModulos(); // Chama o método para preencher os módulos
            var topicos = _context.Topicos.Include(t => t.Modulo).ToList();
            return View(topicos);
        }

        // GET: /Topico/Details/5
        public IActionResult Details(int id)
        {
            var topico = _context.Topicos.Include(t => t.Modulo).FirstOrDefault(t => t.Id == id);
            if (topico == null)
            {
                return NotFound();
            }
            ViewBag.Modulos = _context.Modulos.ToList();
            return View(topico);
        }

        // GET: /Topico/Create
        public IActionResult Create()
        {
            PopulateModulos(); // Preenche os módulos
            return View();
        }

        // POST: /Topico/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Titulo,Conteudo,ModuloId")] Topico topico)
        {
            if (ModelState.IsValid)
            {
                _context.Add(topico);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            PopulateModulos(); // Preenche os módulos em caso de erro
            return View(topico);
        }

        // GET: /Topico/Edit/5
        public IActionResult Edit(int id)
        {
            var topico = _context.Topicos.Include(t => t.Modulo).FirstOrDefault(t => t.Id == id);
            if (topico == null)
            {
                return NotFound();
            }
            PopulateModulos(); // Preenche os módulos
            return View(topico);
        }

        // POST: /Topico/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Titulo,Conteudo,ModuloId")] Topico topico)
        {
            if (id != topico.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topico);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Topicos.Any(e => e.Id == topico.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateModulos(); // Preenche os módulos em caso de erro
            return View(topico);
        }

        // GET: /Topico/Delete/5
        public IActionResult Delete(int id)
        {
            var topico = _context.Topicos.Find(id);
            if (topico == null)
            {
                return NotFound();
            }
            ViewBag.Modulos = _context.Modulos.ToList();
            return View(topico);
        }

        // POST: /Topico/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var topico = _context.Topicos.Find(id);
            if (topico != null)
            {
                _context.Topicos.Remove(topico);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /Topico/GetByModuloId/5
        [HttpGet]
        public IActionResult GetByModuloId(int id)
        {
            var topicos = _context.Topicos
                .Where(t => t.ModuloId == id)
                .ToList();

            return PartialView("_TopicosPartial", topicos);
        }
    }
}
