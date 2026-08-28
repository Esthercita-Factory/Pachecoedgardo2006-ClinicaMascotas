using System;

namespace ClinicaSalud.Models;

// TASK 5: Excepción personalizada cuando un paciente no existe
public class PacienteNoEncontradoException : Exception
{
    public int IdBuscado { get; }

    public PacienteNoEncontradoException(int id)
        : base($"No se encontró ningún paciente con el ID #{id} en el sistema.")
    {
        IdBuscado = id;
    }

    public PacienteNoEncontradoException(string message, Exception innerException)
        : base(message, innerException)
    {
        IdBuscado = 0;
    }
}
