using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Resources;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.YunaEngine.Gui
{
    public class StockImage : GuiComponent
    {
        Sprite _sprite;
        Vector2 _scaliedPosition;
        float _scaling;

        public StockImage(YunaTexture texture, Vector2 position)
        {
            _sprite = new Sprite(texture);
            _sprite.Position = position;
            CalculateScaling();
        }

        private void CalculateScaling()
        {
            _scaling = (float)Globals.Viewport.Width / 1920;

            float widthDif = 1920f - (float)Globals.Viewport.Width;
            float heightDif = 1080f - (float)Globals.Viewport.Height;

            _scaliedPosition = new Vector2(_sprite.Position.X - widthDif * (_sprite.Position.X / 1920), _sprite.Position.Y - heightDif * (_sprite.Position.Y / 1080f));
        }

        public override void Draw(IRender render)
        {
            _sprite.Position = _scaliedPosition;
            _sprite.Scale = _scaling;
            _sprite.Draw(render, new Camera());
        }
    }
}
