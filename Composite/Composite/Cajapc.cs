using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Composite
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
