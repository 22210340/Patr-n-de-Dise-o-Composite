using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Composite
{
    public abstract class ComponentePC
    {
        private string _nombre;
        private string _descripcion;
        private string _fabricante;

        public ComponentePC(string nombre, string descripcion, string fabricante)
        {
            _nombre = nombre;
            _descripcion = descripcion;
            _fabricante = fabricante;
        }

        public string Nombre => _nombre;
        public string Descripcion => _descripcion;
        public string Fabricante => _fabricante;

        public abstract void AgregarHijo(ComponentePC c);
        public abstract IList<ComponentePC> ObtenerHijos();
        public abstract double ObtenerPrecio { get; }
        public abstract void Mostrar(int nivel = 0);
    }
}
