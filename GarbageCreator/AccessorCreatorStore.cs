using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarbageCreator
{
    public class AccessorCreatorStore
    {
        private static IAccessorCreator defaultCreator = new BasicAccessorCreator();
        private Dictionary<string, IAccessorCreator> store = new Dictionary<string, IAccessorCreator>();

        public void Add(string type, IAccessorCreator creator)
        {
            store.Add(type, creator);
        }

        public IAccessorCreator this[string type]
        {
            get
            {
                IAccessorCreator creator;
                if (!store.TryGetValue(type, out creator)) creator = defaultCreator;
                return creator;
            }
        }
    }
}
