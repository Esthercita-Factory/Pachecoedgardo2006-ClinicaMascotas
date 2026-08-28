namespace ClinicaSalud.Models;

public class Paciente
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Edad { get; set; }
    public string Sintoma { get; set; } = string.Empty;
}