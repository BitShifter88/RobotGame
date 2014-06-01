using Macalania.Probototaker.Rooms;
using Macalania.YunaEngine;
using System;

namespace Macalania.Probototaker
{
    static class Program
    {
        static YunaGameEngine _engine;
        static void Main(string[] args)
        {
            using ( _engine = new YunaGameEngine())
            {
                _engine.EngineStarted += new YunaGameEngine.EngineStartedEventHandler(OnEngineStart);
                _engine.Run();
            }
        }

        static void OnEngineStart()
        {
            LoadGameRoom room = new LoadGameRoom(_engine);
            _engine.SetActiveRoom(room, true);
        }
    }
}

