using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_Lenguajes;
namespace Proyecto_Lenguajes
{
    class Program
    {
        private static string cadena;
        private static int i;

        static void Main(string[] args)
        {
            var SETS = new Dictionary<string, List<char>>();
            var TOKENS = new Dictionary<string, string>();
            var Error = new Dictionary<string, string>();
            var Actions = new Dictionary<string, string>();
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
                var SetId = linea.Substring(0, linea.IndexOf('=')).Replace("\t", "");
                var TokensArray = linea.Remove(0, linea.IndexOf('=') + 1).Trim().Split('+');
                //validar chars
                foreach (var item in TokensArray)
                {
                    var lol = item.Replace("..", "").Replace("...", "").ToCharArray();
                    if (!SETS.ContainsKey(SetId))
                    {
                        SetId = SetId.Replace("  ", " ");
                        SETS.Add(SetId, new List<char>());
                    }
                    if (lol.Length == 1)
                    {
                        SETS[SetId].Add(lol[0]);
                    }
                    else
                    {

                        for (int i = lol[0]; i <= lol[1]; i++)
                        {

                            SETS[SetId].Add((char)i);
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
                Actions.Add(Array[1], Array[0]);
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

            Console.ReadLine();

            var Posibilidades = new Dictionary<string, List<string>>();
            foreach (var item in SETS)
            {
                foreach (var nodo in item.Value)
                {

                }
            }




            //var ruta = Console.ReadLine().Replace(comillas.ToString(),""); var Archivo = new FileStream(ruta, FileMode.Open); var Reader = new StreamReader(Archivo);
            //var Lectura = string.Empty;
            //while (Lectura!=null)
            //{
            //    foreach (var item in Lectura)
            //    {

            //        switch (switch_on)
            //        {
            //            default:
            //        }
            //    }
            //    Lectura = reader.ReadLine();
            //}


            ////-------------------------------------------------
            //var Estado = 0;
            //    switch (Estado)
            //    {
            //        case 1:
            //            if (cadena[i] == 9 || cadena[i] == 10 || cadena[i] == 13 ||
            //            cadena[i] == 26 || cadena[i] == 32)
            //            {
            //                if (cadena[i] == 13) then i++;
            //            }
            //            else if ((cadena[i] >= 48 && cadena[i] <= 57))
            //            {
            //                Estado = 2;
            //            } else if ((cadena[i] == 61))
            //            {
            //                Estado = 3;
            //            } else if ((cadena[i] == 58))
            //            {

            //                Estado = 4;
            //            }
            //            else if ((cadena[i] >= 97 && cadena[i] <= 122) || (cadena[i] == 95) || (cadena[i] >= 65 && cadena[i] <= 90))
            //            {
            //                Estado = 5;
            //            }
            //            else
            //            {
            //                Error = true;
            //                Salir = true;
            //            };
            //            break;
            //        case 2:
            //            if ((cadena[i] >= 48 && cadena[i] <= 57))
            //                Estado = 2;
            //            else
            //            {
            //                Retroceso();
            //                Salir = true;
            //            };
            //            break;
            //        case 4:
            //            if ((cadena[i] == 61))
            //                Estado = 6;
            //            else
            //            {
            //                Error = true;
            //                Salir = true;
            //            };
            //            break;
            //        case 5:
            //            if ((cadena[i] >= 97 && cadena[i] <= 122)
            //            || (cadena[i] == 95)
            //            || (cadena[i] >= 65 && cadena[i] <= 90)
            //            || (cadena[i] >= 48 && cadena[i] <= 57))
            //                Estado = 5;
            //            else
            //            {
            //                Retroceso();
            //                Salir = true;
            //            };
            //            break;
            //            /*Case para estado y sus correspondiente no. de token*/
            //            switch (Estado)
            //            {
            //                case 2: NumToken = 1; break;
            //                case 3: NumToken = 2; break;
            //                case 5: NumToken = 4; break;
            //                case 6: NumToken = 3; break;
            //                default:
            //                    NumToken = 9;// el error
            //                    Error = true;
            //            };

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
