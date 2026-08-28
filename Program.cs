using System;
using System.Collections.Generic;
using ClinicaSalud.Models;
using ClinicaSalud.Services;

// TASK 1: Colecciones en memoria (List y Dictionary)
List<Paciente> pacientes = new List<Paciente>
{
    new Paciente { Id = 1, Nombre = "Carlos Perez", Telefono = "555-1234", Edad = 28, NombreMascota = "Rocky", Especie = "Perro", Raza = "Labrador", Sintoma = "Vacunación anual" },
    new Paciente { Id = 2, Nombre = "Ana Gómez", Telefono = "555-5678", Edad = 22, NombreMascota = "Michi", Especie = "Gato", Raza = "Sin raza", Sintoma = "Control de parásitos" },
    new Paciente { Id = 3, Nombre = "Beatriz Torres", Telefono = "555-9012", Edad = 45, NombreMascota = "Toby", Especie = "Perro", Raza = "Beagle", Sintoma = "Dolor en pata derecha" },
    new Paciente { Id = 4, Nombre = "David Ramirez", Telefono = "555-3456", Edad = 34, NombreMascota = "Lucas", Especie = "Loro", Raza = "Amazona", Sintoma = "Revisión general" }
};

Dictionary<int, Paciente> pacientesDiccionario = new Dictionary<int, Paciente>();
foreach (var p in pacientes)
{
    pacientesDiccionario[p.Id] = p;
}

bool salir = false;

while (!salir)
{
    Console.WriteLine("\n==========================================");
    Console.WriteLine("    SISTEMA CLÍNICA VETERINARIA SALUD+    ");
    Console.WriteLine("==========================================");
    Console.WriteLine("1. Registrar paciente y mascota");
    Console.WriteLine("2. Listar todos los pacientes");
    Console.WriteLine("3. Modificar datos de paciente");
    Console.WriteLine("4. Eliminar paciente");
    Console.WriteLine("5. Búsqueda rápida por ID (Dictionary)");
    Console.WriteLine("6. Reportes y consultas con LINQ");
    Console.WriteLine("7. Salir");
    Console.Write("Seleccione una opción: ");

    string? opcion = Console.ReadLine();

    switch (opcion)
    {
        case "1":
            PacienteService.RegistrarPaciente(pacientes, pacientesDiccionario);
            break;
        case "2":
            PacienteService.ListarPacientes(pacientes);
            break;
        case "3":
            PacienteService.ModificarPaciente(pacientes, pacientesDiccionario);
            break;
        case "4":
            PacienteService.EliminarPaciente(pacientes, pacientesDiccionario);
            break;
        case "5":
            PacienteService.BuscarPorIdEnDiccionario(pacientesDiccionario);
            break;
        case "6":
            PacienteService.EjecutarConsultasLinq(pacientes);
            break;
        case "7":
            salir = true;
            Console.WriteLine("Saliendo del sistema...");
            break;
        default:
            Console.WriteLine("Opción no válida. Intente nuevamente.");
            break;
    }
}
