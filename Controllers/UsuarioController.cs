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

        // Listagem de Usuários
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return View(usuarios);
        }

        // Detalhes do Usuário
        public async Task<IActionResult> Details(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // Formulário para Criação
        public IActionResult Create()
        {
            return View();
        }

        // Criação de Usuário (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Username,Senha")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // Formulário para Edição
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // Edição de Usuário (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Username,Senha")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Não foi possível atualizar o registro. Tente novamente.");
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // Exclusão - Visualizar
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // Exclusão Confirmada (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Página de Login - GET
        public IActionResult Login()
        {
            return View();
        }

        // Validação de Login - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Username, string Senha)
        {
            // Criação do usuário padrão caso não exista
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

            // Busca o usuário no banco de dados
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == Username && u.Senha == Senha);

            if (usuario != null)
            {
                TempData["Success"] = "Login realizado com sucesso!";
                return RedirectToAction("Index", "Modulo"); // Redireciona Modulo
            }

            ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
            return View();
        }

        // Método Auxiliar para Verificar Existência
        private async Task<bool> UsuarioExists(int id)
        {
            return await _context.Usuarios.AnyAsync(e => e.Id == id);
        }
    }
}
