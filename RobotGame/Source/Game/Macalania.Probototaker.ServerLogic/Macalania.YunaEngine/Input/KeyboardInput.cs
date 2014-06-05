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
    public class KeyboardInput :  IDisposable
    {
        private static KeyboardHook m_Hook;
        private static object locker = new object();
        private static KeyboardState m_PreviusKeyboardState;
        private static KeyboardState m_KeyboardState;
        private static string m_KeyboardStringBuffer = "";
        private static string m_KeyboardString = "";

        public KeyboardInput(bool enableKeyboardHook)
        {
            if (enableKeyboardHook == true)
            {
                m_Hook = new KeyboardHook();
                m_Hook.KeyPress += new System.Windows.Forms.KeyPressEventHandler(KeyPress);
            }
        }

        private static void KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            lock (locker)
            {
                m_KeyboardStringBuffer += e.KeyChar;
            }
        }

        public void EngineUpdate(GameTime gameTime)
        {
            lock (locker)
            {
                m_KeyboardString = m_KeyboardStringBuffer;
                m_KeyboardStringBuffer = null;
            }

            m_PreviusKeyboardState = m_KeyboardState;
            m_KeyboardState = Keyboard.GetState();
        }

        public static bool IsKeyDown(Keys key)
        {
            if (m_KeyboardState.IsKeyDown(key))
                return true;

            return false;
        }

        public static bool IsKeyUp(Keys key)
        {
            if (m_KeyboardState.IsKeyUp(key))
                return true;

            return false;
        }

        public static bool IsKeyClicked(Keys key)
        {
            if (m_KeyboardState.IsKeyUp(key) && m_PreviusKeyboardState.IsKeyDown(key))
                return true;

            return false;
        }

        public static string GetKeyboardString()
        {
            return m_KeyboardString;
        }

        public void Dispose()
        {
            if (m_Hook != null)
            {
                m_Hook.Stop();
            }
        }
    }
}
