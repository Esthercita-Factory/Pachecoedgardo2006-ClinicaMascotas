using System;
using System.Collections.Generic;
using ClinicaSalud.Models;
using ClinicaSalud.Services;

// Datos de prueba iniciales
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
    Console.WriteLine("  CLÍNICA VETERINARIA SALUD+ (SEMANA 4)   ");
    Console.WriteLine("==========================================");
    Console.WriteLine("1. Registrar paciente y mascota(s) (IRegistrable)");
    Console.WriteLine("2. Listar todos los pacientes y mascotas");
    Console.WriteLine("3. Buscar mascota (Prueba MascotaNoEncontradaException)");
    Console.WriteLine("4. Atender servicio veterinario (IAtendible)");
    Console.WriteLine("5. Enviar recordatorio de cita (INotificable)");
    Console.WriteLine("6. Ejecutar prueba de depuración (Breakpoints / DivideByZero)");
    Console.WriteLine("7. Salir");
    Console.Write("Seleccione una opción: ");

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
            PacienteService.BuscarMascotaPorNombre(pacientes);
            break;
        case "4":
            PacienteService.AtenderServicioVeterinario(pacientes);
            break;
        case "5":
            PacienteService.EnviarRecordatorioCita(pacientes);
            break;
        case "6":
            PacienteService.ProbarEscenarioDepuracion();
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
