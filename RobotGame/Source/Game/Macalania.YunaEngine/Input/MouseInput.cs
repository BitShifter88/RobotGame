using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Macalania.YunaEngine.Input
{
    public class MouseInput
    {
        private static MouseState m_MouseState;
        private static MouseState m_PreviusMouseState;

        public static MouseState PreviusMouseState
        {
            get { return MouseInput.m_PreviusMouseState; }
        }

        public static int X
        {
            get { return m_MouseState.X; }
        }

        public static int Y
        {
            get { return m_MouseState.Y; }
        }

        public MouseInput()
        {
            m_MouseState = Mouse.GetState();
            m_PreviusMouseState = Mouse.GetState();
        }

        public void EngineUpdate(GameTime gameTime)
        {
            m_PreviusMouseState = m_MouseState;
            m_MouseState = Mouse.GetState();
        }

        public static bool IsMiddleMousePressed()
        {
            if (m_MouseState.MiddleButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public static bool IsRightMousePressed()
        {
            if (m_MouseState.RightButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public static bool IsLeftMousePressed()
        {
            if (m_MouseState.LeftButton == ButtonState.Pressed)
                return true;
            return false;
        }

        public static bool IsMiddleMouseClicked()
        {
            if (m_MouseState.MiddleButton == ButtonState.Released && m_PreviusMouseState.MiddleButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public static bool IsRightMouseClicked()
        {
            if (m_MouseState.RightButton == ButtonState.Released && m_PreviusMouseState.RightButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public static bool IsLeftMouseClicked()
        {
            if (m_MouseState.LeftButton == ButtonState.Released && m_PreviusMouseState.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public static int GetScrollWheelValue()
        {
            return m_MouseState.ScrollWheelValue;
        }

        public static int GetScrollWheelDifference()
        {
            return m_PreviusMouseState.ScrollWheelValue - m_MouseState.ScrollWheelValue;
        }

        public static int GetMousePositionXDifference()
        {
            return m_MouseState.X - m_PreviusMouseState.X;
        }

        public static int GetMousePositionYDifference()
        {
            return m_MouseState.Y - m_PreviusMouseState.Y;
        }

        public static void SetMousePosition(int x, int y)
        {
            Mouse.SetPosition(x, y);
        }

        public static void SetMouseToPreviusPosition()
        {
            Mouse.SetPosition(m_PreviusMouseState.X, m_PreviusMouseState.Y);
        }
    }
}
