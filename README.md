# Clínica Veterinaria Salud+ (Semana 5)

Aplicación de consola en .NET enfocada en programación asíncrona (`async`/`await`), gestión concurrente con `Task` (`Task.WhenAll`, `Task.WhenAny`), manejo de excepciones de dominio y convenciones de codificación de C#.

## Diagrama de Arquitectura de Clases UML

```mermaid
classDiagram
    %% INTERFACES
    class IRegistrable {
        <<interface>>
        +Registrar() void
    }

    class INotificable {
        <<interface>>
        +EnviarNotificacion(string mensaje) void
    }

    class IAtendible {
        <<interface>>
        +NombreServicio: string
        +CostoBase: decimal
        +Atender(Paciente paciente, Mascota mascota) void
    }

    %% JERARQUÍA DE ANIMALES
    class Animal {
        <<abstract>>
        -string _nombre
        -int _edad
        -string _especie
        +Nombre: string
        +Edad: int
        +Especie: string
        #Animal()
        #Animal(string nombre, int edad, string especie)
        +EmitirSonido()* string
    }

    class Mascota {
        -string _raza
        +Raza: string
        +Mascota()
        +Mascota(string nombre, int edad, string especie, string raza)
        +EmitirSonido() string
        +MostrarInformacion() void
        +Registrar() void
    }

    %% MODELO PACIENTE (DUEÑO)
    class Paciente {
        -int _id
        -string _nombre
        -int _edad
        -string _direccion
        -string _telefono
        +Id: int
        +Nombre: string
        +Edad: int
        +Direccion: string
        +Telefono: string
        +Mascotas: List~Mascota~
        +Paciente()
        +Paciente(int id, string nombre, int edad, string direccion, string telefono)
        +AgregarMascota(Mascota mascota) void
        +MostrarInformacion() void
        +Registrar() void
        +EnviarNotificacion(string mensaje) void
    }

    %% JERARQUÍA DE SERVICIOS
    class ServicioVeterinario {
        <<abstract>>
        +NombreServicio: string
        +CostoBase: decimal
        +Atender(Paciente paciente, Mascota mascota)* void
    }

    class ConsultaGeneral {
        +Diagnostico: string
        +ConsultaGeneral()
        +Atender(Paciente paciente, Mascota mascota) void
    }

    class Vacunacion {
        +TipoVacuna: string
        +Vacunacion(string tipoVacuna)
        +Atender(Paciente paciente, Mascota mascota) void
    }

    %% EXCEPCIONES DE DOMINIO
    class Exception {
        <<System>>
        +Message: string
    }

    class PacienteNoEncontradoException {
        +IdBuscado: int
        +PacienteNoEncontradoException(int id)
        +PacienteNoEncontradoException(string message, Exception inner)
    }

    class MascotaNoEncontradaException {
        +NombreMascotaBuscada: string
        +MascotaNoEncontradaException(string nombreMascota)
        +MascotaNoEncontradaException(string message, Exception inner)
    }

    %% SERVICIOS ASÍNCRONOS Y LOGS
    class LoggerService {
        <<static>>
        +LogErrorAsync(string mensaje, Exception? ex)$ Task
        +LogInfoAsync(string mensaje)$ Task
    }

    class PacienteService {
        <<static>>
        +RegistrarPacienteAsync(List~Paciente~ lista)$ Task
        +EjecutarProcesosParalelosAsync(List~Paciente~ lista)$ Task
        +DemostrarWhenAllVsWhenAnyAsync()$ Task
        +SimularAtencionConcurrenteAsync(List~Paciente~ lista)$ Task
        +ListarPacientes(List~Paciente~ lista)$ void
    }

    %% RELACIONES
    Animal <|-- Mascota : Herencia
    IRegistrable <|.. Mascota : Implementa
    IRegistrable <|.. Paciente : Implementa
    INotificable <|.. Paciente : Implementa
    Paciente "1" o-- "0..*" Mascota : Agregación (Tiene)
    
    IAtendible <|.. ServicioVeterinario : Implementa
    ServicioVeterinario <|-- ConsultaGeneral : Herencia
    ServicioVeterinario <|-- Vacunacion : Herencia

    Exception <|-- PacienteNoEncontradoException : Herencia
    Exception <|-- MascotaNoEncontradaException : Herencia
```

## Fundamentos y Buenas Prácticas Asíncronas

### 1. `async` / `await`
- Permite liberar el hilo de ejecución principal durante operaciones de entrada/salida (*I/O-bound*) simuladas con `Task.Delay` o persistencia asíncrona de archivos (`File.AppendAllTextAsync`).

### 2. Gestión de Tareas Paralelas
- **`Task.WhenAll`**: Ejecuta múltiples tareas independientes en paralelo (carga de historial médico, agendamiento de cita y envío de SMS), reduciendo drásticamente el tiempo total de respuesta.
- **`Task.WhenAny`**: Permite reaccionar al primer resultado disponible entre múltiples servicios asíncronos en competencia (ejemplo: respuesta más rápida de laboratorios).

### 3. Buenas Prácticas y Convenciones de C#
- **Cero bloqueos síncronos:** No se utiliza `.Result` ni `.Wait()`, evitando bloqueos del hilo principal (*deadlocks*).
- **Sufijo `Async`:** Todos los métodos asíncronos llevan el sufijo correspondiente (`RegistrarPacienteAsync`, `LogErrorAsync`).
- **Nomenclatura uniforme:**
  - `PascalCase` para clases, métodos e interfaces.
  - `camelCase` para variables y parámetros.
  - `_camelCase` para atributos privados.
