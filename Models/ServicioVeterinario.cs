namespace ClinicaSalud.Models;

/*
 * TASK 1 y 2: ServicioVeterinario como clase base abstracta e implementando IAtendible.
 * - Clase abstracta: Provee estado y propiedades comunes (NombreServicio, CostoBase).
 * - Interfaz IAtendible: Define el contrato de ejecución polimórfica del servicio.
 */
public abstract class ServicioVeterinario : IAtendible
{
    public string NombreServicio { get; protected set; } = string.Empty;
    public decimal CostoBase { get; protected set; }

    public abstract void Atender(Paciente paciente, Mascota mascota);
}
