using System;

using NetFwTypeLib;
namespace InternetControl
{
    class FirewallPortBlock : IDisposable
    {
        const string TCP_HTTP_HTTPS_BLOCK_RULENAME = "ClassNet Internet Control_TCP_HTTP HTTPS";
        const string UDP_HTTP_BLOCK_RULENAME = "ClassNet Internet Control_UDP_HTTP";
        private bool disposedValue;


        public bool NowCtrlStatus { get; set; }

        public FirewallPortBlock()
        {
            // 초기는 실행되지 않은 상태로 설정
            NowCtrlStatus = false;

        }

        public void CtrlStatusEventCheck(bool newCtrlStatus)
        {
            if (NowCtrlStatus != newCtrlStatus)
            {
                NowCtrlStatus = newCtrlStatus;
                this.PortControl(NowCtrlStatus);
            }
        }

        private void PortControl(bool ctrlStatus)
        {
            if (ctrlStatus)
            {
                TcpHttpHttpsBlock();
                UdpHttpHttpsBlock();
            }
            else if(!ctrlStatus){
                RuleRemove();
            }
        }


        public void TcpHttpHttpsBlock()
        {
            INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));

            firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
            firewallRule.Description = "TCP Based HTTP, HTTPS Protocol Block";
            firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
            firewallRule.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
            firewallRule.RemotePorts = "80,443";
            firewallRule.Enabled = true;
            firewallRule.InterfaceTypes = "All";
            firewallRule.Name = TCP_HTTP_HTTPS_BLOCK_RULENAME;

            // 설정하기 위한 룰을 만드는 작업

            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            firewallPolicy.Rules.Add(firewallRule);

        }

        public void UdpHttpHttpsBlock()
        {
            INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));

            firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
            firewallRule.Description = "UDP Based HTTP, HTTPS Protocol Block";
            firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
            firewallRule.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP;
            firewallRule.RemotePorts = "80,443";
            firewallRule.Enabled = true;
            firewallRule.InterfaceTypes = "All";
            firewallRule.Name = UDP_HTTP_BLOCK_RULENAME;

            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            firewallPolicy.Rules.Add(firewallRule);

          
        }

        public void RuleRemove()
        {
            INetFwPolicy2 policyRemover = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

            policyRemover.Rules.Remove(TCP_HTTP_HTTPS_BLOCK_RULENAME);
            policyRemover.Rules.Remove(UDP_HTTP_BLOCK_RULENAME);
                
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        // ~FirewallPortBlock()
        // {
        //     // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
