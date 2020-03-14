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
        static void Main(string[] args)
        {
            Console.WriteLine("Arrastrar El Archivo de prueba hacia la consola");
            var linea_actual = 0;
            var path = Console.ReadLine();
            var reader = new StreamReader(path);
            var linea = reader.ReadLine(); linea_actual++;
            linea = linea.Replace(" ","");
            var SETS = new Dictionary<string, List<char>>();
            var TOKENS = new Dictionary<string,string>();
            var Error = new Dictionary<string,string>();
            var Actions = new Dictionary<string,string>();
            var MegaExpresion = string.Empty;
            var comillas = '"';
            //C:\Users\roche\Desktop\Lenguajes\ProyectoLenguajes\Archivo Prueba.txt
            //leer Sets
          
            while (linea != "TOKENS")
            {
               linea = reader.ReadLine(); linea_actual++;
                if (linea == "TOKENS")
                {
                    break;
                }
                var SetId = linea.Substring(0,linea.IndexOf('=')).Replace("\t","");
               var TokensArray = linea.Remove(0,linea.IndexOf('=')+1).Trim().Split('+');
                //validar chars
                 foreach (var item in TokensArray)
                 {
                    var lol = item.Replace("..", "").Replace("...","").ToCharArray();
                    if (!SETS.ContainsKey(SetId))
                    {
                        SetId = SetId.Replace("  "," ");
                        SETS.Add(SetId, new List<char>());
                    }
                    if (lol.Length==1)
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
            linea = reader.ReadLine(); linea_actual++;
            while (linea != "ACTIONS")
                {
                
                if (linea.Contains("'''"))
                {

                    linea = linea.Replace("\t","").Replace("'''", "'").Replace("  ", " ").Replace(("'"+'"'+"'").ToString(),comillas.ToString());
                }
                else
                {
                     linea =linea.Replace("\t","").Replace("  ", " ");

                }

                if (linea == "" || linea == " " || linea == "  ")
                {

                }
                else
                {

                    var TokenId = linea.Substring(0,linea.IndexOf('=')).Replace("  ", " ");
                    var Expresion = linea.Remove(0,linea.IndexOf('=')+1).Trim();
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
            var Raiz = new NodoExpresion();
            try
            {
            Raiz =Arbol_De_Expresiones.Instance.Generar_Arbol(MegaExpresion,SETS.Keys.ToArray());

            }
            catch (Exception)
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error => Token no encontrado");

                Console.ReadLine();
                Environment.Exit(0);
                throw;
            }
            //Arbol_De_Expresiones.Instance.Inorder(Arbol_De_Expresiones.Instance.Diccionario_Nodos);
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
                Console.WriteLine($"Hoja No.{n}: {item.Nombre}");
                n++;
            }

            Console.ReadLine();

            void Inorder(NodoExpresion Root)
            {
                if (Root != null)
                {
                    Inorder(Root.C1);
                    if (Root.Nombre=="|"|| Root.Nombre == "." || Root.Nombre == "*" || Root.Nombre == "?" || Root.Nombre == "+")
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
