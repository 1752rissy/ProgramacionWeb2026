using demo_clase3.Datos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace demo_clase3.Pages
{
    public class altaAlumnoModel : PageModel
    {

        private readonly AlumnoRepositorio _repo;

        public altaAlumnoModel(AlumnoRepositorio repo)
        {
            _repo = repo;
        }

        [BindProperty]
        public AlumnoInput Alumno { get; set; } = new AlumnoInput();

        public void OnGet()
        {  
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            int id = _repo.Insertar(Alumno.Nombre, Alumno.Email, Alumno.Carrera, Alumno.Edad);  

            // Guardar nombre en sesión para mostrar en Index
            HttpContext.Session.SetString("UltimoAlumno", Alumno.Nombre);
            HttpContext.Session.SetString("UltimoEmail", Alumno.Email); 

            TempData["Mensaje"] = $"Alumno guardado correctamente: {Alumno.Nombre}";

            return RedirectToPage("Index");
        }
    }

    public class AlumnoInput
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La carrera es obligatoria.")]
        [Display(Name = "Carrera")]
        public string Carrera { get; set; } = string.Empty;

        [Required(ErrorMessage = "La edad es obligatoria.")]
        [Range(18, 60, ErrorMessage = "La edad debe estar entre 18 y 60 años.")]
        [Display(Name = "Edad")]
        public int Edad { get; set; }

    }
}