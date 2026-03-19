using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Composite
{
    class Program
    {
        
        static CajaPC miPC = new CajaPC("Mi PC Personalizada", "Ensamblaje Oasis PC");
        static double totalAcumulado = 0;

        
        static readonly Pieza[,] catalogo = new Pieza[,]
        {
            
            {
                new Pieza("CPU Basica",  "Intel Core i3-12100 3.3GHz 4 nucleos",       "Intel",    2800.00),
                new Pieza("CPU Media",   "AMD Ryzen 5 5600X 3.7GHz 6 nucleos",         "AMD",      4500.00),
                new Pieza("CPU Alta",    "Intel Core i9-13900K 3.0GHz 24 nucleos",      "Intel",   12000.00)
            },
            
            {
                new Pieza("RAM Basica",  "Kingston 8GB DDR4 3200MHz",                  "Kingston",  550.00),
                new Pieza("RAM Media",   "Corsair Vengeance 16GB DDR4 3600MHz",         "Corsair",  1100.00),
                new Pieza("RAM Alta",    "G.Skill Trident Z5 32GB DDR5 6000MHz",        "G.Skill",  3500.00)
            },
            
            {
                new Pieza("HDD Basica",  "Seagate Barracuda 1TB HDD 7200RPM",          "Seagate",   700.00),
                new Pieza("SSD Media",   "Samsung 870 EVO 500GB SATA + WD Blue 1TB",   "Samsung",  1580.00),
                new Pieza("NVMe Alta",   "Samsung 990 Pro 1TB PCIe 4.0 + WD SN850X 2TB","Samsung", 5300.00)
            },
            
            {
                new Pieza("GPU Basica",  "Intel UHD 730 (Integrada, sin tarjeta)",     "Intel",       0.00),
                new Pieza("GPU Media",   "NVIDIA GTX 1650 4GB GDDR6",                  "ASUS",     3200.00),
                new Pieza("GPU Alta",    "NVIDIA RTX 4080 Super 16GB GDDR6X",          "ASUS ROG",22000.00)
            },
            
            {
                new Pieza("Motherboard Basica",  "ASUS Prime H510M-E Micro-ATX",       "ASUS",     1200.00),
                new Pieza("Motherboard Media",   "MSI B550-A PRO ATX",                 "MSI",      1800.00),
                new Pieza("Motherboard Alta",    "ASUS ROG Strix Z790-E Gaming WiFi",  "ASUS ROG", 6500.00)
            },
            
            {
                new Pieza("PSU Basica",  "EVGA 450W 80+ White",                        "EVGA",      600.00),
                new Pieza("PSU Media",   "Corsair CV650 650W 80+ Bronze",              "Corsair",   900.00),
                new Pieza("PSU Alta",    "Seasonic FOCUS GX-1000W 80+ Gold",           "Seasonic", 2600.00)
            },
            
            {
                new Pieza("Gabinete Basica", "Cougar MX340 Mid-Tower",                 "Cougar",    450.00),
                new Pieza("Gabinete Media",  "NZXT H510 Flow Mid-Tower",               "NZXT",     1200.00),
                new Pieza("Gabinete Alta",   "Lian Li O11 Dynamic EVO XL Full-Tower",  "Lian Li",  3200.00)
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

            for (int i = 0; i < categorias.Length; i++)
            {
                Pieza elegida = MostrarCategoria(i);
                totalAcumulado += elegida.ObtenerPrecio;
                miPC.AgregarHijo(elegida);
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

                
                string[] etiquetas = { "1", "2", "3" };
                string[] nombreGama = { "GAMA BASICA", "GAMA MEDIA", "GAMA ALTA" };
                ConsoleColor[] colores = {
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

                // Tecla invalida — volver a mostrar
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

            double subtotal = 0;
            foreach (var hijo in miPC.ObtenerHijos())
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"    + {hijo.Nombre,-30}");
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
            if (!ultimo)
                Console.WriteLine("\n  Presiona cualquier tecla para continuar con el siguiente componente...");
            else
                Console.WriteLine("\n  Presiona cualquier tecla para ver el resumen final...");
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
