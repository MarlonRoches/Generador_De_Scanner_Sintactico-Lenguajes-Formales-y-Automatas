using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_Lenguajes;
using Newtonsoft.Json;
namespace Proyecto_Lenguajes
{
    class Program
    {
        static void Main(string[] args)
        {
            var SETS = new Dictionary<string, List<char>>();
            var TOKENS = new Dictionary<string, string>();
            var Error = new Dictionary<string, string>();
            var ACTIONS = new Dictionary<string, string>();
            #region Fase 1

            Console.WriteLine("Arrastrar El Archivo de prueba hacia la consola");
            var linea_actual = 0;
            var path = "C:\\Users\\roche\\Desktop\\Lenguajes\\archivo 19.txt";
            var reader = new StreamReader(path);
            var linea = reader.ReadLine(); linea_actual++;
            linea = linea.Replace(" ", "");
            var MegaExpresion = string.Empty;
            var comillas = '"';
            var comillaSimple = "'";
            //C:\Users\roche\Desktop\Lenguajes\archivo 19.txt
            //leer Sets

            while (linea != "TOKENS")
            {
                linea = reader.ReadLine(); linea_actual++;
                if (linea == "TOKENS")
                {
                    break;
                }
                var SetId = linea.Substring(0, linea.IndexOf('=')).Replace("\t", "").Trim();
                var TokensArray = linea.Remove(0, linea.IndexOf('=') + 1).Trim().Split('+');
                //validar chars
                foreach (var item in TokensArray)
                {

                    var lal = item.Replace("..", "").Replace($"'","").Replace($"{comillaSimple}{comillaSimple}","");
                    var lol = lal.ToCharArray();
                    if (!SETS.ContainsKey(SetId.Trim()))
                    {
                        SetId = SetId.Replace("  ", " ");
                        SETS.Add(SetId.Trim(), new List<char>());
                    }
                    if (lol.Length == 1)
                    {
                        SETS[SetId].Add(lol[0]);
                    }
                    else
                    {

                        for (int i = lol[0]; i <= lol[1]; i++)
                        {

                            SETS[SetId.Trim()].Add((char)i);
                        }
                    }

                }

            }

            //leer
            linea = reader.ReadLine();
            linea_actual++;
            while (linea != "ACTIONS")
            {

                if (linea.Contains("'''"))
                {

                    linea = linea.Replace("\t", "").Replace("'''", "'").Replace("  ", " ").Replace(("'" + '"' + "'").ToString(), comillas.ToString());
                }
                else
                {
                    linea = linea.Replace("\t", "").Replace("  ", " ");

                }

                if (linea == "" || linea == " " || linea == "  ")
                {

                }
                else
                {

                    var TokenId = linea.Substring(0, linea.IndexOf('=')).Replace("  ", " ");
                    var Expresion = linea.Remove(0, linea.IndexOf('=') + 1).Trim();
                    if (TOKENS.ContainsKey(TokenId))
                    {
                        //ya existe el token
                        Console.Clear();
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error en la linea {linea_actual}");
                        Console.WriteLine($"{TokenId} => {Expresion}  repetido");


                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                    else
                    {
                        TOKENS.Add(TokenId, Expresion);

                    }


                    MegaExpresion += $"({Expresion})☼";//☼ = 15
                }
                linea = reader.ReadLine(); linea_actual++;
            }
            MegaExpresion += "Ø";//Ø=157
                                 //try
                                 // {
            var Transiciones = Arbol_De_Expresiones.Instance.Generar_Arbol(MegaExpresion, SETS.Keys.ToArray());
            var Raiz = Arbol_De_Expresiones.Raiz;
            //}
            //catch (Exception)
            //{
            //    Console.Clear();
            //    Console.BackgroundColor = ConsoleColor.White;
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine($"Error => Token no encontrado");

            //    Console.ReadLine();
            //    Environment.Exit(0);
            //    throw;
            //}
            //Arbol_De_Expresiones.Instance.Inorder(Arbol_De_Expresiones.Instance.Diccionario_Nodos);

            while (linea.Trim() != "{")
            {
                linea = reader.ReadLine();

            }

            linea = reader.ReadLine();
            while (linea.Trim() != "}")
            {

                var Array = linea.Trim().Replace(" ", "").Replace(comillaSimple, "").Split('=');
                ACTIONS.Add(Array[1], Array[0]);
                linea = reader.ReadLine();

            }

            while (linea != null)
            {
                if (linea.ToLower().Contains("error"))
                {
                    var Array = linea.Trim().Replace(" ", "").Replace(comillaSimple, "").Split('=');
                    Error.Add(Array[1], Array[0]);
                }
                linea = reader.ReadLine();
            }
            ///mostrar SETS
            foreach (var set in SETS)
            {
                Console.WriteLine($"El SET '{set.Key}' contiene los siguiente scaracteres:");
                foreach (var caract in set.Value)
                {
                    Console.Write($"'{caract}', ");
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            ///mostrar validacion
            ///errores
            ///enumeracion de tokens
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("TOKENS");
            foreach (var item in TOKENS)
            {
                Console.WriteLine($"{item.Key}: { item.Value}");
            }
            ///escritura de tokens

            ///firts
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("FIRSTS");
            EscribirFirsts(Raiz);

            ///lasts
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("LASTS");
            EscribirLst(Raiz);

            ///follows
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("FOLLOWS");
            EscribirFollows();

            // PRIMERA FASE
            #region Comentado
            //while (linea != null)
            //{
            //    if (linea == "ACTIONS")
            //    {
            //        linea = reader.ReadLine();
            //        linea = reader.ReadLine();
            //        linea = reader.ReadLine().Replace("\t\t", "");
            //        while (!linea.Contains("\t}"))
            //        {

            //            Actions.Add(linea.Split('=')[0],linea.Split('=')[1]);
            //            linea = reader.ReadLine().Replace("\t\t", "");
            //        }
            //        linea = reader.ReadLine();
            //    }
            //    if (linea.Contains("ERROR"))
            //    {
            //        Error.Add(linea.Replace(" ", "").Split('=')[1], linea.Replace(" ", "").Split('=')[0]);
            //        linea = reader.ReadLine();
            //    }

            //        linea = reader.ReadLine();
            //}
            //Console.Clear();

            //Console.WriteLine("Sets");
            //Console.WriteLine("");
            //foreach (var item in SETS)
            //{
            //    foreach (var nodo in item.Value)
            //    {
            //        Console.WriteLine($"{item.Key} => {nodo}");
            //    }
            //}

            //Console.WriteLine("");

            ////Console.WriteLine("Tokens");
            ////Console.WriteLine("");
            ////foreach (var item in TOKENS)
            ////{
            ////    Console.WriteLine($"{item.Key} => {item.Value}");

            ////}

            //// Console.WriteLine("");

            ////Console.WriteLine("Actions");
            ////Console.WriteLine("");
            ////foreach (var item in Actions        )
            ////{
            ////    Console.WriteLine($"{item.Key} => {item.Value}");

            ////}




            ////Console.WriteLine("");

            ////Console.WriteLine("Error");
            ////Console.WriteLine("");
            ////foreach (var item in Error)
            ////{
            ////    Console.WriteLine($"{item.Key} => {item.Value}");

            ////}

            #endregion

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("------HOJAS-------");
            var n = 1;
            foreach (var item in Arbol_De_Expresiones.Instance.NodosHoja)
            {
                Console.WriteLine($"Hoja No.{n}: {item.Value.id}");
                n++;
            }
            #endregion

            //Console.ReadLine();

            var Compuestos = new Dictionary<string, string>();
            var Simples = new Dictionary<string, string>();
            var Alfabeto = "";

            
            foreach (var item in SETS)
            {
                foreach (var nodo in item.Value)
                {
                    Alfabeto += $"{nodo},";
                }
            }

            foreach (var item in TOKENS)
            {
                var key = item.Key.Split('^');
                if (!Compuestos.ContainsKey(item.Key))
                {
                    var aux = new string[1];
                     var lel = new string[1];
                    if (item.Value.Contains($"{comillaSimple}"))
                    {
                    aux = item.Value.Replace("+","").Replace("*","").Replace("|","").Replace(".","").Replace("(", "").Replace(")", "").Replace("'", "").Replace("  ", " ").Trim().Split(' ');
                        for (int i = 0; i < aux.Length; i++)
                        {
                            aux[i] = $"{aux[i]}";
                        }
                    lel = SinRepetidos(aux);
                        Simples.Add(aux[0], item.Key.Trim());
                    }
                    else
                    {
                        aux = item.Value.Replace("+","").Replace("*","").Replace("|","").Replace(".","").Replace("(", "").Replace(")", "").Replace("'", "").Replace("  ", " ").Trim().Split(' ');
                        lel = SinRepetidos(aux);
                        Compuestos.Add(item.Key.Trim(), "");
                        
                        for (int i = 0; i < lel.Count(); i++)
                        {
                            if (SETS.ContainsKey(lel[i]))
                            {
                                foreach (var node in SETS[lel[i]])
                                {

                                    Compuestos[item.Key.Trim()] += $"{node},";
                                }
                                    Compuestos[item.Key.Trim()] += "↓";
                            }
                            else
                            {

                                    Compuestos[item.Key.Trim()] += $"{lel[i]},";
                            }
                        }
                    }
                    
                }
            }

            List<KeyValuePair<string, string>> myList = Compuestos.ToList();

            myList.Sort(delegate (KeyValuePair<string, string> pair1, KeyValuePair<string, string> pair2)
                { return pair1.Value.CompareTo(pair2.Value); } );

            Compuestos = new Dictionary<string, string>();
            foreach (var item in myList)
            {
                Compuestos.Add(item.Key,item.Value);
            }
            
            //meta data a enviar
            var json = JsonConvert.SerializeObject(Compuestos);
            var json2 = JsonConvert.SerializeObject(Simples);

            //armando de las actions
            var ifs = "";
            foreach (var item in ACTIONS)
            {
                ifs+= $"case {comillas}{item.Key}{comillas}:\n";
                ifs += $"return {item.Value};\n";
                ifs += "break;\n";
            }
            ifs += "default:\n";


            //probando algoritmos
            var arreglo = "156 x a := b c = d const a".Replace("  "," ").Split(' ');
            foreach (var item in arreglo)
            {
                //busqueda de simpl, si no, busca en compuestos
                if (Simples.ContainsKey(item))
                {
                    var resultado = Simples[item];
                }
                else
                {   //compuesto
                    var lol3 = PerteneceAlLenguajUnico("TOKEN 4", item);
                    var lol = PerteneceAlLenguajUnico("TOKEN 1", item);

                }
               
            }

            //armando el texto
            ifs += "break;\n";
            var codigoDeSalida = "";
            codigoDeSalida+="using System;\n";
            codigoDeSalida+= "using System.Collections.Generic;\n";
            codigoDeSalida+="using System.IO;\n";
            codigoDeSalida+="using System.Linq;\n";
            codigoDeSalida+="using System.Text;\n";
            codigoDeSalida+= "using System.Threading.Tasks;\n";
            codigoDeSalida+= "using Proyecto_Lenguajes;\n";
            codigoDeSalida+= "using Newtonsoft.Json;\n";
            codigoDeSalida+= "namespace Proyecto_Lenguajes\n";
            codigoDeSalida+= "    {\n";
            codigoDeSalida+= "        class Program\n";
            codigoDeSalida+= "        {\n";
            codigoDeSalida+= "            static void Main(string[] args)\n";
            codigoDeSalida+= "            {\n";
            codigoDeSalida+=                    "var Archivo = Console.ReadLine().Trim();\n";
            codigoDeSalida+=                    "var File = new StreamReader(Archivo);\n";
            codigoDeSalida+=                    "var LineaActual = File.ReadLine();\n";
            codigoDeSalida+=                    $"var RESULTADO = {comillas}{comillas};\n";
           codigoDeSalida+= " var Compuestos = new Dictionary<string, string>();\n";
            codigoDeSalida += " var Simples = new Dictionary<string, string>();\n";
            codigoDeSalida += " var Alfabeto = "+";\n";
            codigoDeSalida +=                    "while (LineaActual != null)\n";
            codigoDeSalida+=                    "{\n";
                     

            codigoDeSalida += " var arreglo = LineaActual.Split(' ');\n";
            codigoDeSalida += " foreach (var item in arreglo)\n";
            codigoDeSalida += " {\n";
            codigoDeSalida += "     switch (item)\n";
            codigoDeSalida += "     {\n";

            //GENERANDO LOS cases de los actions
            foreach (var item in ACTIONS)
            {
                codigoDeSalida += $"case {comillas}{item.Key}{comillas}:\n";
                codigoDeSalida += $"RESULTADO = {comillas}{item.Value}{comillas};\n";
               // codigoDeSalida += $"return;\n";
                codigoDeSalida += "break;\n";
            }
            codigoDeSalida += "default:\n";
            // ARMAR IFS DE LOS RESTANTES


            codigoDeSalida += "break;\n";
            codigoDeSalida += "     }\n";
            codigoDeSalida += " }\n";
            codigoDeSalida+= "}\n";



            //metodos
            codigoDeSalida += "bool PerteneceAlLenguajUnico(string Token, string entrada)\n";
            codigoDeSalida += "{\n";
            codigoDeSalida += "    var anterior = false;\n";
            codigoDeSalida += "    var actual = false;\n";


            codigoDeSalida += "    var lenguajes = Compuestos[Token].Split('↓');\n";
            codigoDeSalida += "    for (int i = 0; i < lenguajes.Count(); i++)\n";
            codigoDeSalida += "    {\n";

            codigoDeSalida += $"        if (lenguajes[i] != {comillas}{comillas})\n";
            codigoDeSalida += "        {\n";
            codigoDeSalida += "            for (int j = 0; j < entrada.Length; j++)\n";
            codigoDeSalida += "            {\n";


            codigoDeSalida += $"                if (lenguajes[i].Contains(entrada[j].ToString().Replace({comillas},{comillas}, {comillas}{comillas})))\n";
            codigoDeSalida += "                {\n";
            codigoDeSalida += "                    actual = true;\n";
            codigoDeSalida += "                }\n";
            codigoDeSalida += "                else\n";
            codigoDeSalida += "                {\n";
            codigoDeSalida += "                    actual = false;\n";

            codigoDeSalida += "                }\n";
            codigoDeSalida += "                if (j == 0)\n";
            codigoDeSalida += "                {\n";
            codigoDeSalida += "                    anterior = actual || anterior;\n";
            codigoDeSalida += "                }\n";
            codigoDeSalida += "                else if (j == entrada.Length - 1)\n";
            codigoDeSalida += "                {\n";
            codigoDeSalida += "                    anterior = actual || anterior;\n";
            codigoDeSalida += "                }\n";
            codigoDeSalida += "                else\n";
            codigoDeSalida += "                {\n";
            codigoDeSalida += "                    anterior = actual || anterior;\n";
            codigoDeSalida += "                }\n";
            codigoDeSalida += "            }\n";

            codigoDeSalida += "        }\n";

            codigoDeSalida += "    }\n";
            codigoDeSalida += " return (anterior || actual);\n";
            codigoDeSalida += "}\n";

            // codigoDeSalida += "    return (anterior || actual);
            // codigoDeSalida += "}

            codigoDeSalida += "            }\n";
            codigoDeSalida+= "        }\n";
            codigoDeSalida += "    }\n";

            Console.ReadLine();







            bool PerteneceAlLenguajUnico(string Token, string entrada)
            {
                var anterior = false;
                var actual = false;


                var lenguajes = Compuestos[Token].Split('↓');
                for (int i = 0; i < lenguajes.Count(); i++)
                {

                    if (lenguajes[i] != "")
                    {
                        for (int j = 0; j < entrada.Length; j++)
                        {


                            if (lenguajes[i].Contains(entrada[j].ToString().Replace(",", "")))
                            {
                                actual = true;
                            }
                            else
                            {
                                actual = false;

                            }
                            if (j == 0)
                            {
                                anterior = actual || anterior;
                            }
                            else if (j == entrada.Length - 1)
                            {

                                anterior = actual || anterior;
                            }
                            else
                            {

                                anterior = actual || anterior;
                            }
                        }

                    }

                }


                return (anterior || actual);
            }

            string[] SinRepetidos(string[] Arreglo)
            {
                var dicaux = new Dictionary<string, bool>();
                for (int i = 0; i < Arreglo.Length; i++)
                {
                    if (!dicaux.ContainsKey(Arreglo[i]))
                    {
                        dicaux.Add(Arreglo[i], true);
                    }
                }
                

                return dicaux.Keys.ToArray();
            }
            void Inorder(NodoExpresion Root)
            {
                if (Root != null)
                {
                    Inorder(Root.C1);
                    if (Root.Nombre == "|" || Root.Nombre == "." || Root.Nombre == "*" || Root.Nombre == "?" || Root.Nombre == "+")
                    {

                        Console.Write(Root.Nombre + " ");
                    }
                    else
                    {
                        Console.Write($"'{Root.Nombre}'" + " ");

                    }
                    Inorder(Root.C2);
                }
            }
            void EscribirFirsts(NodoExpresion Root)
            {
                if (Root != null)
                {
                    EscribirFirsts(Root.C1);

                    Console.WriteLine($"Hoja:{Root.Nombre}: First =>{ Root.First}");

                    EscribirFirsts(Root.C2);
                }
            }
            void EscribirLst(NodoExpresion Root)
            {
                if (Root != null)
                {
                    EscribirLst(Root.C1);
                    Console.WriteLine($"Hoja:{Root.Nombre}: First =>    { Root.Last}");

                    EscribirLst(Root.C2);
                }
            }
            void EscribirFollows()
            {
                foreach (var item in Arbol_De_Expresiones.Instance.Follows)
                {
                    Console.WriteLine($"Nodo No. {item.Key}: Follow =>  { item.Value}");
                }
            }
        }

    }
    
}
