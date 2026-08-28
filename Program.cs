using System;
using System.Collections.Generic;
using ClinicaSalud.Models;
using ClinicaSalud.Services;

List<Paciente> pacientes = new List<Paciente>();
bool salir = false;

while (!salir)
{
    Console.WriteLine("\n=============================");
    Console.WriteLine("   SISTEMA CLÍNICA SALUD     ");
    Console.WriteLine("=============================");
    Console.WriteLine("1. Registrar paciente");
    Console.WriteLine("2. Listar pacientes");
    Console.WriteLine("3. Buscar paciente");
    Console.WriteLine("4. Salir");
    Console.Write("Seleccione Una opción: ");

    string? opcion = Console.ReadLine();

    switch (opcion)
    {
        case "1":
            PacienteService.RegistrarPaciente(pacientes);
            break;
        case "2":
            PacienteService.ListarPacientes(pacientes);
            break;
        case "3":
            Console.Write("\nIngrese el nombre a buscar: ");
            string? busqueda = Console.ReadLine();
            PacienteService.BuscarPacientePorNombre(pacientes, busqueda ?? string.Empty);
            break;
        case "4":
            salir = true;
            Console.WriteLine("Saliendo del sistema...");
            break;
        default:
            Console.WriteLine("Opción no válida. Intente de nuevo.");
            break;
    }
}