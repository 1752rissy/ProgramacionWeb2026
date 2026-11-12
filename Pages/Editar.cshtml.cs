// Pages/Editar.cshtml.cs
using demo_clase3.Datos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace demo_clase3.Pages;

public class EditarModel : PageModel
{
    private readonly AlumnoRepositorio _repo;
    public EditarModel(AlumnoRepositorio repo) { _repo = repo; }

    [BindProperty]
    public int Id { get; set; }

    [BindProperty]
    public AlumnoInput Alumno { get; set; } = new();

    public IActionResult OnGet(int? id)
    {
        if (id is null || id <= 0) return BadRequest();

        var alumno = _repo.ObtenerPorId(id.Value);
        if (alumno is null) return NotFound();

        Id = alumno.Id;
        Alumno.Nombre = alumno.Nombre;
        Alumno.Email = alumno.Email;
        Alumno.Carrera = alumno.Carrera;
        Alumno.Edad = alumno.Edad;

        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();

        _repo.Actualizar(Id, Alumno.Nombre, Alumno.Email, Alumno.Carrera, Alumno.Edad);

        TempData["Mensaje"] = $"Alumno actualizado correctamente: {Alumno.Nombre}";
        return RedirectToPage("Listado");
    }
}