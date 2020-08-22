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

        Hashtable loginHashtable = new Hashtable();

        public void SetloginHashtable(string data)
        {
            String[] result = data.Replace("\0", string.Empty).Split(' '); // 끝마침문자 제거 및 공백 단위로 배열 저장.
            if (!loginHashtable.ContainsKey(result[1])) // 헤시테이블 내 중복키 확인
            {
                Console.WriteLine(result[1] + result[2]); // 헤시테이블 입력값 확인
                loginHashtable.Add(result[1], result[2]);
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
