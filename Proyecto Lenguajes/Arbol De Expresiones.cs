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
        
        public static NodoExpresion Raiz = new NodoExpresion();
        public List<NodoExpresion> SubArboles = new List<NodoExpresion>();
        public Dictionary<string, NodoExpresion> Substituicion = new Dictionary<string, NodoExpresion>();
        public List<NodoExpresion> NodosHoja = new List<NodoExpresion>();
        
        public static Arbol_De_Expresiones Instance
        {
            get
            {
                if (_instance == null) _instance = new Arbol_De_Expresiones();
                return _instance;
            }
        }

        public Dictionary<string, string> Follows= new Dictionary<string, string>();



        static int IndiceSubstiucion = 1;

        public NodoExpresion Generar_Arbol(string MegaExpresion, string[]SETS)
        {
            //regularizacion
            var comillas = '"';
            var comilla = "'";
            MegaExpresion = MegaExpresion.Replace(" ?", "?").Replace(" *", "*").Replace(" +", "+").Replace($" {comillas}", $"{comillas}").Replace($"{comillas} ", $"{comillas}").Replace($" {comilla}", $"{comilla}").Replace($"{comilla} ", $"{comilla}").Replace("''", ".").Replace("('", "(").Replace("')", ")").Replace(" ( ", "(").Replace(" | ", "|").Replace(" )", ")").Replace(" ", ".");
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
            while (SubArboles.Count!=2)
            {
                SubArboles[0] = JuntarArbolConOr(SubArboles[0], SubArboles[1]);
                SubArboles.RemoveAt(1);
            }
            Raiz = ObtenerRaiz(SubArboles[0],SubArboles[1]);
            CalcularFollows();
            return Raiz;
        }
        void LecturaDinamica(string ExpresionActual,string[] SETS)
        {
            //representacion
            //Original
           // var DiccionarioCamino = new Dictionary<string, string>();
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
                        id=IndiceSubstiucion,
                        PrimeraPos=$"{IndiceSubstiucion},",
                        UltimaPos=$"{IndiceSubstiucion},",
                        Nullable=false,
                        
                    };
                    NodosHoja.Add(nievo);

                    SubArboles.Add(nievo);
                    IndiceSubstiucion++;
                    completado = true;
                }
                if (masPequeño=="*"|| masPequeño == "+" || masPequeño == "?" || masPequeño == "." || masPequeño == "|")
                {
                    //armar sub arbol especial
                    var nievo = new NodoExpresion()
                    {
                        Nombre = masPequeño,
                        id = IndiceSubstiucion,
                        PrimeraPos = $"{IndiceSubstiucion},",
                        UltimaPos = $"{IndiceSubstiucion},",
                        Nullable = false,

                    };
                    NodosHoja.Add(nievo);

                    SubArboles.Add(nievo);
                    IndiceSubstiucion++;
                    completado = true;
                }
                //*
                while (masPequeño.Contains('*') && completado == false)
                {
                var Asterisco_index = masPequeño.IndexOf('*');

                if (Asterisco_index!=-1 && completado == false)
                {
                        var Aconstruir = A_Construir(masPequeño, Asterisco_index, "*");
                        // agregar y construir
                        var Referencia = Asterisco(Aconstruir,descompresion);
                    masPequeño = masPequeño.Replace(Aconstruir,Referencia);
                }
                }

                //+     
                while (masPequeño.Contains('+')&& completado==false)
                {

                    var Mas_index = masPequeño.IndexOf('+');
                    if (Mas_index != -1 && completado == false)
                    {
                        var Aconstruir = A_Construir(masPequeño, Mas_index, "+");
                        // agregar y construir
                        var Referencia = Mas(Aconstruir, descompresion);
                        masPequeño = masPequeño.Replace(Aconstruir, Referencia);
                        // agregar y construir
                    }
                }
                //?     
                while (masPequeño.Contains('?') && completado == false)
                {

                    var Mas_index = masPequeño.IndexOf('?');
                    if (Mas_index != -1 && completado == false)
                    {
                        var Aconstruir = A_Construir(masPequeño, Mas_index, "?");
                        // agregar y construir
                        var Referencia = Interrogacion(Aconstruir, descompresion);
                        masPequeño = masPequeño.Replace(Aconstruir, Referencia);
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
        string ArmarInterior(string ExpresionActual, string[]SETS)
        {
            var completado = false;
            var Compresion = Indexado(ExpresionActual, SETS);
            var descompresion = Compresion.Key;
            ExpresionActual = Compresion.Value;
            //no contiene ningun operador
            if (!ExpresionActual.Contains('*') && !ExpresionActual.Contains('|') && !ExpresionActual.Contains('+') && !ExpresionActual.Contains('?') && !ExpresionActual.Contains('.'))
            {
                var nievo = new NodoExpresion()
                {
                    Nombre = ExpresionActual,
                    id = IndiceSubstiucion,
                    PrimeraPos = $"{IndiceSubstiucion},",
                    UltimaPos = $"{IndiceSubstiucion},",
                    Nullable = false,

                };
                NodosHoja.Add(nievo);

                SubArboles.Add(nievo);
                IndiceSubstiucion++;
                completado = true;
            }
            if (ExpresionActual == "*" || ExpresionActual == "+" || ExpresionActual == "?" || ExpresionActual == "." || ExpresionActual == "|")
            {
                //armar sub arbol especial
                var nievo = new NodoExpresion()
                {
                    Nombre = ExpresionActual,
                    id = IndiceSubstiucion,
                    PrimeraPos = $"{IndiceSubstiucion},",
                    UltimaPos = $"{IndiceSubstiucion},",
                    Nullable = false,

                };
                NodosHoja.Add(nievo);

                SubArboles.Add(nievo);
                IndiceSubstiucion++;
                completado = true;
            }
            //*
            while (ExpresionActual.Contains('*') && completado == false)
            {
                var Asterisco_index = ExpresionActual.IndexOf('*');

                if (Asterisco_index != -1 && completado == false)
                {
                    var Aconstruir = A_Construir(ExpresionActual, Asterisco_index, "*");
                    // agregar y construir
                    var Referencia = Asterisco(Aconstruir, descompresion);
                    ExpresionActual = ExpresionActual.Replace(Aconstruir, Referencia);
                }
            }

            //+     
            while (ExpresionActual.Contains('+') && completado == false)
            {

                var Mas_index = ExpresionActual.IndexOf('+');
                if (Mas_index != -1 && completado == false)
                {
                    var Aconstruir = A_Construir(ExpresionActual, Mas_index, "+");
                    // agregar y construir
                    var Referencia = Mas(Aconstruir, descompresion);
                    ExpresionActual = ExpresionActual.Replace(Aconstruir, Referencia);
                    // agregar y construir
                }
            }
            //?     
            while (ExpresionActual.Contains('?') && completado == false)
            {

                var Mas_index = ExpresionActual.IndexOf('?');
                if (Mas_index != -1 && completado == false)
                {
                    var Aconstruir = A_Construir(ExpresionActual, Mas_index, "?");
                    // agregar y construir
                    var Referencia = Interrogacion(Aconstruir, descompresion);
                    ExpresionActual = ExpresionActual.Replace(Aconstruir, Referencia);
                    // agregar y construir
                }
            }
            //.     
            while (ExpresionActual.Contains('.') && completado == false)
            {

                var Concatenacion_index = ExpresionActual.IndexOf('.');
                if (Concatenacion_index != -1 && completado == false)
                {
                    var Aconstruir = A_Construir(ExpresionActual, Concatenacion_index, ".");

                    var eliminar = Concatenacion(Aconstruir, descompresion);

                    // agregar y construir
                    ExpresionActual = ExpresionActual.Replace(Aconstruir, eliminar);
                    if (Substituicion.ContainsKey(ExpresionActual))
                    {
                        SubArboles.Add(Substituicion[ExpresionActual]);
                        completado = true;
                    }
                }

            }

            //|
            while (ExpresionActual.Contains('|') && completado == false)
            {

                var Concatenacion_index = ExpresionActual.IndexOf('|');
                if (Concatenacion_index != -1 && completado == false)
                {
                    var Aconstruir = A_Construir(ExpresionActual, Concatenacion_index, "|");

                    var eliminar = Or(Aconstruir, descompresion);

                    // agregar y construir
                    ExpresionActual = ExpresionActual.Replace(Aconstruir, eliminar);
                    if (Substituicion.ContainsKey(ExpresionActual))
                    {
                       // SubArboles.Add(Substituicion[masPequeño]);
                        completado = true;
                    }
                }
            }
            return ExpresionActual;
        }
        //↑↑↑arreglar agregandole un bool para que sepa cuando guardar y cuando no ↑↑↑↑↑↑
        #region Construcciones
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
                devolver.PrimeraPos = devolver.C1.PrimeraPos;
                devolver.UltimaPos = devolver.C1.UltimaPos;
                devolver.Nullable = false;
                IndiceSubstiucion++;
                //existe, asignar
                //
            }
            else
            //si no existe ninguno, uno nuevo
            {
                var hoja = new NodoExpresion()
                {
                    id = IndiceSubstiucion,
                    Nombre = diccionaro[expresion],
                    Nullable = false,
                    Padre = devolver
                };
                hoja.PrimeraPos += $"{IndiceSubstiucion},";
                hoja.UltimaPos += $"{IndiceSubstiucion},";


                devolver.Nombre = "+";
                devolver.C1 = hoja;
                devolver.PrimeraPos = devolver.C1.PrimeraPos;
                devolver.UltimaPos = devolver.C1.UltimaPos;
                devolver.Nullable = false;
                NodosHoja.Add(hoja);


                
                IndiceSubstiucion++;
            }
            if (Substituicion.ContainsKey($"[{IndiceSubstiucion}]"))
            {
                IndiceSubstiucion++;
                Substituicion.Add($"[{IndiceSubstiucion}]", devolver);
            }
            else
            {

                Substituicion.Add($"[{IndiceSubstiucion}]", devolver);
            }
            return $"[{IndiceSubstiucion}]";
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
                devolver.PrimeraPos = devolver.C1.PrimeraPos;
                devolver.UltimaPos = devolver.C1.UltimaPos;
                devolver.Nullable = true;
                IndiceSubstiucion++;
                //
            }
            else
            //si no existe ninguno, uno nuevo
            {
                var hoja= new NodoExpresion()
                {
                    id = IndiceSubstiucion,
                    Nombre = diccionaro[expresion],
                    Nullable = false,
                    Padre = devolver
                };
                hoja.PrimeraPos += $"{IndiceSubstiucion},";
                hoja.UltimaPos += $"{IndiceSubstiucion},";


                devolver.Nombre = "*";
                devolver.C1 = hoja;
                devolver.PrimeraPos = devolver.C1.PrimeraPos;
                devolver.UltimaPos= devolver.C1.UltimaPos;
                devolver.Nullable = true;
                NodosHoja.Add(hoja);

                IndiceSubstiucion++;
            }

            if (Substituicion.ContainsKey($"[{IndiceSubstiucion}]"))
            {
                IndiceSubstiucion++;
            Substituicion.Add($"[{IndiceSubstiucion}]",devolver);
            }
            else
            {

            Substituicion.Add($"[{IndiceSubstiucion}]",devolver);
            }

            return $"[{IndiceSubstiucion}]";
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
                        PrimeraPos = $"{IndiceSubstiucion},",
                        UltimaPos = $"{IndiceSubstiucion},",
                        Nullable = false,
                        Padre = nuevo,
                        id = IndiceSubstiucion,
                    };

                    NodosHoja.Add(nuevo.C1);
                }
                else
                {
                    nuevo.C1 = new NodoExpresion()
                    {
                        Nombre = izq,
                        PrimeraPos = $"{IndiceSubstiucion},",
                        UltimaPos = $"{IndiceSubstiucion},",
                        Nullable = false,
                        Padre = nuevo,
                        id = IndiceSubstiucion,
                    };
                    NodosHoja.Add(nuevo.C1);

                }
                IndiceSubstiucion++;
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
                        PrimeraPos = $"{IndiceSubstiucion},",
                        UltimaPos = $"{IndiceSubstiucion},",
                        Nullable = false,
                        Padre = nuevo,
                        id = IndiceSubstiucion,
                    };
                    NodosHoja.Add(nuevo.C2);

                }
                else
                {
                    nuevo.C2 = new NodoExpresion()
                    {
                        Nombre = der,
                        PrimeraPos = $"{IndiceSubstiucion},",
                        UltimaPos = $"{IndiceSubstiucion},",
                        Nullable = false,
                        Padre = nuevo,
                        id = IndiceSubstiucion,
                    };
                    NodosHoja.Add(nuevo.C2);

                }
                IndiceSubstiucion++;
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
                nuevo.PrimeraPos = $"{nuevo.C1.PrimeraPos}{nuevo.C2.PrimeraPos}";
            }
            else
            {//false
                nuevo.PrimeraPos = nuevo.C1.PrimeraPos;

            }

            //last
            if (nuevo.C2.Nullable == true)
            {//true 
                nuevo.UltimaPos = $"{nuevo.C1.UltimaPos}{nuevo.C2.UltimaPos}";

            }
            else
            {//false
                nuevo.UltimaPos = nuevo.C2.UltimaPos;
            }


            if (Substituicion.ContainsKey($"[{IndiceSubstiucion}]"))
            {
                IndiceSubstiucion++;
                Substituicion.Add($"[{IndiceSubstiucion}]", nuevo);
            }
            else
            {
                Substituicion.Add($"[{IndiceSubstiucion}]", nuevo);

            }

            return $"[{IndiceSubstiucion}]";
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
                        PrimeraPos = $"{IndiceSubstiucion},",
                        UltimaPos = $"{IndiceSubstiucion},",
                        Nullable = false,
                        Padre = nuevo,
                        id = IndiceSubstiucion,
                    };
                    NodosHoja.Add(nuevo.C1);

                }
                else
                {
                    nuevo.C1 = new NodoExpresion()
                    {
                        Nombre = izq,
                        PrimeraPos = $"{IndiceSubstiucion},",
                        UltimaPos = $"{IndiceSubstiucion},",
                        Nullable = false,
                        Padre = nuevo,
                        id = IndiceSubstiucion,
                    };
                    NodosHoja.Add(nuevo.C1);

                }
                IndiceSubstiucion++;
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
                        PrimeraPos = $"{IndiceSubstiucion},",
                        UltimaPos = $"{IndiceSubstiucion},",
                        Nullable = false,
                        Padre = nuevo,
                        id = IndiceSubstiucion,
                    };
                    NodosHoja.Add(nuevo.C2);

                }
                else
                {
                    nuevo.C2 = new NodoExpresion()
                    {
                        Nombre = der,
                        PrimeraPos = $"{IndiceSubstiucion},",
                        UltimaPos = $"{IndiceSubstiucion},",
                        Nullable = false,
                        Padre = nuevo,
                        id = IndiceSubstiucion,
                    };
                    NodosHoja.Add(nuevo.C2);

                }
                IndiceSubstiucion++;
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
            nuevo.PrimeraPos = $"{nuevo.C1.PrimeraPos}{nuevo.C2.PrimeraPos}";
            

            //last
            nuevo.UltimaPos = $"{nuevo.C1.UltimaPos}{nuevo.C2.UltimaPos}";

            
            if (Substituicion.ContainsKey($"[{IndiceSubstiucion}]"))
            {
                IndiceSubstiucion++;
            Substituicion.Add($"[{IndiceSubstiucion}]", nuevo);
            }
            else
            {
            Substituicion.Add($"[{IndiceSubstiucion}]", nuevo);

            }

            return $"[{IndiceSubstiucion}]";

        }
        string Interrogacion(string expresion, Dictionary<string, string> diccionaro)
        {
            var devolver = new NodoExpresion();
            expresion = expresion.Replace("?", "");

            //ir a traer
            if (Substituicion.ContainsKey(expresion))
            {
                //existe, asignar
                var nuevo = Substituicion[expresion];
                devolver.Nombre = "*";
                devolver.C1 = nuevo;
                devolver.PrimeraPos = devolver.C1.PrimeraPos;
                devolver.UltimaPos = devolver.C1.UltimaPos;
                devolver.Nullable = false;
                IndiceSubstiucion++;
                //existe, asignar
                //
            }
            else
            //si no existe ninguno, uno nuevo
            {
                var hoja = new NodoExpresion()
                {
                    id = IndiceSubstiucion,
                    Nombre = diccionaro[expresion],
                    Nullable = false,
                    Padre = devolver
                };
                hoja.PrimeraPos += $"{IndiceSubstiucion},";
                hoja.UltimaPos += $"{IndiceSubstiucion},";


                devolver.Nombre = "?";
                devolver.C1 = hoja;
                devolver.PrimeraPos = devolver.C1.PrimeraPos;
                devolver.UltimaPos = devolver.C1.UltimaPos;
                devolver.Nullable = true;
                NodosHoja.Add(hoja);

                IndiceSubstiucion++;
            }
            if (Substituicion.ContainsKey($"[{IndiceSubstiucion}]"))
            {
                IndiceSubstiucion++;
                Substituicion.Add($"[{IndiceSubstiucion}]", devolver);
            }
            else
            {

                Substituicion.Add($"[{IndiceSubstiucion}]", devolver);
            }
            return $"[{IndiceSubstiucion}]";
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
                var retorno = string.Empty;
                for (int i = 1; i < copia.Length; i++)
                {
                    retorno += copia[i];
                }
                copia = retorno;
            }
            return copia;
        }
        NodoExpresion JuntarArbolConOr(NodoExpresion nC1, NodoExpresion nC2)
       {
            NodoExpresion nuevo = new NodoExpresion()
            {
                C1 = nC1,
                C2 = nC2
            };
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
            nuevo.Nombre = "|";
            //FIRSTs
            nuevo.PrimeraPos = $"{nuevo.C1.PrimeraPos}{nuevo.C2.PrimeraPos}";
            //last
            nuevo.UltimaPos = $"{nuevo.C1.UltimaPos}{nuevo.C2.UltimaPos}";
            return nuevo;
        }
        NodoExpresion ObtenerRaiz(NodoExpresion nC1, NodoExpresion Final)
        {
            NodoExpresion nuevo = new NodoExpresion()
            {
                C1 = nC1,
                C2 = Final
            };
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
                nuevo.PrimeraPos = $"{nuevo.C1.PrimeraPos}{nuevo.C2.PrimeraPos}";
            }
            else
            {//false
                nuevo.PrimeraPos = nuevo.C1.PrimeraPos;

            }

            //last
            if (nuevo.C2.Nullable == true)
            {//true 
                nuevo.UltimaPos = $"{nuevo.C1.UltimaPos}{nuevo.C2.UltimaPos}";

            }
            else
            {//false
                nuevo.UltimaPos = nuevo.C2.UltimaPos;
            }
            return nuevo;
        }
        #endregion
       void CalcularFollows()
       {
            foreach (var item in NodosHoja)
            {
                Follows.Add(item.id.ToString(),"");
            }
            Inorder(Raiz);
       }
        void Inorder(NodoExpresion Root)
        {
            if (Root != null)
            {
               
                Inorder(Root.C1);
                if (Root.Nombre == ".")
                {

                    var Firsts = Root.C1.UltimaPos.Split(',');
                    var Lasts = Root.C2.PrimeraPos.Split(',');
                    foreach (var primera in Firsts)
                    {
                        if (primera!="")
                        {

                            foreach (var ultima in Lasts)
                            {
                                if (ultima != "")
                                {
                                    Follows[primera] += $"{ultima},";
                                }
                            }
                        }
                    }
                }
                else if (Root.Nombre == "*")
                {
                    var Firsts = Root.UltimaPos.Split(',');
                    var Lasts = Root.PrimeraPos.Split(',');
                    foreach (var primera in Firsts)
                    {
                        if (primera != "")
                        {

                            foreach (var ultima in Lasts)
                            {
                                if (ultima != "")
                                {
                                    Follows[primera] += $"{ultima},";
                                }
                            }
                        }
                    }
                }
                Inorder(Root.C2);
            }
        }
    }
}
