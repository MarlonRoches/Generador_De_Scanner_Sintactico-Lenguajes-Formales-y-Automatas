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
            var SETS = new Dictionary<string, List<string>>();
            var Tokens = new Dictionary<string, List<string>>();
            while (linea != null)
            {

                if (linea == "SETS")
                {
                    var contador = 0;
                    linea = reader.ReadLine().Replace("\t", "").Replace(" ", "");
                    while (linea != "TOKENS" &&  linea != "ACTIONS")
                    {//partimos y quitmos espacios
                        var arreglo = linea.Split('=');
                        SETS.Add(arreglo[0], new List<string>());
                        var items = arreglo[1].Replace("'","");
                        foreach (var item in items.Split('+'))
                        {
                            SETS[arreglo[0]].Add(item);
                        }
                        linea = reader.ReadLine().Replace("\t", "").Replace(" ", "");
                    }

                    if (linea == "TOKENS")
                    {

                    }

                    if (linea == "ACTIONS")
                    {

                    }



                }



                Console.ReadLine();



            }
        }
    }
}
