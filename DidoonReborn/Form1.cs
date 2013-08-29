using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;
using System.Threading;

namespace DidoonReborn
{
    public partial class Form1 : Form
    {
        public ScriptController controller;

        public Form1()
        {
            controller = new ScriptController();
            controller.form = this;
            InitializeComponent();
        }

        private void controlButton_Click(object sender, EventArgs e)
        {
            // Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Debug.WriteLine("Form1.button_Click");

            if (this.controller.isRunning == false)
            {
                if (this.controller.run())
                {
                    this.controlButton.Text = "Stop";
                }
                else
                {
                    this.controlButton.Text = "Start";
                }
            }
            else
            {
                if (this.controller.stop())
                {
                    this.controlButton.Text = "Start";
                }
                else
                {
                    this.controlButton.Text = "Stop";
                }
            }
        }



        private delegate void SetTextCallback(String text);

        public void setStatusText(String text)
        {
            new Thread(new ThreadStart(delegate
            {
                setStatusTextImpl(text);
            })).Start();
        }

        public void setStatusTextImpl(String text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.statusLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(setStatusTextImpl);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.statusLabel.Text = text;
            }
        }
    }
}
