using Macalania.Probototaker.Map;
using Macalania.YunaEngine.Rooms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.WorldEditor
{
    class EditorRoom : Room
    {
        GameMap _gameMap = null;
        public override void Load(IServiceProvider serviceProvider)
        {
            base.Load(serviceProvider);

            if (File.Exists("map.ptm") == false)
            {
                GameMap g = new GameMap(128, 128, this);
                g.TileStyle = new JungleStyle();
                g.TileStyle.Load(Content);
                g.CreateNextLayer();
                AddGameObject(g);
                _gameMap = g;
                _gameMap.SaveToFile("map.ptm");
            }
            else
            {
                _gameMap = GameMap.LoadFromFile("map.ptm", this);
            }
            
        }

    }
}
