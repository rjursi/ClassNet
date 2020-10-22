using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Server
{
    public partial class Viewer : Form
    {
        public static int pastClientsCount;
        public static int currentClientsCount; // 현재 접속 클라이언트 수

        public static Test test;

        public static Dictionary<string, Student> clientsList;
        public static Dictionary<string, PictureBox> clientsPicture;

        public String dir;
        public class Student
        {
            public string info;
            public Image img;
        }

        public static Timer renderingTimer;
        public static Timer focusingTimer;

        public Viewer()
        {
            InitializeComponent();

            pastClientsCount = 0;
            currentClientsCount = 0;

            clientsList = new Dictionary<string, Student>(); // 클라이언트 리스트
            clientsPicture = new Dictionary<string, PictureBox>(); // 클라이언트 이미지

            renderingTimer = new Timer();
            renderingTimer.Tick += new EventHandler(IterateShowViews);
            renderingTimer.Interval = 500;
            renderingTimer.Start();

            focusingTimer = new Timer();
           

        }
        public Button allSaveBtnCreate()
        {
            Button btn = new Button
            {
                Name = "btnAllSave",
                Text = "전체 저장",
            }; 
            btn.Click += new EventHandler(btnAllSave_Click);

            return btn;
        }
        public void getPicture(PictureBox box, MouseEventArgs e)
        {
                var filePath = string.Empty;
                if (e.Button == MouseButtons.Right)
                {
                    using (FolderBrowserDialog fileDialog = new FolderBrowserDialog())
                    {
                        fileDialog.SelectedPath = "C:\\";

                        if (fileDialog.ShowDialog() == DialogResult.OK)
                        {
                            filePath = fileDialog.SelectedPath;
                        }
                    }

                    Bitmap bmp = new Bitmap(box.Image);
                    String str = box.Name.ToString().Trim('\0');
                    Console.WriteLine($"{filePath}{str}.png");
                    bmp.Save($"{filePath}{str}.png", System.Drawing.Imaging.ImageFormat.Png);
                }
        }
        public void fullPicture(Student sendStu)
        {
            focusingTimer.Tick += new EventHandler((sender, e) => InterateFocusView(sendStu.img));
            focusingTimer.Interval = 500;
            focusingTimer.Start();

            test = new Test(sendStu);
            test.ShowDialog();

            renderingTimer.Interval = 700;
        }
        public Panel AddClientPanel(string key)
        {
            Student stu = clientsList[key] as Student;

            Panel panClient = new Panel();

            Label lblClient = new Label
            {
                Width = 160,
                Height = 20,
                Location = new Point(5, 8),
                Text = stu.info // 학번(이름)
            };

            PictureBox pbClient = new PictureBox
            {
                Name = stu.info, 
                Width = 160,
                Height = 120,
                Location = new Point(5, 30),
                BackColor = Color.Black,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = stu.img // 캡처 이미지
            };
            MouseEventHandler customMouseEvent = (sender, e) => getPicture(pbClient, e);
            EventHandler customEvent = (sender, e) => fullPicture(stu);
            pbClient.MouseDown += customMouseEvent;
            pbClient.DoubleClick += customEvent;

            if (clientsPicture.ContainsKey(key))
            {
                clientsPicture[key] = pbClient;
            }
            else
            {
               clientsPicture.Add(key, pbClient);
            }

            panClient.Controls.Add(lblClient);
            panClient.Controls.Add(pbClient);

            panClient.Size = new Size(170, 170);
            panClient.Location = new Point(0, 0);

            return panClient;
        }

        private void IterateShowViews(object sender, EventArgs e)
        {

            if (pastClientsCount == currentClientsCount)
            {
                foreach (string key in clientsPicture.Keys)
                {
                    Student stu = clientsList[key] as Student;
                    clientsPicture[key].Image = stu.img;
                }
            }
            else if (pastClientsCount < currentClientsCount)
            {
                pastClientsCount++;

                clientsViewPanel.Controls.Clear();
                clientsViewPanel.Controls.Add(allSaveBtnCreate()); //위치를 어떻게 할 것인지 생각해보자!
                foreach (string key in clientsList.Keys)
                {
                    clientsViewPanel.Controls.Add(AddClientPanel(key));
                }
            }
            else if (pastClientsCount > currentClientsCount)
            {
                pastClientsCount--;

                clientsViewPanel.Controls.Clear();
                clientsViewPanel.Controls.Add(allSaveBtnCreate());
                foreach (string key in clientsList.Keys)
                {
                   clientsViewPanel.Controls.Add(AddClientPanel(key));
                }
            }
        }

        private void InterateFocusView(Image sendImg)
        {
            //어떻게 처리하지?
            Test.focusStudent.img = sendImg;
        }

        private void btnAllSave_Click(object sender, EventArgs e)
        {
           
                var filePath = string.Empty;
                if (pastClientsCount == 0)
                {
                    MessageBox.Show("접속한 학습자가 없습니다!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    using (FolderBrowserDialog fileDialog = new FolderBrowserDialog())
                    {
                        fileDialog.SelectedPath = "C:\\";

                        if (fileDialog.ShowDialog() == DialogResult.OK)
                        {
                            filePath = fileDialog.SelectedPath;
                        }
                    }
                    dir = filePath;

                    foreach (string key in clientsPicture.Keys)
                    {
                        String str = clientsPicture[key].Name;
                        Bitmap bmp = new Bitmap(clientsPicture[key].Image);
                        bmp.Save($"{filePath}{str}" + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    }
                }

        }
    }
}
