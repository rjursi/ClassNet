using System;

namespace Server
{
    [Serializable]
    class SignalObj
    {
        public SignalObj()
        {
            IsServerShutdown = false;
            IsServerControlling = false;
            IsServerInternetControlling = false;
        }

        // 서버가 꺼져있는지, 서버가 컨트롤 중인지는 서버만 제어
        public bool IsServerShutdown { get; set; }
        public bool IsServerControlling { get; set; }
        public bool IsServerInternetControlling { get; set; }

        // 화면 전송 데이터 getter, setter
        public byte[] ServerScreenData { get; set; } = null;
    }
}
