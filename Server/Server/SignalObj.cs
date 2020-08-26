using System;
using System.Collections;

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

        readonly Hashtable loginHashtable = new Hashtable();

        public void SetloginHashtable(string clientAddr, string clientInfo)
        {
            if(!loginHashtable.ContainsKey(clientAddr))
            {
                //Console.WriteLine(clientAddr + " : " + clientInfo); // 해시테이블 입력값 확인
                loginHashtable.Add(clientAddr, clientInfo);
            }
        }

        // 서버가 꺼져있는지, 서버가 컨트롤 중인지는 서버만 제어
        public bool IsServerShutdown { get; set; }
        public bool IsServerControlling { get; set; }
        public bool IsServerInternetControlling { get; set; }

        // 화면 전송 데이터 getter, setter
        public byte[] ServerScreenData { get; set; } = null;
    }
}
