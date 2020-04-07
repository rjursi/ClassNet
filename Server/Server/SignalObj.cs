using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [Serializable]
    class SignalObj
    {

        // 서버 방송 여부 및 상태, 제어명령 상태 저장 변수
        private bool isServerShutdown;
        private bool isServerBroadcasting;
        private bool isServerControlling;

        // 서버에서 보내는 이미지 관련 데이터
        private byte[] imgData;

        public SignalObj()
        {
            isServerShutdown = false;
            isServerBroadcasting = false;
            isServerControlling = false;

        }


        // 위 서버가 꺼져있는지, 서버가 방송중인지, 서버가 컨트롤 상태 중인지는 서버만 제어함
        public bool IsServerShutdown { get => isServerShutdown; set => isServerShutdown = value; }
        public bool IsServerBroadcasting { get => isServerBroadcasting; set => isServerBroadcasting = value; }
        public bool IsServerControlling { get => isServerControlling; set => isServerControlling = value; }
        public byte[] ImgData { get => imgData; set => imgData = value; }


        // 이미지 관련 데이터 getter, setter

    }

}
