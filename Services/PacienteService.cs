using System;
using System.Collections.Generic;
using System.Linq;
using ClinicaSalud.Models;

namespace ClinicaSalud.Services;

public static class PacienteService
{
    // TASK 2 y 5: Registrar paciente con try-catch-finally y validaciones
    public static void RegistrarPaciente(List<Paciente> lista)
    {
        Console.WriteLine("\n--- REGISTRO DE NUEVO PACIENTE Y MASCOTA ---");

        try
        {
            string nombre = LeerTextoNoVacio("Nombre del paciente/dueño: ");
            int edad = LeerEntero("Edad: ");
            string direccion = LeerTextoNoVacio("Dirección: ");
            string telefono = LeerTextoNoVacio("Teléfono de contacto: ");

            int nuevoId = lista.Count > 0 ? lista.Max(p => p.Id) + 1 : 1;
            var paciente = new Paciente(nuevoId, nombre, edad, direccion, telefono);

            Console.WriteLine("\n--- DATOS DE LA MASCOTA PRINCIPAL ---");
            Mascota mascota = CrearMascotaDesdeConsola();
            paciente.AgregarMascota(mascota);

            Console.Write("¿Desea registrar una mascota adicional? (s/n): ");
            if (Console.ReadLine()?.Trim().ToLower() == "s")
            {
                Mascota otraMascota = CrearMascotaDesdeConsola();
                paciente.AgregarMascota(otraMascota);
            }

            lista.Add(paciente);

            // TASK 2: Invocación polimórfica de IRegistrable
            paciente.Registrar();
            foreach (var m in paciente.Mascotas)
            {
                m.Registrar();
            }

            LoggerService.LogInfo($"Paciente #{paciente.Id} registrado exitosamente.");
            Console.WriteLine($"\n¡Paciente #{paciente.Id} guardado con éxito!");
        }
        catch (Exception ex)
        {
            LoggerService.LogError("Error durante el registro del paciente.", ex);
            Console.WriteLine($"[Error al registrar]: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("[Fin del proceso de registro]");
        }
    }

    // TASK 5: Buscar mascota con excepción personalizada MascotaNoEncontradaException
    public static void BuscarMascotaPorNombre(List<Paciente> lista)
    {
        Console.WriteLine("\n--- BÚSQUEDA DE MASCOTA POR NOMBRE ---");
        Console.Write("Ingrese el nombre de la mascota a buscar: ");
        string? nombreBuscado = Console.ReadLine();

        try
        {
            if (string.IsNullOrWhiteSpace(nombreBuscado))
            {
                throw new ArgumentException("El nombre de la mascota no puede estar vacío.");
            }

            var pacienteDueño = lista.FirstOrDefault(p => p.Mascotas.Any(m => m.Nombre.Equals(nombreBuscado.Trim(), StringComparison.OrdinalIgnoreCase)));

            if (pacienteDueño == null)
            {
                // TASK 5: Lanzamiento de excepción personalizada
                throw new MascotaNoEncontradaException(nombreBuscado.Trim());
            }

            var mascotaEncontrada = pacienteDueño.Mascotas.First(m => m.Nombre.Equals(nombreBuscado.Trim(), StringComparison.OrdinalIgnoreCase));

            Console.WriteLine("\n[Mascota Encontrada]");
            mascotaEncontrada.MostrarInformacion();
            Console.WriteLine($"Dueño responsable: {pacienteDueño.Nombre} (Tel: {pacienteDueño.Telefono})");
        }
        catch (MascotaNoEncontradaException ex)
        {
            LoggerService.LogError($"Búsqueda fallida de mascota: {ex.NombreMascotaBuscada}", ex);
            Console.WriteLine($"[Aviso]: {ex.Message}");
        }
        catch (Exception ex)
        {
            LoggerService.LogError("Error inesperado en búsqueda de mascota.", ex);
            Console.WriteLine($"[Error]: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("[Búsqueda finalizada]");
        }
    }

    // TASK 3: Demostrar interfaz INotificable
    public static void EnviarRecordatorioCita(List<Paciente> lista)
    {
        Console.WriteLine("\n--- ENVIAR RECORDATORIO DE CITA (INotificable) ---");
        int id = LeerEntero("Ingrese el ID del paciente/dueño a notificar: ");

        try
        {
            var paciente = lista.FirstOrDefault(p => p.Id == id);
            if (paciente == null)
            {
                throw new PacienteNoEncontradoException(id);
            }

            string mensaje = $"Estimado/a {paciente.Nombre}, le recordamos la cita médica de su mascota para el día de mañana.";

            // Invocación a través de la interfaz INotificable
            INotificable canalNotificacion = paciente;
            canalNotificacion.EnviarNotificacion(mensaje);

            LoggerService.LogInfo($"Notificación enviada a paciente #{id}.");
        }
        catch (PacienteNoEncontradoException ex)
        {
            LoggerService.LogError($"Fallo al enviar notificación: Paciente ID {ex.IdBuscado} no existe.", ex);
            Console.WriteLine($"[Aviso]: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("[Proceso de notificación concluido]");
        }
    }

    // TASK 2: Demostrar interfaz IAtendible con servicios veterinarios
    public static void AtenderServicioVeterinario(List<Paciente> lista)
    {
        Console.WriteLine("\n--- ATENCIÓN DE SERVICIO VETERINARIO (IAtendible) ---");
        int id = LeerEntero("Ingrese el ID del paciente/dueño: ");

        try
        {
            var paciente = lista.FirstOrDefault(p => p.Id == id);
            if (paciente == null)
            {
                throw new PacienteNoEncontradoException(id);
            }

            if (!paciente.Mascotas.Any())
            {
                throw new InvalidOperationException($"El paciente {paciente.Nombre} no tiene mascotas registradas.");
            }

            var mascota = paciente.Mascotas.First();

            Console.WriteLine("\nSeleccione el servicio veterinario:");
            Console.WriteLine("1. Consulta General ($35.00)");
            Console.WriteLine("2. Vacunación ($25.00)");
            Console.Write("Opción: ");
            string? opcion = Console.ReadLine();

            // Uso polimórfico de la interfaz IAtendible
            IAtendible servicio = opcion == "2"
                ? new Vacunacion(LeerTextoNoVacio("Nombre de la vacuna aplicada: "))
                : new ConsultaGeneral { Diagnostico = LeerTextoNoVacio("Diagnóstico / Motivo de consulta: ") };

            servicio.Atender(paciente, mascota);
            LoggerService.LogInfo($"Servicio '{servicio.NombreServicio}' atendido para mascota '{mascota.Nombre}'.");
        }
        catch (PacienteNoEncontradoException ex)
        {
            LoggerService.LogError($"Fallo al atender servicio: {ex.Message}", ex);
            Console.WriteLine($"[Aviso]: {ex.Message}");
        }
        catch (Exception ex)
        {
            LoggerService.LogError("Error durante la atención médica.", ex);
            Console.WriteLine($"[Error]: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("[Atención médica finalizada]");
        }
    }

    // TASK 4: Escenario de depuración (Breakpoints e inspección de variables)
    public static void ProbarEscenarioDepuracion()
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine("     ESCENARIO DE DEPURACIÓN Y DEBUG      ");
        Console.WriteLine("==========================================");
        Console.WriteLine("-> Coloque un Breakpoint (Punto de interrupción) en la siguiente línea:");
        Console.WriteLine("   int totalPacientes = 10;");

        // Breakpoint sugerido aquí:
        int totalPacientes = 10;
        int divisor = 0; // Provocamos división por cero controlada para depurar

        Console.WriteLine($"Variables actuales en memoria: totalPacientes = {totalPacientes}, divisor = {divisor}");

        try
        {
            Console.WriteLine("Calculando promedio de atención (totalPacientes / divisor)...");
            int promedio = totalPacientes / divisor; // Lanza DivideByZeroException
            Console.WriteLine($"Promedio calculado: {promedio}");
        }
        catch (DivideByZeroException ex)
        {
            LoggerService.LogError("Error de división por cero detectado y controlado en modo depuración.", ex);
            Console.WriteLine($"\n[Excepción capturada con éxito]: {ex.GetType().Name} -> {ex.Message}");
            Console.WriteLine("[Depuración]: Se ha verificado que el divisor era 0 antes de realizar la operación.");
        }
        finally
        {
            Console.WriteLine("[Bloque finally ejecutado tras la depuración]");
        }
    }

    public static void ListarPacientes(List<Paciente> lista)
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine("       LISTA DE PACIENTES Y MASCOTAS      ");
        Console.WriteLine("==========================================");

        if (!lista.Any())
        {
            Console.WriteLine("No hay pacientes registrados.");
            return;
        }

        foreach (var p in lista)
        {
            p.MostrarInformacion();
        }
    }

    // Validaciones
    private static Mascota CrearMascotaDesdeConsola()
    {
        string nombreMascota = LeerTextoNoVacio("Nombre de la mascota: ");
        string especie = LeerTextoNoVacio("Especie (ej. Perro, Gato, Ave): ");
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
