using BehringerAudioVolume.Func;
using BehringerAudioVolume.Global;
using BehringerAudioVolume.Param;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using Behringer.X32;
using System.Net;

namespace BehringerAudioVolume
{
    class Program
    {
        static string _fileSetup = "param.json";
        static SetupConsole _setup;

        static X32Console _console;

        static bool _connect = false;
        static void Main(string[] args)
        {
            _setup = LoadProperties();
            X32ConnectCheck x32Connect = new X32ConnectCheck(_setup.IpAdress, _setup.Port);
            x32Connect.Connect += X32Connect_Connect;

            while (true)
            {
                Thread.Sleep(100);
            }
        }

        private static void X32Connect_Connect(object sender, bool e)
        {
            if (!_connect && e)
                ConnectConsole();

            _connect = e;
        }

        static void ConnectConsole()
        {
            _console = new X32Console()
            {
                IPAddress = IPAddress.Parse(_setup.IpAdress),
                Port = _setup.Port,
                Interval = 1000
            };
            _console.OnChannelMute += _console_OnChannelMute;
            _console.Connect();
        }

        private static void _console_OnChannelMute(object sender, OSC.OSCPacket packet)
        {
            if(int.Parse(packet.Nodes[2]) == _setup.Channel)
            {
                VolumeDown();
            }
        }

        static void VolumeDown()
        {
            _console.SendParameter(_console.Aux[0].Strip.Fader);
        }

        static SetupConsole LoadProperties()
        {
            SetupConsole setup;

            if (!Directory.Exists(PathSystem.Properties))
                Directory.CreateDirectory(PathSystem.Properties);

            if (File.Exists(Path.Combine(PathSystem.Properties, _fileSetup)))
                return setup = JsonConvert.DeserializeObject<SetupConsole>(File.ReadAllText(Path.Combine(PathSystem.Properties, _fileSetup)));

            setup = new SetupConsole()
            {
                IpAdress = "127.0.0.1",
                Port = 10023,
                Aux = 0,
                Channel = 0,
                HideConsole = false,
                Miliseccond = 1000
            };

            File.WriteAllText(Path.Combine(PathSystem.Properties, _fileSetup), JsonConvert.SerializeObject(setup));

            return setup;
        }
    }
}
