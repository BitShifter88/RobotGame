using Macalania.Probototaker.Tanks;
using Macalania.Probototaker.Tanks.Hulls;
using Macalania.Probototaker.Tanks.Plugins;
using Macalania.Probototaker.Tanks.Plugins.MainGuns;
using Macalania.Probototaker.Tanks.Plugins.Mic;
using Macalania.Probototaker.Tanks.Tracks;
using Macalania.Probototaker.Tanks.Turrets;
using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Input;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker
{
    public class OtherPlayer : GameObject
    {
        Tank _tank;
        int _tankNumber;

        public OtherPlayer(Room room, int tankNumber)
            : base(room)
        {
            _tankNumber = tankNumber;
        }
        public override void Inizialize()
        {
            base.Inizialize();
        }

        public override void Load(ResourceManager content)
        {
            if (_tankNumber == 1)
                _tank = TankGenerator.GenerateTank1(Room, content, new Vector2(800, 200));
            if (_tankNumber == 2)
                _tank = TankGenerator.GenerateTank2(Room, content, new Vector2(1000, 200));

            Room.AddGameObjectWhileRunning(_tank);

            base.Load(content);
        }

        public override void Update(double dt)
        {
            if (_tankNumber == 2)
            {
                if (KeyboardInput.IsKeyClicked(Keys.NumPad9))
                    _tank.ActivatePlugin(_tank.Turret.Plugins.Where(i => i.GetType() == typeof(ShieldPlugin)).FirstOrDefault(), Vector2.Zero, null);
            }
            base.Update(dt);
        }

        public override void Draw(IRender render, Camera camera)
        {
            base.Draw(render, camera);
        }
    }
}
