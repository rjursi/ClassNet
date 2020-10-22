using System;

namespace Client
{
    [Serializable]
    public class SignalObj : IDisposable
    {
        private bool disposed;

        public SignalObj()
        {

            IsShutdown = false;
            IsLock = false;
            IsInternet = false;
            IsPower = false;
            IsMonitoring = false;
            IsTaskMgrEnabled = false;

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

        // 명령 관련 데이터 getter, setter
        public bool IsShutdown { get; set; }
        public bool IsLock { get; set; }
        public bool IsInternet { get; set; }
        public bool IsPower { get; set; }
        public bool IsMonitoring { get; set; }
        public bool IsTaskMgrEnabled { get; set; }

        public bool SubmitAssignment { get; set; }

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
