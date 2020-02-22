using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Lenguajes
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Console.ReadLine();
            var reader = new StreamReader(path);
            var linea = reader.ReadLine();
            linea = linea.Replace(" ","");
            var SETS = new Dictionary<string, List<char>>();
            var TOKENS = new Dictionary<string,string>();
            var Error = new Dictionary<string,string>();
            var Actions = new Dictionary<string,string>();
            var MegaExpresion = string.Empty;
            var comillas = '"';

            //leer Sets
            while (linea != "TOKENS")
            {
               linea = reader.ReadLine();
                if (linea == "TOKENS")
                {
                    break;
                }
                var SetId = linea.Substring(0,linea.IndexOf('=')).Replace("\t","");
               var TokensArray = linea.Remove(0,linea.IndexOf('=')+1).Trim().Split('+');
                //validar chars
                 foreach (var item in TokensArray)
                 {
                    var lol = item.Replace("'", "").Replace("..", "").Replace("...","").ToCharArray();
                    if (!SETS.ContainsKey(SetId))
                    {
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
            linea = reader.ReadLine();
                while (linea != "ACTIONS")
            {
                
                if (linea.Contains("'''"))
                {

                linea = linea.Replace("\t","").Replace("'''", "'").Replace("  ", " ").Replace(("'"+'"'+"'").ToString(),comillas.ToString());
                }
                else
                {
                linea =linea.Replace("\t","").Replace("'", "").Replace("  ", " ");

                }
                var TokenId = linea.Substring(0,linea.IndexOf('='));
                var Expresion = linea.Remove(0,linea.IndexOf('=')+1).Trim();
                TOKENS.Add(TokenId, Expresion);
                MegaExpresion += $"({Expresion})|";
                linea = reader.ReadLine();
            }


            #region Comentado
            //while (linea != null)
            //{

            //    if (linea == "SETS ")
            //    {
            //        linea = reader.ReadLine().Replace("\t", "").Replace(" ", "");
            //        while (linea != "TOKENS" &&  linea != "ACTIONS")
            //        {//partimos y quitmos espacios
            //            var arreglo = linea.Split('=');
            //            SETS.Add(arreglo[0], new List<string>());
            //            var items = arreglo[1].Replace("'","");
            //            foreach (var item in items.Split('+'))
            //            {
            //                SETS[arreglo[0]].Add(item);
            //            }
            //            linea = reader.ReadLine().Replace("\t", "").Replace(" ", "");
            //        }

            //    }
            //    if (linea == "TOKENS")
            //    {
            //        linea = reader.ReadLine().Replace("\t", "");
            //        while (linea != "SETS" && linea != "ACTIONS")
            //        {//partimos y quitmos espacios
            //            var SplitIndex = linea.IndexOf('=');
            //            var id = linea.Substring(0, SplitIndex - 1);
            //            linea = linea.Remove(0, SplitIndex+2);
            //            var Token = linea.Replace("'","");
            //            TOKENS.Add(Token, id);
            //            linea = reader.ReadLine().Replace("\t", "");
            //        }
            //    }

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

            //Console.WriteLine("Tokens");
            //Console.WriteLine("");
            //foreach (var item in TOKENS)
            //{
            //    Console.WriteLine($"{item.Key} => {item.Value}");

            //}

            // Console.WriteLine("");

            //Console.WriteLine("Actions");
            //Console.WriteLine("");
            //foreach (var item in Actions        )
            //{
            //    Console.WriteLine($"{item.Key} => {item.Value}");

            //}




            //Console.WriteLine("");

            //Console.WriteLine("Error");
            //Console.WriteLine("");
            //foreach (var item in Error)
            //{
            //    Console.WriteLine($"{item.Key} => {item.Value}");

            //}

            #endregion


            Console.ReadLine();
        }
    }
}
