using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Lenguajes
{
    class Arbol_De_Expresiones
    {

        private static Arbol_De_Expresiones _instance = null;
        public static Arbol_De_Expresiones Instance
        {
            get
            {
                if (_instance == null) _instance = new Arbol_De_Expresiones();
                return _instance;
            }
        }

        public List<NodoExpresion> SubArboles = new List<NodoExpresion>();
        public Dictionary<int, NodoExpresion> Diccionario_Nodos = new Dictionary<int, NodoExpresion>(); 
        public void GenerarArbol(string MegaExpresion, Dictionary<string, string> Tokens, string[]SETS)
        {
            //regularizacion
            var comillas = '"';
            var comilla = "'";
            MegaExpresion = MegaExpresion.Replace(" *", "*").Replace($" {comillas}", $"{comillas}").Replace($"{comillas} ", $"{comillas}").Replace($" {comilla}", $"{comilla}").Replace($"{comilla} ", $"{comilla}").Replace("''", ".").Replace("('", "(").Replace("')", ")").Replace(" ( ", "(").Replace(" | ", "|").Replace(" )", ")").Replace(" ", ".");
            var arreglo = MegaExpresion.Substring(0, MegaExpresion.Length - 2).Split('☼');
            for (int i = 0; i < SETS.Length; i++)
            {
                SETS[i] = SETS[i].Replace(" ", "");
            }
            foreach (var item in arreglo)
            {
                LecturaDinamica(item,SETS);
            }
            
            //    foreach (var subexpr in arreglo)
        //    {
        //        var General = subexpr.Substring(1, subexpr.Length - 2);
        //        var lista = new List<string>();
        //        lista.Add(General);
        //        var flag = true;
        //        var Actual = General;
        //        var interior = string.Empty;
        //        var n = 0;
        //        while (flag)
        //        {
        //            if (Actual.Contains('('))
        //            {// buscar parentesis mas pequeño
        //                var index1 = Actual.IndexOf('(');
        //                var index2 = Actual.LastIndexOf(')');
        //                var lol = index2-index1;
        //                Actual = Actual.Substring(index1+1,lol-1);
        //                lista.Add(Actual);
        //                General =General.Replace($"({Actual})", "[]");
        //            }
        //            else
        //            {// armar
        //                var Original = subexpr;
        //                var Expresion = subexpr;
        //                for (int i = 0; i < SETS.Length; i++)
        //                {
        //                    SETS[i] = SETS[i].Replace(" ", "");
        //                    if (Expresion.Contains(SETS[i]))
        //                    {
        //                        Expresion = Expresion.Replace(SETS[i],i.ToString());
        //                    }
        //                }
                        
        //                // ya esta fixeado
        //                while (Expresion!="")
        //                {
        //                    var index1 = Actual.IndexOf('(');
        //                    var index2 = Actual.LastIndexOf(')');
        //                    var lol = (index2 - index1)-1;
        //                }
        //                flag = false;
        //            }
        //        }
                
        //    }
        //
        }
        void LecturaDinamica(string ExpresionActual,string[] SETS)
        {
            //representacion
            //Original
            var n = 0;
            var diccionario = new Dictionary<string, string>();
            string masPequeño = ExpresionActual.Substring(1, ExpresionActual.Length-2);
            var camino = new List<string>();
            //ir al mas pequeño

            if (masPequeño.Contains('('))
            {
                while (masPequeño.Contains('('))
                {
                    var index1 = masPequeño.IndexOf('(')+1;
                    var index2 = masPequeño.LastIndexOf(')');
                    var lol = index2 - index1;
                    var aux = masPequeño.Substring(index1, lol);
                    masPequeño = masPequeño.Replace(aux, $"[{n}]");
                        n++;
                    camino.Add(masPequeño);
                    diccionario.Add($"[{n}]", masPequeño);
                    masPequeño = aux;
                }
            }
            else
            {

               var xd = Indexado(masPequeño,SETS);
            //*
                var Asterisco_index = masPequeño.IndexOf('*');
                //+     _index
                var Mas_index = masPequeño.IndexOf('+');
            //.     _index
                var Concatenacion_index = masPequeño.IndexOf('.');
            //|     _index
                var Or_index = masPequeño.IndexOf('|');
            }
        }
        string ArmarSubexpresionInterior(string original, string armar)
        {
            return "";
        }
        NodoExpresion Mas(string expresion)
        {
            return null;
        }
        NodoExpresion Asterisco(string expresion)
        {
            return null;

        }
        NodoExpresion Concatenacion(string expresion)
        {
            var nuevo = new NodoExpresion();
            var izq = expresion.Split('|')[0];
            var der = expresion.Split('|')[1];
            nuevo.Nombre = "*";
            nuevo.Izquierdo = new NodoExpresion()
            {
                Nombre = izq,
                First = izq,
                Last = izq,
                Nullable = false,
                Padre = nuevo,

            };
            nuevo.Derecho = new NodoExpresion()
            {
                Nombre = der,
                First = der,
                Last = der,
                Nullable = false,
                Padre = nuevo,

            };
            //validar nullabilidad de el nuevo
            return nuevo;
        }
        NodoExpresion Or(string expresion)
        {
            var nuevo = new NodoExpresion();
            var izq = expresion.Split('|')[0];
            var der = expresion.Split('|')[1];
            nuevo.Nombre = "|";
            //ir a traer
            nuevo.Izquierdo = new NodoExpresion()
            {
                Nombre = izq,
                First = izq,
                Last = izq,
                Nullable = false,
                Padre=nuevo,

            };
            nuevo.Derecho = new NodoExpresion()
            {
                Nombre = der,
                First = der,
                Last = der,
                Nullable = false,
                Padre = nuevo,

            };
            //validar nullabilidad de el nuevo
            return null;

        }
        NodoExpresion Interrogacion(string expresion)
        {
            return null;

        }
      KeyValuePair<Dictionary<string,string>,string> Indexado(string expresion, string[] SETS)
        {
            var diccionario = new Dictionary<string, string>();
            for (byte i = 0; i < SETS.Length; i++)
            {
                if (expresion.Contains(SETS[i]))
                {
                    expresion = expresion.Replace(SETS[i], ((char)i).ToString());
                    diccionario.Add(((char)i).ToString(),SETS[i]);
                
                }
            }

            return new KeyValuePair<Dictionary<string, string>, string>(diccionario,expresion);
        }
    }
}
