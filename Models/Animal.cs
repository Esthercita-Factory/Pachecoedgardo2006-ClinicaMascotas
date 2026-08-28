namespace ClinicaSalud.Models;

public abstract class Animal
{
    // TASK 4: Encapsulación con campos privados y modificadores de acceso
    private string _nombre = string.Empty;
    private int _edad;
    private string _especie = string.Empty;

    public string Nombre
    {
        get => _nombre;
        set => _nombre = !string.IsNullOrWhiteSpace(value) ? value.Trim() : "Desconocido";
    }

    public int Edad
    {
        get => _edad;
        set => _edad = value >= 0 ? value : 0;
    }

    public string Especie
    {
        get => _especie;
        set => _especie = !string.IsNullOrWhiteSpace(value) ? value.Trim() : "No especificada";
    }

    protected Animal() { }

    protected Animal(string nombre, int edad, string especie)
    {
        Nombre = nombre;
        Edad = edad;
        Especie = especie;
    }

    // TASK 5: Polimorfismo - Método abstracto a ser sobrescrito por las clases derivadas
    public abstract string EmitirSonido();
}
