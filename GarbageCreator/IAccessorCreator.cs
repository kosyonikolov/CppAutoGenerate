using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarbageCreator
{
    public struct FieldInfo
    {
        public string type;
        public string name;
    }

    public interface IAccessorCreator
    {
        string MakeGetter(FieldInfo info);
        string MakeSetter(FieldInfo info);
    }
}
