using Microsoft.AspNetCore.Mvc;
using WikiSistemaASP.NET.Models;
using WikiSistemaASP.NET.Data;
using Microsoft.EntityFrameworkCore;

namespace WikiSistemaASP.NET.Controllers
{
    public class AdminController : Controller
    {
        private readonly WikiDbContext _context;

        public AdminController(WikiDbContext context)
        {
            _context = context;
        }

        // Método de Login - GET
        public IActionResult Login()
        {
            return View();
        }

        // Método de Login - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Username, string Senha)
        {
            // Log de depuração para verificar os valores recebidos
            Console.WriteLine($"Tentativa de login com Username: '{Username}' e Senha: '{Senha}'");

            // Verifica se os campos foram preenchidos
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Senha))
            {
                ModelState.AddModelError(string.Empty, "Preencha todos os campos.");
                return View();
            }

            // Busca o usuário no banco de dados
            var usuario = await _context.Usuarios
                .AsNoTracking() // Evita tracking para melhor performance
                .FirstOrDefaultAsync(u => u.Username == Username && u.Senha == Senha);

            // Verifica se encontrou o usuário
            if (usuario != null)
            {
                TempData["Success"] = "Login realizado com sucesso!";
                return RedirectToAction("Index", "Home"); // Redireciona para a Home
            }

            // Se não encontrou, exibe mensagem de erro
            ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
            return View();
        }
    }
}
