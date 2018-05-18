using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarbageCreator
{
    public class BasicAccessorCreator : IAccessorCreator
    {
        public static string Capitalize(string str)
        {
            StringBuilder builder = new StringBuilder(str);
            if (builder[0] >= 'a' && builder[0] <= 'z') builder[0] = (char)(builder[0] + 'A' - 'a');
            return builder.ToString();
        }

        public string MakeGetter(FieldInfo info)
        {
            return string.Format(R.basicGetter, info.type, Capitalize(info.name), info.name);
        }

        public string MakeSetter(FieldInfo info)
        {
            return string.Format(R.basicSetter, info.type, Capitalize(info.name), info.name);
        }
    }
}
