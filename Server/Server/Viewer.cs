using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Server
{
    public partial class Viewer : Form
    {
        public Dictionary<string, Student> clientsList;

        private static FullViewer fullViewer;

        private static Timer timer;

        private static EventHandler showAll;
        private static EventHandler showFull;

        private static String selectedKey;

        public delegate void InsertPanelDelegate(String key);
        public InsertPanelDelegate IPD;

        public delegate void DeletePanelDelegate(Panel pan);
        public DeletePanelDelegate DPD;

        public Viewer()
        {
            InitializeComponent();

            clientsList = new Dictionary<string, Student>(); // 클라이언트 리스트

            IPD = new InsertPanelDelegate(InsertStudentPanel);
            DPD = new DeletePanelDelegate(DeleteStudentPanel);

            showAll = new EventHandler(IterateShowViews);
            showFull = new EventHandler(IterateFocusView);

            timer = new Timer();
            timer.Tick += showAll;
            timer.Interval = 300;
        }

        private void Viewer_Load(object sender, EventArgs e)
        {
            selectedKey = null;

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
                        String str = box.Name.Split('_')[2];
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
            Panel panClient = new Panel
            {
                Name = $"pan_{key}"
            };

            Label lblClient = new Label
            {
                Width = 160,
                Height = 20,
                Location = new Point(5, 30),
                Text = clientsList[key].info // 학번(이름)
            };

            PictureBox pbClient = new PictureBox
            {
                Name = $"pb_{key}_{clientsList[key].info}",
                Width = 160,
                Height = 120,
                Location = new Point(5, 50),
                BackColor = Color.Black,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = clientsList[key].img // 캡처 이미지
            };

            void customMouseEvent(object sender, MouseEventArgs e) => GetPicture(pbClient, e);
            void customEvent(object sender, EventArgs e) => FullPicture(key);

            pbClient.MouseDown += customMouseEvent;
            pbClient.DoubleClick += customEvent;

            panClient.Controls.Add(lblClient);
            panClient.Controls.Add(pbClient);

            panClient.Size = new Size(170, 170);
            panClient.Location = new Point(0, 0);

            return panClient;
        }

        private void IterateShowViews(object sender, EventArgs e)
        {
            try
            {
                List<string> list = new List<string>(clientsList.Keys);

                foreach (string key in list)
                {
                    if (clientsList.ContainsKey(key))
                    {
                        Control[] ctrl = this.Controls.Find($"pb_{key}_{clientsList[key].info}", true);
                        if (ctrl.Length > 0)
                        {
                            PictureBox pb = ctrl[0] as PictureBox;
                            pb.Image = clientsList[key].img;
                        }
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
                fullViewer.AccessControl(clientsList[selectedKey].img, clientsList[selectedKey].info);
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

            List<string> list = new List<string>(clientsList.Keys);
            if (list.Count == 0)
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

                        foreach (string key in list)
                        {
                            if (clientsList.ContainsKey(key))
                            {
                                Control[] ctrl = this.Controls.Find($"pb_{key}_{clientsList[key].info}", true);
                                if (ctrl.Length > 0)
                                {
                                    PictureBox pb = ctrl[0] as PictureBox;
                                    pb.Image = clientsList[key].img;

                                    String str = pb.Name.Split('_')[2];
                                    Bitmap bmp = new Bitmap(pb.Image);
                                    bmp.Save($"{filePath}\\{str}.png", System.Drawing.Imaging.ImageFormat.Png);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void InsertStudentPanel(String key)
        {
            this.clientsViewPanel.Controls.Add(AddClientPanel(key));
        }

        public void DeleteStudentPanel(Panel pan)
        {
            this.clientsViewPanel.Controls.Remove(pan);
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