using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.WinService
{
    public class Win32
    {
        [DllImport("Wtsapi32.dll")]
        private static extern bool WTSQuerySessionInformation(IntPtr hServer, int sessionId, WtsInfoClass wtsInfoClass, out System.IntPtr ppBuffer, out int pBytesReturned);
        [DllImport("Wtsapi32.dll")]
        private static extern void WTSFreeMemory(IntPtr pointer);

        public enum WtsInfoClass
        {
            WTSInitialProgram,
            WTSApplicationName,
            WTSWorkingDirectory,
            WTSOEMId,
            WTSSessionId,
            WTSUserName,
            WTSWinStationName,
            WTSDomainName,
            WTSConnectState,
            WTSClientBuildNumber,
            WTSClientName,
            WTSClientDirectory,
            WTSClientProductId,
            WTSClientHardwareId,
            WTSClientAddress,
            WTSClientDisplay,
            WTSClientProtocolType,
            WTSIdleTime,
            WTSLogonTime,
            WTSIncomingBytes,
            WTSOutgoingBytes,
            WTSIncomingFrames,
            WTSOutgoingFrames,
            WTSClientInfo,
            WTSSessionInfo,
        }

        public static string GetUsernameBySessionId(int sessionId, bool prependDomain)
        {
            IntPtr buffer;
            int strLen;
            string username = "SYSTEM";
            if (WTSQuerySessionInformation(IntPtr.Zero, sessionId, WtsInfoClass.WTSUserName, out buffer, out strLen) && strLen > 1)
            {
                username = Marshal.PtrToStringAnsi(buffer);
                WTSFreeMemory(buffer);
                if (prependDomain)
                {
                    if (WTSQuerySessionInformation(IntPtr.Zero, sessionId, WtsInfoClass.WTSDomainName, out buffer, out strLen) && strLen > 1)
                    {
                        username = Marshal.PtrToStringAnsi(buffer) + "\\" + username;
                        WTSFreeMemory(buffer);
                    }
                }
            }
            return username;
        }

        public static string GetSidByUsername(string username)
        {
            NTAccount f = new NTAccount(username);
            SecurityIdentifier s = (SecurityIdentifier)f.Translate(typeof(SecurityIdentifier));
            String sidString = s.ToString();
            return sidString;
        }

        public static IEnumerable<Tuple<string,uint>> GetCurrentUsers()
        {
            
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE Name=\'explorer.exe\'");
            ManagementObjectCollection collection = searcher.Get();
            foreach( ManagementObject obj in collection)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                int retval = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                string username = string.Join("\\", argList.Reverse());
                uint sessionId = (uint) obj["SessionId"];
                yield return new Tuple<string, uint>(username, sessionId);
            }
        }


        public const int TOKEN_QUERY = 0X00000008;

        const int ERROR_NO_MORE_ITEMS = 259;

        enum TokenInformationClass
        {
            TokenUser = 1,
            TokenGroups,
            TokenPrivileges,
            TokenOwner,
            TokenPrimaryGroup,
            TokenDefaultDacl,
            TokenSource,
            TokenType,
            TokenImpersonationLevel,
            TokenStatistics,
            TokenRestrictedSids,
            TokenSessionId,
            TokenGroupsAndPrivileges,
            TokenSessionReference,
            TokenSandBoxInert,
            TokenAuditPolicy,
            TokenOrigin
        }

        enum TokenType
        {
            TokenPrimary = 1,
            TokenImpersonation
        }

        enum SecurityImpersonationLevel
        {
            SecurityAnonymous,
            SecurityIdentification,
            SecurityImpersonation,
            SecurityDelegation
        }

        [StructLayout(LayoutKind.Sequential)]
        struct TokenStatistics
        {
            public Int64 TokenId;
            public Int64 AuthenticationId;
            public Int64 ExpirationTime;
            public TokenType TokenType;
            public SecurityImpersonationLevel ImpersonationLevel;
            public Int32 DynamicCharged;
            public Int32 DynamicAvailable;
            public Int32 GroupCount;
            public Int32 PrivilegeCount;
            public Int64 ModifiedId;
        }

        struct TokenOrigin
        {
            public Int64 OriginatingLogonSession;
        }

        [DllImport("advapi32.dll", EntryPoint = "GetTokenInformation", SetLastError = true)]
        static extern bool GetTokenInformation(
            IntPtr tokenHandle,
            TokenInformationClass tokenInformationClass,
            IntPtr tokenInformation,
            int tokenInformationLength,
            out int ReturnLength);

        public const int ERROR_INSUFFICIENT_BUFFER = 0x7a;


        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern int WTSQueryUserToken(UInt32 sessionId, out IntPtr Token);


        [DllImport("advapi32")]
        static extern bool OpenProcessToken(
            IntPtr ProcessHandle, // handle to process
            int DesiredAccess, // desired access to process
            ref IntPtr TokenHandle // handle to open access token
        );

        [DllImport("kernel32")]
        static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32")]
        static extern bool CloseHandle(IntPtr handle);


    }
}
