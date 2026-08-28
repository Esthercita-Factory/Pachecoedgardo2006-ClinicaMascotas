using System;
using System.Collections.Generic;
using System.Linq;
using ClinicaSalud.Models;

namespace ClinicaSalud.Services;

public static class PacienteService
{
    // ==========================================
    // TASK 1: GESTIÓN DE COLECCIONES (List y Dictionary)
    // ==========================================

    public static void RegistrarPaciente(List<Paciente> lista, Dictionary<int, Paciente> diccionario)
    {
        Console.WriteLine("\n--- REGISTRAR PACIENTE Y MASCOTA ---");

        string nombre = LeerTextoNoVacio("Nombre del dueño: ");
        string telefono = LeerTextoNoVacio("Teléfono: ");
        int edad = LeerEntero("Edad del dueño: ");
        string nombreMascota = LeerTextoNoVacio("Nombre de la mascota: ");
        string especie = LeerTextoNoVacio("Especie (ej. Perro, Gato): ");
        Console.Write("Raza (deje vacío si no tiene): ");
        string raza = Console.ReadLine()?.Trim() ?? string.Empty;
        string sintoma = LeerTextoNoVacio("Síntoma: ");

        int nuevoId = lista.Count > 0 ? lista.Max(p => p.Id) + 1 : 1;

        var nuevoPaciente = new Paciente
        {
            Id = nuevoId,
            Nombre = nombre,
            Telefono = telefono,
            Edad = edad,
            NombreMascota = nombreMascota,
            Especie = especie,
            Raza = string.IsNullOrWhiteSpace(raza) ? "Sin raza" : raza,
            Sintoma = sintoma
        };

        lista.Add(nuevoPaciente);
        diccionario[nuevoId] = nuevoPaciente; // Acceso rápido por clave en Diccionario

        Console.WriteLine($"\n¡Paciente #{nuevoId} registrado con éxito!");
    }

    public static void ModificarPaciente(List<Paciente> lista, Dictionary<int, Paciente> diccionario)
    {
        Console.WriteLine("\n--- MODIFICAR PACIENTE ---");
        int id = LeerEntero("Ingrese el ID del paciente a modificar: ");

        // Uso de Dictionary para acceso rápido por ID (O(1))
        if (!diccionario.TryGetValue(id, out var paciente))
        {
            Console.WriteLine($"No se encontró ningún paciente con el ID {id}.");
            return;
        }

        Console.WriteLine($"Modificando datos de: {paciente.Nombre} (Mascota: {paciente.NombreMascota})");
        paciente.Telefono = LeerTextoNoVacio($"Nuevo teléfono ({paciente.Telefono}): ");
        paciente.Sintoma = LeerTextoNoVacio($"Nuevo síntoma ({paciente.Sintoma}): ");

        Console.WriteLine("¡Paciente modificado con éxito!");
    }

    public static void EliminarPaciente(List<Paciente> lista, Dictionary<int, Paciente> diccionario)
    {
        Console.WriteLine("\n--- ELIMINAR PACIENTE ---");
        int id = LeerEntero("Ingrese el ID del paciente a eliminar: ");

        if (diccionario.TryGetValue(id, out var paciente))
        {
            lista.Remove(paciente);
            diccionario.Remove(id);
            Console.WriteLine($"Paciente con ID {id} eliminado correctamente.");
        }
        else
        {
            Console.WriteLine($"No se encontró ningún paciente con el ID {id}.");
        }
    }

    public static void ListarPacientes(List<Paciente> lista)
    {
        Console.WriteLine("\n--- LISTA DE PACIENTES REGISTRADOS ---");

        if (!lista.Any()) // Uso de LINQ: Any() para verificar si la colección contiene elementos
        {
            Console.WriteLine("No hay pacientes registrados.");
            return;
        }

        foreach (var p in lista)
        {
            Console.WriteLine($"ID: {p.Id} | Dueño: {p.Nombre} (Edad: {p.Edad}, Tel: {p.Telefono}) | Mascota: {p.NombreMascota} | Especie: {p.Especie} | Raza: {p.Raza} | Síntoma: {p.Sintoma}");
        }
    }

    public static void BuscarPorIdEnDiccionario(Dictionary<int, Paciente> diccionario)
    {
        Console.WriteLine("\n--- BÚSQUEDA RÁPIDA POR ID (DICTIONARY) ---");
        int id = LeerEntero("Ingrese el ID a consultar: ");

        if (diccionario.TryGetValue(id, out var p))
        {
            Console.WriteLine($"[Encontrado en Diccionario] ID: {p.Id} | Dueño: {p.Nombre} | Tel: {p.Telefono} | Mascota: {p.NombreMascota} ({p.Especie}) | Síntoma: {p.Sintoma}");
        }
        else
        {
            Console.WriteLine($"No existe paciente con el ID {id}.");
        }
    }

    // ==========================================
    // TASK 2, 4 Y 5: CONSULTAS CON LINQ
    // ==========================================

    public static void EjecutarConsultasLinq(List<Paciente> lista)
    {
        if (!lista.Any())
        {
            Console.WriteLine("\nDebe registrar al menos un paciente para realizar consultas LINQ.");
            return;
        }

        Console.WriteLine("\n==========================================");
        Console.WriteLine("        REPORTES Y CONSULTAS CON LINQ     ");
        Console.WriteLine("==========================================");

        // --- TASK 2: Comparación Sintaxis de Consulta vs Sintaxis de Métodos ---
        Console.WriteLine("\n1. [Sintaxis de Consulta] Pacientes mayores de 25 años:");
        // Sintaxis de consulta (Query Syntax): from ... where ... select
        var consultaEdad = from p in lista
                           where p.Edad > 25
                           select p;
        foreach (var p in consultaEdad)
            Console.WriteLine($"   - {p.Nombre} ({p.Edad} años)");

        Console.WriteLine("\n2. [Sintaxis de Métodos] Pacientes con mascotas ordenados descendentemente por edad:");
        // Sintaxis de métodos con OrderByDescending y Select
        var metodosOrden = lista
            .OrderByDescending(p => p.Edad)
            .Select(p => new { p.Nombre, p.Edad, p.NombreMascota });
        foreach (var item in metodosOrden)
            Console.WriteLine($"   - Dueño: {item.Nombre}, Edad: {item.Edad}, Mascota: {item.NombreMascota}");

        // --- TASK 4: Consultas encadenadas ---
        Console.WriteLine("\n3. [Consulta Encadenada] Pacientes con especie 'Perro', ordenados por edad (solo Nombre y Teléfono):");
        // Encadenamiento de Where + OrderBy + Select
        var perrosOrdenados = lista
            .Where(p => p.Especie.Equals("Perro", StringComparison.OrdinalIgnoreCase))
            .OrderBy(p => p.Edad)
            .Select(p => new { p.Nombre, p.Telefono, p.Edad });

        if (perrosOrdenados.Any())
        {
            foreach (var item in perrosOrdenados)
                Console.WriteLine($"   - Dueño: {item.Nombre} | Tel: {item.Telefono} | Edad: {item.Edad}");
        }
        else
        {
            Console.WriteLine("   - No hay pacientes con especie 'Perro'.");
        }

        // --- TASK 5: Problemas prácticos con LINQ ---

        // A. Paciente más joven y paciente de mayor edad (First / OrderBy)
        var masJoven = lista.OrderBy(p => p.Edad).First();
        var mayorEdad = lista.OrderByDescending(p => p.Edad).First();
        Console.WriteLine($"\n4. Paciente más joven: {masJoven.Nombre} ({masJoven.Edad} años)");
        Console.WriteLine($"   Paciente de mayor edad: {mayorEdad.Nombre} ({mayorEdad.Edad} años)");

        // B. Contar cuántas mascotas hay por cada especie (GroupBy + Count)
        Console.WriteLine("\n5. Cantidad de mascotas por especie (GroupBy + Count):");
        var conteoPorEspecie = lista
            .GroupBy(p => p.Especie, StringComparer.OrdinalIgnoreCase)
            .Select(grupo => new { Especie = grupo.Key, Total = grupo.Count() });

        foreach (var grupo in conteoPorEspecie)
            Console.WriteLine($"   - Especie: {grupo.Especie} -> {grupo.Total} mascota(s)");

        // C. Verificar si existe al menos un paciente con mascota sin raza (Any) y si todos tienen teléfono (All)
        bool existeSinRaza = lista.Any(p => p.Raza.Equals("Sin raza", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(p.Raza));
        bool todosConTelefono = lista.All(p => !string.IsNullOrWhiteSpace(p.Telefono));
        Console.WriteLine($"\n6. ¿Existe alguna mascota sin raza definida? (Any): {(existeSinRaza ? "Sí" : "No")}");
        Console.WriteLine($"   ¿Todos los pacientes tienen teléfono registrado? (All): {(todosConTelefono ? "Sí" : "No")}");

        // D. Listar nombres en mayúsculas ordenados alfabéticamente (Select + OrderBy)
        Console.WriteLine("\n7. Nombres de dueños en mayúsculas y orden alfabético:");
        var nombresOrdenados = lista
            .Select(p => p.Nombre.ToUpper())
            .OrderBy(n => n);

        foreach (var nombre in nombresOrdenados)
            Console.WriteLine($"   - {nombre}");
    }

    // ==========================================
    // MÉTODOS AUXILIARES Y VALIDACIONES (TASK 7 / Robustez)
    // ==========================================

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
