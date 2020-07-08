using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    [Serializable]
    public class SignalObj : IDisposable
    {
        private bool disposed;

        // 서버 상태, 제어명령 상태 저장 변수
        private bool isServerShutdown;
        private bool isServerControlling;

        // 서버에서 보내는 이미지 관련 데이터
        private byte[] serverScreenData;

        public SignalObj()
        {
            isServerShutdown = false;
            isServerControlling = false;
        }

        ~SignalObj()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        // 위 서버가 꺼져있는지, 서버가 컨트롤 중인지는 서버만 제어함
        public bool IsServerShutdown { get => isServerShutdown; set => isServerShutdown = value; }
        public bool IsServerControlling { get => isServerControlling; set => isServerControlling = value; }

        // 방송 관련 데이터 getter, setter
        public byte[] ServerScreenData { get => serverScreenData; set => serverScreenData = value; }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;
            if (disposing)
            {
                serverScreenData = null;
            }
            this.disposed = true;
        }
    }
}
