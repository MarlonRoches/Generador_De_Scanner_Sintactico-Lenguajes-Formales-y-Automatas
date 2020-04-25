using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_Lenguajes;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Proyecto_Lenguajes
{
    class Program
    {
        static void Main(string[] args)
        {
            var SETS = new Dictionary<string, List<char>>();
            var TOKENS = new Dictionary<string, string>();
            var ERRORS = new Dictionary<string, string>();
            var ACTIONS = new Dictionary<string, string>();
            #region Fase 1
            var barrasdobles = (char)92;
            Console.WriteLine("Arrastrar El Archivo de prueba hacia la consola");
            var linea_actual = 0;
            var path = "C:\\Users\\roche\\Desktop\\Lenguajes\\archivo 19.txt";
            //path = "C:\\Users\\roche\\Desktop\\Prueba De Automata\\ProyectoLenguajes\\Archivo Prueba.txt";

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
                    if (lal.Contains('('))
                    {
                    SETS.Add(SetId, new List<char>());
                        var aux = lal.Replace(')',',').Replace("CHR(", "").Split(',');
                        var listaaux = new List<int>();
                        for (int i = 0; i < aux.Count(); i++)
                        {
                            if (aux[i]!="")
                            {
                                listaaux.Add(int.Parse(aux[i].Trim()));
                            }
                        }
                        listaaux.Sort((x, y) => x.CompareTo(y));
                        var desde = listaaux[0];
                        var hasta = listaaux[listaaux.Count - 1];

                        for (int i = desde; i < hasta; i++)
                        {
                            SETS[SetId].Add((char)((byte)i));
                        }
                    }
                    else
                    {
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
                    ERRORS.Add(Array[1], Array[0]);
                }
                linea = reader.ReadLine();
            }
            ///mostrar SETS
            foreach (var set in SETS)
            {
                Console.WriteLine($"El SET '{set.Key}' contiene los siguientes caracteres:");
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
                    aux = item.Value.Replace("(", "").Replace(")", "").Replace("'", "").Replace("  ", " ").Trim().Split(' ');
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

            var File = new FileStream("D:\\MetaData\\SETS.txt", FileMode.Create);
            var writer = new StreamWriter(File);
            foreach (var item in SETS)
            {
                var aux = "";
                foreach (var item2 in item.Value)
                {
                    aux += $"{item2}◙";
                }
                writer.WriteLine($"{item.Key}♪{aux}");
            }
            writer.Close();
            File.Close();
            File = new FileStream("D:\\MetaData\\TOKENS.txt", FileMode.Create);
            writer = new StreamWriter(File);
            foreach (var item in TOKENS)
            {
                writer.WriteLine($"{item.Key}♪{item.Value}");
            }
            writer.Close();
            File.Close(); 
            File = new FileStream("D:\\MetaData\\ERRORS.txt", FileMode.Create);
            writer = new StreamWriter(File);
            foreach (var item in ERRORS)
            {
                writer.WriteLine($"{item.Key}♪{item.Value}");
            }
            writer.Close();
            File.Close();
            File = new FileStream("D:\\MetaData\\ACTIONS.txt", FileMode.Create);
            writer = new StreamWriter(File);
            foreach (var item in ACTIONS)
            {
                writer.WriteLine($"{item.Key}♪{item.Value}");
            }
            writer.Close();
            File.Close();
            File = new FileStream("D:\\MetaData\\Transiciones.txt", FileMode.Create);
            writer = new StreamWriter(File);
            foreach (var item in Transiciones)
            {
                writer.WriteLine($"{item.Key}♪{item.Value}");
            }
            writer.Close();
            File.Close();
            File = new FileStream("D:\\MetaData\\Simples.txt", FileMode.Create);
            writer = new StreamWriter(File);
            foreach (var item in Simples)
            {
                writer.WriteLine($"{item.Key}♪{item.Value}");
            }
            writer.Close();
            File.Close();

            var cs = "";
            cs += "        using System;\n";
            cs += "        using System.Collections.Generic;\n";
            cs += "        using System.IO;\n";
            cs += "        using System.Linq;\n";
            cs += "namespace ConsoleApp1\n";
            cs += "{\n";
            cs += "    class Program\n";
            cs += "    {\n";
            cs += "        static void Main(string[] args)\n";
            cs += "        {\n";
            cs += "            var SETS = new Dictionary<string, List<char>>();\n";
            cs += "            var TOKENS = new Dictionary<string, string>();\n";
            cs += "            var ERRORS = new Dictionary<string, string>();\n";
            cs += "            var ACTIONS = new Dictionary<string, string>();\n";
            cs += "            var Transiciones = new Dictionary<string, string>();\n";
            cs += "            var Simples = new Dictionary<string, string>();\n";
            cs += "            ObtenerMetaData();\n";
            cs += $"            Console.WriteLine({comillas}Ingrese la ruta el archivo sin comillas{comillas});\n";
            cs += $"            Console.WriteLine({comillas}{comillas});\n";
            cs += "            var fil = Console.ReadLine();\n";
            cs += "            var Reader = new StreamReader(fil);\n";
            cs += $"            var linea = Reader.ReadLine().Replace({comillas}  {comillas}, {comillas} {comillas}).Replace({comillas} {comillas}, {comillas}Ø{comillas}).Trim();\n";
            cs += $"            var sets ={comillas}{comillas};\n";
            cs += $"            var palabraActual = {comillas}{comillas};\n";
            cs += "            var Qn = 0;\n";
            cs += "            bool encontrado = false;\n";
            cs += "            bool error = false;\n";
            cs += "            Console.ForegroundColor = ConsoleColor.Green;\n";
            cs += "            while (linea != null)\n";
            cs += "            {\n";
            cs += $"                var entradaejemplo = linea.Replace({comillas}  {comillas}, {comillas} {comillas}).Replace({comillas} {comillas}, {comillas}Ø{comillas}).Trim();\n";
            cs += "                foreach (var item in entradaejemplo)\n";
            cs += "                {\n";
            cs += "                    //error encontrado, navegar hasta que encuentr todo el token y lo devalide\n";
            cs += "                    if (error)\n";
            cs += "                    {\n";
            cs += "                        if (item == 'Ø')\n";
            cs += "                        {\n";
            cs += "                            Console.ForegroundColor = ConsoleColor.DarkRed;\n";
            cs += $"                            Console.WriteLine(${comillas}" + "{ERRORS.ToArray()[0].Value} {ERRORS.ToArray()[0].Key}: ({palabraActual})" + $"{comillas});\n";
            cs += "                            Console.ForegroundColor = ConsoleColor.DarkGreen;\n";
            cs += $"                           palabraActual = {comillas}{comillas};\n";
            cs += $"                            sets = {comillas}{comillas};\n";
            cs += "                            Qn = 0;\n";
            cs += "                            error = false;\n";
            cs += "                        }\n";
            cs += "                        else\n";
            cs += "                        {\n";
            cs += "\n";
            cs += "                            palabraActual += item;\n";
            cs += "                        }\n";
            cs += "                    }\n";
            cs += "                    else\n";
            cs += "                        if (item != 'Ø')\n";
            cs += "                    {\n";
            cs += "                        var actual = A_Que_SET_Pertenece(item);\n";
            cs += $"                        if (Transiciones.ContainsKey(${comillas}Q" + "{Qn}^{actual}" + comillas + ") && actual !=" + comillas + comillas + ")\n";
            cs += "                        {\n";
            cs += $"                            var TokenPerteneiente = ${comillas}Q" + "{Qn}^{actual}" + comillas + ";\n";
            cs += "                            var lol = Transiciones[TokenPerteneiente];\n";
            cs += $"                            Qn = int.Parse(lol.Replace({comillas}Q{comillas}, {comillas}{comillas}));\n";
            cs += $"                            sets += actual + {comillas},{comillas};\n";
            cs += "                            palabraActual += item;\n";
            cs += "                        }\n";
            cs += "                        else\n";
            cs += "                        {\n";
            cs += "                            //fuera de los sets\n";
            cs += $"                            if (Transiciones.ContainsKey(${comillas}Q" + "{Qn}^{item}" + comillas + "))\n";
            cs += "                            {\n";
            cs += $"                                var TokenPerteneiente = ${comillas}Q" + "{Qn}^{item}" + comillas + ";\n";
            cs += "                                var lol = Transiciones[TokenPerteneiente];\n";
            cs += $"                                Qn = int.Parse(lol.Replace({comillas}Q{comillas}, {comillas}{comillas}));\n";
            cs += $"                                sets += item + {comillas},{comillas};\n";
            cs += "                                palabraActual += item;\n";
            cs += "                                if (Simples.ContainsKey(palabraActual))\n";
            cs += "                                {\n";
            cs += $"                                    Console.WriteLine(${comillas}" + "{palabraActual} = {Simples[palabraActual]}" + comillas + ");\n";
            cs += "                                    encontrado = true;\n";
            cs += "                                }\n";
            cs += "                                else\n";
            cs += "                                {\n";
            cs += $"                                    var r = {comillas}{comillas};\n";
            cs += "                                    foreach (var tok in TOKENS)\n";
            cs += "                                    {\n";
            cs += $"                                        if (tok.Value.Contains({comillas}CHARSET{comillas}))\n";
            cs += "                                        {\n";
            cs += "                                            r = tok.Key;\n";
            cs += "                                            break;\n";
            cs += "                                        }\n";
            cs += "                                    }\n";
            cs += "                                    Console.WriteLine(palabraActual + " + '"' + " = " + '"' + " + r);\n";
            cs += $"                                    palabraActual = {comillas}{comillas};\n";
            cs += $"                                    sets = {comillas}{comillas};\n";
            cs += "                                    Qn = 0;\n";
            cs += "                                }\n";
            cs += "                            }\n";
            cs += "                            else\n";
            cs += "                            {\n";
            cs += "                                palabraActual += item;\n";
            cs += $"                                sets = {comillas}{comillas};\n";
            cs += "                                Qn = 0;\n";
            cs += "                                error = true;\n";
            cs += "                            }\n";
            cs += "                        }\n";
            cs += "                    }\n";
            cs += "                    else\n";
            cs += "                    {\n";
            cs += "                        if (!encontrado)\n";
            cs += "                        {\n";
            cs += "                            if (ACTIONS.ContainsKey(palabraActual))\n";
            cs += "                            {\n";
            cs += "                                var xd = TokenPerteneciente(sets.Split(','));\n";
            cs += $"                                Console.WriteLine(${comillas}" + "{palabraActual} = TOKEN {ACTIONS[palabraActual]}" + comillas + ");\n";
            cs += $"                                palabraActual = {comillas}{comillas};\n";
            cs += $"                                sets = {comillas}{comillas};\n";
            cs += "                                Qn = 0;\n";
            cs += "                            }\n";
            cs += "                            else if (Simples.ContainsKey(palabraActual))\n";
            cs += "                            {\n";
            cs += $"                                Console.WriteLine(${comillas}" + "{palabraActual} = {Simples[palabraActual]}" + comillas + ");\n";
            cs += $"                                palabraActual = {comillas}{comillas};\n";
            cs += $"                                sets = {comillas}{comillas};\n";
            cs += "                                Qn = 0;\n";
            cs += "                            }\n";
            cs += "                            else\n";
            cs += "                            {\n";
            cs += "                                //buscar token correspondiente\n";
            cs += "                                var xd = TokenPerteneciente(sets.Split(','));\n";
            cs += "                                Console.WriteLine(palabraActual + " + comillas + " = " + comillas + " + xd);\n";
            cs += $"                                palabraActual ={comillas}{comillas};\n";
            cs += $"                                sets = {comillas}{comillas};\n";
            cs += $"                                Qn = 0;\n";
            cs += "                            }\n";
            cs += "                        }\n";
            cs += "                        else\n";
            cs += "                        {\n";
            cs += "                            encontrado = false;\n";
            cs += $"                            palabraActual = {comillas}{comillas};\n";
            cs += $"                            sets = {comillas}{comillas};\n";
            cs += "                            Qn = 0;\n";
            cs += "                        }\n";
            cs += "                    }\n";
            cs += "                }\n";
            cs += "                var uno = TokenPerteneciente(sets.Split(','));\n";
            cs += "                Console.WriteLine(palabraActual + " + comillas + " = " + comillas + " + uno);\n";
            cs += $"                palabraActual = {comillas}{comillas};\n";
            cs += $"                sets = {comillas}{comillas};\n";
            cs += "                Qn = 0;\n";
            cs += "                linea = Reader.ReadLine();\n";
            cs += $"                Console.WriteLine({comillas}{comillas});\n";
            cs += "            }\n";
            cs += "            Console.WriteLine();\n";
            cs += $"            Console.WriteLine({comillas}Cualquier tecla para terminar...{comillas});\n";
            cs += "            Console.ReadKey();\n";

            cs += "            string[] SinRepetidos(string[] Arreglo)\n";
            cs += "            {\n";
            cs += "                var dicaux = new Dictionary<string, bool>();\n";
            cs += "                for (int i = 0; i < Arreglo.Length; i++)\n";
            cs += "                {\n";
            cs += "                    if (!dicaux.ContainsKey(Arreglo[i]))\n";
            cs += "                    {\n";
            cs += "                        dicaux.Add(Arreglo[i], true);\n";
            cs += "                    }\n";
            cs += "                }\n";
            cs += "\n";
            cs += "\n";
            cs += "                return dicaux.Keys.ToArray();\n";
            cs += "            }\n";
            cs += "            string TokenPerteneciente(string[] SetsEncontrados)\n";
            cs += "            {\n";
            cs += "                var sin = SinRepetidos(SetsEncontrados);\n";
            cs += "                var anterior = true;\n";
            cs += $"                var salida = {comillas}{comillas};\n";
            cs += "                foreach (var item in TOKENS)\n";
            cs += "                {\n";
            cs += "                    foreach (var token in sin)\n";
            cs += "                    {\n";
            cs += $"                        if (token != {comillas}{comillas})\n";
            cs += "                        {\n";
            cs += "\n";
            cs += "                            anterior = anterior && item.Value.Contains(token);\n";
            cs += "                        }\n";
            cs += "                    }\n";
            cs += "                    if (anterior)\n";
            cs += "                    {\n";
            cs += "                        salida = item.Key;\n";
            cs += "                        break;\n";
            cs += "                    }\n";
            cs += "                    else\n";
            cs += "                    {\n";
            cs += "                        anterior = true;\n";
            cs += "                    }\n";
            cs += "                }\n";
            cs += "\n";
            cs += "                return salida;\n";
            cs += "            }\n";

            cs += "            string A_Que_SET_Pertenece(char actual)\n";
            cs += "            {\n";
            cs += $"                var salida = {comillas}{comillas};\n";
            cs += "                foreach (var set in SETS)\n";
            cs += "                {\n";
            cs += "                    var lista = set.Value;\n";
            cs += "                    foreach (var chara in lista)\n";
            cs += "                    {\n";
            cs += "                        if (chara == actual)\n";
            cs += "                        {\n";
            cs += "                            salida = set.Key;\n";
            cs += "                            break;\n";
            cs += "                        }\n";
            cs += "                    }\n";
            cs += $"                    if (salida != {comillas}{comillas})\n";
            cs += "                    {\n";
            cs += "                        break;\n";
            cs += "                    }\n";
            cs += "                }\n";
            cs += "                return salida;\n";
            cs += "            }\n";

            cs += "            void ObtenerMetaData()\n";
            cs += "            {\n";
            cs += $"                var File = new FileStream({comillas}D:" + barrasdobles + barrasdobles + "MetaData" + barrasdobles + barrasdobles + "SETS.txt"+comillas+", FileMode.Open);\n";
            cs += "                var reader = new StreamReader(File);\n";
            cs += "                var lineal = reader.ReadLine();\n";
            cs += "                while (lineal != null)\n";
            cs += "                {\n";
            cs += "                    var ax = lineal.Split('♪');\n";
            cs += "                    var lista = new List<char>();\n";
            cs += "                    var ax2 = ax[1].Split('◙');\n";
            cs += "                    foreach (var item in ax2)\n";
            cs += "                    {\n";
            cs += $"                        if (item != {comillas}{comillas})\n";
            cs += "                        {\n";
            cs += "                            lista.Add(item.ToCharArray()[0]);\n";
            cs += "                        }\n";
            cs += "                    }\n";
            cs += "                    SETS.Add(ax[0].Trim(), lista);\n";
            cs += "                    lineal = reader.ReadLine();\n";
            cs += "                }\n";
            cs += "                reader.Close();\n";
            cs += "                File.Close();\n";
            cs += "\n";
            cs += "\n";
            cs += $"   ; File = new FileStream({comillas}D:" + barrasdobles + barrasdobles + "MetaData" + barrasdobles + barrasdobles + "TOKENS.txt" + comillas + ", FileMode.Open);\n";
            cs += "                reader = new StreamReader(File);\n";
            cs += "                lineal = reader.ReadLine();\n";
            cs += "                while (lineal != null)\n";
            cs += "                {\n";
            cs += "\n";
            cs += "                    TOKENS.Add(lineal.Split('♪')[0], lineal.Split('♪')[1].Trim());\n";
            cs += "                    lineal = reader.ReadLine();\n";
            cs += "\n";
            cs += "                }\n";
            cs += "                reader.Close();\n";
            cs += "                File.Close();\n";
            cs += "\n";
            cs += $"                File = new FileStream({comillas}D:" + barrasdobles + barrasdobles + "MetaData" + barrasdobles + barrasdobles + "ERRORS.txt" + comillas + ", FileMode.Open);\n";
            cs += "                reader = new StreamReader(File);\n";
            cs += "                lineal = reader.ReadLine();\n";
            cs += "                while (lineal != null)\n";
            cs += "                {\n";
            cs += "\n";
            cs += "                    ERRORS.Add(lineal.Split('♪')[0], lineal.Split('♪')[1].Trim()); lineal = reader.ReadLine();\n";
            cs += "\n";
            cs += "                }\n";
            cs += "                reader.Close();\n";
            cs += "                File.Close();\n";
            cs += "\n";
            cs += "\n";
            cs += $"                File = new FileStream({comillas}D:" + barrasdobles + barrasdobles + "MetaData" + barrasdobles + barrasdobles + "ACTIONS.txt" + comillas + ", FileMode.Open);\n";
            cs += "                reader = new StreamReader(File);\n";
            cs += "                lineal = reader.ReadLine();\n";
            cs += "                while (lineal != null)\n";
            cs += "                {\n";
            cs += "\n";
            cs += "                    ACTIONS.Add(lineal.Split('♪')[0], lineal.Split('♪')[1].Trim()); lineal = reader.ReadLine();\n";

            cs += "\n";
            cs += "                }\n";
            cs += "                reader.Close();\n";
            cs += "                File.Close();\n";
            cs += "\n";
            cs += $"                File = new FileStream({comillas}D:" + barrasdobles + barrasdobles + "MetaData" + barrasdobles + barrasdobles + "Transiciones.txt" + comillas + ", FileMode.Open);\n";
            cs += "                reader = new StreamReader(File);\n";
            cs += "                lineal = reader.ReadLine();\n";
            cs += "                while (lineal != null)\n";
            cs += "                {\n";
            cs += "\n";
            cs += "                    Transiciones.Add(lineal.Split('♪')[0], lineal.Split('♪')[1].Trim()); lineal = reader.ReadLine();\n";
            cs += "\n";
            cs += "                }\n";
            cs += "                reader.Close();\n";
            cs += "                File.Close();\n";
            cs += "\n";
            cs += "\n";
            cs += "\n";
            cs += "\n";
            cs += $"                File = new FileStream({comillas}D:" + barrasdobles + barrasdobles + "MetaData" + barrasdobles + barrasdobles + "Simples.txt" + comillas + ", FileMode.Open);\n";
            cs += "                reader = new StreamReader(File);\n";
            cs += "                lineal = reader.ReadLine();\n";
            cs += "                while (lineal != null)\n";
            cs += "                {\n";
            cs += "\n";
            cs += "                    Simples.Add(lineal.Split('♪')[0], lineal.Split('♪')[1].Trim()); lineal = reader.ReadLine();\n";
            cs += "\n";
            cs += "                }\n";
            cs += "                reader.Close();\n";
            cs += "                File.Close();\n";
            cs += "\n";
            cs += "            }\n";
            cs += "        }\n";
            cs += "    }\n";
            cs += "}\n";


            File = new FileStream("Scanner.cs", FileMode.Create);
            writer = new StreamWriter(File);
            writer.WriteLine(cs);
            writer.Close();
            File.Close();

            var process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.CreateNoWindow= true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.StandardInput.WriteLine("csc Scanner.cs");
            process.StandardInput.Flush();
            process.StandardInput.Close();
            process.WaitForExit();
            Console.WriteLine(process.StandardOutput.ReadToEnd());

            Process.Start("Scanner.exe");


            Console.ReadKey();

            void PruebaScanner()
            {
                var json = JsonConvert.SerializeObject(Compuestos);
                var json2 = JsonConvert.SerializeObject(Simples);
                var arreglo = "156 x a := b c = d const a".Replace("  ", " ").Split(' ');
                var entradaejemplo = $"{comillas}Ü{comillas} PROGRAM program VAR var v4r 459 a ?<= <= 5".Replace("  ", " ").Replace(" ", "Ø").Trim();
                Console.WriteLine($"Texto de entrada: {entradaejemplo}");

                var sets = "";
                var palabraActual = "";
                var Qn = 0;
                bool encontrado = false;
                bool error = false;
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine("Identificador De Tokens");
                Console.ForegroundColor = ConsoleColor.DarkGreen;

                foreach (var item in entradaejemplo)
                {
                    //error encontrado, navegar hasta que encuentr todo el token y lo devalide
                    if (error)
                    {
                        if (item == 'Ø')
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine($"{ERRORS.ToArray()[0].Value} {ERRORS.ToArray()[0].Key}: ({palabraActual})");
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            palabraActual = "";
                            sets = "";
                            Qn = 0;
                            error = false;
                        }
                        else
                        {

                            palabraActual += item;
                        }
                    }
                    else
                        if (item != 'Ø')
                    {
                        var actual = A_Que_SET_Pertenece(item);
                        if (Transiciones.ContainsKey($"Q{Qn}^{actual}") && actual != "")
                        {
                            var TokenPerteneiente = $"Q{Qn}^{actual}";
                            var lol = Transiciones[TokenPerteneiente];
                            Qn = int.Parse(lol.Replace("Q", ""));
                            sets += actual + ",";
                            palabraActual += item;
                        }
                        else
                        {
                            //fuera de los sets
                            if (Transiciones.ContainsKey($"Q{Qn}^{item}"))
                            {
                                var TokenPerteneiente = $"Q{Qn}^{item}";
                                var lol = Transiciones[TokenPerteneiente];
                                Qn = int.Parse(lol.Replace("Q", ""));
                                sets += item + ",";
                                palabraActual += item;
                                if (Simples.ContainsKey(palabraActual))
                                {
                                    Console.WriteLine($"{palabraActual} = {Simples[palabraActual]}");
                                    encontrado = true;
                                }
                                else
                                {
                                    var r = "";
                                    foreach (var tok in TOKENS)
                                    {
                                        if (tok.Value.Contains("CHARSET"))
                                        {
                                            r = tok.Key;
                                            break;
                                        }
                                    }
                                    Console.WriteLine(palabraActual + " = " + r);
                                    palabraActual = "";
                                    sets = "";
                                    Qn = 0;
                                }
                            }
                            else
                            {
                                palabraActual += item;
                                sets = "";
                                Qn = 0;
                                error = true;
                            }
                        }
                    }
                    else
                    {
                        if (!encontrado)
                        {
                            if (ACTIONS.ContainsKey(palabraActual))
                            {
                                var xd = TokenPerteneciente(sets.Split(','));
                                Console.WriteLine($"{palabraActual} = TOKEN {ACTIONS[palabraActual]}");
                                palabraActual = "";
                                sets = "";
                                Qn = 0;
                            }
                            else if (Simples.ContainsKey(palabraActual))
                            {
                                Console.WriteLine($"{palabraActual} = {Simples[palabraActual]}");
                                palabraActual = "";
                                sets = "";
                                Qn = 0;
                            }
                            else
                            {
                                //buscar token correspondiente
                                var xd = TokenPerteneciente(sets.Split(','));
                                Console.WriteLine(palabraActual + "=" + xd);
                                palabraActual = "";
                                sets = "";
                                Qn = 0;
                            }
                        }
                        else
                        {
                            encontrado = false;
                            palabraActual = "";
                            sets = "";
                            Qn = 0;
                        }
                    }
                    //}
                    //else
                    //{
                    //    Console.WriteLine(palabraActual + " no pertenece al lenguaje actual");
                    //    palabraActual = "";
                    //    sets = "";
                    //    Qn = 0;
                    //}
                }
                var uno = TokenPerteneciente(sets.Split(','));
                Console.WriteLine(palabraActual + "=" + uno);
                palabraActual = "";
                sets = "";
                Qn = 0;
                //agregar las trancsiciones
                Console.ReadLine();
            }
            string TokenPerteneciente(string[] SetsEncontrados)
            {
                var sin = SinRepetidos(SetsEncontrados);
                var anterior = true;
                var salida = "";
                foreach (var item in TOKENS)
                {
                    foreach (var token in sin)
                    {
                        if (token != "")
                        {

                            anterior = anterior&& item.Value.Contains(token);   
                        }
                    }
                    if (anterior)
                    {
                        salida = item.Key;
                        break;
                    }
                    else
                    {
                        anterior = true;
                    }
                }

                return salida;
            }
            string A_Que_SET_Pertenece(char actual)
            {
                var salida = "";
                foreach (var set in SETS)
                {
                    var lista = set.Value;
                    foreach (var chara in lista)
                    {
                        if (chara== actual)
                        {
                            salida = set.Key;
                            break;
                        }
                    }
                    if (salida != "")
                    {
                        break;
                    }
                }
                return salida;
            }
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
            #region CodigoQuemado
           // var ifs = "";
           // ifs += "foreach (var item in arreglo)\n";
           // ifs += "{\n";
           // //busqueda de simpl, si no, busca en compuestos
           // ifs += "if (Simples.ContainsKey(item))\n";
           //    ifs += " {\n";
           // ifs += "resultado = Simples[item];\n";
           //     ifs += "  }\n";
           // ifs += " else\n";
           // ifs += " {   //compuesto\n";
           // foreach (var item in Compuestos)
           // {
           //     ifs += $"if (PerteneceAlLenguajUnico({comillas}{item.Key}{comillas}, item))\n";
           //     ifs += "{\n";
           //     //RETORNAR
           //     ifs += "} else ";
           // }
           //// var lol3 = PerteneceAlLenguajUnico("TOKEN 4", item);
           //    //     var lol = PerteneceAlLenguajUnico("TOKEN 1", item);

           //     ifs += " {\n";
           //     ifs += " }\n";

           // ifs += "}\n";
           // ifs += "else\n";

           // //probando algoritmos
           //  arreglo = "156 x a := b c = d const a".Replace("  "," ").Split(' ');
           // foreach (var item in arreglo)
           // {
           //     //busqueda de simpl, si no, busca en compuestos
           //     if (Simples.ContainsKey(item))
           //     {
           //         var resultado = Simples[item];
           //     }
           //     else
           //     {   //compuesto
           //         var lol3 = PerteneceAlLenguajUnico("TOKEN 4", item);
           //         var lol = PerteneceAlLenguajUnico("TOKEN 1", item);

           //     }
               
           // }

           // //armando el texto
           // ifs += "break;\n";

           // var codigoDeSalida = "";
           // codigoDeSalida+="using System;\n";
           // codigoDeSalida+= "using System.Collections.Generic;\n";
           // codigoDeSalida+="using System.IO;\n";
           // codigoDeSalida+="using System.Linq;\n";
           // codigoDeSalida+="using System.Text;\n";
           // codigoDeSalida+= "using System.Threading.Tasks;\n";
           // codigoDeSalida+= "using Proyecto_Lenguajes;\n";
           // codigoDeSalida+= "using Newtonsoft.Json;\n";
           // codigoDeSalida+= "namespace Proyecto_Lenguajes\n";
           // codigoDeSalida+= "    {\n";
           // codigoDeSalida+= "        class Program\n";
           // codigoDeSalida+= "        {\n";
           // codigoDeSalida+= "            static void Main(string[] args)\n";
           // codigoDeSalida+= "            {\n";
           // codigoDeSalida+=                    "var Archivo = Console.ReadLine().Trim();\n";
           // codigoDeSalida+=                    "var File = new StreamReader(Archivo);\n";
           // codigoDeSalida+=                    "var LineaActual = File.ReadLine();\n";
           // codigoDeSalida+=                    $"var RESULTADO = {comillas}{comillas};\n";
           //codigoDeSalida+= " var Compuestos = new Dictionary<string, string>();\n";
           // codigoDeSalida += " var Simples = new Dictionary<string, string>();\n";
           // codigoDeSalida += $" var Alfabeto = {comillas}{comillas};\n";
           // codigoDeSalida +=                    "while (LineaActual != null)\n";
           // codigoDeSalida+=                    "{\n";
           // codigoDeSalida += " var arreglo = LineaActual.Split(' ');\n";
           // codigoDeSalida += " foreach (var item in arreglo)\n";
           // codigoDeSalida += " {\n";
           // codigoDeSalida += "     switch (item)\n";
           // codigoDeSalida += "     {\n";
           // //GENERANDO LOS cases de los actions
           // foreach (var item in ACTIONS)
           // {
           //     codigoDeSalida += $"case {comillas}{item.Key}{comillas}:\n";
           //     codigoDeSalida += $"RESULTADO = {comillas}{item.Value}{comillas};\n";
           //    // codigoDeSalida += $"return;\n";
           //     codigoDeSalida += "break;\n";
           // }
           // codigoDeSalida += "default:\n";
           // // ARMAR IFS DE LOS RESTANTES
           // codigoDeSalida += "foreach (var item2 in arreglo)\n";
           // codigoDeSalida += "{\n";
           // //busqueda de simpl, si no, busca en compuestos
           // codigoDeSalida += "if (Simples.ContainsKey(item2))\n";
           // codigoDeSalida += " {\n";
           // codigoDeSalida += $"Console.WriteLine(item2 + {comillas}={comillas} +Simples[item2]);\n";
           // codigoDeSalida += "  }\n";
           // codigoDeSalida += " else\n";
           // codigoDeSalida += " {   //compuesto\n";
           // foreach (var item in Compuestos)
           // {
           //     codigoDeSalida += $"if (PerteneceAlLenguajUnico({comillas}{item.Key}{comillas}, item2))\n";
           //     codigoDeSalida += "{\n";
           //     //RETORNAR
           // codigoDeSalida += $"Console.WriteLine(item2 + {comillas}={comillas} + {comillas}{item.Key}{comillas});\n";
           //     codigoDeSalida += "} else ";
           // }
           // // var lol3 = PerteneceAlLenguajUnico("TOKEN 4", item);
           // //     var lol = PerteneceAlLenguajUnico("TOKEN 1", item);
           // codigoDeSalida += " {\n";
           // codigoDeSalida += " }\n";
           // codigoDeSalida += "}\n";
           // codigoDeSalida += "     }\n";
           // codigoDeSalida += "break;\n";
           // codigoDeSalida += " }\n";
           // codigoDeSalida+= "}\n";



           // //metodos
           // codigoDeSalida += "bool PerteneceAlLenguajUnico(string Token, string entrada)\n";
           // codigoDeSalida += "{\n";
           // codigoDeSalida += "    var anterior = false;\n";
           // codigoDeSalida += "    var actual = false;\n";


           // codigoDeSalida += "    var lenguajes = Compuestos[Token].Split('↓');\n";
           // codigoDeSalida += "    for (int i = 0; i < lenguajes.Count(); i++)\n";
           // codigoDeSalida += "    {\n";

           // codigoDeSalida += $"        if (lenguajes[i] != {comillas}{comillas})\n";
           // codigoDeSalida += "        {\n";
           // codigoDeSalida += "            for (int j = 0; j < entrada.Length; j++)\n";
           // codigoDeSalida += "            {\n";


           // codigoDeSalida += $"                if (lenguajes[i].Contains(entrada[j].ToString().Replace({comillas},{comillas}, {comillas}{comillas})))\n";
           // codigoDeSalida += "                {\n";
           // codigoDeSalida += "                    actual = true;\n";
           // codigoDeSalida += "                }\n";
           // codigoDeSalida += "                else\n";
           // codigoDeSalida += "                {\n";
           // codigoDeSalida += "                    actual = false;\n";

           // codigoDeSalida += "                }\n";
           // codigoDeSalida += "                if (j == 0)\n";
           // codigoDeSalida += "                {\n";
           // codigoDeSalida += "                    anterior = actual || anterior;\n";
           // codigoDeSalida += "                }\n";
           // codigoDeSalida += "                else if (j == entrada.Length - 1)\n";
           // codigoDeSalida += "                {\n";
           // codigoDeSalida += "                    anterior = actual || anterior;\n";
           // codigoDeSalida += "                }\n";
           // codigoDeSalida += "                else\n";
           // codigoDeSalida += "                {\n";
           // codigoDeSalida += "                    anterior = actual || anterior;\n";
           // codigoDeSalida += "                }\n";
           // codigoDeSalida += "            }\n";

           // codigoDeSalida += "        }\n";

           // codigoDeSalida += "    }\n";
           // codigoDeSalida += " return (anterior || actual);\n";
           // codigoDeSalida += "}\n";

           // // codigoDeSalida += "    return (anterior || actual);
           // // codigoDeSalida += "}

           // codigoDeSalida += "            }\n";
           // codigoDeSalida+= "        }\n";
           // codigoDeSalida += "    }\n";
           // codigoDeSalida += "    }\n";
            #endregion
        }

    }
    
}
