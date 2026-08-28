# Clínica Veterinaria Salud+ (Semana 2)

Aplicación de consola en .NET enfocada en la gestión de colecciones y consultas avanzadas con LINQ.

## Funcionalidades
- **Gestión de Colecciones:** Operaciones de agregar, modificar, eliminar y listar usando `List<Paciente>` y `Dictionary<int, Paciente>` para búsquedas por ID en $O(1)$.
- **Consultas con LINQ (Sintaxis de Consulta y Métodos):**
  - Filtrado y proyección con `Where` y `Select`.
  - Ordenamiento con `OrderBy` y `OrderByDescending`.
  - Agrupamiento y conteo con `GroupBy` y `Count` por especie.
  - Verificaciones y cuantificadores con `Any` y `All`.
  - Búsqueda de extremos con `First` (paciente más joven y de mayor edad).
  - Consultas encadenadas y formateo de cadenas (`ToUpper`).

## Estructura
- `Models/`: Modelo `Paciente` con propiedades del dueño y su mascota.
- `Services/`: Métodos de negocio, validaciones y consultas LINQ comentadas (`PacienteService`).
- `Program.cs`: Menú interactivo en consola y precarga de datos de prueba.
