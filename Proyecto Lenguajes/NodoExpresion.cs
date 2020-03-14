using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Lenguajes
{
    class NodoExpresion
    {
        public NodoExpresion C1 { get; set; }
        public NodoExpresion C2 { get; set; }
        public NodoExpresion Padre { get; set; }
        public bool Nullable { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public int id { get; set; }
        public string Nombre{ get; set; }
    }
}
