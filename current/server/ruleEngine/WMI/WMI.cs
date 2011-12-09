using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ruleEngine.WMI
{
    class WMI : NativeWMI
    {
        private bool _hadBeenInit;
        private void initWMI()
        {
          
                 
        }


    }

    internal class NativeWMI
    {
        protected enum CoInit
        {
            MultiThreaded = 0x0,
            ApartmentThreaded = 0x2,
            DisableOle1Dde = 0x4,
            SpeedOverMemory = 0x8
        }
        [DllImport(@"ole32.dll")]
        protected static extern int CoInitializeEx(IntPtr reserved, CoInit coInit);

        protected enum RpcAuthnLevel
        {
            Default = 0,
            None = 1,
            Connect = 2,
            Call = 3,
            Pkt = 4,
            PktIntegrity = 5,
            PktPrivacy = 6
        }

        protected enum RpcImpLevel
        {
            Default = 0,
            Anonymous = 1,
            Identify = 2,
            Impersonate = 3,
            Delegate = 4
        }

        protected enum EoAuthnCap
        {
            None = 0x00,
            MutualAuth = 0x01,
            StaticCloaking = 0x20,
            DynamicCloaking = 0x40,
            AnyAuthority = 0x80,
            MakeFullSIC = 0x100,
            Default = 0x800,
            SecureRefs = 0x02,
            AccessControl = 0x04,
            AppID = 0x08,
            Dynamic = 0x10,
            RequireFullSIC = 0x200,
            AutoImpersonate = 0x400,
            NoCustomMarshal = 0x2000,
            DisableAAA = 0x1000
        }

        [DllImport(@"ole32.dll")]
        protected static extern int CoInitializeSecurity(IntPtr pVoid, int cAuthSvc, IntPtr asAuthSvc, IntPtr pReserved1, RpcAuthnLevel level,
                                                            RpcImpLevel impers, IntPtr pAuthList, EoAuthnCap dwCapabilities, IntPtr pReserved3);


    }
}
