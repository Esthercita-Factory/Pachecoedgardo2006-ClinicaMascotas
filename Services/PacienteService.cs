using System;
using System.Collections.Generic;
using System.Linq;
using ClinicaSalud.Models;

namespace ClinicaSalud.Services;

public static class PacienteService
{
    public static void RegistrarPaciente(List<Paciente> lista)
    {
        Console.WriteLine("\n--- REGISTRAR PACIENTE ---");

        string nombre = LeerTextoNoVacio("Nombre: ");
        int edad = LeerEntero("Edad: ");
        string sintoma = LeerTextoNoVacio("Síntoma: ");

        int nuevoId = lista.Count > 0 ? lista.Max(p => p.Id) + 1 : 1;

        lista.Add(new Paciente
        {
            Id = nuevoId,
            Nombre = nombre,
            Edad = edad,
            Sintoma = sintoma
        });

        Console.WriteLine("¡Paciente registrado con éxito!");
    }

    public static void ListarPacientes(List<Paciente> lista)
    {
        Console.WriteLine("\n--- LISTA DE PACIENTES ---");

        if (lista.Count == 0)
        {
            Console.WriteLine("No hay pacientes registrados.");
            return;
        }

        foreach (var p in lista)
        {
            Console.WriteLine($"ID: {p.Id} | Nombre: {p.Nombre} | Edad: {p.Edad} | Síntoma: {p.Sintoma}");
        }
    }

    public static void BuscarPacientePorNombre(List<Paciente> lista, string nombre)
    {
        Console.WriteLine("\n--- BUSCAR PACIENTE ---");

        var encontrado = lista.FirstOrDefault(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));

        if (encontrado != null)
        {
            Console.WriteLine($"[Encontrado] ID: {encontrado.Id} | Nombre: {encontrado.Nombre} | Edad: {encontrado.Edad} | Síntoma: {encontrado.Sintoma}");
        }
        else
        {
            Console.WriteLine($"No se encontró ningún paciente con el nombre '{nombre}'.");
        }
    }

    // Validaciones e ingreso con try-catch (Task 7)
    private static string LeerTextoNoVacio(string mensaje)
    {
        string? entrada;
        do
        {
            Console.Write(mensaje);
            entrada = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(entrada))
            {
                Console.WriteLine("Error: El campo no puede estar vacío.");
            }
        } while (string.IsNullOrWhiteSpace(entrada));

        return entrada.Trim();
    }

    private static int LeerEntero(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? entrada = Console.ReadLine();

            try
            {
                int valor = int.Parse(entrada!);
                if (valor < 0)
                {
                    Console.WriteLine("Error: Debe ser un número entero positivo.");
                    continue;
                }
                return valor;
            }
            catch (Exception)
            {
                Console.WriteLine("Error: Entrada inválida. Ingrese un número entero.");
            }
        }
    }
}