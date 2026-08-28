using System;

namespace ClinicaSalud.Models;

// TASK 5: Excepción personalizada del dominio de la clínica
public class MascotaNoEncontradaException : Exception
{
    public string NombreMascotaBuscada { get; }

    public MascotaNoEncontradaException(string nombreMascota)
        : base($"No se encontró ninguna mascota con el nombre o identificador '{nombreMascota}'.")
    {
        NombreMascotaBuscada = nombreMascota;
    }

    public MascotaNoEncontradaException(string message, Exception innerException)
        : base(message, innerException)
    {
        NombreMascotaBuscada = string.Empty;
    }
}
