using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LostTeller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool Decode(string str)
        {
            if (Directory.Exists(str))
            {
                if (HasEncrypt(str))
                {
                    DirectoryInfo d = new DirectoryInfo(str);
                    d.MoveTo(str.Substring(0, str.LastIndexOf('.')));                    
                }
                return true;
            }
            else return false;
        }


        private bool HasEncrypt(string str)
        {
            if (str.LastIndexOf(".{2559a1f2-21d7-11d4-bdaf-00c04f60b9f0}") == -1)
                return false;
            else return true;
        }

        private bool Encrypt(string str)
        {
            if (Directory.Exists(str))
            {
                DirectoryInfo d = new DirectoryInfo(str);
                d.MoveTo(d.Parent.FullName + "\\" + d.Name + ".{2559a1f2-21d7-11d4-bdaf-00c04f60b9f0}");
                return true;
            }
            else return false;
        }

        //单文件夹加密或解密
        public bool SingleEncrypt(string path)
        {
            try
            {
                //如果没加密过
                if (!HasEncrypt(path))
                {
                    //如果路径正确
                    if (Encrypt(path))
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else//加密过则解密
                {
                    if (Decode(path))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                ErrInformLog(ex.Message);
                return false;
            }
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            Graphics grap = panel1.CreateGraphics();
            Brush brush = null;
            grap.Clear(Color.AliceBlue);
            
            string[] dirs = e.Data.GetData(DataFormats.FileDrop) as string[];
            foreach (string s in dirs)
            {
                if(SingleEncrypt(s))
                {
                    brush = new SolidBrush(Color.Aqua);
                    grap.DrawString("OK", new Font("楷体", 25), brush, panel1.Width / 2 - 25, panel1.Height / 2 - 20);
                    ErrInformLog("OK");

                }
                else
                {
                    grap.Clear(Color.AliceBlue);
                    brush = new SolidBrush(Color.OrangeRed);
                    grap.DrawString("NO", new Font("楷体", 25), brush, panel1.Width / 2 - 25, panel1.Height / 2 - 20);
                    ErrInformLog("Error!");
                    break;
                }
            }
            brush.Dispose();
            grap.Dispose();
        
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private delegate void ErrInformLogDelegate(string str);
        private void ErrInformLog(string str)
        {
            if (ErrInfo.InvokeRequired)
            {
                ErrInformLogDelegate d = ErrInformLog;
                ErrInfo.Invoke(d, str);
            }
            else
            {
                ErrInfo.Text = str;
            }
        }

       
    }
}
