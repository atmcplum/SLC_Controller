using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLC_Controller {
    /// <summary>
    /// 싸이클 내에서 각 채널의 설정값을 나타내는 클래스입니다. Ima, Wus, Dus 세 가지 속성을 포함
    /// 1싸이클은 4채널로 이루어져 있고 각 채널은 아래의 설정값을 가짐
    /// Ima는 전류값을 나타내며 Dus만큼 대기후 Wus 만큼 펄스를 유지하는 형태로 동작
    /// </summary>
    public class ChannelSettings {
        public int Ima { get; set; }
        public int Wus { get; set; }
        public int Dus { get; set; }
    }
}
