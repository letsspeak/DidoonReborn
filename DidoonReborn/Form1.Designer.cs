using System;
using System.Drawing;
using System.Windows.Forms;

using System.Diagnostics;

using System.Runtime.InteropServices;
using System.Text;

namespace DidoonReborn
{
    partial class Form1
    {
        Button button;

        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = "DidoonBorn - beta";


            button = new Button()
            {
                Text = "Start",
                Location = new Point(10, 10),
                Size = new Size(160, 40),
            };
            button.Click += new EventHandler(button_Click);
            this.Controls.Add(button);

        }

        #endregion

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

        void button_Click(object sender, EventArgs e)
        {
            // Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Debug.WriteLine("Form1.button_Click");

            if (isFF14LauncherBooted() == false)
            {
                button.Text = "Stop";
                Process.Start(@"D:\Program Files\SquareEnix\FINAL FANTASY XIV - A Realm Reborn\boot\ffxivboot.exe");
            }

        }

        bool isFF14LauncherBooted()
        {
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
                    return true;
                }
            }
            return false;
        }









    }
}

