using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWeb.Patterns
{
    interface IRepo<T> : IDisposable
        where T : class
    {
        int GetID();
    }
}
