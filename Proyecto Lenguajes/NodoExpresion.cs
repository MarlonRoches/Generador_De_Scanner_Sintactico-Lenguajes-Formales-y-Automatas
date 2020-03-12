using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Lenguajes
{
    class NodoExpresion
    {
        public NodoExpresion Izquierdo = new NodoExpresion();
        public NodoExpresion Derecho = new NodoExpresion(); 
        public NodoExpresion Padre = new NodoExpresion(); 
        public bool Nullable { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string Nombre{ get; set; }
    }
}
