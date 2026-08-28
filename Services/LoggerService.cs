using System;
using System.IO;

namespace ClinicaSalud.Services;

/*
 * TASK 6: Sistema básico de Logging
 * - En un entorno real, registrar errores con timestamp y stack trace permite diagnosticar fallos
 *   sin interrumpir al usuario y facilita el soporte técnico post-mortem.
 */
public static class LoggerService
{
    private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "clinica_errores.log");

    public static void LogError(string mensaje, Exception? ex = null)
    {
        string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ERROR] {mensaje}";
        if (ex != null)
        {
            logEntry += $"\n   Tipo: {ex.GetType().Name}\n   Detalle: {ex.Message}\n   StackTrace: {ex.StackTrace}";
        }

        try
        {
            File.AppendAllText(LogFilePath, logEntry + Environment.NewLine + new string('-', 60) + Environment.NewLine);
        }
        catch
        {
            // Failsafe en caso de problemas de I/O
            Console.WriteLine("[WARN] No se pudo escribir en el archivo de log.");
        }
    }

    public static void LogInfo(string mensaje)
    {
        string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [INFO] {mensaje}";
        try
        {
            File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
        }
        catch { }
    }

    public static string ObtenerRutaLog() => LogFilePath;
}
