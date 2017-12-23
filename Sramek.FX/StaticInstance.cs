using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sramek.FX
{
    public abstract class StaticInstance<T>
        where T : new()
    {
        public static T I { get; protected set; } = new T();
    }
   
}
