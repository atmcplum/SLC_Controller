using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLC_Controller {
    public class ChannelSettings {
        public int Ima { get; set; } // 전류 설정값
        public int Wus { get; set; } // 온 시간(us)
        public int Dus { get; set; } // 오프 시간(us)
    }
}
