namespace ClinicaSalud.Models;

public class Paciente
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public int Edad { get; set; }
    public string Sintoma { get; set; } = string.Empty;
    public string NombreMascota { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string Raza { get; set; } = string.Empty;
}
