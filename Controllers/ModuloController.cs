using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WikiSistemaASP.NET.Data;
using WikiSistemaASP.NET.Models;

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
                .Include(m => m.Topicos) // Inclui os tópicos associados ao módulo
                .ToListAsync();
            return View(modulos);
        }

        // Exibir detalhes de um Módulo e seus Tópicos
        public async Task<IActionResult> Details(int id)
        {
            var modulo = await _context.Modulos
                .Include(m => m.Topicos) // Carrega os tópicos relacionados
                .FirstOrDefaultAsync(m => m.Id == id);

            if (modulo == null)
            {
                return NotFound(); // Retorna 404 caso o módulo não seja encontrado
            }

            return View(modulo);
        }

        // Criar um novo Módulo
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Modulo modulo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modulo);
        }

        // Criar Tópico associado a um Módulo
        public IActionResult CreateTopico(int id)
        {
            var modulo = _context.Modulos.Find(id);
            if (modulo == null)
            {
                return NotFound();
            }

            ViewData["ModuloId"] = id;
            ViewData["ModuloNome"] = modulo.Nome; // Exibe o nome do módulo no formulário
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTopico(Topico topico)
        {
            if (ModelState.IsValid)
            {
                _context.Topicos.Add(topico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = topico.ModuloId });
            }

            // Caso haja erro, recarrega as informações necessárias
            var modulo = await _context.Modulos.FindAsync(topico.ModuloId);
            ViewData["ModuloId"] = topico.ModuloId;
            ViewData["ModuloNome"] = modulo?.Nome ?? "Módulo Desconhecido";
            return View(topico);
        }
    }
}
