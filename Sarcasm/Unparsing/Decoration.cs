using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarcasm.Unparsing
{
    public class Decoration
    {
        private Dictionary<object, object> keyToValue = new Dictionary<object, object>();

        public void Add<T>(T value)
        {
            Add(typeof(T), value);
        }

        public void Add(object key, object value)
        {
            keyToValue.Add(key, value);
        }

        public T Get<T>()
        {
            return Get<T>(typeof(T));
        }

        public TValue Get<TValue>(object key)
        {
            return (TValue)keyToValue[key];
        }
    }
}
