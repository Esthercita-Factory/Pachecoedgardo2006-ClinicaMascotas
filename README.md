# Clínica Veterinaria Salud+ (Semana 4)

Aplicación de consola en .NET enfocada en interfaces múltiples, manejo estructurado de excepciones, depuración y sistema de logging.

## Diagrama de Clases UML (Actualizado Semana 4)

```mermaid
classDiagram
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
        +Atender(Paciente, Mascota) void
    }

    class Animal {
        <<abstract>>
        -string _nombre
        -int _edad
        -string _especie
        +Nombre: string
        +Edad: int
        +Especie: string
        +EmitirSonido()* string
    }

    class Mascota {
        -string _raza
        +Raza: string
        +EmitirSonido() string
        +MostrarInformacion() void
        +Registrar() void
    }

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
        +AgregarMascota(Mascota) void
        +MostrarInformacion() void
        +Registrar() void
        +EnviarNotificacion(string) void
    }

    class ServicioVeterinario {
        <<abstract>>
        +NombreServicio: string
        +CostoBase: decimal
        +Atender(Paciente, Mascota)* void
    }

    class ConsultaGeneral {
        +Diagnostico: string
        +Atender(Paciente, Mascota) void
    }

    class Vacunacion {
        +TipoVacuna: string
        +Atender(Paciente, Mascota) void
    }

    class Exception {
        <<System>>
    }

    class MascotaNoEncontradaException {
        +NombreMascotaBuscada: string
    }

    class PacienteNoEncontradoException {
        +IdBuscado: int
    }

    %% Relaciones de herencia e interfaces
    Animal <|-- Mascota : Herencia
    IRegistrable <|.. Mascota : Implementa
    IRegistrable <|.. Paciente : Implementa
    INotificable <|.. Paciente : Implementa (Múltiples Interfaces)
    Paciente "1" o-- "0..*" Mascota : Asociación
    IAtendible <|.. ServicioVeterinario : Implementa
    ServicioVeterinario <|-- ConsultaGeneral : Herencia
    ServicioVeterinario <|-- Vacunacion : Herencia

    Exception <|-- MascotaNoEncontradaException : Herencia
    Exception <|-- PacienteNoEncontradoException : Herencia
```

## Novedades y Decisiones de Diseño

### 1. Interfaces vs Clases Abstractas
- `IRegistrable`: Contrato uniforme para persistencia en consola/memoria (`Paciente` y `Mascota`).
- `INotificable`: Contrato de comunicación implementado en `Paciente`.
- `IAtendible`: Contrato desacoplado para servicios veterinarios.
- `Paciente` implementa **múltiples interfaces** (`IRegistrable` e `INotificable`).
- `ServicioVeterinario` combina clase base abstracta con la interfaz `IAtendible` para compartir estado (`CostoBase`, `NombreServicio`).

### 2. Manejo de Excepciones y Logging
- Excepciones de dominio: `MascotaNoEncontradaException` y `PacienteNoEncontradoException`.
- Bloques `try-catch-finally` en todas las operaciones.
- `LoggerService`: Registro persistente de errores en el archivo `clinica_errores.log` con fecha/hora y traza de error.
