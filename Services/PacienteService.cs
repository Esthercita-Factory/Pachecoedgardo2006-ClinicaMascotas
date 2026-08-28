using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ClinicaSalud.Models;

namespace ClinicaSalud.Services;

/*
 * FUNDAMENTOS DE PROGRAMACIÓN ASÍNCRONA (TASK 1):
 * - La programación asíncrona (async/await) resuelve el problema de bloqueo de hilos durante operaciones
 *   intensivas de Entrada/Salida (I/O) como lectura de archivos, bases de datos o peticiones web.
 * - En lugar de congelar la interfaz o la aplicación esperando una respuesta, el hilo se libera para atender
 *   otras tareas hasta que la operación asíncrona finaliza.
 * - BUENA PRÁCTICA (TASK 5): Siempre propagar 'await' y evitar '.Result' o '.Wait()' que causan bloqueos (deadlocks).
 */
public static class PacienteService
{
    // =========================================================================
    // TASK 2: MÉTODO ASÍNCRONO CON ASYNC / AWAIT (Simulación no bloqueante)
    // =========================================================================

    public static async Task RegistrarPacienteAsync(List<Paciente> listaPacientes)
    {
        Console.WriteLine("\n--- REGISTRO ASÍNCRONO DE PACIENTE Y MASCOTA ---");

        try
        {
            string nombre = LeerTextoNoVacio("Nombre del paciente/dueño: ");
            int edad = LeerEntero("Edad: ");
            string direccion = LeerTextoNoVacio("Dirección: ");
            string telefono = LeerTextoNoVacio("Teléfono: ");

            int nuevoId = listaPacientes.Count > 0 ? listaPacientes.Max(p => p.Id) + 1 : 1;
            var paciente = new Paciente(nuevoId, nombre, edad, direccion, telefono);

            Console.WriteLine("\n--- DATOS DE LA MASCOTA ---");
            Mascota mascota = CrearMascotaDesdeConsola();
            paciente.AgregarMascota(mascota);

            // Mensajes visuales del ciclo asíncrono
            Console.WriteLine("\n[1/3] Iniciando guardado asíncrono en base de datos / almacenamiento...");
            
            // Simulación no bloqueante de I/O
            await Task.Delay(1500); 

            Console.WriteLine("[2/3] Procesando y validando persistencia en segundo plano...");
            await Task.Delay(1000);

            listaPacientes.Add(paciente);
            paciente.Registrar();
            mascota.Registrar();

            await LoggerService.LogInfoAsync($"Paciente #{paciente.Id} '{paciente.Nombre}' registrado asíncronamente.");

            Console.WriteLine($"[3/3] ¡Éxito! Paciente #{paciente.Id} guardado de forma no bloqueante.");
        }
        catch (Exception excepcion)
        {
            await LoggerService.LogErrorAsync("Error en RegistrarPacienteAsync", excepcion);
            Console.WriteLine($"[Error]: {excepcion.Message}");
        }
        finally
        {
            Console.WriteLine("[Operación de registro finalizada]");
        }
    }

    // =========================================================================
    // TASK 3: TAREAS PARALELAS CON Task.WhenAll
    // =========================================================================

    public static async Task EjecutarProcesosParalelosAsync(List<Paciente> listaPacientes)
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine("    PROCESOS CLÍNICOS EN PARALELO (TASK)  ");
        Console.WriteLine("==========================================");

        if (!listaPacientes.Any())
        {
            Console.WriteLine("No hay pacientes registrados para procesar.");
            return;
        }

        var paciente = listaPacientes.First();
        Console.WriteLine($"Ejecutando 3 tareas simultáneas para el paciente '{paciente.Nombre}' (ID: {paciente.Id})...\n");

        var cronometro = Stopwatch.StartNew();

        // Lanzamos las 3 tareas en paralelo
        Task<string> tareaHistorial = CargarHistorialClinicoAsync(paciente.Id);
        Task<string> tareaCita = AgendarCitaAsync(paciente.Id);
        Task<string> tareaNotificacion = EnviarNotificacionAsync(paciente.Telefono, "Su cita ha sido confirmada.");

        // TASK 3: Esperamos la finalización de todas las tareas concurrentes
        string[] resultados = await Task.WhenAll(tareaHistorial, tareaCita, tareaNotificacion);

        cronometro.Stop();

        Console.WriteLine("\nResultados obtenidos de la ejecución concurrente:");
        foreach (var resultado in resultados)
        {
            Console.WriteLine($"  ✔ {resultado}");
        }

        Console.WriteLine($"\n[Rendimiento]: Todas las tareas completadas en paralelo en solo {cronometro.ElapsedMilliseconds} ms.");
    }

    private static async Task<string> CargarHistorialClinicoAsync(int pacienteId)
    {
        Console.WriteLine("  -> [Inicio] Cargando historial clínico desde servidor central...");
        await Task.Delay(1800); // Simula consulta a base de datos
        Console.WriteLine("  -> [Fin] Historial clínico cargado.");
        return $"Historial médico cargado correctamente para ID #{pacienteId}";
    }

    private static async Task<string> AgendarCitaAsync(int pacienteId)
    {
        Console.WriteLine("  -> [Inicio] Verificando disponibilidad de agenda veterinaria...");
        await Task.Delay(1200); // Simula validación de agenda
        Console.WriteLine("  -> [Fin] Espacio de cita reservado.");
        return $"Cita médica programada con éxito para ID #{pacienteId}";
    }

    private static async Task<string> EnviarNotificacionAsync(string telefono, string mensaje)
    {
        Console.WriteLine("  -> [Inicio] Conectando con pasarela de mensajería SMS/Email...");
        await Task.Delay(1500); // Simula servicio externo de notificaciones
        Console.WriteLine("  -> [Fin] Notificación entregada.");
        return $"Mensaje enviado a '{telefono}': \"{mensaje}\"";
    }

    // =========================================================================
    // TASK 4: Task.WhenAll vs Task.WhenAny (Concurrencia y Competencia)
    // =========================================================================

    public static async Task DemostrarWhenAllVsWhenAnyAsync()
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine("    COMPARATIVA: Task.WhenAll vs WhenAny  ");
        Console.WriteLine("==========================================");

        Console.WriteLine("Escenario: Consultando resultados de laboratorio a 3 laboratorios externos...");

        // Simulamos 3 servicios de laboratorio con diferentes tiempos de respuesta
        Task<string> labA = ConsultarLaboratorioAsync("Laboratorio Central", 2500);
        Task<string> labB = ConsultarLaboratorioAsync("Laboratorio Express", 900);
        Task<string> labC = ConsultarLaboratorioAsync("Laboratorio Metropolitano", 1600);

        var tareasLaboratorio = new List<Task<string>> { labA, labB, labC };

        // 1. Task.WhenAny: Atiende la primera respuesta que llegue
        Console.WriteLine("\n1. [Task.WhenAny] Esperando la respuesta más rápida para atención inmediata:");
        Task<string> tareaMasRapida = await Task.WhenAny(tareasLaboratorio);
        string resultadoRapido = await tareaMasRapida;
        Console.WriteLine($"   ¡Primer resultado disponible recibido!: {resultadoRapido}");

        // 2. Task.WhenAll: Espera que todos los laboratorios terminen su informe
        Console.WriteLine("\n2. [Task.WhenAll] Esperando a que el resto de los laboratorios finalicen...");
        string[] todosLosResultados = await Task.WhenAll(tareasLaboratorio);
        Console.WriteLine("   Todos los reportes han sido consolidados:");
        foreach (var r in todosLosResultados)
        {
            Console.WriteLine($"   - {r}");
        }
    }

    private static async Task<string> ConsultarLaboratorioAsync(string nombreLab, int demoraMs)
    {
        await Task.Delay(demoraMs);
        return $"{nombreLab} (Completado en {demoraMs}ms)";
    }

    // Simulación de atención concurrente de pacientes
    public static async Task SimularAtencionConcurrenteAsync(List<Paciente> listaPacientes)
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine("     ATENCIÓN CONCURRENTE DE MASCOTAS     ");
        Console.WriteLine("==========================================");

        var mascotas = listaPacientes.SelectMany(p => p.Mascotas).ToList();

        if (!mascotas.Any())
        {
            Console.WriteLine("No hay mascotas registradas para atender.");
            return;
        }

        Console.WriteLine($"Atendiendo {mascotas.Count} mascotas simultáneamente en diferentes consultorios:\n");

        var tareasAtencion = mascotas.Select(async m =>
        {
            Console.WriteLine($" [Consultorio] Iniciando revisión de '{m.Nombre}' ({m.Especie})...");
            int tiempoAtencion = new Random().Next(800, 2000);
            await Task.Delay(tiempoAtencion);
            Console.WriteLine($" [Consultorio] ✔ '{m.Nombre}' fue atendido exitosamente ({tiempoAtencion}ms). Sonido: {m.EmitirSonido()}");
        });

        // Espera a que todas las mascotas terminen su consulta
        await Task.WhenAll(tareasAtencion);

        Console.WriteLine("\n¡Todas las consultas concurrentes han concluido satisfactoriamente!");
    }

    public static void ListarPacientes(List<Paciente> listaPacientes)
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine("       LISTA DE PACIENTES Y MASCOTAS      ");
        Console.WriteLine("==========================================");

        if (!listaPacientes.Any())
        {
            Console.WriteLine("No hay pacientes registrados.");
            return;
        }

        foreach (var p in listaPacientes)
        {
            p.MostrarInformacion();
        }
    }

    // Métodos auxiliares de entrada
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
