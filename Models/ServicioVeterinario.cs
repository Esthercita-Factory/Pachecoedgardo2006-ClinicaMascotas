using System;

namespace ClinicaSalud.Models;

// TASK 6: Clase abstracta con método abstracto Atender
public abstract class ServicioVeterinario
{
    public string NombreServicio { get; protected set; } = string.Empty;
    public decimal CostoBase { get; protected set; }

    public abstract void Atender(Paciente paciente, Mascota mascota);
}
