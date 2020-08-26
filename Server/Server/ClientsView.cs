using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Server
{
    public partial class ClientsView : Form
    {
        
        public static int currentClientCount; //접속 클라이언트 수
        public static Dictionary<Socket, string> connectedClientList = new Dictionary<Socket, string>(); //클라이언트 리스트

        public static int oldClientCount;
        
        public static Point initialViewLocationPoint=new Point(0,0);
        public static Point initialLabelLocationPoint = new Point(0, 0);
        public static Timer renderingTimer = new Timer();

        public ClientsView()
        {
            InitializeComponent();

            //this.Load += ClientsView_Load;
            renderingTimer.Tick += new EventHandler(IterateShowViews);
            renderingTimer.Interval = 1000;
            renderingTimer.Start();
        }

        //리스트에 담아서 화면을 뿌려주자!
        public PictureBox AddClientView() //재귀가 좋을까? 반복문이 좋을까? 
        {
            PictureBox clientView = new PictureBox();

            clientView.Width = 150;
            clientView.Height = 115;

            initialViewLocationPoint.X += 20 + clientView.Width;
            if(oldClientCount%5==0) initialViewLocationPoint.Y += 50 + clientView.Height;
            
            clientView.Location = initialViewLocationPoint;
            clientView.BackColor = Color.Purple;
            //Console.WriteLine($"View Point {initialViewLocationPoint.X} : {initialViewLocationPoint.Y}");

            return clientView;
        }

        public Label AddClientLabel()
        {
            Label clientLabel = new Label();
            clientLabel.Text = "123456789"; //학번

            initialLabelLocationPoint.X = initialViewLocationPoint.X;
            initialLabelLocationPoint.Y = (initialViewLocationPoint.Y - 10);

            clientLabel.Location = initialLabelLocationPoint;
            //Console.WriteLine($"Label Point {initialLabelLocationPoint.X} : {initialLabelLocationPoint.Y}");

            return clientLabel;
        }

        private void IterateShowViews(object sender,EventArgs e)
        {

            if (oldClientCount == currentClientCount) { 
                
            }
            else if(oldClientCount < currentClientCount){
                //1)currentClientCount가 더 클 경우 Add 
                oldClientCount++;

                //FlowLayoutPanel이라 위 아래로 나오지 않는 상황이 발생하는 듯함
                //다른 방식의 Panel이나 다른 방법을 모색해 보자!
                clientsViewPanel.Controls.Add(AddClientView());
                clientsViewPanel.Controls.Add(AddClientLabel());
            }
            else if (oldClientCount > currentClientCount)
            {
                //지우게 될 경우 eventHandler를 제거하고 Remove를 해주어야 완벽히 삭제가 된다.
                //2)currentClientCount가 더 작을 경우 Remove? Remove는 대상의 전체를 지움
                //그렇기에 특정 대상만을 지울 수 있는 메서드가 필요
            }

            // Console.WriteLine($"이벤트 도는 중, 현재 수 : {currentClientCount}");
        }
        //해야할 것 Client의 코드를 풀어서 Server에 맞게 만들기
        //Server의 코드를 Client에 맞게 만들기
        //그래야 이미지가 불러들어 갈 수 있음

        private void ClientsView_Load(object sender, EventArgs e)
        {
            //AddClientView();
            //this.Load += IterateShowViews;
        }

        private void ClientsView_Resize(object sender, EventArgs e)
        {
            //this.Text = $"Width : {this.Width}, Height : {this.Height}";
        }
    }
}
