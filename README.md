# Clínica Salud+ (Semana 1)

Aplicación de consola en .NET para el registro y consulta básica de pacientes.

## Funcionalidades
- **Registrar paciente:** Captura de ID, Nombre, Edad y Síntoma con validación de datos (`try-catch` para edad y validación de campos vacíos).
- **Listar pacientes:** Visualización de todos los pacientes en memoria.
- **Buscar paciente:** Búsqueda por coincidencia de nombre.

## Estructura
- `Models/`: Modelo `Paciente`.
- `Services/`: Lógica de negocio y validaciones (`PacienteService`).
- `Program.cs`: Menú interactivo en consola y almacenamiento en `List<Paciente>`.
