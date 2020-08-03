using System;

namespace Client
{
    [Serializable]
    public class SignalObj : IDisposable
    {
        private bool disposed;

        public SignalObj()
        {
            IsServerShutdown = false;
            IsServerControlling = false;
            IsServerInternetControlling = false;
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
        public bool IsServerShutdown { get; set; }
        public bool IsServerControlling { get; set; }
        public bool IsServerInternetControlling { get; set; }

        // 방송 관련 데이터 getter, setter
        public byte[] ServerScreenData { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;
            if (disposing)
            {
                // TODO: 여기에서 자원을 관리하세요.
                ServerScreenData = null;
            }
            this.disposed = true;
        }
    }
}
