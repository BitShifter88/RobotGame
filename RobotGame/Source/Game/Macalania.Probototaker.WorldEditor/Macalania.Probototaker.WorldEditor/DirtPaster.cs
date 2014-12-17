using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.WorldEditor
{
    class DirtPaster : PasterBase
    {
        Random r = new Random();
        public override void Paste(int layer, int x, int y, Map.GameMap gameMap)
        {
            byte tile = (byte)r.Next(0, 3 + 1);

            if (tile != 0)
                tile = (byte)r.Next(0, 3 + 1);
  
            gameMap.SetTile(layer, x, y, tile);
        }
    }
}
