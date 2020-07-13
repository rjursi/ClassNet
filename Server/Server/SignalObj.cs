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
        // 서버 상태, 제어명령 상태 저장 변수
        private bool isServerShutdown;
        private bool isServerControlling;

        private bool isServerUpload;

        // 서버 방송 데이터
        private byte[] serverScreenData = null;

        public SignalObj()
        {
            isServerShutdown = false;
            isServerControlling = false;

            isServerUpload = false;
        }

        // 위 서버가 꺼져있는지, 서버가 컨트롤 중인지는 서버만 제어함
        public bool IsServerShutdown { get => isServerShutdown; set => isServerShutdown = value; }
        public bool IsServerControlling { get => isServerControlling; set => isServerControlling = value; }

        public bool IsServerUpload { get=>isServerUpload; set =>isServerUpload=value; }

        // 화면 전송 데이터 getter, setter
        public byte[] ServerScreenData { get => serverScreenData; set => serverScreenData = value; }
    }
}
