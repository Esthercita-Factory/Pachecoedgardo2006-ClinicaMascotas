namespace ClinicaSalud.Models;

/*
 * TASK 3: Múltiples interfaces
 * - INotificable permite enviar avisos o recordatorios a entidades de la clínica (ej. Dueños/Pacientes)
 *   sin importar su jerarquía de clases.
 */
public interface INotificable
{
    void EnviarNotificacion(string mensaje);
}
