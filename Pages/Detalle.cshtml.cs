using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace demo_clase3.Pages
{
    public class DetalleModel : PageModel
    {
        // Resultado a mostrar en la vista
        public Student? Alumno { get; private set; }

        // Mensaje para informar errores de validación / inexistencia
        public string? ErrorMessage { get; private set; }

        // Lista en memoria (simula almacenamiento). Id empieza en 1.
        private static readonly List<Student> _students = new()
        {
            new Student { Id = 1, Nombre = "Ana García", Carrera = "Ingeniería" },
            new Student { Id = 2, Nombre = "Luis Pérez", Carrera = "Medicina" },
            new Student { Id = 3, Nombre = "María López", Carrera = "Derecho" }
        };

        // El binder rellenará 'id' desde QueryString (por ejemplo: ?id=2)
        public IActionResult OnGet(int? id)
        {
            // Validación 1: parámetro ausente o no numérico -> id == null
            if (id == null)
            {
                ErrorMessage = "Id inválido o ausente en la solicitud.";
                return Page();
            }

            // Validación 2: id no positivo (protección adicional)
            if (id <= 0)
            {
                ErrorMessage = "El identificador debe ser un número positivo.";
                return Page();
            }

            // Búsqueda en la lista en memoria
            var found = _students.FirstOrDefault(s => s.Id == id.Value);
            if (found == null)
            {
                // Validación 3: id inexistente
                ErrorMessage = $"No se encontró ningún alumno con id = {id}.";
                return Page();
            }

            // Si todo OK, asignamos el alumno para que la vista lo renderice
            Alumno = found;
            return Page();
        }

        // Clase simple local para representar cada entrada en memoria
        public class Student
        {
            public int Id { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public string Carrera { get; set; } = string.Empty;
        }
    }
}