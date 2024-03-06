using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace DO_AN_CUA_HAN.Barcode
{
    public partial class Detection : Form
    {
        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice FinalFrame;
        public int mabenhnhan { get; set; }
        public Detection()
        {
            InitializeComponent();
        
        }
     

        private void Barcode_Load(object sender, EventArgs e)
        {
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach(FilterInfo Device in CaptureDevice)
            {
                comboBox1.Items.Add(Device.Name);
            }    
            comboBox1.SelectedIndex = 0;
            FinalFrame= new VideoCaptureDevice();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                BarcodeReader Reader = new BarcodeReader();

                Result result = Reader.Decode((Bitmap)pictureBox1.Image);
                try
                {
                    string decoded = result.Text; 
                    if (decoded != null)
                    {


                        timer1.Stop();
                        DialogResult ketqua = MessageBox.Show("Nhận dạng thành công!", "Thong Bao", MessageBoxButtons.OK);
                        int.TryParse(decoded, out int mabenhnhan);

                        Form form2 = new Info(mabenhnhan);
                        form2.ShowDialog();
                  
                        this.Close();



                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs e)
        {
          
            pictureBox1.Image = (Bitmap)e.Frame.Clone();
          
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(FinalFrame.IsRunning==true)
            {
                FinalFrame.Stop();
               
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
          
        }

        private void buttonChoose_Click(object sender, EventArgs e)
        {
            FinalFrame = new VideoCaptureDevice(CaptureDevice[comboBox1.SelectedIndex].MonikerString);
            FinalFrame.NewFrame += new AForge.Video.NewFrameEventHandler(FinalFrame_NewFrame);
            FinalFrame.Start();
            timer1.Enabled = true;
            timer1.Start();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
