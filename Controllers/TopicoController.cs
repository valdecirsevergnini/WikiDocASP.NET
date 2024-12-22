using Microsoft.AspNetCore.Mvc;
using WikiSistemaASP.NET.Models;
using WikiSistemaASP.NET.Data;
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

        // Método auxiliar para preencher a ViewBag com os módulos disponíveis
        private void PopulateModulos()
        {
            ViewBag.ModuloId = new SelectList(_context.Modulos.OrderBy(m => m.Nome), "Id", "Nome");
        }

        // GET: /Topico/
        public IActionResult Index()
        {
            var topicos = _context.Topicos.Include(t => t.Modulo).ToList();
            return View("~/Views/Topico/Index.cshtml", topicos);
        }

        // GET: /Topico/Details/5
        public IActionResult Details(int id)
        {
            var topico = _context.Topicos.Include(t => t.Modulo).FirstOrDefault(t => t.Id == id);
            if (topico == null)
            {
                TempData["MensagemErro"] = "Tópico não encontrado.";
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Topico/Details.cshtml", topico);
        }

        // GET: /Topico/Create
        public IActionResult Create()
        {
            PopulateModulos();
            return View("~/Views/Topico/Create.cshtml");
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
                TempData["MensagemSucesso"] = "Tópico criado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            TempData["MensagemErro"] = "Erro ao criar o tópico. Verifique os dados fornecidos.";
            PopulateModulos();
            return View("~/Views/Topico/Create.cshtml", topico);
        }

        // GET: /Topico/Edit/5
        public IActionResult Edit(int id)
        {
            var topico = _context.Topicos.Include(t => t.Modulo).FirstOrDefault(t => t.Id == id);
            if (topico == null)
            {
                TempData["MensagemErro"] = "Tópico não encontrado.";
                return RedirectToAction(nameof(Index));
            }
            PopulateModulos();
            return View("~/Views/Topico/Edit.cshtml", topico);
        }

        // POST: /Topico/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Titulo,Conteudo,ModuloId")] Topico topico)
        {
            if (id != topico.Id)
            {
                TempData["MensagemErro"] = "ID do tópico inválido.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topico);
                    _context.SaveChanges();
                    TempData["MensagemSucesso"] = "Tópico editado com sucesso.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Topicos.Any(e => e.Id == topico.Id))
                    {
                        TempData["MensagemErro"] = "Tópico não encontrado.";
                        return RedirectToAction(nameof(Index));
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["MensagemErro"] = "Erro ao editar o tópico. Verifique os dados fornecidos.";
            PopulateModulos();
            return View("~/Views/Topico/Edit.cshtml", topico);
        }

        // GET: /Topico/Delete/5
        public IActionResult Delete(int id)
        {
            var topico = _context.Topicos.Include(t => t.Modulo).FirstOrDefault(t => t.Id == id);
            if (topico == null)
            {
                TempData["MensagemErro"] = "Tópico não encontrado.";
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Topico/Delete.cshtml", topico);
        }

        // POST: /Topico/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var topico = _context.Topicos.Include(t => t.Modulo).FirstOrDefault(t => t.Id == id);
            if (topico == null)
            {
                TempData["MensagemErro"] = "Erro ao excluir o tópico. Tópico não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Topicos.Remove(topico);
                _context.SaveChanges();
                TempData["MensagemSucesso"] = "Tópico excluído com sucesso.";
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao excluir o tópico: {ex.Message}";
            }

            return RedirectToAction("Edit", "Modulo", new { id = topico.ModuloId });
        }

        // GET: /Topico/GetByModuloId/5
        [HttpGet]
        public IActionResult GetByModuloId(int id)
        {
            var topicos = _context.Topicos
                .Where(t => t.ModuloId == id)
                .ToList();

            return PartialView("~/Views/Topico/_TopicosPartial.cshtml", topicos);
        }
    }
}
