using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace FLap_New.CCT_SubForm
{
    public partial class frmRelaxingWebsite : Form
    {
        public frmRelaxingWebsite()
        {
            InitializeComponent();
        }
        private WebView2 webView;
        private async void frmRelaxingWebsite_Load(object sender, EventArgs e)
        {
            webView = new WebView2
            {
                Dock = DockStyle.Fill
            };
            this.Controls.Add(webView);

            await webView.EnsureCoreWebView2Async(null); // Khởi tạo trình duyệt
            webView.Source = new Uri("https://www.youtube.com/");
        }
    }
}
