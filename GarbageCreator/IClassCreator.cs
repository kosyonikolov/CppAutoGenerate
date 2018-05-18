using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarbageCreator
{
    public struct GeneratedClass
    {
        public string headerFile;
        public string cppFile;
    }

    public interface IClassCreator
    {
        GeneratedClass GenerateClass(string className, IList<FieldInfo> fields);
    }
}
