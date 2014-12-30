using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.YunaEngine.Gui
{
    public class GuiSystem
    {
        List<GuiComponent> _guiComponents = new List<GuiComponent>();
        GuiRender _guiRender;

        public GuiSystem()
        {
            _guiRender = new GuiRender(Globals.Device);
        }

        public void AddGuiComponent(GuiComponent gc)
        {
            _guiComponents.Add(gc);
        }

        public void RemoveGuiComponent(GuiComponent gc)
        {
            if (_guiComponents.Contains(gc))
            {
                _guiComponents.Remove(gc);
            }
        }

        public void Update()
        {
            foreach (GuiComponent gc in _guiComponents)
            {
                gc.Update();
            }
        }

        public void Draw()
        {
            _guiRender.Begin(null);
            foreach (GuiComponent gc in _guiComponents)
            {
                gc.Draw(_guiRender);
            }
            _guiRender.End();
        }
    }
}
