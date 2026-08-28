using System;
using System.Collections.Generic;

namespace ClinicaSalud.Models;

// TASK 2 y 4: Encapsulación y constructores. TASK 6: Implementación de IRegistrable
public class Paciente : IRegistrable
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

    // Datos protegidos / encapsulados
    public string Telefono
    {
        get => _telefono;
        set => _telefono = !string.IsNullOrWhiteSpace(value) ? value.Trim() : "Sin teléfono";
    }

    // TASK 3: Asociación 1 a N (un paciente puede tener una o varias mascotas)
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

    // TASK 2: Método para mostrar información del paciente y sus mascotas
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

    // TASK 6: Implementación de IRegistrable
    public void Registrar()
    {
        Console.WriteLine($"[Registro IRegistrable] Paciente (Dueño) '{Nombre}' (ID: {Id}) registrado con {Mascotas.Count} mascota(s).");
    }
}
