using System;

namespace ClinicaSalud.Models;

// TASK 6: Subclase de ServicioVeterinario
public class Vacunacion : ServicioVeterinario
{
    public string TipoVacuna { get; set; } = string.Empty;

    public Vacunacion(string tipoVacuna)
    {
        NombreServicio = "Servicio de Vacunación";
        CostoBase = 25.00m;
        TipoVacuna = !string.IsNullOrWhiteSpace(tipoVacuna) ? tipoVacuna : "Rabia / Múltiple";
    }

    public override void Atender(Paciente paciente, Mascota mascota)
    {
        Console.WriteLine($"\n==========================================");
        Console.WriteLine($"      ATENCIÓN: {NombreServicio.ToUpper()}");
        Console.WriteLine($"==========================================");
        Console.WriteLine($"Paciente (Dueño): {paciente.Nombre} | Tel: {paciente.Telefono}");
        Console.WriteLine($"Mascota: {mascota.Nombre} ({mascota.Especie} - {mascota.Raza})");
        Console.WriteLine($"Vacuna aplicada: {TipoVacuna}");
        Console.WriteLine($"Resultado: Carnet de vacunación expedido/actualizado.");
        Console.WriteLine($"Total a pagar: ${CostoBase:F2}");
        Console.WriteLine("------------------------------------------");
    }
}
