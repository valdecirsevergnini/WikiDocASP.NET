using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WikiSistemaASP.NET.Data;
using WikiSistemaASP.NET.Models;

namespace WikiSistemaASP.NET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WikiDbContext _context;

        public HomeController(ILogger<HomeController> logger, WikiDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Página inicial
        public async Task<IActionResult> Index()
        {
            // Recupera os módulos do banco de dados, incluindo os tópicos relacionados, em ordem alfabética
            var modules = await _context.Modulos
                .Include(m => m.Topicos)
                .OrderBy(m => m.Nome)
                .ToListAsync();

             return View(modules ?? new List<Modulo>()); // Retorna uma lista vazia se for null
        }

        // Página de política de privacidade
        public IActionResult Privacy()
        {
            return View();
        }

        // Método para busca global nos módulos e tópicos
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction("Index");
            }

            // Busca nos módulos e tópicos com case-insensitive e suporte a substrings
            var results = await _context.Modulos
                .Include(m => m.Topicos)
                .Where(m => EF.Functions.ILike(m.Nome, $"%{searchTerm}%") ||
                            m.Topicos.Any(t => EF.Functions.ILike(t.Titulo, $"%{searchTerm}%")))
                .ToListAsync();

            return View("Index", results);
        }

        // Retorna a página de erro
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
