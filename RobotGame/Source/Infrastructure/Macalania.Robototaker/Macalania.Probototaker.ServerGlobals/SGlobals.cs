using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macalania.Probototaker.ServerGlobals
{
    public static class SGlobals
    {
        // Når der bliver skudt blive clienterne ikke informeret om det med det samme. Der ventes et antal opdateringer for at spare package header.
        public static int UpdatesBetweenProjectileSends = 2;
    }
}
