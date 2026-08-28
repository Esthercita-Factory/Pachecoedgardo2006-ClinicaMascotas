# Clínica Veterinaria Salud+ (Semana 3)

Aplicación de consola en .NET enfocada en Programación Orientada a Objetos (POO), Herencia, Polimorfismo, Encapsulación, Abstracción y Diseño UML.

## Diagrama de Clases UML

```mermaid
classDiagram
    class IRegistrable {
        <<interface>>
        +Registrar() void
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

    Animal <|-- Mascota : Herencia
    IRegistrable <|.. Mascota : Implementa
    IRegistrable <|.. Paciente : Implementa
    Paciente "1" o-- "0..*" Mascota : Asociación (Composición)
    ServicioVeterinario <|-- ConsultaGeneral : Herencia
    ServicioVeterinario <|-- Vacunacion : Herencia
```

## Conceptos de POO Aplicados
- **Encapsulación:** Atributos privados con propiedades públicas validadas.
- **Herencia:** `Mascota` hereda de la clase base `Animal`. `ConsultaGeneral` y `Vacunacion` heredan de `ServicioVeterinario`.
- **Polimorfismo:** Sobrescritura de `EmitirSonido()` en `Mascota` y `Atender()` en servicios veterinarios.
- **Abstracción:** Clases abstractas `Animal`, `ServicioVeterinario` e interfaz `IRegistrable`.
- **Relaciones:** Asociación 1 a N (`Paciente` puede tener múltiples `Mascota`).
