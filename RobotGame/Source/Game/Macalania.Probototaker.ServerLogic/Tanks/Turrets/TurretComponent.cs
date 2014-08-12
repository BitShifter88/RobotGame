using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Turrets
{
    public class TurretComponent : TankComponent
    {
        public bool CanAttachLeft { get; set; }
        public bool CanAttachRight { get; set; }
        public bool CanAttachTop { get; set; }
        public bool CanAttachBottom { get; set; }

        protected int _x;
        protected int _y;

        public TurretComponent(Room room) : base(room)
        {

        }


        public virtual void Load(ResourceManager content)
        {

        }

        public virtual void Update(double dt)
        {

        }

        public virtual void Draw(IRender render, Camera camera)
        {

        }

        public virtual void SetLocation(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }
}
