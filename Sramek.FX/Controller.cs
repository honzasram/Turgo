using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sramek.FX
{
    public abstract class Controller<T>
        where T : new()
    {
        public static T I { get; } = new T();
    }
}
