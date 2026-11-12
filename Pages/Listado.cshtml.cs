// Pages/Listado.cshtml.cs
using demo_clase3.Datos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace demo_clase3.Pages;

public class ListadoModel : PageModel
{
    private readonly AlumnoRepositorio _repo;
    public ListadoModel(AlumnoRepositorio repo) { _repo = repo; }

    public List<Alumno> Alumnos { get; set; } = new();

    public void OnGet() { 
        Alumnos = _repo.Listar(); 
    }

    public IActionResult OnPostEliminar(int id)
    {
        _repo.Eliminar(id);
        return RedirectToPage();
    }
}