using System;

namespace ClinicaSalud.Models;

// TASK 6: Subclase de ServicioVeterinario
public class ConsultaGeneral : ServicioVeterinario
{
    public string Diagnostico { get; set; } = string.Empty;

    public ConsultaGeneral()
    {
        NombreServicio = "Consulta General";
        CostoBase = 35.00m;
    }

    public override void Atender(Paciente paciente, Mascota mascota)
    {
        Console.WriteLine($"\n==========================================");
        Console.WriteLine($"      ATENCIÓN: {NombreServicio.ToUpper()}");
        Console.WriteLine($"==========================================");
        Console.WriteLine($"Paciente (Dueño): {paciente.Nombre} | Tel: {paciente.Telefono}");
        Console.WriteLine($"Mascota: {mascota.Nombre} ({mascota.Especie} - {mascota.Raza})");
        Console.WriteLine($"Procedimiento: Examen físico completo y evaluación médica.");
        Console.WriteLine($"Diagnóstico / Motivo: {(string.IsNullOrWhiteSpace(Diagnostico) ? "Revisión médica preventiva" : Diagnostico)}");
        Console.WriteLine($"Total a pagar: ${CostoBase:F2}");
        Console.WriteLine("------------------------------------------");
    }
}
