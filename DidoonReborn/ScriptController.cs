using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;

using System.ComponentModel;
using System.Windows.Forms;
//using mshtml;

namespace DidoonReborn
{
    enum ScriptControllerStatus
    {
        Uninitialized,
        Initialized,
        BootingLauncher,
        LoginingAccount,
        BootingClient,
        WaitingTitle,
        StartingGame,
        LoginingWorld,
        FinishedLogin,
    }

    public class ScriptController
    {
        public bool isRunning;
        private bool stopFlag;
        private ScriptControllerStatus status;

        private System.Timers.Timer timer;

        IntPtr launcherWindowHandle;
        IntPtr ieServerHandle;

        public Form1 form;

        public ScriptController()
        {
            this.isRunning = false;
            this.stopFlag = false;
            this.form = null;
            this.status = ScriptControllerStatus.Uninitialized;
        }

        public bool run()
        {
            Debug.WriteLine("ScriptController.run()");

            if (this.isRunning == true) return false;
            this.isRunning = true;
            this.stopFlag = false;

            this.execute();

            return true;
        }

        public bool stop()
        {
            Debug.WriteLine("ScriptController.stop()");

            this.stopFlag = true;
            return true;
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern uint RegisterWindowMessage(string lpString);

        [Flags]
        public enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0,
            SMTO_BLOCK = 0x1,
            SMTO_ABORTIFHUNG = 0x2,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
            SMTO_ERRORONEXIT = 0x0020
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(
            IntPtr windowHandle,
            uint Msg,
            IntPtr wParam,
            IntPtr lParam,
            SendMessageTimeoutFlags flags,
            uint timeout,
            out IntPtr result);

        [DllImport("oleacc.dll", PreserveSig = false)]
        [return: MarshalAs(UnmanagedType.Interface)]
        static extern object ObjectFromLresult(UIntPtr lResult,
             [MarshalAs(UnmanagedType.LPStruct)] Guid refiid, IntPtr wParam);

        private void execute()
        {
            Debug.WriteLine("ScriptController.execute()");

            if (this.status == ScriptControllerStatus.Uninitialized)
            {
                this.setStatusText("初期化中...");
                this.fireTimer();
                this.status = ScriptControllerStatus.Initialized;
                this.launcherWindowHandle = IntPtr.Zero;

                this.setStatusText("起動チェック中...");
                if (this.isFF14LauncherBooted() == false)
                {
                    this.launchFF14();
                }
                this.setStatusText("ランチャー起動中...");
                this.status = ScriptControllerStatus.BootingLauncher;

                return;
            }

            if (this.status == ScriptControllerStatus.BootingLauncher)
            {
                if (this.launcherWindowHandle == IntPtr.Zero) {
                    // error
                    return;
                }
                // const int EM_SETPASSWORDCHAR = 0xCC;
                // PostMessage(this.launcherWindowHandle, EM_SETPASSWORDCHAR, 0, 0);

                Debug.WriteLine("========================");
                EnumChildWindows(this.launcherWindowHandle, EnumIEServerProc, ref this.ieServerHandle);
                if (this.ieServerHandle == IntPtr.Zero) return;

                Debug.WriteLine("Internet Explorer_Server found!");

                uint msg = RegisterWindowMessage("WM_HTML_GETOBJECT");
                IntPtr result = IntPtr.Zero;
                SendMessageTimeout(this.ieServerHandle, msg, IntPtr.Zero, IntPtr.Zero, SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 1000, out result);
                if (result == IntPtr.Zero) return;

                Debug.WriteLine("result received!");
                //Object spDoc = null;

                Guid IID_IHTMLDocument = new Guid("626FC520-A41E-11CF-A731-00A0C9082637");
                // IHTMLDocument2 document;
//                ObjectFromLresult(result, 
//                If Not lRes = IntPtr.Zero Then
//                    ObjectFromLresult(lRes, IID_IHTMLDocument, 0, spDoc)
//                End If
                
                return;
            }

            return;
        }

        private void fireTimer()
        {
            this.timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Interval = 2000;
            timer.Elapsed += new ElapsedEventHandler(OnTimerEvent);
        }

        private void OnTimerEvent(object source, ElapsedEventArgs e)
        {
            this.execute();
        }

        private void setStatusText(string text)
        {
            this.form.setStatusText(text);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll")]
        static extern int GetWindowRect(IntPtr hWnd, out RECT rect);

        [DllImport("user32.dll", SetLastError = true)]
        static extern
            int GetWindowText(IntPtr hWnd, StringBuilder lpFilename, int nSize);
        
        [DllImport("user32.dll", SetLastError = true)]
        static extern
            int GetClassName(IntPtr hWnd, StringBuilder lpFilename, int nSize);

        bool isFF14LauncherBooted()
        {
            Debug.WriteLine("ScriptController.isFF14LauncherBooted()");

            foreach (Process process in Process.GetProcesses())
            {
                // Debug.WriteLine("process.name:" + process.ProcessName);
                if (process.ProcessName != "ffxivlauncher") continue;

                IntPtr windowHandle = process.MainWindowHandle;
                if (windowHandle == IntPtr.Zero) continue;

                // StringBuilder exePath = new StringBuilder(1024);
                // int exePathLen = GetWindowText(windowHandle, exePath, exePath.Capacity);
                // Debug.WriteLine("windowText:" + exePath);

                RECT rect = new RECT();
                GetWindowRect(windowHandle, out rect);
                // Debug.WriteLine("windowRect:(" + rect.left + ", " + rect.top + ", " + rect.right + ", " + rect.bottom + ")");

                Size size = new Size(rect.right - rect.left, rect.bottom - rect.top);
                Debug.WriteLine("size:" + size);

                if (size.Width == 1024 && size.Height == 604)
                {
                    Debug.WriteLine("Lancher did booted.");
                    this.launcherWindowHandle = windowHandle;
                    return true;
                }
            }
            return false;
        }

        delegate bool WNDENUMPROC(IntPtr hwnd, ref IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern int EnumChildWindows(
            IntPtr hWndParent,
            WNDENUMPROC lpEnumFunc,
            ref IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern int PostMessage(
            IntPtr hwnd,
            int wMsg,
            int wParam,
            int lParam);

        static bool EnumIEServerProc(IntPtr hWndParent, ref IntPtr lParam)
        {
            StringBuilder className = new StringBuilder(1024);
            GetClassName(hWndParent, className, className.Capacity);
            if (className.ToString() == "Internet Explorer_Server")
            {
                lParam = hWndParent;
                return false;
            }

            // EnumChildWindows(hWndParent, EnumIEServerProc, lParam);
            return true;
        }

        void launchFF14 ()
        {
            Process.Start(@"D:\Program Files\SquareEnix\FINAL FANTASY XIV - A Realm Reborn\boot\ffxivboot.exe");
        }

    }
}
