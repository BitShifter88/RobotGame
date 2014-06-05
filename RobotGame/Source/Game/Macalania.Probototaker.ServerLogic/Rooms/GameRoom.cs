﻿using Macalania.Probototaker.Effects;
using Macalania.Probototaker.Tanks;
using Macalania.YunaEngine;
using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Rooms
{
    public class GameRoom : Room
    {
        public List<Tank> Tanks { get; set; }
        public List<Shield> Shields { get; set; }

        public GameRoom(YunaGameEngine engine) : base(engine)
        {
            Tanks = new List<Tank>();
            Shields = new List<Shield>();
        }

        protected override void RemoveGameObject(GameObject obj)
        {
            if (obj.GetType() == typeof(Tank))
                Tanks.Remove((Tank)obj);
            if (obj.GetType() == typeof(Shield))
                Shields.Remove((Shield)obj);

            base.RemoveGameObject(obj);
        }

        public override void AddGameObject(GameObject obj)
        {
            RegisterGameObject(obj);

            base.AddGameObject(obj);
        }

        private void RegisterGameObject(GameObject obj)
        {
            if (obj.GetType() == typeof(Tank))
            {
                Tanks.Add((Tank)obj);
            }
            if (obj.GetType() == typeof(Shield))
            {
                Shields.Add((Shield)obj);
            }
        }

        public override void AddGameObjectWhileRunning(GameObject obj)
        {
            RegisterGameObject(obj);

            base.AddGameObjectWhileRunning(obj);
        }

        public override void Inizialize()
        {
            Player player = new Player(this);
            AddGameObject(player);

            OtherPlayer op = new OtherPlayer(this, 1);
            AddGameObject(op);

            OtherPlayer op2 = new OtherPlayer(this, 2);
            AddGameObject(op2);

            base.Inizialize();
        }
    }
}