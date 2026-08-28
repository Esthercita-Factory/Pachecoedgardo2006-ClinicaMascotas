using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicaSalud.Models;
using ClinicaSalud.Services;

// Datos de prueba iniciales
var paciente1 = new Paciente(1, "Carlos Perez", 28, "Av. Central 123", "555-1234");
paciente1.AgregarMascota(new Mascota("Rocky", 3, "Perro", "Labrador"));
paciente1.AgregarMascota(new Mascota("Michi", 2, "Gato", "Siamés"));

var paciente2 = new Paciente(2, "Beatriz Torres", 45, "Calle Sol 456", "555-9012");
paciente2.AgregarMascota(new Mascota("Lucas", 5, "Loro", "Amazona"));

List<Paciente> listaPacientes = new List<Paciente> { paciente1, paciente2 };

bool salir = false;

while (!salir)
{
    Console.WriteLine("\n==========================================");
    Console.WriteLine("  CLÍNICA VETERINARIA SALUD+ (SEMANA 5)   ");
    Console.WriteLine("   Programación Asíncrona y Convenciones  ");
    Console.WriteLine("==========================================");
    Console.WriteLine("1. Registrar paciente y mascota (Async / Await)");
    Console.WriteLine("2. Listar todos los pacientes y mascotas");
    Console.WriteLine("3. Ejecutar procesos clínicos en paralelo (Task.WhenAll)");
    Console.WriteLine("4. Comparativa de concurrencia (Task.WhenAll vs Task.WhenAny)");
    Console.WriteLine("5. Simular atención médica concurrente de mascotas");
    Console.WriteLine("6. Salir");
    Console.Write("Seleccione una opción: ");

    string? opcion = Console.ReadLine();

    switch (opcion)
    {
        case "1":
            await PacienteService.RegistrarPacienteAsync(listaPacientes);
            break;
        case "2":
            PacienteService.ListarPacientes(listaPacientes);
            break;
        case "3":
            await PacienteService.EjecutarProcesosParalelosAsync(listaPacientes);
            break;
        case "4":
            await PacienteService.DemostrarWhenAllVsWhenAnyAsync();
            break;
        case "5":
            await PacienteService.SimularAtencionConcurrenteAsync(listaPacientes);
            break;
        case "6":
            salir = true;
            Console.WriteLine("Saliendo del sistema...");
            break;
        default:
            Console.WriteLine("Opción no válida. Intente nuevamente.");
            break;
    }
}
