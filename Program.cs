using System;
using System.Collections.Generic;
using ClinicaSalud.Models;
using ClinicaSalud.Services;

// Datos de prueba iniciales (TASK 3: Instanciación y relaciones 1 a N)
var paciente1 = new Paciente(1, "Carlos Perez", 28, "Av. Central 123", "555-1234");
paciente1.AgregarMascota(new Mascota("Rocky", 3, "Perro", "Labrador"));
paciente1.AgregarMascota(new Mascota("Michi", 2, "Gato", "Siamés"));

var paciente2 = new Paciente(2, "Beatriz Torres", 45, "Calle Sol 456", "555-9012");
paciente2.AgregarMascota(new Mascota("Lucas", 5, "Loro", "Amazona"));

List<Paciente> pacientes = new List<Paciente> { paciente1, paciente2 };

bool salir = false;

while (!salir)
{
    Console.WriteLine("\n==========================================");
    Console.WriteLine("  CLÍNICA VETERINARIA SALUD+ (POO / UML)  ");
    Console.WriteLine("==========================================");
    Console.WriteLine("1. Registrar paciente (dueño) y mascota(s)");
    Console.WriteLine("2. Agregar mascota a paciente existente");
    Console.WriteLine("3. Listar pacientes y sus mascotas");
    Console.WriteLine("4. Probar Polimorfismo (EmitirSonido en Animales)");
    Console.WriteLine("5. Probar Abstracción (Servicios Veterinarios)");
    Console.WriteLine("6. Probar Interfaz (IRegistrable)");
    Console.WriteLine("7. Salir");
    Console.Write("Seleccione una opción: ");

    string? opcion = Console.ReadLine();

    switch (opcion)
    {
        case "1":
            PacienteService.RegistrarPaciente(pacientes);
            break;
        case "2":
            PacienteService.AgregarMascotaAPaciente(pacientes);
            break;
        case "3":
            PacienteService.ListarPacientesYMascotas(pacientes);
            break;
        case "4":
            PacienteService.DemostrarPolimorfismoSonidos(pacientes);
            break;
        case "5":
            PacienteService.DemostrarServiciosVeterinarios(pacientes);
            break;
        case "6":
            PacienteService.DemostrarInterfazRegistrable(pacientes);
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
