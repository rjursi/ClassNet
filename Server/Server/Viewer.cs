using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Server
{
    public partial class Viewer : Form
    {
        public class Student
        {
            public string info;
            public Image img;
        }

        public static int pastClientsCount;
        public static int currentClientsCount; // 현재 접속 클라이언트 수

        public static FullViewer fullViewer;

        public static Dictionary<string, Student> clientsList;

        public String dir;

        public static Timer renderingTimer;

        private static EventHandler showAll;
        private static EventHandler showFull;

        private static String selectedKey;
        private static Control[] ctrl;
        private static Student stu;

        public Viewer()
        {
            InitializeComponent();

            pastClientsCount = 0;
            currentClientsCount = 0;

            clientsList = new Dictionary<string, Student>(); // 클라이언트 리스트

            showAll = new EventHandler(IterateShowViews);
            showFull = new EventHandler(IterateFocusView);

            renderingTimer = new Timer();
            renderingTimer.Tick += showAll;
            renderingTimer.Interval = 500;
            renderingTimer.Start();
        }

        private void Viewer_Load(object sender, EventArgs e)
        {
            selectedKey = null;
            ctrl = null;
            stu = null;

            fullViewer = new FullViewer();
            fullViewer.FormClosed += new FormClosedEventHandler(FullViewer_FormClosed);
        }

        public void GetPicture(PictureBox box, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                using (FolderBrowserDialog fileDialog = new FolderBrowserDialog())
                {
                    fileDialog.SelectedPath = "C:\\";

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = fileDialog.SelectedPath;

                        Bitmap bmp = new Bitmap(box.Image);
                        String str = box.Name.ToString().Trim('\0');
                        bmp.Save($"{filePath}\\{str}.png", System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
        }

        public void FullPicture(String key)
        {
            selectedKey = key;

            renderingTimer.Tick -= showAll;
            renderingTimer.Tick += showFull;
            this.Opacity = 0;

            fullViewer.ShowDialog();
        }

        public Panel AddClientPanel(string key)
        {
            stu = clientsList[key] as Student;

            Panel panClient = new Panel();

            Label lblClient = new Label
            {
                Width = 160,
                Height = 20,
                Location = new Point(5, 30),
                Text = stu.info // 학번(이름)
            };

            PictureBox pbClient = new PictureBox
            {
                Name = stu.info, 
                Width = 160,
                Height = 120,
                Location = new Point(5, 50),
                BackColor = Color.Black,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = stu.img // 캡처 이미지
            };

            void customMouseEvent(object sender, MouseEventArgs e) => GetPicture(pbClient, e);
            void customEvent(object sender, EventArgs e) => FullPicture(key);

            pbClient.MouseDown += customMouseEvent;
            pbClient.DoubleClick += customEvent;

            panClient.Controls.Add(lblClient);
            panClient.Controls.Add(pbClient);

            panClient.Size = new Size(170, 170);
            panClient.Location = new Point(0, 0);

            stu = null;

            return panClient;
        }

        private void IterateShowViews(object sender, EventArgs e)
        {
            try
            {
                List<string> list = new List<string>(clientsList.Keys);

                if (pastClientsCount == currentClientsCount)
                {
                    foreach (string key in clientsList.Keys)
                    {
                        stu = clientsList[key] as Student;

                        ctrl = this.Controls.Find(stu.info, true);
                        if (ctrl.Length > 0)
                        {
                            PictureBox pb = ctrl[0] as PictureBox;
                            pb.Image = stu.img;
                        }
                        ctrl = null;

                        stu = null;
                    }
                }
                else if (pastClientsCount < currentClientsCount)
                {
                    ++pastClientsCount;

                    clientsViewPanel.Controls.Clear();
                    foreach (string key in list)
                    {
                        clientsViewPanel.Controls.Add(AddClientPanel(key));
                    }
                }
                else if (pastClientsCount > currentClientsCount)
                {
                    --pastClientsCount;

                    clientsViewPanel.Controls.Clear();
                    foreach (string key in list)
                    {
                        clientsViewPanel.Controls.Add(AddClientPanel(key));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void IterateFocusView(object sender, EventArgs e)
        {
            stu = clientsList[selectedKey] as Student;
            fullViewer.AccessControl(stu.img, stu.info);
            stu = null;
        }

        private void BtnAllSave_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;
            if (pastClientsCount == 0)
            {
                MessageBox.Show("접속한 학생이 없습니다.", "화면 캡처 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (FolderBrowserDialog fileDialog = new FolderBrowserDialog())
                {
                    fileDialog.SelectedPath = "C:\\";

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        filePath = fileDialog.SelectedPath;

                        foreach (string key in clientsList.Keys)
                        {
                            stu = clientsList[key] as Student;

                            ctrl = this.Controls.Find(stu.info, true);
                            if (ctrl.Length > 0)
                            {
                                PictureBox pb = ctrl[0] as PictureBox;
                                pb.Image = stu.img;

                                String str = pb.Name.ToString().Trim('\0');
                                Bitmap bmp = new Bitmap(pb.Image);
                                bmp.Save($"{filePath}\\{str}.png", System.Drawing.Imaging.ImageFormat.Png);
                            }
                            ctrl = null;

                            stu = null;
                        }
                    }
                }                
            }
        }

        private void FullViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            renderingTimer.Tick -= showFull;
            renderingTimer.Tick += showAll;
            this.Opacity = 1;

            selectedKey = null;
        }
    }
}
