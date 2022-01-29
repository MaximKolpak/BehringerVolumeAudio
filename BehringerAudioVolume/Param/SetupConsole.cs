using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehringerAudioVolume.Param
{
    public class SetupConsole
    {
        public string IpAdress { get; set; }
        public int Port { get; set; }
        public int Aux { get; set; }
        public int Channel { get; set; }
        public bool HideConsole { get; set; }
        public int Miliseccond { get; set; }
    }
}
