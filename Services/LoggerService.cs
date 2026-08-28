using System;
using System.IO;
using System.Threading.Tasks;

namespace ClinicaSalud.Services;

/*
 * CONVENCIÓN DE CODIFICACIÓN:
 * - PascalCase: Nombre de clase (LoggerService), métodos (LogErrorAsync, LogInfoAsync)
 * - camelCase: Parámetros y variables locales (mensaje, excepcion, rutaArchivo)
 * - Programación Asíncrona: Escritura I/O no bloqueante con File.AppendAllTextAsync
 */
public static class LoggerService
{
    private static readonly string RutaArchivoLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "clinica_errores.log");

    public static async Task LogErrorAsync(string mensaje, Exception? excepcion = null)
    {
        string entradaLog = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ERROR] {mensaje}";
        if (excepcion != null)
        {
            entradaLog += $"\n   Tipo: {excepcion.GetType().Name}\n   Detalle: {excepcion.Message}\n   StackTrace: {excepcion.StackTrace}";
        }

        try
        {
            await File.AppendAllTextAsync(RutaArchivoLog, entradaLog + Environment.NewLine + new string('-', 60) + Environment.NewLine);
        }
        catch
        {
            Console.WriteLine("[WARN] No se pudo escribir en el archivo de log.");
        }
    }

    public static async Task LogInfoAsync(string mensaje)
    {
        string entradaLog = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [INFO] {mensaje}";
        try
        {
            await File.AppendAllTextAsync(RutaArchivoLog, entradaLog + Environment.NewLine);
        }
        catch { }
    }

    public static string ObtenerRutaLog() => RutaArchivoLog;
}
