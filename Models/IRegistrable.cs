namespace ClinicaSalud.Models;

/*
 * TASK 2: Interfaz IRegistrable
 * - Define el contrato para cualquier entidad que deba persistirse o registrarse en el sistema.
 * - Paciente y Mascota la implementan de forma consistente.
 */
public interface IRegistrable
{
    void Registrar();
}
