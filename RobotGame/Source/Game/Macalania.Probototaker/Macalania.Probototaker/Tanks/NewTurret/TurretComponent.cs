using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.NewTurret
{
    public class TurretComponent
    {
        public bool CanAttachLeft { get; set; }
        public bool CanAttachRight { get; set; }
        public bool CanAttachTop { get; set; }
        public bool CanAttachBottom { get; set; }

        protected int _x;
        protected int _y;

        public virtual void Load(ResourceManager content)
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
