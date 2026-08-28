using System;

namespace ClinicaSalud.Models;

/*
 * DECISIÓN DE DISEÑO: INTERFAZ vs CLASE ABSTRACTA
 * - Usamos una interfaz (IAtendible) para definir la capacidad de un servicio de atender a un paciente y su mascota,
 *   permitiendo desacoplar completamente la implementación del tipo de servicio o permitir que diferentes clases
 *   no relacionadas por herencia puedan ser atendibles.
 */
public interface IAtendible
{
    string NombreServicio { get; }
    decimal CostoBase { get; }
    void Atender(Paciente paciente, Mascota mascota);
}
