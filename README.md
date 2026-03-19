# Patr-n-de-Dise-o-Composite

# 🖥️ Reporte de Práctica — Patrón de Diseño Composite
### Configurador Interactivo de Computadoras — Oasis PC

**Materia:** Patrones de Diseño de Software  
**Práctica:** Aplicación del Patrón Composite  
**Tema:** Sistema de armado de computadoras con menú interactivo por gama  
**Fecha:** 2026  

---

## 📌 Tabla de Contenidos

1. [Descripción del Patrón Composite](#1-descripción-del-patrón-composite)
2. [Contexto de la Práctica](#2-contexto-de-la-práctica)
3. [Estructura del Proyecto](#3-estructura-del-proyecto)
4. [Código Fuente](#4-código-fuente)
5. [Vista de Árbol — Estructura Composite](#5-vista-de-árbol--estructura-composite)
6. [Diagrama UML](#6-diagrama-uml)
7. [Captura de Pantalla — Ejecución del Programa](#7-captura-de-pantalla--ejecución-del-programa)
8. [Conclusión](#8-conclusión)
9. [Referencias](#9-referencias)

---

## 1. Descripción del Patrón Composite

### ¿Qué es?

El **Patrón Composite** es un patrón de diseño estructural del catálogo GoF (*Gang of Four*) que permite componer objetos en **estructuras de árbol** para representar jerarquías parte-todo. Su característica fundamental es que permite al cliente **tratar de forma uniforme** tanto a un objeto individual (hoja) como a una composición de objetos (nodo compuesto), sin necesidad de distinguir entre ellos en el código cliente.

### Componentes del Patrón

| Rol | Clase en este proyecto | Descripción |
|---|---|---|
| **Component** (Componente abstracto) | `ComponentePC` | Define la interfaz común: `ObtenerPrecio`, `AgregarHijo`, `ObtenerHijos`, `Mostrar` |
| **Leaf** (Hoja) | `Pieza` | Elemento individual de hardware. No tiene hijos. Retorna su propio precio |
| **Composite** (Compuesto) | `CajaPC` | Contenedor que agrupa piezas u otras cajas. Calcula el precio de forma recursiva sobre sus hijos |
| **Client** (Cliente) | `Program` | Construye el árbol, interactúa con él únicamente a través de la interfaz `ComponentePC` |

### ¿Por qué aplica aquí?

Una computadora ensamblada es naturalmente una estructura jerárquica: está compuesta por piezas individuales (CPU, RAM, disco...), y esas piezas pueden agruparse en paquetes o gamas. El cliente necesita poder consultar el precio de **una sola pieza** o de **toda la configuración armada** usando exactamente la misma llamada (`ObtenerPrecio`), sin importar si apunta a una hoja o a un compuesto. Eso es exactamente lo que resuelve el Composite.

---

## 2. Contexto de la Práctica

**Empresa ficticia:** *Oasis PC*

El programa simula un **configurador interactivo de consola** donde el usuario arma su computadora paso a paso. Por cada categoría de componente (CPU, RAM, almacenamiento, GPU, tarjeta madre, fuente de poder y gabinete), se presentan **3 opciones** correspondientes a las 3 gamas disponibles. El usuario elige presionando `1`, `2` o `3`, y el sistema muestra el **total acumulado actualizado** después de cada elección.

### Las 3 Gamas

| Gama | Etiqueta | Rango de precio aprox. | Perfil de usuario |
|---|---|---|---|
| 🟢 **Básica** | Oasis Entry | $0 — $10,000 MXN | Ofimática, navegación, tareas cotidianas |
| 🔵 **Media** | Oasis Pro | $10,000 — $30,000 MXN | Estudiantes, trabajo remoto, multimedia, gaming ligero |
| 🔴 **Alta** | Oasis Ultra | $30,000+ MXN | Gaming 4K, diseño 3D, edición de video, IA |

Al finalizar la configuración, el programa detecta automáticamente en qué gama cae el equipo armado según el precio total acumulado.

---

## 3. Estructura del Proyecto

```
OasisPC_Menu/
│
├── ComponentePC.cs     ← Clase abstracta (Component del patrón)
├── Pieza.cs            ← Hoja / Leaf (componente individual de hardware)
├── CajaPC.cs           ← Compuesto / Composite (agrupador de piezas)
└── Program.cs          ← Cliente: menú interactivo + lógica del configurador
```

**Relación con el código de clase:**

| Código de clase (Directorio/Archivo) | Este proyecto (OasisPC) |
|---|---|
| `Componente` (abstracta) | `ComponentePC` (abstracta) |
| `Directorio` (compuesto) | `CajaPC` (compuesto) |
| `Archivo` (hoja) | `Pieza` (hoja) |
| `ObtenerTamaño` (int) | `ObtenerPrecio` (double) |
| Árbol de directorios | Árbol de piezas de hardware |

---

## 4. Código Fuente

> **Nota:** El código es de elaboración propia, tomando como base la estructura del patrón Composite visto en clase (namespace `Composite`, clase abstracta, clase compuesta y clase hoja). Se adaptaron nombres, propiedades, tipos de datos y toda la lógica de interacción al dominio de venta de computadoras.

---

### 📄 `ComponentePC.cs` — Clase abstracta (Component)

```csharp
using System;
using System.Collections.Generic;

namespace OasisPC
{
    
    public abstract class ComponentePC
    {
        private string _nombre;
        private string _descripcion;
        private string _fabricante;

        public ComponentePC(string nombre, string descripcion, string fabricante)
        {
            _nombre      = nombre;
            _descripcion = descripcion;
            _fabricante  = fabricante;
        }

        public string Nombre      => _nombre;
        public string Descripcion => _descripcion;
        public string Fabricante  => _fabricante;

        
        public abstract void AgregarHijo(ComponentePC c);
        public abstract IList<ComponentePC> ObtenerHijos();

        
        public abstract double ObtenerPrecio { get; }

        
        public abstract void Mostrar(int nivel = 0);
    }
}
```

---

### 📄 `Pieza.cs` — Hoja (Leaf)

```csharp
using System;
using System.Collections.Generic;

namespace OasisPC
{
    
    public class Pieza : ComponentePC
    {
        private double _precio;

        public Pieza(string nombre, string descripcion, string fabricante, double precio)
            : base(nombre, descripcion, fabricante)
        {
            _precio = precio;
        }

        
        public override void AgregarHijo(ComponentePC c) { }

        public override IList<ComponentePC> ObtenerHijos() => null;

        
        public override double ObtenerPrecio => _precio;

        public override void Mostrar(int nivel = 0)
        {
            string indent = new string(' ', nivel * 4);
            Console.WriteLine($"{indent}  Nombre      : {Nombre}");
            Console.WriteLine($"{indent}  Descripcion : {Descripcion}");
            Console.WriteLine($"{indent}  Fabricante  : {Fabricante}");
            Console.WriteLine($"{indent}  Precio      : ${_precio:F2} MXN");
        }
    }
}
```

---

### 📄 `CajaPC.cs` — Compuesto (Composite)

```csharp
using System;
using System.Collections.Generic;

namespace OasisPC
{
   
    public class CajaPC : ComponentePC
    {
        private List<ComponentePC> _hijos;

        public CajaPC(string nombre, string descripcion, string fabricante = "Oasis PC")
            : base(nombre, descripcion, fabricante)
        {
            _hijos = new List<ComponentePC>();
        }

        public override void AgregarHijo(ComponentePC c) => _hijos.Add(c);

        public override IList<ComponentePC> ObtenerHijos() => _hijos.ToArray();

        
        public override double ObtenerPrecio
        {
            get
            {
                double total = 0;
                foreach (var hijo in _hijos)
                    total += hijo.ObtenerPrecio;
                return total;
            }
        }

        public override void Mostrar(int nivel = 0)
        {
            string indent = new string(' ', nivel * 4);
            Console.WriteLine($"{indent}[{Nombre}] — {Descripcion}");
            foreach (var hijo in _hijos)
                hijo.Mostrar(nivel + 1);
            Console.WriteLine($"{indent}  TOTAL: ${ObtenerPrecio:F2} MXN");
        }
    }
}
```

---

### 📄 `Program.cs` — Cliente / Menú Interactivo

```csharp
using System;
using System.Collections.Generic;

namespace OasisPC
{
    class Program
    {
        // Acumulador de piezas elegidas — CajaPC actúa como nodo raíz
        static CajaPC miPC = new CajaPC("Mi PC Personalizada", "Ensamblaje Oasis PC");
        static double totalAcumulado = 0;

        
        static readonly Pieza[,] catalogo = new Pieza[,]
        {
            
            {
                new Pieza("CPU Basica",  "Intel Core i3-12100 3.3GHz 4 nucleos",        "Intel",    2800.00),
                new Pieza("CPU Media",   "AMD Ryzen 5 5600X 3.7GHz 6 nucleos",          "AMD",      4500.00),
                new Pieza("CPU Alta",    "Intel Core i9-13900K 3.0GHz 24 nucleos",      "Intel",   12000.00)
            },
            
            {
                new Pieza("RAM Basica",  "Kingston 8GB DDR4 3200MHz",                   "Kingston",  550.00),
                new Pieza("RAM Media",   "Corsair Vengeance 16GB DDR4 3600MHz",          "Corsair",  1100.00),
                new Pieza("RAM Alta",    "G.Skill Trident Z5 32GB DDR5 6000MHz",         "G.Skill",  3500.00)
            },
            
            {
                new Pieza("HDD Basica",  "Seagate Barracuda 1TB HDD 7200RPM",           "Seagate",   700.00),
                new Pieza("SSD Media",   "Samsung 870 EVO 500GB SATA + WD Blue 1TB",    "Samsung",  1580.00),
                new Pieza("NVMe Alta",   "Samsung 990 Pro 1TB PCIe 4.0 + WD SN850X 2TB","Samsung",  5300.00)
            },
            
            {
                new Pieza("GPU Basica",  "Intel UHD 730 (Integrada, sin tarjeta)",      "Intel",       0.00),
                new Pieza("GPU Media",   "NVIDIA GTX 1650 4GB GDDR6",                   "ASUS",     3200.00),
                new Pieza("GPU Alta",    "NVIDIA RTX 4080 Super 16GB GDDR6X",           "ASUS ROG",22000.00)
            },
            
            {
                new Pieza("Motherboard Basica",  "ASUS Prime H510M-E Micro-ATX",        "ASUS",     1200.00),
                new Pieza("Motherboard Media",   "MSI B550-A PRO ATX",                  "MSI",      1800.00),
                new Pieza("Motherboard Alta",    "ASUS ROG Strix Z790-E Gaming WiFi",   "ASUS ROG", 6500.00)
            },
            
            {
                new Pieza("PSU Basica",  "EVGA 450W 80+ White",                         "EVGA",      600.00),
                new Pieza("PSU Media",   "Corsair CV650 650W 80+ Bronze",               "Corsair",   900.00),
                new Pieza("PSU Alta",    "Seasonic FOCUS GX-1000W 80+ Gold",            "Seasonic", 2600.00)
            },
            
            {
                new Pieza("Gabinete Basica", "Cougar MX340 Mid-Tower",                  "Cougar",    450.00),
                new Pieza("Gabinete Media",  "NZXT H510 Flow Mid-Tower",                "NZXT",     1200.00),
                new Pieza("Gabinete Alta",   "Lian Li O11 Dynamic EVO XL Full-Tower",   "Lian Li",  3200.00)
            },
        };

        static readonly string[] categorias =
        {
            "CPU", "RAM", "Almacenamiento", "GPU",
            "Tarjeta Madre", "Fuente de Poder", "Gabinete"
        };

        
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Oasis PC — Armado Interactivo";

            Encabezado();
            Console.WriteLine("  Bienvenido al configurador de equipos Oasis PC.");
            Console.WriteLine("  Elige un componente por categoria presionando 1, 2 o 3.\n");
            Linea();
            Console.ReadKey(true);

            // Iterar las 7 categorias de componentes
            for (int i = 0; i < categorias.Length; i++)
            {
                Pieza elegida = MostrarCategoria(i);
                totalAcumulado += elegida.ObtenerPrecio;
                miPC.AgregarHijo(elegida);          // <- Composite: agregar hoja al compuesto
                MostrarConfirmacion(elegida, i);
            }

            MostrarResumenFinal();
        }

        
        static Pieza MostrarCategoria(int idxCategoria)
        {
            while (true)
            {
                Console.Clear();
                Encabezado();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  Paso {idxCategoria + 1} de {categorias.Length}");
                Console.ResetColor();

                if (totalAcumulado > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"  Total acumulado hasta ahora: ${totalAcumulado:F2} MXN");
                    Console.ResetColor();
                }

                Linea();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n  >> Selecciona tu {categorias[idxCategoria].ToUpper()} <<\n");
                Console.ResetColor();

                string[]        etiquetas  = { "1", "2", "3" };
                string[]        nombreGama = { "GAMA BASICA", "GAMA MEDIA", "GAMA ALTA" };
                ConsoleColor[]  colores    = {
                    ConsoleColor.Green,
                    ConsoleColor.Blue,
                    ConsoleColor.Magenta
                };

                for (int g = 0; g < 3; g++)
                {
                    Pieza p = catalogo[idxCategoria, g];

                    Console.ForegroundColor = colores[g];
                    Console.WriteLine($"  [{etiquetas[g]}] {nombreGama[g]}");
                    Console.ResetColor();
                    Console.WriteLine($"       Nombre      : {p.Nombre}");
                    Console.WriteLine($"       Descripcion : {p.Descripcion}");
                    Console.WriteLine($"       Fabricante  : {p.Fabricante}");

                    if (p.ObtenerPrecio == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"       Precio      : INCLUIDA (sin costo adicional)");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"       Precio      : ${p.ObtenerPrecio:F2} MXN");
                    }
                    Console.ResetColor();
                    Console.WriteLine();
                }

                Linea();
                Console.Write("  Elige una opcion (1 / 2 / 3): ");

                ConsoleKeyInfo key = Console.ReadKey(true);
                Console.WriteLine(key.KeyChar);

                if (key.KeyChar == '1') return catalogo[idxCategoria, 0];
                if (key.KeyChar == '2') return catalogo[idxCategoria, 1];
                if (key.KeyChar == '3') return catalogo[idxCategoria, 2];

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  Opcion invalida. Presiona 1, 2 o 3.");
                Console.ResetColor();
                System.Threading.Thread.Sleep(1000);
            }
        }

        
        static void MostrarConfirmacion(Pieza elegida, int idxCategoria)
        {
            Console.Clear();
            Encabezado();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  OK  {categorias[idxCategoria]} agregado!\n");
            Console.ResetColor();

            Console.WriteLine($"  Pieza   : {elegida.Nombre}");
            Console.WriteLine($"  Detalle : {elegida.Descripcion}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  Precio  : ${elegida.ObtenerPrecio:F2} MXN");
            Console.ResetColor();

            Linea();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n  Componentes seleccionados hasta ahora:\n");
            Console.ResetColor();

            // Recorrer el árbol Composite para mostrar las piezas acumuladas
            double subtotal = 0;
            foreach (var hijo in miPC.ObtenerHijos())
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"    + {hijo.Nombre,-32}");

                if (hijo.ObtenerPrecio == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("   INCLUIDA");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"  ${hijo.ObtenerPrecio,10:F2} MXN");
                }
                Console.ResetColor();
                subtotal += hijo.ObtenerPrecio;
            }

            Console.WriteLine();
            Linea();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  TOTAL ACUMULADO: ${subtotal:F2} MXN");
            Console.ResetColor();
            Linea();

            bool ultimo = (idxCategoria == categorias.Length - 1);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(ultimo
                ? "\n  Presiona cualquier tecla para ver el resumen final..."
                : "\n  Presiona cualquier tecla para continuar con el siguiente componente...");
            Console.ResetColor();

            Console.ReadKey(true);
        }

        
        static void MostrarResumenFinal()
        {
            Console.Clear();
            Encabezado();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  *** TU PC OASIS ESTA LISTA ***\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  Resumen de tu configuracion:\n");
            Console.ResetColor();

            double total = 0;
            foreach (var hijo in miPC.ObtenerHijos())
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"    {hijo.Nombre,-32}");

                if (hijo.ObtenerPrecio == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("   INCLUIDA");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"  ${hijo.ObtenerPrecio,10:F2} MXN");
                }
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"    -> {hijo.Descripcion}");
                Console.ResetColor();
                total += hijo.ObtenerPrecio;
            }

            Console.WriteLine();
            Linea('=');
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  PRECIO TOTAL DE TU PC: ${total:F2} MXN");
            Console.ResetColor();
            Linea('=');

            
            string gama = total < 10000 ? "BASICA" : total < 30000 ? "MEDIA" : "ALTA";
            Console.ForegroundColor = gama == "ALTA" ? ConsoleColor.Magenta :
                                       gama == "MEDIA" ? ConsoleColor.Blue : ConsoleColor.Green;
            Console.WriteLine($"\n  Tu configuracion cae en la gama: {gama}");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n  Gracias por armar tu PC con Oasis PC.");
            Console.WriteLine("  Presiona cualquier tecla para salir...");
            Console.ResetColor();

            Console.ReadKey(true);
        }

        
        static void Encabezado()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  ╔══════════════════════════════════════════════╗");
            Console.WriteLine("  ║       OASIS PC — Configurador Interactivo    ║");
            Console.WriteLine("  ║       Patron de Diseno: Composite            ║");
            Console.WriteLine("  ╚══════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();
        }

        static void Linea(char c = '-')
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  " + new string(c, 48));
            Console.ResetColor();
        }
    }
}
```

---

## 5. Vista de Árbol — Estructura Composite

### Vista General (Compacta)

```
                        [CajaPC: Catálogo Oasis PC] ← Compuesto (nodo raíz)
                        /               |               \
                       /                |                \
        [CajaPC: GamaBaja]    [CajaPC: GamaMedia]    [CajaPC: GamaAlta]
          ← Compuesto            ← Compuesto            ← Compuesto
          / | | | | \            / | | | | | \          / | | | | | | \
        CPU RAM HDD MB PSU GAB  CPU RAM SSD HDD GPU MB PSU GAB   ...
         ↑                                                  ↑
   Hojas (Pieza)                                     Hojas (Pieza)
```

### Vista Detallada — Todas las Piezas Individuales

```
[CajaPC: Mi PC Personalizada]  ← Compuesto RAÍZ (construido dinámicamente)
│
├── [Pieza: CPU elegido]
│     Depende de la elección del usuario:
│     Opción 1 → Intel Core i3-12100 3.3GHz    — Intel         $2,800.00
│     Opción 2 → AMD Ryzen 5 5600X 3.7GHz      — AMD           $4,500.00
│     Opción 3 → Intel Core i9-13900K 24 núcl. — Intel        $12,000.00
│
├── [Pieza: RAM elegida]
│     Opción 1 → Kingston 8GB DDR4 3200MHz     — Kingston         $550.00
│     Opción 2 → Corsair 16GB DDR4 3600MHz     — Corsair        $1,100.00
│     Opción 3 → G.Skill 32GB DDR5 6000MHz     — G.Skill        $3,500.00
│
├── [Pieza: Almacenamiento elegido]
│     Opción 1 → Seagate Barracuda 1TB HDD     — Seagate          $700.00
│     Opción 2 → Samsung 870 EVO + WD Blue 1TB — Samsung        $1,580.00
│     Opción 3 → Samsung 990 Pro + WD SN850X   — Samsung        $5,300.00
│
├── [Pieza: GPU elegida]
│     Opción 1 → Intel UHD 730 Integrada       — Intel             $0.00
│     Opción 2 → NVIDIA GTX 1650 4GB GDDR6     — ASUS           $3,200.00
│     Opción 3 → NVIDIA RTX 4080 Super 16GB    — ASUS ROG      $22,000.00
│
├── [Pieza: Tarjeta Madre elegida]
│     Opción 1 → ASUS Prime H510M-E            — ASUS           $1,200.00
│     Opción 2 → MSI B550-A PRO ATX            — MSI            $1,800.00
│     Opción 3 → ASUS ROG Strix Z790-E WiFi    — ASUS ROG       $6,500.00
│
├── [Pieza: Fuente de Poder elegida]
│     Opción 1 → EVGA 450W 80+ White           — EVGA             $600.00
│     Opción 2 → Corsair CV650 650W Bronze     — Corsair          $900.00
│     Opción 3 → Seasonic FOCUS GX-1000W Gold  — Seasonic       $2,600.00
│
└── [Pieza: Gabinete elegido]
      Opción 1 → Cougar MX340 Mid-Tower        — Cougar           $450.00
      Opción 2 → NZXT H510 Flow Mid-Tower      — NZXT           $1,200.00
      Opción 3 → Lian Li O11 Dynamic EVO XL    — Lian Li        $3,200.00

══════════════════════════════════════════════════════════════
  RANGO DE PRECIO TOTAL SEGÚN COMBINACIÓN:
  Mínimo posible (todo Básica) :  $6,300.00 MXN
  Rango Medio   (todo Media)   : $14,280.00 MXN
  Máximo posible (todo Alta)   : $58,900.00 MXN
══════════════════════════════════════════════════════════════
```

---

## 6. Diagrama UML

```
┌────────────────────────────────────────────────────────┐
│                  <<abstract>>                          │
│                  ComponentePC                          │
│────────────────────────────────────────────────────────│
│ - _nombre      : string                                │
│ - _descripcion : string                                │
│ - _fabricante  : string                                │
│────────────────────────────────────────────────────────│
│ + ComponentePC(nombre, descripcion, fabricante)        │
│ + Nombre       : string   {get}                        │
│ + Descripcion  : string   {get}                        │
│ + Fabricante   : string   {get}                        │
│ + AgregarHijo(c: ComponentePC) : void  <<abstract>>    │
│ + ObtenerHijos() : IList<ComponentePC> <<abstract>>    │
│ + ObtenerPrecio  : double              <<abstract>>    │
│ + Mostrar(nivel: int) : void           <<abstract>>    │
└──────────────────────┬─────────────────────────────────┘
                       │  <<hereda>>
           ┌───────────┴────────────┐
           │                        │
┌──────────▼──────────┐  ┌──────────▼──────────────────────────┐
│       Pieza         │  │              CajaPC                  │
│  (Leaf / Hoja)      │  │   (Composite / Compuesto)            │
│─────────────────────│  │──────────────────────────────────────│
│ - _precio : double  │  │ - _hijos : List<ComponentePC>        │
│─────────────────────│  │──────────────────────────────────────│
│ + Pieza(nombre,     │  │ + CajaPC(nombre, desc, fab)          │
│    desc, fab, prec) │  │ + AgregarHijo(c) : void              │
│ + AgregarHijo()     │  │   _hijos.Add(c)                      │
│   (vacío — no hijos)│  │ + ObtenerHijos() : IList<>           │
│ + ObtenerHijos()    │  │   return _hijos.ToArray()            │
│   return null       │  │ + ObtenerPrecio : double             │
│ + ObtenerPrecio     │  │   foreach hijo: total+=hijo.precio   │
│   return _precio    │  │   (SUMA RECURSIVA)                   │
│ + Mostrar(nivel)    │  │ + Mostrar(nivel)                     │
└─────────────────────┘  └──────────┬──────────────────────────┘
                                    │ contiene  0..*
                                    │◄──────────────────────┐
                                    │                       │
                         ┌──────────▼──────────┐           │
                         │  ComponentePC       │───────────┘
                         │  (Pieza o CajaPC)   │
                         └─────────────────────┘


┌─────────────────────────────────────────────────────────┐
│                    Program (Cliente)                    │
│─────────────────────────────────────────────────────────│
│ - miPC           : CajaPC  (nodo raíz)                  │
│ - totalAcumulado : double                               │
│ - catalogo       : Pieza[7, 3]  (7 categorias x 3 gamas)│
│ - categorias     : string[]                             │
│─────────────────────────────────────────────────────────│
│ + Main()                                                │
│ + MostrarCategoria(idx) : Pieza                         │
│ + MostrarConfirmacion(pieza, idx) : void                │
│ + MostrarResumenFinal() : void                          │
│ + Encabezado() : void                                   │
│ + Linea(char) : void                                    │
└─────────────────────────────────────────────────────────┘
         │ usa únicamente la interfaz ComponentePC
         ▼
   miPC.AgregarHijo(elegida)
   miPC.ObtenerHijos()
   miPC.ObtenerPrecio
```

---

## 7. Captura de Pantalla — Ejecución del Programa

A continuación se muestra la secuencia de pantallas que produce el programa al ejecutarse:

### Pantalla 1 — Bienvenida

```
  ╔══════════════════════════════════════════════╗
  ║       OASIS PC — Configurador Interactivo    ║
  ║       Patron de Diseno: Composite            ║
  ╚══════════════════════════════════════════════╝

  Bienvenido al configurador de equipos Oasis PC.
  Elige un componente por categoria presionando 1, 2 o 3.

  ────────────────────────────────────────────────────
```

### Pantalla 2 — Selección de CPU (Paso 1/7)

```
  ╔══════════════════════════════════════════════╗
  ║       OASIS PC — Configurador Interactivo    ║
  ╚══════════════════════════════════════════════╝

  Paso 1 de 7
  ────────────────────────────────────────────────────

  >> Selecciona tu CPU <<

  [1] GAMA BASICA
       Nombre      : CPU Basica
       Descripcion : Intel Core i3-12100 3.3GHz 4 nucleos
       Fabricante  : Intel
       Precio      : $2800.00 MXN

  [2] GAMA MEDIA
       Nombre      : CPU Media
       Descripcion : AMD Ryzen 5 5600X 3.7GHz 6 nucleos
       Fabricante  : AMD
       Precio      : $4500.00 MXN

  [3] GAMA ALTA
       Nombre      : CPU Alta
       Descripcion : Intel Core i9-13900K 3.0GHz 24 nucleos
       Fabricante  : Intel
       Precio      : $12000.00 MXN

  ────────────────────────────────────────────────────
  Elige una opcion (1 / 2 / 3): 2
```

### Pantalla 3 — Confirmación + Total acumulado tras CPU

```
  ╔══════════════════════════════════════════════╗
  ║       OASIS PC — Configurador Interactivo    ║
  ╚══════════════════════════════════════════════╝

  OK  CPU agregado!

  Pieza   : CPU Media
  Detalle : AMD Ryzen 5 5600X 3.7GHz 6 nucleos
  Precio  : $4500.00 MXN

  ────────────────────────────────────────────────────

  Componentes seleccionados hasta ahora:

    + CPU Media                          $   4500.00 MXN

  ────────────────────────────────────────────────────
  TOTAL ACUMULADO: $4500.00 MXN
  ────────────────────────────────────────────────────

  Presiona cualquier tecla para continuar...
```

### Pantalla 4 — Selección de RAM (Paso 2/7) con total visible

```
  Paso 2 de 7
  Total acumulado hasta ahora: $4500.00 MXN
  ────────────────────────────────────────────────────

  >> Selecciona tu RAM <<

  [1] GAMA BASICA  — Kingston 8GB DDR4   $550.00 MXN
  [2] GAMA MEDIA   — Corsair 16GB DDR4   $1100.00 MXN
  [3] GAMA ALTA    — G.Skill 32GB DDR5   $3500.00 MXN
```

### Pantalla 5 — Confirmación acumulada (CPU + RAM)

```
  Componentes seleccionados hasta ahora:

    + CPU Media                          $   4500.00 MXN
    + RAM Media                          $   1100.00 MXN

  ────────────────────────────────────────────────────
  TOTAL ACUMULADO: $5600.00 MXN
```

### Pantalla 6 — Resumen Final

```
  *** TU PC OASIS ESTA LISTA ***

  Resumen de tu configuracion:

    CPU Media                            $   4500.00 MXN
    -> AMD Ryzen 5 5600X 3.7GHz 6 nucleos
    RAM Media                            $   1100.00 MXN
    -> Corsair Vengeance 16GB DDR4 3600MHz
    SSD Media                            $   1580.00 MXN
    -> Samsung 870 EVO 500GB SATA + WD Blue 1TB
    GPU Media                            $   3200.00 MXN
    -> NVIDIA GTX 1650 4GB GDDR6
    Motherboard Media                    $   1800.00 MXN
    -> MSI B550-A PRO ATX
    PSU Media                            $    900.00 MXN
    -> Corsair CV650 650W 80+ Bronze
    Gabinete Media                       $   1200.00 MXN
    -> NZXT H510 Flow Mid-Tower

  ════════════════════════════════════════════════════
  PRECIO TOTAL DE TU PC: $14280.00 MXN
  ════════════════════════════════════════════════════

  Tu configuracion cae en la gama: MEDIA

  Gracias por armar tu PC con Oasis PC.
  Presiona cualquier tecla para salir...
```


## 8. Conclusión

Al Eloborar la practica pues nos da una nocion mas para comprender de manera practica y aplicada los patrones estructurares mas utiles en este caso el Patron Composite al realizar la practica se identifica con claridad la diferencia de los roles de hojas y compuesto ya que ambos son transparentesntes por el cliente mediante un interfaz comun.

Adaptar el ejemplo de los directorios y archivos visto en clase a la venta de computadoras con componentes resulto un ejersicio revelador asi como la analogia es directa asi como un directorio contienes arcivhos u otros directorios unacaja de venta puede contener piezas individuales u otras cajas la operación que antes calculaba el tamaño total ahora calcula el precio total de forma recursiva, lo cual demuestra que el patrón es independiente del dominio y puede reutilizarse en distintos contextos sin modificar su lógica estructural.

Uno de los aprendizajes más importantes fue el principio de uniformidad: el cliente (en Main) no necesita saber si está interactuando con una pieza individual o con un paquete completo. Esto reduce el acoplamiento y mejora la extensibilidad del sistema; si en el futuro se quisiera agregar una nueva categoría de gama o un subpaquete (por ejemplo, un kit de periféricos), bastaría con crear un nuevo CajaPC y agregarlo al árbol sin modificar el código existente.

Finalmente, esta práctica refuerza el valor de los patrones de diseño como herramientas de comunicación entre desarrolladores: al decir "esto usa Composite", cualquier programador familiarizado con el patrón entiende de inmediato la estructura sin necesidad de leer el código completo.

---

