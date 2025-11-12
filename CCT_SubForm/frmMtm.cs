using FLap_New.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FLap_New.CCT_SubForm
{
    public partial class frmMtm : Form
    {
        static ConnectionToSql conn = new ConnectionToSql("CicsConnectionString");
        static string[] allConnection = conn.GetRawConnectionString();
        static string cicsConnect = allConnection[0];
        public frmMtm()
        {
            InitializeComponent();
        }
        #region StartDate
        private ToolStripDropDown dropDown;
        private MonthCalendar monthCalendar;
        private void ShowCalendar(TextBox targetTextBox, PictureBox picture)
        {
            // Nếu popup đang mở thì đóng lại
            if (dropDown != null && dropDown.Visible)
            {
                dropDown.Close();
                return;
            }

            monthCalendar = new MonthCalendar();
            monthCalendar.MaxSelectionCount = 1; // Chỉ cho chọn 1 ngày
            monthCalendar.DateSelected += (s, e) =>
            {
                targetTextBox.Text = e.Start.ToString("yyyy-MM-dd");
                dropDown.Close(); // Ẩn popup sau khi chọn
            };

            ToolStripControlHost host = new ToolStripControlHost(monthCalendar)
            {
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                AutoSize = false,
                Size = monthCalendar.Size
            };

            dropDown = new ToolStripDropDown
            {
                Padding = Padding.Empty
            };
            dropDown.Items.Add(host);

            // Hiển thị lịch ngay dưới TextBox
            var location = picture.PointToScreen(new Point(0, picture.Height));
            dropDown.Show(location);
        }
        #endregion StartDate
        #region WaterMark
        private string watermarkText = "yyyy-mm-dd";
        private void SetupWatermark(TextBox textBox)
        {
            // Nếu ban đầu trống thì gán watermark
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = watermarkText;
                textBox.ForeColor = Color.Gray;
            }

            textBox.GotFocus += (s, e) =>
            {
                // Nếu đang hiển thị watermark thì xóa đi để nhập
                if (textBox.Text == watermarkText && textBox.ForeColor == Color.Gray)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };

            textBox.LostFocus += (s, e) =>
            {
                // Nếu người dùng không nhập gì thì gán lại watermark
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = watermarkText;
                    textBox.ForeColor = Color.Gray;
                }
                else
                {
                    textBox.ForeColor = Color.Black; // Giữ màu đen nếu người dùng có nhập
                }
            };
        }
        #endregion WaterMark
        private void CustomizeGrid1()
        {
            var dgv = dgvMtm_mst;

            // Tắt redraw để tránh lag khi render nhiều thay đổi
            dgv.SuspendLayout();

            // Font & màu sắc cơ bản
            dgv.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dgv.EnableHeadersVisualStyles = false;
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.GridColor = Color.FromArgb(230, 230, 230);
            dgv.BackgroundColor = Color.White;

            // Header
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 35;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Dòng dữ liệu
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 240, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.Padding = new Padding(3, 2, 3, 2);
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False; // 🔹 tránh ngắt dòng khiến chữ bị ẩn

            // Dòng xen kẽ (alternate row)
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);

            // Kích thước tự động (chỉ theo nội dung, không fill toàn bảng)
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells; // 🔹 nhanh hơn Fill
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; // 🔹 tránh auto height gây lag
            // Không cho người dùng chỉnh sửa hoặc thêm dòng
            dgv.ReadOnly = false;
            dgv.AllowUserToAddRows = false;
            //dgv.AllowUserToResizeRows = false;

            // Chiều cao dòng vừa đủ cho font (tránh bị cắt chữ)
            dgv.RowTemplate.Height = 30;
            // Cuộn mượt hơn
            //dgv.DoubleBuffered(true); // 🔹 custom extension bên dưới
            dgv.ResumeLayout();
        }
        private void CustomizeGrid2()
        {
            var dgv = dgvMtm_detail;

            // Tắt redraw để tránh lag khi render nhiều thay đổi
            dgv.SuspendLayout();

            // Font & màu sắc cơ bản
            dgv.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dgv.EnableHeadersVisualStyles = false;
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.GridColor = Color.FromArgb(230, 230, 230);
            dgv.BackgroundColor = Color.White;

            // Header
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 35;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Dòng dữ liệu
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 240, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.Padding = new Padding(3, 2, 3, 2);
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False; // 🔹 tránh ngắt dòng khiến chữ bị ẩn
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Dòng xen kẽ (alternate row)
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);

            // Kích thước tự động (chỉ theo nội dung, không fill toàn bảng)
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells; // 🔹 nhanh hơn Fill
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; // 🔹 tránh auto height gây lag
            // Không cho người dùng chỉnh sửa hoặc thêm dòng
            dgv.ReadOnly = false;
            dgv.AllowUserToAddRows = false;
            //dgv.AllowUserToResizeRows = false;

            // Chiều cao dòng vừa đủ cho font (tránh bị cắt chữ)
            dgv.RowTemplate.Height = 33;
            // Cuộn mượt hơn
            //dgv.DoubleBuffered(true); // 🔹 custom extension bên dưới
            dgv.ResumeLayout();
        }
        private void pnMtmContent_Paint(object sender, PaintEventArgs e)
        {
            if (dgvMtm_mst.Rows.Count == 0)
            {
                string message = "❌ Không có dữ liệu";
                Font font = new Font("Segoe UI", 14, FontStyle.Bold);
                Color textColor = Color.FromArgb(231, 76, 60); // đỏ cam hiện đại
                SizeF textSize = e.Graphics.MeasureString(message, font);

                float x = (pnMtmContent.Width - textSize.Width) / 2;
                float y = (pnMtmContent.Height - textSize.Height) / 2;

                e.Graphics.DrawString(message, font, new SolidBrush(textColor), x, y);
            }
        }
        private void pcbMtmStartDate_Click(object sender, EventArgs e)
        {
            txtMtmStartDate.Focus();
            ShowCalendar(txtMtmStartDate, pcbMtmStartDate);
        }

        private void pcbMtmEndDate_Click(object sender, EventArgs e)
        {
            txtMtmEndDate.Focus();
            ShowCalendar(txtMtmEndDate, pcbMtmEndDate);
        }

        private void frmMtm_Load(object sender, EventArgs e)
        {
            SetupWatermark(txtMtmStartDate);
            SetupWatermark(txtMtmEndDate);
            CustomizeGrid1();
            CustomizeGrid2();
        }

        private void btnMtmSearch_Click(object sender, EventArgs e)
        {
            List<string> data = new List<string>();
            List<string> para = new List<string>();
            if
                (!Equals(txtMtmOldSo.Text.Trim(), ""))
            {
                string oldSo = txtMtmOldSo.Text.Trim();
                data.Add(oldSo);
                string parameter = "@FromSo";
                para.Add(parameter);
            }
            if (!Equals(txtMtmBatch.Text.Trim(), ""))
            {
                string batch = txtMtmBatch.Text.Trim();
                data.Add(batch);
                string parameter = "@Batch";
                para.Add(parameter);
            }
            if (!Equals(txtMtmNewSo.Text.Trim(), ""))
            {
                string newSo = txtMtmNewSo.Text.Trim();
                data.Add(newSo);
                string parameter = "@NewSo";
                para.Add(parameter);
            }
            string mtmStartDate = (txtMtmStartDate.Text == watermarkText && txtMtmStartDate.ForeColor == Color.Gray) ? "" : txtMtmStartDate.Text;
            string mtmEndDate = (txtMtmEndDate.Text == watermarkText && txtMtmEndDate.ForeColor == Color.Gray) ? "" : txtMtmEndDate.Text;
            if (!Equals(mtmStartDate, ""))
            {
                data.Add(mtmStartDate);
                string parameter = "@startDate";
                para.Add(parameter);
            }
            if (!Equals(mtmEndDate, ""))
            {
                data.Add(mtmEndDate);
                string parameter = "@endDate";
                para.Add(parameter);
            }
            GetCicsData(data, para);
        }
        public void GetCicsData(List<string> data, List<string> para)
        {
            if (data.Count > 0)
            {
                pnMtmContent.Visible = true;
                using (SqlConnection sqlcon = new SqlConnection(cicsConnect))
                {
                    try
                    {
                        sqlcon.Open();
                        string storedProcName;
                            storedProcName = "RP_CCT_CuttingReport_mst";
                        using (SqlCommand cmd = new SqlCommand(storedProcName, sqlcon))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            for (int i = 0; i < data.Count; i++)
                            {
                                cmd.Parameters.AddWithValue(para[i], data[i]); // Ví dụ DeptId = 5
                            }
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            dgvMtm_mst.DataSource = dt;
                        }
                        string storedProcName1;
                            storedProcName1 = "RP_CCT_CuttingReport_dtl";
                        using (SqlCommand cmd1 = new SqlCommand(storedProcName1, sqlcon))
                        {
                            cmd1.CommandType = CommandType.StoredProcedure;
                            for (int i = 0; i < data.Count; i++)
                            {
                                cmd1.Parameters.AddWithValue(para[i], data[i]); // Ví dụ DeptId = 5
                            }
                            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                            DataTable dt1 = new DataTable();
                            da1.Fill(dt1);

                            dgvMtm_detail.DataSource = dt1;
                        }
                        int rows = dgvMtm_detail.Rows.Count;
                        if (rows <= 0)
                        {
                            pnMtmContent.Visible = false;
                            dgvMtm_detail.DataSource = null;
                            dgvMtm_detail.Columns.Clear();
                            pnMtmContent.Paint += pnMtmContent_Paint;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập dữ liệu .", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtMtmOldSo.Focus();
            }
        }
    }
}
