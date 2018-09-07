using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

/***************************************/
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System.Diagnostics;
using AForge.Video.FFMPEG;
using AForge.Controls;
namespace USB_Camera
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection videoDevices;
        private  VideoCaptureDevice videoSource;
        private  VideoFileWriter videoWriter;
        private  Bitmap bmp1;
        bool video_flag = false;
        public Form1()
        {
            InitializeComponent();
        }
        //窗体加载
        private void Form1_Load(object sender, EventArgs e)
        {


            try
            {
                // 枚举所有视频输入设备
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);//实例化对象

                if (videoDevices.Count == 0)
                    throw new ApplicationException();

                foreach (FilterInfo device in videoDevices)//如果,检测到视频输入设备
                {
                    comboBox1.Items.Add(device.Name);//显示视频设备名称
                }

                   comboBox1.SelectedIndex = 0;
            }
            catch (ApplicationException)//没有检测到视频输入设备
            {
                comboBox1.Items.Add("No local capture devices");
                videoDevices = null;
            }

    


        }
        //连接摄像头
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "连接")
            {

                try
                {
            

                    if (videoDevices != null)
                    {
                        button1.Text = "断开";
                      
                        Open_Camrea();
                  
                    }

                }
                catch
                {

                    MessageBox.Show("连接失败");


                }

            }
            else if (button1.Text == "断开")
            {
                try
                {

                    button1.Text = "连接";
                    Close_Camrea();
                    comboBox1.Text = "";


                }
                catch
                {


                    MessageBox.Show("断开失败");
                }


            }
        }
        private void Open_Camrea()
            {
              videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);//实例化对象

            videoSourcePlayer2.VideoSource = videoSource;
  
            videoSourcePlayer2.Start();
       

            }
   
        //窗体关闭事件
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            Close_Camrea();//关闭摄像头
             Process p = Process.GetCurrentProcess();//获取进程句柄
            if (p.HasExited == false)//判断进程是否关闭,若没有关闭则杀死进程
            {
                p.Kill();
            }
        }
        //拍照
        private void button3_Click(object sender, EventArgs e)
         {
            Time time = new Time();

            try
            {
                if (this.videoSource.IsRunning && this.videoSourcePlayer2.IsRunning)
                {
                    Bitmap bitmap = this.videoSourcePlayer2.GetCurrentVideoFrame();
                    string bit_name = Path.Combine("./image/", time.GetTime() + ".jpeg");
                  
                    bitmap.Save(bit_name, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitmap.Dispose();
                }
                else
                    MessageBox.Show("摄像头没有运行", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);



            }
            catch
            {

                MessageBox.Show("摄像头没有运行", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
          
        }
        //暂停录像
        private void button5_Click(object sender, EventArgs e)
        {
           

        }
        //录像
        private void button4_Click(object sender, EventArgs e)
        {


            Time time = new Time();
            videoWriter = new VideoFileWriter();
            Bitmap bitmap = videoSourcePlayer2.GetCurrentVideoFrame();
            string video_name = Path.Combine("./video/", time.GetTime() + ".mp4");
            videoWriter.Open(video_name, bitmap.Width, bitmap.Height, 20, VideoCodec.MPEG4);
            bitmap.Dispose();
            video_flag = true;

        }

        //停止录像
        private void button6_Click(object sender, EventArgs e)
        {



            
                videoWriter.Close();
                videoWriter.Dispose();
                video_flag = false;
        }

        private void videoSourcePlayer1_NewFrame(object sender, ref Bitmap image)
        {
      
            if (video_flag==true) { 

              
                videoWriter.WriteVideoFrame(image);

            }

        }


        private void Close_Camrea()
        {

            videoSourcePlayer2.SignalToStop();
            videoSourcePlayer2.WaitForStop();

        }

      
   
    }
    }

