using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GarbageCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            AccessorCreatorStore store = new AccessorCreatorStore();
            store.Add("char*", new StringAccessorCreator());
            BasicClassCreator classCreator = new BasicClassCreator();

            DisplayUsage();

            List<FieldInfo> fields = new List<FieldInfo>();
            while (true)
            {
                string[] input = Console.ReadLine().Split(' ');

                if (input.Length == 1)
                {
                    string termination = input[0];
                    if (termination.Length == 0 || termination[0] != '.') continue;

                    if (termination.Length == 1)
                    {
                        foreach (var field in fields)
                        {
                            IAccessorCreator creator = store[field.type];
                            Console.WriteLine(creator.MakeGetter(field));
                            Console.WriteLine();
                            Console.WriteLine(creator.MakeSetter(field));
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        string className = termination.Substring(1);
                        GeneratedClass generatedClass = classCreator.GenerateClass(className, fields);

                        File.WriteAllText(className + ".h", generatedClass.headerFile);
                        File.WriteAllText(className + ".cpp", generatedClass.cppFile);

                        Console.WriteLine("Class saved in {0}", System.Environment.CurrentDirectory);
                    }

                    fields.Clear();
                }

                if (input.Length != 2) continue;
                FieldInfo current = new FieldInfo();
                current.type = input[0];
                current.name = input[1];
                fields.Add(current);
            }
        }

        static void DisplayUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("Enter the fields on seperate lines in format [type] [name] - without ;");
            Console.WriteLine("When done, type .[<optional>class name]");
            Console.WriteLine("Example:");
            Console.WriteLine("int size");
            Console.WriteLine("char* name");
            Console.WriteLine(".Truck");

            Console.WriteLine();
        }
    }
}
