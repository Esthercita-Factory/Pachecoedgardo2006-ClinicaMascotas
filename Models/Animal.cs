namespace ClinicaSalud.Models;

/*
 * CONVENCIONES DE CODIFICACIÓN (TASK 6):
 * - Clases y Propiedades: PascalCase
 * - Campos privados: _camelCase
 * - Parámetros: camelCase
 */
public abstract class Animal
{
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

    public abstract string EmitirSonido();
}
