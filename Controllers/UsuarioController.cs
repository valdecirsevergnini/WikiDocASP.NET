using Microsoft.AspNetCore.Mvc;
using WikiSistemaASP.NET.Models;
using WikiSistemaASP.NET.Data;
using Microsoft.EntityFrameworkCore;

namespace WikiSistemaASP.NET.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly WikiDbContext _context;

        public UsuarioController(WikiDbContext context)
        {
            _context = context;
        }

        // Página de Login - GET
        public IActionResult Login()
        {
            return View("~/Views/Usuario/Login.cshtml");
        }

        // Validação de Login - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Username, string Senha)
        {
            // Criação de usuário padrão, caso não exista
            if (!_context.Usuarios.Any(u => u.Username == "admin"))
            {
                var defaultUser = new Usuario
                {
                    Nome = "Administrador",
                    Username = "admin",
                    Senha = "admin123"
                };

                _context.Usuarios.Add(defaultUser);
                await _context.SaveChangesAsync();
            }

            // Verifica se o usuário existe no banco
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == Username && u.Senha == Senha);

            if (usuario != null)
            {
                TempData["Success"] = "Login realizado com sucesso!";
                return RedirectToAction("Index", "Modulo"); // Redireciona para gerenciamento de módulos
            }

            ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
            return View("~/Views/Usuario/Login.cshtml");
        }
    }
}
