using Macalania.Probototaker.Rooms;
using Macalania.YunaEngine;
using Macalania.YunaEngine.Rooms;
using System;
using System.Windows.Forms;

namespace Macalania.Probototaker
{
    static class Program
    {
        static YunaGameEngine _engine;
        static void Main(string[] args)
        {
            try
            {
                using (_engine = new YunaGameEngine())
                {
                    _engine.EngineStarted += new YunaGameEngine.EngineStartedEventHandler(OnEngineStart);
                    _engine.Run();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        static void OnEngineStart()
        {
            LoadGameRoom room = new LoadGameRoom();
            RoomManager.Instance.SetActiveRoom(room, true, YunaGameEngine.Instance.Services);
        }
    }
}

