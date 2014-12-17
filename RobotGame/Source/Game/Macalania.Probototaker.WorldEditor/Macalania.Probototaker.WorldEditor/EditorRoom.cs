using Macalania.Probototaker.Map;
using Macalania.YunaEngine.Input;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.WorldEditor
{
    enum PasterType
    {
        DirtPaster,
    }

    class EditorRoom : Room
    {
        PasterBase _activePaster;
        int _activeLayer = 0;
        GameMap _gameMap = null;

        DirtPaster _dirtPaster = new DirtPaster();

        public override void Load(IServiceProvider serviceProvider)
        {
            _activePaster = _dirtPaster;
            base.Load(serviceProvider);

            if (File.Exists("map.ptm") == false)
            {
                GameMap g = new GameMap(64, 64, this);
                g.TileStyle = new JungleStyle();
                g.TileStyle.Load(Content);
                g.CreateNextLayer();

                for (int i = 0; i < g.DimX; i++)
                {
                    for (int j = 0; j < g.DimY; j++)
                    {
                        _dirtPaster.Paste(0, i, j, g);
                    }
                }

                    _gameMap = g;
                _gameMap.SaveToFile("map.ptm");
            }
            else
            {

                _gameMap = GameMap.LoadFromFile("map.ptm", this);
            }
            AddGameObject(_gameMap);
        }

        public override void Update(double dt)
        {
            if (KeyboardInput.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
                Camera.Position += new Vector2(0, 5);
            if (KeyboardInput.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
                Camera.Position += new Vector2(0, -5);
            if (KeyboardInput.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                Camera.Position += new Vector2(5, 0);
            if (KeyboardInput.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                Camera.Position += new Vector2(-5, 0);

            if (MouseInput.IsLeftMousePressed())
            {
                int tileX = ( MouseInput.X-(int)Camera.Position.X ) / 64;
                int tileY = ( MouseInput.Y - (int)Camera.Position.Y)/ 64;
                _activePaster.Paste(_activeLayer, tileX, tileY, _gameMap);
            }

            base.Update(dt);
        }
    }
}
