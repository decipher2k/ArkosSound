using FMUtils.KeyboardHook;
using NAudio.Midi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace ArkosSound
{
    public partial class Form1 : Form
    {
        Process python = new Process();
        StreamReader sr = null;
        StreamWriter si = null;
        MidiOut midiOut = new MidiOut(0);
        int lastNote = 0;
        bool cancel=false;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        FMUtils.KeyboardHook.Hook hook = new FMUtils.KeyboardHook.Hook("hook");
        bool free = true;
        String loading = "";

        public static Form1 self;

        bool Exit { set
            {
                Application.Exit();
            } }
        public Form1()
        {
            InitializeComponent();
            self = this;
            splash.Show();
            new System.Threading.Thread(initPython).Start();
           
            hook.KeyDownEvent += keyDown;            
        }

        private void keyDown(KeyboardHookEventArgs e)
        {
           // hook.isPaused = true;
            if(free)
            {
                free = false;
                if (e.Key == Keys.Down)
                {
                    String ret = "";
                    int count = 0;
                    do
                    {
                        count++;
                        if (count > 3)
                        {
                            free = true;
                            return;
                        }
                        si.WriteLine("a");
                        ret = sr.ReadLine();

                    } while (int.Parse(ret) >= lastNote && lastNote != 0);
                    lastNote = int.Parse(ret);
                    //String ret = sr.ReadToEnd().Split('\n')[sr.ReadToEnd().Split('\n').Length - 1];
                    playNote(int.Parse(ret));
                }
                else if (e.Key == Keys.Up)
                {
                    String ret = "";
                    int count = 0;
                    do
                    {
                        count++;
                        if (count > 3)
                        {
                            free = true;
                            return;
                        }
                        ret = "";
                        si.WriteLine("d");
                        ret = sr.ReadLine();
                    } while (int.Parse(ret) <= lastNote && lastNote != 0);
                    lastNote = int.Parse(ret);
                    //String ret = sr.ReadToEnd().Split('\n')[sr.ReadToEnd().Split('\n').Length - 1];
                    playNote(int.Parse(ret));
                }
                else if(e.Key==Keys.Right)
                    playNote(lastNote);
            }
            free = true;
            // hook.isPaused = false;
        }

        public void playNote(int note)
        {
            //Thread.Sleep inside GUI is just for example
            
            {
                midiOut.Volume = 65535;
                midiOut.Send(MidiMessage.StopNote(60, 0, 1).RawData);
                midiOut.Send(MidiMessage.StartNote(note, 127, 1).RawData);                
                //Thread.Sleep(1000);
              
            }
        }
        Splash splash = new Splash();
        public void initPython(object param=null)
        {

            loading = "";
            String pyFile = "test.py";
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                Arguments = pyFile,
                FileName = "python.exe",
                RedirectStandardOutput = true,
                RedirectStandardInput = true
            };
            if(param!=null)
            {
                psi.Arguments = (String)param;
            }
            python.StartInfo = psi;
            try
            {
                python.Start();
                if (python.HasExited)
                {
                    cancel = true;
                    Form1.self.Invoke(new Action(() => { Form1.self.WindowState = FormWindowState.Minimized; }));
                    splash.Invoke(new Action(() => { splash.Close(); }));
                    Form1.self.Invoke(new Action(() => { MessageBox.Show("ArkosSound requires Python to run."); Form1.self.Invoke(new Action(() => { })); }));
                    Process.GetCurrentProcess().Kill();
                    return;
                }
            }
            catch (Exception ex)
            {
                cancel = true;
                Form1.self.Invoke(new Action(() => { Form1.self.WindowState = FormWindowState.Minimized; }));
                splash.Invoke(new Action(() => { splash.Close(); }));
                Form1.self.Invoke(new Action(() => { MessageBox.Show("ArkosSound requires Python to run."); Form1.self.Invoke(new Action(() => { })); }));
                Process.GetCurrentProcess().Kill();
                return;
            }



            sr = python.StandardOutput;
            si = python.StandardInput;

            while (!loading.Contains("RDY"))
            {
                loading += sr.ReadLine();
                System.Threading.Thread.Sleep(100);
            }
      
            splash.Invoke(new Action(() => splash.Close()));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String ret = "";
            do
            {
                si.WriteLine("d");
                ret = sr.ReadLine();
            } while (ret!=null && int.Parse(ret) <= lastNote && lastNote!=0);
            if(ret!=null)
                lastNote = int.Parse(ret);
            //String ret = sr.ReadToEnd().Split('\n')[sr.ReadToEnd().Split('\n').Length - 1];
            playNote(int.Parse(ret));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String ret = "";
            do
            {
                si.WriteLine("a");
                ret = sr.ReadLine();
            } while (ret != null && int.Parse(ret) >= lastNote && lastNote!=0);
            lastNote = int.Parse(ret);
            //String ret = sr.ReadToEnd().Split('\n')[sr.ReadToEnd().Split('\n').Length - 1];
            playNote(int.Parse(ret));
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if(python.HasExited==false)
                    python.Kill();
            }catch (Exception ex)
            {
                Form1_FormClosing(sender, e);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void button1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void button2_KeyPress(object sender, KeyPressEventArgs e)
        {
          
        }

        private void button1_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            playNote(lastNote);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Init dlg=new Init();
            String param= "";
            if(dlg.ShowDialog()==DialogResult.OK)
            {
                List<int> list = dlg.ret;
                param = "test.py";
                for (int i=0;i<list.Count;i++)
                {
                    param += " "+list[i];
                }
                param = param.Trim();
                
                python.Kill();
                splash = new Splash();
                splash.Show();
                new System.Threading.Thread(new ParameterizedThreadStart(initPython)).Start(param);
            }
        }
    }
}
