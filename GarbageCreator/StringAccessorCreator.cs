using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarbageCreator
{
    public class StringAccessorCreator : IAccessorCreator
    {
        public string MakeGetter(FieldInfo info)
        {
            return string.Format(R.stringGetter, info.name, BasicAccessorCreator.Capitalize(info.name));
        }

        public string MakeSetter(FieldInfo info)
        {
            return string.Format(R.stringSetter, info.name, BasicAccessorCreator.Capitalize(info.name));
        }
    }
}
