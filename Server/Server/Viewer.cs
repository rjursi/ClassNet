using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Server
{
    public partial class Viewer : Form
    {
        public int pastClientsCount;
        public int currentClientsCount; // 현재 접속 클라이언트 수

        public Dictionary<string, Student> clientsList;

        private static FullViewer fullViewer;

        private static Timer timer;

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

            timer = new Timer();
            timer.Tick += showAll;
            timer.Interval = 500;
        }

        private void Viewer_Load(object sender, EventArgs e)
        {
            selectedKey = null;
            ctrl = null;
            stu = null;

            fullViewer = new FullViewer();
            fullViewer.FormClosed += new FormClosedEventHandler(FullViewer_FormClosed);

            timer.Start();
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

            timer.Tick -= showAll;
            timer.Tick += showFull;
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

                list.Sort();

                if (pastClientsCount == currentClientsCount)
                {
                    foreach (string key in list)
                    {
                        if (clientsList.ContainsKey(key))
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
                }
                else if (pastClientsCount < currentClientsCount)
                {
                    ++pastClientsCount;

                    clientsViewPanel.Controls.Clear();
                    foreach (string key in list)
                    {
                        if (clientsList.ContainsKey(key)) clientsViewPanel.Controls.Add(AddClientPanel(key));
                    }
                }
                else if (pastClientsCount > currentClientsCount)
                {
                    --pastClientsCount;

                    clientsViewPanel.Controls.Clear();
                    foreach (string key in list)
                    {
                        if (clientsList.ContainsKey(key)) clientsViewPanel.Controls.Add(AddClientPanel(key));
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
            if (clientsList.ContainsKey(selectedKey))
            {
                stu = clientsList[selectedKey] as Student;
                fullViewer.AccessControl(stu.img, stu.info);
                stu = null;
            }
            else
            {
                timer.Stop();

                MessageBox.Show("해당 학습자와 연결이 끊겼습니다.", "전체 화면 오류",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                fullViewer.Close();

                timer.Start();
            }
        }

        private void BtnAllSave_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;
            if (pastClientsCount == 0)
            {
                MessageBox.Show("접속한 학생이 없습니다.", "화면 캡처 오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (FolderBrowserDialog fileDialog = new FolderBrowserDialog())
                {
                    fileDialog.SelectedPath = "C:\\";

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        filePath = fileDialog.SelectedPath;

                        List<string> list = new List<string>(clientsList.Keys);
                        foreach (string key in list)
                        {
                            if (clientsList.ContainsKey(key))
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
        }

        private void FullViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Tick -= showFull;
            timer.Tick += showAll;
            this.Opacity = 1;

            selectedKey = null;
        }
    }
}
