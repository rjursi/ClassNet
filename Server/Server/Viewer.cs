using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Server
{
    public partial class Viewer : Form
    {
        public static int oldClientsCount;
        public static int currentClientsCount; // 현재 접속 클라이언트 수

        public static Hashtable connectedClientsList;

        public static Timer renderingTimer;

        public Viewer()
        {
            InitializeComponent();

            oldClientsCount = 0;

            connectedClientsList = new Hashtable(); // 클라이언트 리스트

            renderingTimer = new Timer();
            renderingTimer.Tick += new EventHandler(IterateShowViews);
            renderingTimer.Interval = 1000;
            renderingTimer.Start();
        }

        private void ClientsView_Load(object sender, EventArgs e)
        {

        }

        public Panel AddClientPanel(string key)
        {
            Panel panClient = new Panel();

            Label lblClient = new Label
            {
                Text = connectedClientsList[key].ToString(), // 학번(이름)
                Width = 160,
                Height = 20,
                Location = new Point(5, 8)
            };

            PictureBox pbClient = new PictureBox
            {
                Width = 160,
                Height = 120,
                Location = new Point(5, 30),
                BackColor = Color.Black
            };

            panClient.Controls.Add(lblClient);
            panClient.Controls.Add(pbClient);

            panClient.Size = new Size(170, 170);
            panClient.Location = new Point(0, 0);

            return panClient;
        }

        private void IterateShowViews(object sender,EventArgs e)
        {

            if (oldClientsCount == currentClientsCount)
            { 
                
            }
            else if(oldClientsCount < currentClientsCount)
            {
                oldClientsCount++;

                clientsViewPanel.Controls.Clear();
                foreach (string key in connectedClientsList.Keys)
                {
                    clientsViewPanel.Controls.Add(AddClientPanel(key));
                }
            }
            else if (oldClientsCount > currentClientsCount)
            {
                oldClientsCount--;

                clientsViewPanel.Controls.Clear();
                foreach (string key in connectedClientsList.Keys)
                {
                    clientsViewPanel.Controls.Add(AddClientPanel(key));
                }
            }
        }

    }
}
