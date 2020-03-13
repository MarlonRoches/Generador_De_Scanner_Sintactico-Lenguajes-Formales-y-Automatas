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
        public Dictionary<string, NodoExpresion> Substituicion = new Dictionary<string, NodoExpresion>();
        public int n = 1;
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
            var DiccionarioCamino = new Dictionary<string, string>();
            string masPequeño = ExpresionActual.Substring(1, ExpresionActual.Length-2);
            var Camino = new List<string>();
            //ir al mas pequeño

            if (masPequeño.Contains('('))
            {
                var xd = Indexado(masPequeño, SETS);
                masPequeño = xd.Value;
                while (masPequeño.Contains('('))
                {
                    var index1 = masPequeño.IndexOf('(')+1;
                    var index2 = masPequeño.LastIndexOf(')');
                    var lol = index2 - index1;
                    var aux = masPequeño.Substring(index1, lol);
                    masPequeño = masPequeño.Replace(aux, $"[{n}]");
                        n++;
                    Camino.Add(masPequeño);
                    DiccionarioCamino.Add($"[{n}]", masPequeño);
                    masPequeño = aux;
                }
                Camino.Add(masPequeño);
            }
            else
            {

                var Compresion = Indexado(masPequeño,SETS);
                var descompresion = Compresion.Key;
                masPequeño = Compresion.Value;
                
                //*
                var Asterisco_index = masPequeño.IndexOf('*');
                if (Asterisco_index!=-1)
                {
                    var Aconstruir  = masPequeño.Substring(Asterisco_index - 1, 2);
                    // agregar y construir
                    var xdd = Asterisco(Aconstruir,descompresion);
                    masPequeño = masPequeño.Replace(Aconstruir,xdd);
                }
                
                
                //+     
                var Mas_index = masPequeño.IndexOf('+');
                if (Mas_index != -1)
                {

                    // agregar y construir
                }
                //.     
                var Concatenacion_index = masPequeño.IndexOf('.');
                if (Concatenacion_index != -1)
                {
                    var Aconstruir = masPequeño.Substring(Asterisco_index - 1, 2);

                    var eliminar = Concatenacion(masPequeño,descompresion);
                    // agregar y construir
                }
                //|     

                var Or_index = masPequeño.IndexOf('|');
                if (Or_index != -1)
                {

                    // agregar y construir
                }
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
        string Asterisco(string expresion, Dictionary<string, string> diccionaro)
        {
            var devolver = new NodoExpresion();
            expresion = expresion.Replace("*", "");

            //ir a traer
            if (Substituicion.ContainsKey(expresion))
            {
            //existe, asignar
                //
            }
            else
            //si no existe ninguno, uno nuevo
            {
                var hoja= new NodoExpresion()
                {
                    id = n,
                    Nombre = diccionaro[expresion],
                    Nullable = false,
                    Padre = devolver
                };
                hoja.First += $"{n},";
                hoja.Last += $"{n},";


                devolver.Nombre = "*";
                devolver.Single = hoja;
                devolver.First = devolver.Single.First;
                devolver.Last= devolver.Single.Last;
                devolver.Nullable = true;
                n++;
            }
            Substituicion.Add($"[{n}]",devolver);

            return $"[{n}]";
        }
        string Concatenacion(string expresion, Dictionary<string, string> descompresion)
        {
            var nuevo = new NodoExpresion();
            var izq = expresion.Split('.')[0];
            var der = expresion.Split('.')[1];
            if (Substituicion.ContainsKey(izq))
            {
                nuevo.C1 = Substituicion[izq];

            }
            else
            {
                nuevo.C1 = new NodoExpresion()
                {
                    Nombre = descompresion[izq],
                    First = $"{n},",
                    Last = $"{n},",
                    Nullable = false,
                    Padre = nuevo,
                    id = n,
                };
                n++;
            }
            if (Substituicion.ContainsKey(der))
            {
                nuevo.C2 = Substituicion[der];
            }
            else
            {
                nuevo.C2 = new NodoExpresion()
                {
                    Nombre = descompresion[der],
                    First = $"{n},",
                    Last = $"{n},",
                    Nullable = false,
                    Padre = nuevo,
                    id = n,
                };
                n++;
            }
            nuevo.Nombre = ".";
            //validar nullabilidad de el nuevo
            if (nuevo.C1.Nullable && nuevo.C2.Nullable)
            {
                nuevo.Nullable = true;
            }
            else
            {
                nuevo.Nullable = false;
            }



            //first
            if (nuevo.C1.Nullable == true)
            {//true
                nuevo.First = $"{nuevo.C1.First}{nuevo.C2.First}";
            }
            else
            {//false
                nuevo.First = nuevo.C1.First;

            }
            //last
            if (nuevo.C2.Nullable ==true  )
            {//true 
                nuevo.Last = $"{nuevo.C1.Last}{nuevo.C2.Last}";

            }
            else
            {//false
                nuevo.Last = nuevo.C2.Last;
            }
            Substituicion.Add($"[{n}]", nuevo);

            return $"[{n}]";
        }
        NodoExpresion Or(string expresion)
        {
            var nuevo = new NodoExpresion();
            var izq = expresion.Split('|')[0];
            var der = expresion.Split('|')[1];
            nuevo.Nombre = "|";
            //ir a traer
            nuevo.C1 = new NodoExpresion()
            {
                Nombre = izq,
                First = izq,
                Last = izq,
                Nullable = false,
                Padre=nuevo,

            };
            nuevo.C2 = new NodoExpresion()
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
                    expresion = expresion.Replace(SETS[i], ((char)(i)).ToString());
                    diccionario.Add(((char)i).ToString(),SETS[i]);
                
                }
            }

            return new KeyValuePair<Dictionary<string, string>, string>(diccionario,expresion);
        }
    }
}
