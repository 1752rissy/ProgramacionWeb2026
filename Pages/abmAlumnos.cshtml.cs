// Pages/AbmAlumnos.cshtml.cs
using demo_clase3.Datos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace demo_clase3.Pages;

public class AbmAlumnosModel : PageModel
{
    private readonly AlumnoRepositorio _repo;
    public AbmAlumnosModel(AlumnoRepositorio repo) { _repo = repo; }

    // El mismo AlumnoInput que ya tenemos en Alta y Editar (está declarado en altaAlumnoModel.cs)
    [BindProperty]
    public AlumnoInput Alumno { get; set; } = new();

    // 0 = estamos dando de alta; > 0 = estamos editando ese alumno
    [BindProperty]
    public int Id { get; set; }

    public List<Alumno> Alumnos { get; set; } = new();

    public bool EnEdicion => Id > 0;

    public void OnGet(int? id)
    {
        Alumnos = _repo.Listar();

        if (id is > 0)
        {
            var existente = _repo.ObtenerPorId(id.Value);
            if (existente is not null)
            {
                Id = existente.Id;
                Alumno.Nombre = existente.Nombre;
                Alumno.Email = existente.Email;
                Alumno.Carrera = existente.Carrera;
                Alumno.Edad = existente.Edad;
            }
        }
    }

    // Alta y Modificación comparten formulario y handler:
    // el Id oculto es lo único que decide cuál de las dos operaciones hacer.
    public IActionResult OnPostGuardar()
    {
        if (!ModelState.IsValid)
        {
            Alumnos = _repo.Listar();   
            return Page();
        }

        if (Id > 0)
        {
            _repo.Actualizar(Id, Alumno.Nombre, Alumno.Email, Alumno.Carrera, Alumno.Edad);
            TempData["Mensaje"] = $"Alumno actualizado: {Alumno.Nombre}";
        }
        else
        {
            _repo.Insertar(Alumno.Nombre, Alumno.Email, Alumno.Carrera, Alumno.Edad);
            TempData["Mensaje"] = $"Alumno guardado: {Alumno.Nombre}";
        }

        return RedirectToPage();   //  limpia el formulario y vuelve en modo alta
    }

    public IActionResult OnPostEliminar(int id)
    {
        _repo.Eliminar(id);
        TempData["Mensaje"] = "Alumno eliminado.";
        return RedirectToPage();
    }
}