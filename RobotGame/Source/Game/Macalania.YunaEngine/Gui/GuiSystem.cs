using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.YunaEngine.Gui
{
    public class GuiSystem : GameObject
    {
        List<GuiComponent> _guiComponents = new List<GuiComponent>();

        public GuiSystem(Room room) : base(room)
        {

        }

        public void AddGuiComponent(GuiComponent gc)
        {
            _guiComponents.Add(gc);
        }

        public override void Draw(IRender render, Graphics.Camera camera)
        {
            foreach (GuiComponent gc in _guiComponents)
            {
                gc.Draw(render);
            }
        }
    }
}
