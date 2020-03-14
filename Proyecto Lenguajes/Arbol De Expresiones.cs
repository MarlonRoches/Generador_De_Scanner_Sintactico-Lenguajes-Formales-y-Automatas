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
        public Dictionary<string, NodoExpresion> Diccionario_Nodos = new Dictionary<string, NodoExpresion>();
        public Dictionary<string, NodoExpresion> Substituicion = new Dictionary<string, NodoExpresion>();
        static int n = 1;
        static int p = 1;
        public void GenerarArbol(string MegaExpresion, Dictionary<string, string> Tokens, string[]SETS)
        {
            //regularizacion
            var comillas = '"';
            var comilla = "'";
            MegaExpresion = MegaExpresion.Replace(" *", "*").Replace(" +", "+").Replace($" {comillas}", $"{comillas}").Replace($"{comillas} ", $"{comillas}").Replace($" {comilla}", $"{comilla}").Replace($"{comilla} ", $"{comilla}").Replace("''", ".").Replace("('", "(").Replace("')", ")").Replace(" ( ", "(").Replace(" | ", "|").Replace(" )", ")").Replace(" ", ".");
            var arreglo = MegaExpresion.Substring(0, MegaExpresion.Length).Split('☼');
            for (int i = 0; i < SETS.Length; i++)
            {
                SETS[i] = SETS[i].Replace(" ", "");
            }
            foreach (var item in arreglo)
            {
                LecturaDinamica(item,SETS);
            }

            // armar arbol general

        }
        void LecturaDinamica(string ExpresionActual,string[] SETS)
        {
            //representacion
            //Original
            var DiccionarioCamino = new Dictionary<string, string>();
            string masPequeño = string.Empty;
            if (ExpresionActual.Length==1)
            {
                masPequeño = ExpresionActual;
            }
            else
            {

            masPequeño = ExpresionActual.Substring(1, ExpresionActual.Length-2);
            }
            //var Camino = new List<string>();
            //ir al mas pequeño

            if (masPequeño.Contains('('))
            {
                var xd = Indexado(masPequeño, SETS);
                masPequeño = xd.Value;
                while (!Substituicion.ContainsKey(masPequeño))
                {
                    var index1 = masPequeño.IndexOf('(')+1;
                    var index2 = masPequeño.LastIndexOf(')');
                    var CantidadDeSubstring = index2 - index1;
                    var aux = string.Empty;
                    if (index1!=-1&& index2 != -1)
                    {

                     aux = masPequeño.Substring(index1, CantidadDeSubstring);
                    }
                    else
                    {
                        aux = masPequeño;

                    }
                    // mandar a armar el aux
                    var alv = ArmarInterior(aux, SETS);
                    // mandar a armar el aux
                    masPequeño = masPequeño.Replace(aux, alv);
                    //limpiamos 
                    masPequeño = masPequeño.Replace($"({alv}", $".{alv}").Replace($"{alv})", $"{alv}");
                    
                    //Camino.Add(masPequeño);
                    //DiccionarioCamino.Add($"[{n}]", masPequeño);
                    //masPequeño = aux;
                }
                //Camino.Add(masPequeño);
            
            }
            else
            {// una expresion simple
                var completado = false;
                var Compresion = Indexado(masPequeño,SETS);
                var descompresion = Compresion.Key;
                masPequeño = Compresion.Value;
                var arreglado = ArreglarTexto(masPequeño,SETS,descompresion.Keys.ToArray());
                masPequeño = arreglado;
                //no contiene ningun operador
                if (!masPequeño.Contains('*') && !masPequeño.Contains('|')&& !masPequeño.Contains('+')&& !masPequeño.Contains('?')&& !masPequeño.Contains('.'))
                {
                    var nievo = new NodoExpresion()
                    {
                        Nombre = masPequeño,
                        id=n,
                        First=$"{n},",
                        Last=$"{n},",
                        Nullable=false,
                        
                    };
                    SubArboles.Add(nievo);
                    n++;
                    completado = true;
                }
                if (masPequeño=="*"|| masPequeño == "+" || masPequeño == "?" || masPequeño == "." || masPequeño == "|")
                {
                    //armar sub arbol especial
                    var nievo = new NodoExpresion()
                    {
                        Nombre = masPequeño,
                        id = n,
                        First = $"{n},",
                        Last = $"{n},",
                        Nullable = false,

                    };
                    SubArboles.Add(nievo);
                    n++;
                    completado = true;
                }
                //*
                while (masPequeño.Contains('*') && completado == false)
                {
                var Asterisco_index = masPequeño.IndexOf('*');

                if (Asterisco_index!=-1 && completado == false)
                {
                    var Aconstruir  = masPequeño.Substring(Asterisco_index - 1, 2);
                    // agregar y construir
                    var xdd = Asterisco(Aconstruir,descompresion);
                    masPequeño = masPequeño.Replace(Aconstruir,xdd);
                }
                }

                //+     
                while (masPequeño.Contains('+')&& completado==false)
                {

                    var Mas_index = masPequeño.IndexOf('+');
                    if (Mas_index != -1 && completado == false)
                    {
                        var Aconstruir = masPequeño.Substring(Mas_index - 1, 2);
                        // agregar y construir
                        var xdd = Mas(Aconstruir, descompresion);
                        masPequeño = masPequeño.Replace(Aconstruir, xdd);
                        // agregar y construir
                    }
                }

                //.     
                while (masPequeño.Contains('.') && completado == false)
                {

                    var Concatenacion_index = masPequeño.IndexOf('.');
                    if (Concatenacion_index != -1 && completado == false)
                    {
                        var Aconstruir = A_Construir(masPequeño,Concatenacion_index,".");

                        var eliminar = Concatenacion(Aconstruir,descompresion);

                        // agregar y construir
                        masPequeño = masPequeño.Replace(Aconstruir,eliminar);
                        if (Substituicion.ContainsKey(masPequeño))
                        {
                            SubArboles.Add(Substituicion[masPequeño]);
                            completado = true;
                        }
                    }

                }

                //|
                while (masPequeño.Contains('|')&&completado == false)
                {

                    var Concatenacion_index = masPequeño.IndexOf('|');
                    if (Concatenacion_index != -1 && completado == false)
                    {
                        var Aconstruir = A_Construir(masPequeño, Concatenacion_index, "|");

                        var eliminar = Or(Aconstruir, descompresion);

                        // agregar y construir
                        masPequeño = masPequeño.Replace(Aconstruir, eliminar);
                        if (Substituicion.ContainsKey(masPequeño))
                        {
                            SubArboles.Add(Substituicion[masPequeño]);
                            completado = true;
                        }
                    }
                }
            }
        }

        string ArmarSubexpresionInterior(string original, string armar)
        {
            return "";
        }
        string Mas(string expresion, Dictionary<string, string> diccionaro)
        {
            var devolver = new NodoExpresion();
            expresion = expresion.Replace("+", "");

            //ir a traer
            if (Substituicion.ContainsKey(expresion))
            {
                //existe, asignar
                var nuevo = Substituicion[expresion];
                devolver.Nombre = "*";
                devolver.C1 = nuevo;
                devolver.First = devolver.C1.First;
                devolver.Last = devolver.C1.Last;
                devolver.Nullable = true;
                n++;
                //existe, asignar
                //
            }
            else
            //si no existe ninguno, uno nuevo
            {
                var hoja = new NodoExpresion()
                {
                    id = n,
                    Nombre = diccionaro[expresion],
                    Nullable = false,
                    Padre = devolver
                };
                hoja.First += $"{n},";
                hoja.Last += $"{n},";


                devolver.Nombre = "+";
                devolver.C1 = hoja;
                devolver.First = devolver.C1.First;
                devolver.Last = devolver.C1.Last;
                devolver.Nullable = true;
                n++;
            }
            if (Substituicion.ContainsKey($"[{n}]"))
            {
                n++;
                Substituicion.Add($"[{n}]", devolver);
            }
            else
            {

                Substituicion.Add($"[{n}]", devolver);
            }
            return $"[{n}]";
        }
        string Asterisco(string expresion, Dictionary<string, string> diccionaro)
        {
            var devolver = new NodoExpresion();
            expresion = expresion.Replace("*", "");

            //ir a traer
            if (Substituicion.ContainsKey(expresion))
            {
                //existe, asignar
                var nuevo = Substituicion[expresion];
                devolver.Nombre = "*";
                devolver.C1 = nuevo;
                devolver.First = devolver.C1.First;
                devolver.Last = devolver.C1.Last;
                devolver.Nullable = true;
                n++;
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
                devolver.C1 = hoja;
                devolver.First = devolver.C1.First;
                devolver.Last= devolver.C1.Last;
                devolver.Nullable = true;
                n++;
            }

            if (Substituicion.ContainsKey($"[{n}]"))
            {
                n++;
            Substituicion.Add($"[{n}]",devolver);
            }
            else
            {

            Substituicion.Add($"[{n}]",devolver);
            }

            return $"[{n}]";
        }
        string Concatenacion(string expresion, Dictionary<string, string> descompresion)
        {
            var nuevo = new NodoExpresion();
            var izq = expresion.Split('.')[0];
            var der = expresion.Split('.')[1];
           
            
            if (Substituicion.ContainsKey(izq))
            {//obtener nodo representativo
                nuevo.C1 = Substituicion[izq];

            }
            else
            {//asignar set decompreso o el valor de el actual
                if (descompresion.ContainsKey(izq))
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
                }
                else
                {
                    nuevo.C1 = new NodoExpresion()
                    {
                        Nombre = izq,
                        First = $"{n},",
                        Last = $"{n},",
                        Nullable = false,
                        Padre = nuevo,
                        id = n,
                    };
                }
                n++;
            }
            if (Substituicion.ContainsKey(der))
            {
                nuevo.C2 = Substituicion[der];
            }
            else
            {
                if (descompresion.ContainsKey(der))
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
                }
                else
                {
                    nuevo.C2 = new NodoExpresion()
                    {
                        Nombre = der,
                        First = $"{n},",
                        Last = $"{n},",
                        Nullable = false,
                        Padre = nuevo,
                        id = n,
                    };
                }
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
            if (Substituicion.ContainsKey($"[{n}]"))
            {
                n++;
                Substituicion.Add($"[{n}]", nuevo);
            }
            else
            {
                Substituicion.Add($"[{n}]", nuevo);

            }

            return $"[{n}]";
        }
        string Or(string expresion, Dictionary<string, string> descompresion)
        {
            var nuevo = new NodoExpresion();
            var izq = expresion.Split('|')[0];
            var der = expresion.Split('|')[1];
            if (Substituicion.ContainsKey(izq))
            {//obtener nodo representativo
                nuevo.C1 = Substituicion[izq];

            }
            else
            {//asignar set decompreso o el valor de el actual
                if (descompresion.ContainsKey(izq))
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
                }
                else
                {
                    nuevo.C1 = new NodoExpresion()
                    {
                        Nombre = izq,
                        First = $"{n},",
                        Last = $"{n},",
                        Nullable = false,
                        Padre = nuevo,
                        id = n,
                    };
                }
                n++;
            }
            if (Substituicion.ContainsKey(der))
            {
                nuevo.C2 = Substituicion[der];
            }
            else
            {
                if (descompresion.ContainsKey(der))
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
                }
                else
                {
                    nuevo.C2 = new NodoExpresion()
                    {
                        Nombre = der,
                        First = $"{n},",
                        Last = $"{n},",
                        Nullable = false,
                        Padre = nuevo,
                        id = n,
                    };
                }
                n++;
            }
            nuevo.Nombre = "|";
            //validar nullabilidad de el nuevo
            if (nuevo.C1.Nullable || nuevo.C2.Nullable)
            {
                nuevo.Nullable = true;
            }
            else
            {
                nuevo.Nullable = false;
            }



            //FIRSTs
            nuevo.First = $"{nuevo.C1.First}{nuevo.C2.First}";
            

            //last
            nuevo.Last = $"{nuevo.C1.Last}{nuevo.C2.Last}";

            
            if (Substituicion.ContainsKey($"[{n}]"))
            {
                n++;
            Substituicion.Add($"[{n}]", nuevo);
            }
            else
            {
            Substituicion.Add($"[{n}]", nuevo);

            }

            return $"[{n}]";

        }
        string Interrogacion(string expresion, Dictionary<string, string> descompresion)
        {
            return "";
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
        string A_Construir(string completo, int indice,string operacion)
        {
            var devolver = string.Empty;
            var de = completo.IndexOf(operacion);
            if (completo[indice-1] == ']')
            {
                var last = completo.IndexOf('[');
                devolver += $"{completo.Substring(last, (indice - last))}";

            }
            else
            {
                devolver += completo[indice - 1];
            }
            if (operacion == "|"|| operacion == ".")
            {

                if (completo[indice + 1] == '[')
                {
                    var last = completo.LastIndexOf(']');
                    devolver += $"{operacion}{completo.Substring(indice + 1, (last - indice))}";
                }
                else
                {
                    devolver += $"{operacion}{completo[indice + 1]}";

                }
            }
            else
            {
                devolver += operacion;


            }
            return devolver;
        }

        string ArreglarTexto(string expresion,string[] SETS,string[] demas)
        {//fixear el texto por los caracteres que no esten para concatenarlos
            var copia = string.Empty;
            for (int i = 0; i < expresion.Length; i++)
            {
                for (int y = 0; y < demas.Length; y++)
                {
                    if (expresion[i].ToString() != demas[y])
                    {
                        if (expresion[i].ToString() != "+"&& expresion[i].ToString() != "*" && expresion[i].ToString() != "|" && expresion[i].ToString() != "." && expresion[i].ToString() != "?")
                        {
                            copia += $".{expresion[i]}.";
                        }
                        else
                        {

                            copia += expresion[i];
                        }
                    }
                    else
                    {
                            copia += expresion[i];
                    }
                }
            }
            copia = copia.Replace(".|.", "|").Replace("..", ".").Replace(".*", "*").Replace("..", ".").Replace(".?", "?");
            if (copia=="")
            {
                copia = expresion;
            }
            else if (copia[0]=='.')
            {
                var xd = string.Empty;
                for (int i = 1; i < copia.Length; i++)
                {
                    xd += copia[i];
                }
                copia = xd;
            }
            return copia;
        }
       
        string ArmarInterior(string masPequeño, string[]SETS)
        {
            var completado = false;
            var Compresion = Indexado(masPequeño, SETS);
            var descompresion = Compresion.Key;
            masPequeño = Compresion.Value;
            var arreglado = ArreglarTexto(masPequeño, SETS, descompresion.Keys.ToArray());
            masPequeño = arreglado;
            //no contiene ningun operador
            if (!masPequeño.Contains('*') && !masPequeño.Contains('|') && !masPequeño.Contains('+') && !masPequeño.Contains('?') && !masPequeño.Contains('.'))
            {
                var nievo = new NodoExpresion()
                {
                    Nombre = masPequeño,
                    id = n,
                    First = $"{n},",
                    Last = $"{n},",
                    Nullable = false,

                };
                SubArboles.Add(nievo);
                n++;
                completado = true;
            }
            if (masPequeño == "*" || masPequeño == "+" || masPequeño == "?" || masPequeño == "." || masPequeño == "|")
            {
                //armar sub arbol especial
                var nievo = new NodoExpresion()
                {
                    Nombre = masPequeño,
                    id = n,
                    First = $"{n},",
                    Last = $"{n},",
                    Nullable = false,

                };
                SubArboles.Add(nievo);
                n++;
                completado = true;
            }
            //*
            while (masPequeño.Contains('*') && completado == false)
            {
                var Asterisco_index = masPequeño.IndexOf('*');

                if (Asterisco_index != -1 && completado == false)
                {
                    var Aconstruir = A_Construir(masPequeño, Asterisco_index, "*");
                    // agregar y construir
                    var xdd = Asterisco(Aconstruir, descompresion);
                    masPequeño = masPequeño.Replace(Aconstruir, xdd);
                }
            }

            //+     
            while (masPequeño.Contains('+') && completado == false)
            {

                var Mas_index = masPequeño.IndexOf('+');
                if (Mas_index != -1 && completado == false)
                {
                    var Aconstruir = masPequeño.Substring(Mas_index - 1, 2);
                    // agregar y construir
                    var xdd = Mas(Aconstruir, descompresion);
                    masPequeño = masPequeño.Replace(Aconstruir, xdd);
                    // agregar y construir
                }
            }

            //.     
            while (masPequeño.Contains('.') && completado == false)
            {

                var Concatenacion_index = masPequeño.IndexOf('.');
                if (Concatenacion_index != -1 && completado == false)
                {
                    var Aconstruir = A_Construir(masPequeño, Concatenacion_index, ".");

                    var eliminar = Concatenacion(Aconstruir, descompresion);

                    // agregar y construir
                    masPequeño = masPequeño.Replace(Aconstruir, eliminar);
                    if (Substituicion.ContainsKey(masPequeño))
                    {
                        SubArboles.Add(Substituicion[masPequeño]);
                        completado = true;
                    }
                }

            }

            //|
            while (masPequeño.Contains('|') && completado == false)
            {

                var Concatenacion_index = masPequeño.IndexOf('|');
                if (Concatenacion_index != -1 && completado == false)
                {
                    var Aconstruir = A_Construir(masPequeño, Concatenacion_index, "|");

                    var eliminar = Or(Aconstruir, descompresion);

                    // agregar y construir
                    masPequeño = masPequeño.Replace(Aconstruir, eliminar);
                    if (Substituicion.ContainsKey(masPequeño))
                    {
                        SubArboles.Add(Substituicion[masPequeño]);
                        completado = true;
                    }
                }
            }
            return masPequeño;
        }
    }
}
