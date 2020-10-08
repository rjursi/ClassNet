using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Server
{
    public partial class Viewer : Form
    {
        public static int pastClientsCount;
        public static int currentClientsCount; // 현재 접속 클라이언트 수

        public static Dictionary<string, Student> clientsList;

        public static Dictionary<string, PictureBox> clientsPicture;

        public class Student
        {
            public string info;
            public Image img;
        }

        public static Timer renderingTimer;

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
                Width = 160,
                Height = 120,
                Location = new Point(5, 30),
                BackColor = Color.Black,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = stu.img // 캡처 이미지
            };

            clientsPicture.Add(key, pbClient);

            panClient.Controls.Add(lblClient);
            panClient.Controls.Add(pbClient);

            panClient.Size = new Size(170, 170);
            panClient.Location = new Point(0, 0);

            return panClient;
        }

        private void IterateShowViews(object sender,EventArgs e)
        {

            if (pastClientsCount == currentClientsCount)
            {
                foreach (string key in clientsPicture.Keys)
                {
                    Student stu = clientsList[key] as Student;
                    clientsPicture[key].Image = stu.img;
                }
            }
            else if(pastClientsCount < currentClientsCount)
            {
                pastClientsCount++;

                clientsViewPanel.Controls.Clear();
                foreach (string key in clientsList.Keys)
                {
                    clientsViewPanel.Controls.Add(AddClientPanel(key));
                }
            }
            else if (pastClientsCount > currentClientsCount)
            {
                pastClientsCount--;

                clientsViewPanel.Controls.Clear();
                foreach (string key in clientsList.Keys)
                {
                    clientsViewPanel.Controls.Add(AddClientPanel(key));
                }
            }
        }
    }
}
