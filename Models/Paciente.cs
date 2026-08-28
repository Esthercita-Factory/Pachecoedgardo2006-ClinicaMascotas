using System;
using System.Collections.Generic;

namespace ClinicaSalud.Models;

/*
 * TASK 3: Implementación de múltiples interfaces (IRegistrable e INotificable)
 * - Muestra la flexibilidad de C# donde una clase puede asumir múltiples contratos de comportamiento.
 */
public class Paciente : IRegistrable, INotificable
{
    private int _id;
    private string _nombre = string.Empty;
    private int _edad;
    private string _direccion = string.Empty;
    private string _telefono = string.Empty;

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public string Nombre
    {
        get => _nombre;
        set => _nombre = !string.IsNullOrWhiteSpace(value) ? value.Trim() : "Desconocido";
    }

    public int Edad
    {
        get => _edad;
        set => _edad = value >= 0 ? value : 0;
    }

    public string Direccion
    {
        get => _direccion;
        set => _direccion = !string.IsNullOrWhiteSpace(value) ? value.Trim() : "No especificada";
    }

    public string Telefono
    {
        get => _telefono;
        set => _telefono = !string.IsNullOrWhiteSpace(value) ? value.Trim() : "Sin teléfono";
    }

    public List<Mascota> Mascotas { get; set; } = new List<Mascota>();

    public Paciente() { }

    public Paciente(int id, string nombre, int edad, string direccion, string telefono)
    {
        Id = id;
        Nombre = nombre;
        Edad = edad;
        Direccion = direccion;
        Telefono = telefono;
    }

    public void AgregarMascota(Mascota mascota)
    {
        if (mascota != null)
        {
            Mascotas.Add(mascota);
        }
    }

    public void MostrarInformacion()
    {
        Console.WriteLine($"\n[Paciente ID: {Id}] Dueño: {Nombre} | Edad: {Edad} años | Teléfono: {Telefono} | Dirección: {Direccion}");
        if (Mascotas.Count == 0)
        {
            Console.WriteLine("   (No tiene mascotas asociadas)");
        }
        else
        {
            Console.WriteLine($"   Mascotas registradas ({Mascotas.Count}):");
            foreach (var mascota in Mascotas)
            {
                mascota.MostrarInformacion();
            }
        }
    }

    // TASK 2: Implementación de IRegistrable
    public void Registrar()
    {
        Console.WriteLine($"[Registro IRegistrable] Paciente (Dueño) '{Nombre}' (ID: {Id}) registrado con {Mascotas.Count} mascota(s).");
    }

    // TASK 3: Implementación de INotificable
    public void EnviarNotificacion(string mensaje)
    {
        Console.WriteLine($"\n[Notificación SMS/Email a {Telefono} ({Nombre})]: \"{mensaje}\"");
    }
}
