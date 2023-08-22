using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BcDemocratize.Data;
using BcDemocratize.Models;

namespace BcDemocratize.Controllers
{
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembersController(ApplicationDbContext context)
        {
            _context = context;
        }
        // j'injecte le contexte

        // GET: Members
        // Afficher tous les membres
        public async Task<IActionResult> Index()
        {
            // Si l'ensemble d'entités 'ApplicationDbContext.Member' n'est pas null, retourne la vue 'Index' avec tous les membres, sinon retourne une erreur
            return _context.Member != null ?
                          View(await _context.Member.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Member'  is null.");
        }

        // GET: Members/Details/5
        // Afficher les détails d'un membre spécifique
        public async Task<IActionResult> Details(int? id)
        {
            // Si l'ID est null ou l'ensemble d'entités 'ApplicationDbContext.Member' est null, retourne une erreur
            if (id == null || _context.Member == null)
            {
                return NotFound();
            }

            // Trouve le membre avec l'ID correspondant et retourne la vue 'Details', sinon retourne une erreur
            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        // Afficher la page de création d'un membre
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // Créer un nouvel membre
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Firstname,Lastname,Cellphone,Phone,Email")] Member member)
        {
            // Si le modèle est valide, ajoute le nouvel membre à l'ensemble d'entités et redirige vers la page 'Index', sinon retourne la vue 'Create' avec le membre non valide
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        // Afficher la page de modification d'un membre
        public async Task<IActionResult> Edit(int? id)
        {
            // Si l'ID est null ou l'ensemble d'entités 'ApplicationDbContext.Member' est null, retourne une erreur
            if (id == null || _context.Member == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // Modifier un membre existant
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Firstname,Lastname,Cellphone,Phone,Email")] Member member)
        {
            // Si l'ID ne correspond pas au membre spécifié, retourne une erreur
            if (id != member.Id)
            {
                return NotFound();
            }

            // Vérifie si le modèle du membre est valide avant de l'enregistrer
            if (ModelState.IsValid)
            {
                try
                {
                    // Met à jour le modèle du membre dans la base de données
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Si le membre n'existe pas, retourne une erreur
                    if (!MemberExists(member.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Redirige vers la vue Index après avoir enregistré les modifications
                return RedirectToAction(nameof(Index));
            }
            // Retourne la vue Edit avec le modèle de membre en cas d'erreur
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Si l'ID est nul ou l'ensemble d'entités Member est nul, retourne une erreur
            if (id == null || _context.Member == null)
            {
                return NotFound();
            }

            // Récupère le membre correspondant à l'ID spécifié
            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.Id == id);
            // Si le membre n'existe pas, retourne une erreur
            if (member == null)
            {
                return NotFound();
            }

            // Affiche la vue Delete avec le modèle d'un membre
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Si l'ensemble d'entités Member est nul, retourne une erreur
            if (_context.Member == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Member'  is null.");
            }
            // Récupère le membre correspondant à l'ID spécifié
            var member = await _context.Member.FindAsync(id);
            // Si le membre existe, le supprime de la base de données
            if (member != null)
            {
                _context.Member.Remove(member);
            }

            // Enregistre les modifications dans la base de données
            await _context.SaveChangesAsync();
            // Redirige vers la vue Index après avoir supprimé le membre
            return RedirectToAction(nameof(Index));
        }

        // Vérifie si le membre avec l'ID spécifié existe dans la base de données
        private bool MemberExists(int id)
        {
          return (_context.Member?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
