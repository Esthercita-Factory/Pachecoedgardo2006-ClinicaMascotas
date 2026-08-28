using System;

namespace ClinicaSalud.Models;

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

    public void MostrarInformacion()
    {
        Console.WriteLine($"      [Mascota] Nombre: {Nombre} | Especie: {Especie} | Raza: {Raza} | Edad: {Edad} años | Sonido: {EmitirSonido()}");
    }

    public void Registrar()
    {
        Console.WriteLine($"[Registro IRegistrable] Mascota '{Nombre}' ({Especie}) registrada.");
    }
}
