using System.Windows.Forms;

namespace HookerProcess
{
    public partial class form_keyMouseControlling : Form
    {
        Hooker hooker;

        public form_keyMouseControlling()
        {
            InitializeComponent();

            hooker = new Hooker();
        }

        private void form_keyMouseControlling_Load(object sender, System.EventArgs e)
        {
            hooker.SetHook();
        }

        private void form_keyMouseControlling_FormClosed(object sender, FormClosedEventArgs e)
        {
            hooker.UnHook();
        }
    }
}
