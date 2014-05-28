using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins
{
    enum PluginDirection
    {
        Top,
        Buttom,
        Left,
        Right,
    }
    class Plugin : TankComponent
    {
        public int Size { get; protected set; }

        public void SetOriginFromTurret(Vector2 origin)
        {

        }
    }
}
