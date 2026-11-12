using Microsoft.Data.SqlClient;

namespace demo_clase3.Datos;

public record Alumno(int Id, string Nombre, string Email, string Carrera, int Edad, DateTime FechaAlta);

public class AlumnoRepositorio
{
    private readonly string _cadena;

    public AlumnoRepositorio(IConfiguration config)
    {
        _cadena = config.GetConnectionString("fundacion")
            ?? throw new InvalidOperationException("error de conexion de DBB.");
    }

    public List<Alumno> Listar()
    {
        var lista = new List<Alumno>();
        using var cn = new SqlConnection(_cadena);//hace la conexion a la base de datos
        using var cmd = new SqlCommand(
            "SELECT Id, Nombre, Email, Carrera, Edad, FechaAlta FROM Alumnos ORDER BY Nombre", cn);
        cn.Open();
        using var rd = cmd.ExecuteReader();
        while (rd.Read())
            lista.Add(new Alumno(rd.GetInt32(0), rd.GetString(1),
                                 rd.GetString(2), rd.GetString(3), rd.GetInt32(4), rd.GetDateTime(5)));
        return lista;
    }

    public Alumno? ObtenerPorId(int id)
    {
        using var cn = new SqlConnection(_cadena);
        using var cmd = new SqlCommand(
            "SELECT Id, Nombre, Email, Carrera, Edad,FechaAlta FROM Alumnos WHERE Id = @Id", cn);
        cmd.Parameters.AddWithValue("@Id", id);
        cn.Open();
        using var rd = cmd.ExecuteReader();
        return rd.Read()
            ? new Alumno(rd.GetInt32(0), rd.GetString(1),
                         rd.GetString(2), rd.GetString(3), rd.GetInt32(4), rd.GetDateTime(5))
            : null;
    }

    public int Insertar(string nombre, string email, string carrera, int edad)
    {
        using var cn = new SqlConnection(_cadena);
        using var cmd = new SqlCommand(
            "INSERT INTO Alumnos (Nombre, Email, Carrera, Edad) OUTPUT INSERTED.Id " +
            "VALUES (@Nombre, @Email, @Carrera, @Edad)", cn);

        cmd.Parameters.AddWithValue("@Nombre", nombre);
        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@Carrera", carrera);
        cmd.Parameters.AddWithValue("@Edad", edad);

        cn.Open();
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public bool Actualizar(int id, string nombre, string email, string carrera, int edad)
    {
        using var cn = new SqlConnection(_cadena);
        using var cmd = new SqlCommand(
            "UPDATE Alumnos SET Nombre=@Nombre, Email=@Email, Carrera=@Carrera, Edad=@Edad " +
            "WHERE Id=@Id", cn);

        cmd.Parameters.AddWithValue("@Nombre", nombre);
        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@Carrera", carrera);
        cmd.Parameters.AddWithValue("@Edad", edad);
        cmd.Parameters.AddWithValue("@Id", id);

        cn.Open();
        return cmd.ExecuteNonQuery() > 0;   // true si encontró y actualizó la fila
    }

    public int Eliminar(int id)
    {
        using var cn = new SqlConnection(_cadena);
        using var cmd = new SqlCommand("DELETE FROM Alumnos WHERE Id = @Id", cn);

        cmd.Parameters.AddWithValue("@Id", id);

        cn.Open();
        return cmd.ExecuteNonQuery();   // filas afectadas
    }
}