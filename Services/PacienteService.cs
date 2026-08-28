using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ClinicaSalud.Models;

namespace ClinicaSalud.Services;

/*
 * =========================================================================
 * TASK 1 & TASK 5: FUNDAMENTOS Y BUENAS PRÁCTICAS ASÍNCRONAS
 * =========================================================================
 * - ¿Qué problemas resuelve la programación asíncrona?
 *   Evita el bloqueo de hilos (thread starvation) durante operaciones I/O-bound
 *   (lectura/escritura de archivos, base de datos, APIs de red, pasarelas de pago).
 * - Síncrono vs Asíncrono:
 *   En síncrono, el hilo espera inactivo a que la operación de I/O termine.
 *   En asíncrono, la palabra clave 'await' cede el hilo de vuelta al ThreadPool,
 *   permitiendo atender otras solicitudes del usuario o procesos del sistema.
 * - Cuándo usar async/await:
 *   Operaciones de Entrada/Salida (I/O) intensivas, llamadas a red o demoras programadas.
 * - Cuándo usar Task.Run:
 *   Operaciones intensivas de procesamiento (CPU-bound) o para delegar tareas a hilos secundarios en segundo plano.
 * - Regla de Oro:
 *   NUNCA usar '.Result' ni '.Wait()' sobre tareas asíncronas para evitar bloqueos mutuos (Deadlocks).
 */
public static class PacienteService
{
    // =========================================================================
    // TASK 2: MÉTODO ASÍNCRONO CON ASYNC Y AWAIT
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

            // Mensajes visuales del ciclo asíncrono (antes, durante y después)
            Console.WriteLine("\n[1/3] [Inicio] Preparando solicitud y guardado no bloqueante...");
            
            // Simulación no bloqueante de I/O
            await Task.Delay(1200); 

            Console.WriteLine("[2/3] [En Progreso] Validando persistencia de datos en almacenamiento asíncrono...");
            await Task.Delay(1000);

            listaPacientes.Add(paciente);
            paciente.Registrar();
            mascota.Registrar();

            // Tarea de registro en segundo plano con Task.Run
            _ = Task.Run(async () =>
            {
                await LoggerService.LogInfoAsync($"Paciente #{paciente.Id} '{paciente.Nombre}' registrado en segundo plano.");
            });

            Console.WriteLine($"[3/3] [Finalizado] ¡Éxito! Paciente #{paciente.Id} guardado de forma asíncrona sin congelar la app.");
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
    // TASK 3: TAREAS PARALELAS CON TASK, Task.Run Y Task.WhenAll
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
        Console.WriteLine($"Lanzando 3 tareas simultáneas en segundo plano (Task.Run) para '{paciente.Nombre}' (ID: {paciente.Id})...\n");

        var cronometro = Stopwatch.StartNew();

        // TASK 3: Uso explícito de Task.Run para delegar cada tarea concurrente a segundo plano
        Task<string> tareaHistorial = Task.Run(() => CargarHistorialClinicoAsync(paciente.Id));
        Task<string> tareaCita = Task.Run(() => AgendarCitaAsync(paciente.Id));
        Task<string> tareaNotificacion = Task.Run(() => EnviarNotificacionAsync(paciente.Telefono, "Su cita veterinaria ha sido confirmada."));

        // Espera no bloqueante de todas las tareas paralelas
        string[] resultados = await Task.WhenAll(tareaHistorial, tareaCita, tareaNotificacion);

        cronometro.Stop();

        Console.WriteLine("\nResultados obtenidos de la ejecución concurrente:");
        foreach (var resultado in resultados)
        {
            Console.WriteLine($"  ✔ {resultado}");
        }

        Console.WriteLine($"\n[Rendimiento]: Todas las tareas concurrentes finalizaron en {cronometro.ElapsedMilliseconds} ms.");
    }

    private static async Task<string> CargarHistorialClinicoAsync(int pacienteId)
    {
        Console.WriteLine("  -> [Inicio] Cargando historial clínico desde servidor central...");
        await Task.Delay(1800); // Simula consulta a BD
        Console.WriteLine("  -> [Fin] Historial clínico cargado.");
        return $"Historial médico cargado correctamente para ID #{pacienteId}";
    }

    private static async Task<string> AgendarCitaAsync(int pacienteId)
    {
        Console.WriteLine("  -> [Inicio] Verificando agenda médica veterinaria...");
        await Task.Delay(1200); // Simula validación de agenda
        Console.WriteLine("  -> [Fin] Espacio de cita reservado.");
        return $"Cita médica programada con éxito para ID #{pacienteId}";
    }

    private static async Task<string> EnviarNotificacionAsync(string telefono, string mensaje)
    {
        Console.WriteLine("  -> [Inicio] Conectando con pasarela SMS de notificaciones...");
        await Task.Delay(1500); // Simula servicio externo SMS
        Console.WriteLine("  -> [Fin] Notificación entregada.");
        return $"Mensaje enviado a '{telefono}': \"{mensaje}\"";
    }

    // =========================================================================
    // TASK 4: CONCURRENCIA, Task.WhenAll vs Task.WhenAny Y REGISTRO MÚLTIPLE
    // =========================================================================

    public static async Task DemostrarWhenAllVsWhenAnyAsync()
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine("    COMPARATIVA: Task.WhenAll vs WhenAny  ");
        Console.WriteLine("==========================================");

        Console.WriteLine("Escenario: Consultando resultados a 3 laboratorios externos que compiten en velocidad...");

        // Simulamos 3 servicios de laboratorio con diferentes tiempos de respuesta
        Task<string> labA = Task.Run(() => ConsultarLaboratorioAsync("Laboratorio Central (Estándar)", 2400));
        Task<string> labB = Task.Run(() => ConsultarLaboratorioAsync("Laboratorio Express (Prioritario)", 800));
        Task<string> labC = Task.Run(() => ConsultarLaboratorioAsync("Laboratorio Metropolitano (Respaldo)", 1500));

        var tareasLaboratorio = new List<Task<string>> { labA, labB, labC };

        // 1. Task.WhenAny: Atiende la primera respuesta que llegue
        Console.WriteLine("\n1. [Task.WhenAny] Reaccionando de inmediato a la primera respuesta disponible:");
        Task<string> tareaMasRapida = await Task.WhenAny(tareasLaboratorio);
        string primerResultado = await tareaMasRapida;
        Console.WriteLine($"   ★ Primer resultado recibido: {primerResultado}");

        // 2. Task.WhenAll: Espera la consolidación de todos los reportes
        Console.WriteLine("\n2. [Task.WhenAll] Esperando a que el resto de los laboratorios terminen su informe:");
        string[] todosLosResultados = await Task.WhenAll(tareasLaboratorio);
        Console.WriteLine("   Consolidado general de todos los laboratorios:");
        foreach (var reporte in todosLosResultados)
        {
            Console.WriteLine($"   - {reporte}");
        }
    }

    private static async Task<string> ConsultarLaboratorioAsync(string nombreLab, int demoraMs)
    {
        await Task.Delay(demoraMs);
        return $"{nombreLab} - Completado en {demoraMs}ms";
    }

    public static async Task SimularRegistroMultipleMascotasAsync(List<Paciente> listaPacientes)
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine("  REGISTRO CONCURRENTE DE MÚLTIPLES MASCOTAS");
        Console.WriteLine("==========================================");

        if (!listaPacientes.Any())
        {
            Console.WriteLine("No hay pacientes registrados.");
            return;
        }

        var paciente = listaPacientes.First();
        var nuevasMascotas = new List<Mascota>
        {
            new Mascota("Max", 2, "Perro", "Pastor Alemán"),
            new Mascota("Luna", 1, "Gato", "Persa"),
            new Mascota("Kiwi", 3, "Loro", "Cacatúa")
        };

        Console.WriteLine($"Iniciando registro paralelo de {nuevasMascotas.Count} mascotas para el dueño '{paciente.Nombre}'...\n");

        var tareasRegistro = nuevasMascotas.Select(m => Task.Run(async () =>
        {
            Console.WriteLine($"  [Inicio Registro] Guardando ficha clínica de '{m.Nombre}' ({m.Especie})...");
            int demora = new Random().Next(700, 1600);
            await Task.Delay(demora);
            paciente.AgregarMascota(m);
            Console.WriteLine($"  [✔ Registrada] Mascota '{m.Nombre}' guardada con éxito ({demora}ms).");
        })).ToList();

        // TASK 4: Esperamos a que todas las mascotas hayan finalizado su registro simultáneo
        await Task.WhenAll(tareasRegistro);

        Console.WriteLine($"\n¡Todas las ({nuevasMascotas.Count}) mascotas fueron registradas simultáneamente con éxito!");
    }

    public static async Task SimularAtencionConcurrenteAsync(List<Paciente> listaPacientes)
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine("     ATENCIÓN CONCURRENTE EN CONSULTORIOS  ");
        Console.WriteLine("==========================================");

        var mascotas = listaPacientes.SelectMany(p => p.Mascotas).ToList();

        if (!mascotas.Any())
        {
            Console.WriteLine("No hay mascotas registradas para atender.");
            return;
        }

        Console.WriteLine($"Atendiendo {mascotas.Count} mascotas simultáneamente en diferentes consultorios con Task.WhenAll:\n");

        var tareasAtencion = mascotas.Select(m => Task.Run(async () =>
        {
            Console.WriteLine($" [Consultorio] Atendiendo a '{m.Nombre}' ({m.Especie})...");
            int tiempoAtencion = new Random().Next(800, 2000);
            await Task.Delay(tiempoAtencion);
            Console.WriteLine($" [Consultorio] ✔ '{m.Nombre}' fue atendido exitosamente ({tiempoAtencion}ms). Sonido: {m.EmitirSonido()}");
        })).ToList();

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

    // Métodos auxiliares de entrada por consola
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
