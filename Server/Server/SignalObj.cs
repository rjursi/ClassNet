using System;

namespace Server
{
    [Serializable]
    class SignalObj
    {
        public SignalObj()
        {
            IsShutdown = false;
            IsLock = false;
            IsInternet = false;
            IsPower = false;
            IsMonitoring = false;
        }

        // 서버가 꺼져있는지, 서버가 컨트롤 중인지는 서버만 제어
        public bool IsShutdown { get; set; }
        public bool IsLock { get; set; }
        public bool IsInternet { get; set; }
        public bool IsPower { get; set; }
        public bool IsMonitoring { get; set; }

        // 화면 전송 데이터 getter, setter
        public byte[] ServerScreenData { get; set; } = null;
    }
}
