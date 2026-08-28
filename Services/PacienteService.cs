using System;
using System.Collections.Generic;
using System.Linq;
using ClinicaSalud.Models;

namespace ClinicaSalud.Services;

public static class PacienteService
{
    // TASK 2 y 3: Registro de paciente con mascota(s)
    public static void RegistrarPaciente(List<Paciente> lista)
    {
        Console.WriteLine("\n--- REGISTRO DE NUEVO PACIENTE (DUEÑO) ---");

        string nombre = LeerTextoNoVacio("Nombre del paciente/dueño: ");
        int edad = LeerEntero("Edad: ");
        string direccion = LeerTextoNoVacio("Dirección: ");
        string telefono = LeerTextoNoVacio("Teléfono de contacto: ");

        int nuevoId = lista.Count > 0 ? lista.Max(p => p.Id) + 1 : 1;

        var paciente = new Paciente(nuevoId, nombre, edad, direccion, telefono);

        Console.WriteLine("\n--- DATOS DE LA MASCOTA PRINCIPAL ---");
        Mascota mascota = CrearMascotaDesdeConsola();
        paciente.AgregarMascota(mascota);

        // Opción para registrar más mascotas al mismo dueño
        Console.Write("¿Desea agregar otra mascota para este paciente? (s/n): ");
        if (Console.ReadLine()?.Trim().ToLower() == "s")
        {
            Mascota otraMascota = CrearMascotaDesdeConsola();
            paciente.AgregarMascota(otraMascota);
        }

        lista.Add(paciente);

        // TASK 6: Demostración de interfaz IRegistrable al registrar
        paciente.Registrar();
        foreach (var m in paciente.Mascotas)
        {
            m.Registrar();
        }

        Console.WriteLine($"\n¡Paciente #{paciente.Id} y mascota(s) registrados exitosamente!");
    }

    // TASK 3: Agregar mascota a un paciente existente
    public static void AgregarMascotaAPaciente(List<Paciente> lista)
    {
        Console.WriteLine("\n--- ASOCIAR NUEVA MASCOTA A PACIENTE EXISTENTE ---");
        if (!lista.Any())
        {
            Console.WriteLine("No hay pacientes registrados.");
            return;
        }

        int id = LeerEntero("Ingrese el ID del paciente/dueño: ");
        var paciente = lista.FirstOrDefault(p => p.Id == id);

        if (paciente == null)
        {
            Console.WriteLine($"No se encontró ningún paciente con el ID {id}.");
            return;
        }

        Console.WriteLine($"Agregando mascota a: {paciente.Nombre}");
        Mascota nuevaMascota = CrearMascotaDesdeConsola();
        paciente.AgregarMascota(nuevaMascota);
        nuevaMascota.Registrar();

        Console.WriteLine($"¡Mascota '{nuevaMascota.Nombre}' agregada con éxito al paciente {paciente.Nombre}!");
    }

    // TASK 2 y 3: Mostrar información estructurada
    public static void ListarPacientesYMascotas(List<Paciente> lista)
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine("       LISTA DE PACIENTES Y MASCOTAS      ");
        Console.WriteLine("==========================================");

        if (!lista.Any())
        {
            Console.WriteLine("No hay registros en el sistema.");
            return;
        }

        foreach (var paciente in lista)
        {
            paciente.MostrarInformacion();
        }
    }

    // TASK 5: Demostración de Polimorfismo con EmitirSonido()
    public static void DemostrarPolimorfismoSonidos(List<Paciente> lista)
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine("    DEMOSTRACIÓN DE POLIMORFISMO (POO)    ");
        Console.WriteLine("==========================================");

        var todasLasMascotas = lista.SelectMany(p => p.Mascotas).ToList();

        if (!todasLasMascotas.Any())
        {
            Console.WriteLine("No hay mascotas registradas para emitir sonidos.");
            return;
        }

        Console.WriteLine("Invocando método polimórfico EmitirSonido() desde la jerarquía Animal -> Mascota:\n");

        foreach (Animal animal in todasLasMascotas)
        {
            Console.WriteLine($" -> {animal.Nombre} ({animal.Especie}): {animal.EmitirSonido()}");
        }
    }

    // TASK 6: Demostración de Abstracción con ServicioVeterinario (ConsultaGeneral / Vacunacion)
    public static void DemostrarServiciosVeterinarios(List<Paciente> lista)
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine("    DEMOSTRACIÓN DE ABSTRACCIÓN Y CLASES  ");
        Console.WriteLine("==========================================");

        if (!lista.Any() || !lista.Any(p => p.Mascotas.Any()))
        {
            Console.WriteLine("Debe haber al menos un paciente con mascota para realizar una atención médica.");
            return;
        }

        int id = LeerEntero("Ingrese el ID del paciente/dueño: ");
        var paciente = lista.FirstOrDefault(p => p.Id == id);

        if (paciente == null || !paciente.Mascotas.Any())
        {
            Console.WriteLine("Paciente no encontrado o sin mascotas registradas.");
            return;
        }

        var mascota = paciente.Mascotas.First();

        Console.WriteLine("\nSeleccione el servicio veterinario:");
        Console.WriteLine("1. Consulta General");
        Console.WriteLine("2. Vacunación");
        Console.Write("Opción: ");
        string? opcion = Console.ReadLine();

        ServicioVeterinario servicio;

        if (opcion == "2")
        {
            string vacuna = LeerTextoNoVacio("Nombre de la vacuna (ej. Antirrábica, Séxtuple): ");
            servicio = new Vacunacion(vacuna);
        }
        else
        {
            string motivo = LeerTextoNoVacio("Motivo de la consulta / diagnóstico preliminar: ");
            servicio = new ConsultaGeneral { Diagnostico = motivo };
        }

        // Llamada polimórfica al método abstracto Atender
        servicio.Atender(paciente, mascota);
    }

    // TASK 6: Demostración de uso de Interfaces (IRegistrable)
    public static void DemostrarInterfazRegistrable(List<Paciente> lista)
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine("    DEMOSTRACIÓN DE INTERFAZ IRegistrable ");
        Console.WriteLine("==========================================");

        List<IRegistrable> registrables = new List<IRegistrable>();

        foreach (var p in lista)
        {
            registrables.Add(p);
            foreach (var m in p.Mascotas)
            {
                registrables.Add(m);
            }
        }

        Console.WriteLine($"Procesando {registrables.Count} elementos que implementan IRegistrable:\n");
        foreach (var item in registrables)
        {
            item.Registrar();
        }
    }

    // Métodos auxiliares
    private static Mascota CrearMascotaDesdeConsola()
    {
        string nombreMascota = LeerTextoNoVacio("Nombre de la mascota: ");
        string especie = LeerTextoNoVacio("Especie (ej. Perro, Gato, Loro): ");
        Console.Write("Raza (opcional): ");
        string raza = Console.ReadLine()?.Trim() ?? string.Empty;
        int edadMascota = LeerEntero("Edad de la mascota (en años): ");

        return new Mascota(nombreMascota, edadMascota, especie, string.IsNullOrWhiteSpace(raza) ? "Sin raza" : raza);
    }

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
