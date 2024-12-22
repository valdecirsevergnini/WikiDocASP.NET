using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WikiSistemaASP.NET.Data;
using WikiSistemaASP.NET.Models;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace WikiSistemaASP.NET.Controllers
{
    public class ModuloController : Controller
    {
        private readonly WikiDbContext _context;

        public ModuloController(WikiDbContext context)
        {
            _context = context;
        }

        // Página principal - Listar Módulos
        public async Task<IActionResult> Index()
        {
            var modulos = await _context.Modulos
                .Include(m => m.Topicos)
                .OrderBy(m => m.Nome)
                .ToListAsync();
            return View("~/Views/Modulo/Index.cshtml", modulos);
        }

        // Exibir detalhes de um Módulo e seus Tópicos
        public async Task<IActionResult> Details(int id)
        {
            var modulo = await _context.Modulos
                .Include(m => m.Topicos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (modulo == null)
            {
                TempData["MensagemErro"] = "Módulo não encontrado.";
                return NotFound();
            }

            return View("~/Views/Modulo/Details.cshtml", modulo);
        }

        // Criar um novo Módulo
        public IActionResult Create()
        {
            return View("~/Views/Modulo/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Modulo modulo)
        {
            if (ModelState.IsValid)
            {
                _context.Modulos.Add(modulo);
                await _context.SaveChangesAsync();
                TempData["MensagemSucesso"] = "Módulo criado com sucesso.";
                return RedirectToAction(nameof(Index));
            }

            var erros = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            TempData["MensagemErro"] = "Erro ao criar o módulo. Detalhes: " + string.Join(", ", erros);
            return View("~/Views/Modulo/Create.cshtml", modulo);
        }

        // Editar Módulo e Tópicos
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var modulo = await _context.Modulos
                .Include(m => m.Topicos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (modulo == null)
            {
                TempData["MensagemErro"] = "Módulo não encontrado.";
                return NotFound();
            }

            modulo.Topicos ??= new List<Topico>();
            return View("~/Views/Modulo/Edit.cshtml", modulo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Modulo modulo, List<Topico> topicos)
        {
            if (id != modulo.Id)
            {
                TempData["MensagemErro"] = "Módulo inválido para edição.";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var moduloExistente = await _context.Modulos
                        .Include(m => m.Topicos)
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (moduloExistente == null)
                    {
                        TempData["MensagemErro"] = "Erro ao encontrar o módulo para edição.";
                        return NotFound();
                    }

                    moduloExistente.Nome = modulo.Nome;
                    moduloExistente.Descricao = modulo.Descricao;

                    foreach (var topico in topicos)
                    {
                        if (topico.Id == 0)
                        {
                            topico.ModuloId = modulo.Id;
                            _context.Topicos.Add(topico);
                        }
                        else
                        {
                            var topicoExistente = moduloExistente.Topicos.FirstOrDefault(t => t.Id == topico.Id);
                            if (topicoExistente != null)
                            {
                                topicoExistente.Titulo = topico.Titulo;
                                topicoExistente.Conteudo = topico.Conteudo;
                            }
                        }
                    }

                    var topicosRemovidos = moduloExistente.Topicos
                        .Where(t => !topicos.Any(nt => nt.Id == t.Id))
                        .ToList();
                    _context.Topicos.RemoveRange(topicosRemovidos);

                    await _context.SaveChangesAsync();
                    TempData["MensagemSucesso"] = "Módulo e tópicos atualizados com sucesso.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ModuloExists(modulo.Id))
                    {
                        TempData["MensagemErro"] = "Módulo não encontrado durante a atualização.";
                        return NotFound();
                    }
                    TempData["MensagemErro"] = "Erro de concorrência ao atualizar o módulo.";
                    throw;
                }
            }

            var errosModelState = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            TempData["MensagemErro"] = "Erro ao validar os dados do módulo. Detalhes: " + string.Join(", ", errosModelState);
            return View("~/Views/Modulo/Edit.cshtml", modulo);
        }

        // Confirmar Exclusão
        public async Task<IActionResult> Delete(int id)
        {
            var modulo = await _context.Modulos
                .Include(m => m.Topicos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (modulo == null)
            {
                TempData["MensagemErro"] = "Módulo não encontrado para exclusão.";
                return NotFound();
            }

            return View("~/Views/Modulo/Delete.cshtml", modulo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modulo = await _context.Modulos
                .Include(m => m.Topicos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (modulo == null)
            {
                TempData["MensagemErro"] = "Módulo não encontrado para exclusão.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                if (modulo.Topicos.Any())
                {
                    _context.Topicos.RemoveRange(modulo.Topicos);
                }

                _context.Modulos.Remove(modulo);
                await _context.SaveChangesAsync();
                TempData["MensagemSucesso"] = "Módulo e tópicos excluídos com sucesso.";
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao excluir o módulo: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // Upload de Imagens/Mídia
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "Nenhum arquivo foi enviado." });
            }

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"/uploads/{fileName}";
            return Json(new { location = fileUrl });
        }

        private async Task<bool> ModuloExists(int id)
        {
            return await _context.Modulos.AnyAsync(e => e.Id == id);
        }
    }
}
