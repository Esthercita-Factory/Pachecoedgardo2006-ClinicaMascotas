using System;

namespace ClinicaSalud.Models;

// TASK 5: Herencia (Mascota hereda de Animal) y TASK 6: Implementación de IRegistrable
public class Mascota : Animal, IRegistrable
{
    private string _raza = string.Empty;

    public string Raza
    {
        get => _raza;
        set => _raza = !string.IsNullOrWhiteSpace(value) ? value.Trim() : "Sin raza";
    }

    public Mascota() { }

    public Mascota(string nombre, int edad, string especie, string raza)
        : base(nombre, edad, especie)
    {
        Raza = raza;
    }

    // TASK 5: Polimorfismo - Sobrescritura de EmitirSonido()
    public override string EmitirSonido()
    {
        return Especie.ToLower() switch
        {
            "perro" => "¡Guau guau!",
            "gato" => "¡Miau miau!",
            "loro" or "ave" => "¡Currucucu!",
            _ => "Sonido de animal"
        };
    }

    // TASK 2: Método para mostrar información de la mascota
    public void MostrarInformacion()
    {
        Console.WriteLine($"      [Mascota] Nombre: {Nombre} | Especie: {Especie} | Raza: {Raza} | Edad: {Edad} años | Sonido: {EmitirSonido()}");
    }

    // TASK 6: Implementación de la interfaz IRegistrable
    public void Registrar()
    {
        Console.WriteLine($"[Registro IRegistrable] Mascota '{Nombre}' ({Especie}) registrada correctamente.");
    }
}
