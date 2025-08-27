using OfficeOpenXml.DataValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Xml.Serialization;
using Excel = Microsoft.Office.Interop.Excel;

namespace QUANLYTHUVIEN
{
    public partial class ThongKe : Form
    {
        public ThongKe()
        {
            InitializeComponent();
        }
       
        void CreateColumnForDataGridView()
        {
            var colMaSach = new DataGridViewTextBoxColumn();
            var colTenSach = new DataGridViewTextBoxColumn();
            var colNXB = new DataGridViewTextBoxColumn();
            var colTenTG = new DataGridViewTextBoxColumn();
            var colKhoLuu = new DataGridViewTextBoxColumn();
            var colSoLuong = new DataGridViewTextBoxColumn();

            colMaSach.HeaderText = "Mã sách";
            colTenSach.HeaderText = "Tên sách";
            colNXB.HeaderText = "Năm xuất bản";
            colTenTG.HeaderText = "Tên tác giả";
            colKhoLuu.HeaderText = "Kho lưu trữ";
            colSoLuong.HeaderText = "Số lượng";

            colMaSach.DataPropertyName = "masach";
            colTenSach.DataPropertyName = "tensach";
            colNXB.DataPropertyName = "namxb";
            colTenTG.DataPropertyName = "tentg";
            colKhoLuu.DataPropertyName = "kholuu";
            colSoLuong.DataPropertyName = "soluong";

            colMaSach.Width = 100;
            colTenSach.Width = 250;
            colNXB.Width = 205;
            colTenTG.Width = 250;
            colKhoLuu.Width = 130;
            colSoLuong.Width = 100;

            dataGridView.Columns.AddRange(new DataGridViewColumn[] { colMaSach, colTenSach, colNXB, colTenTG, colKhoLuu, colSoLuong });
        }
            
        private void btThoat_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Form fm= new FrmMain();
            fm.Show();
        }

        private void ThongKe_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btThongKe_Click(object sender, EventArgs e)
        {
            List<ThongTinSach> listSearch = new List<ThongTinSach>();
            foreach (var item in ListSachcs.Instance.Listsach)
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    listSearch.Add(item);
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    if (item.Kholuu.ToLower().CompareTo("Nhà Bè".ToLower()) == 0)
                    {
                        listSearch.Add(item);
                    }
                }
                else if (comboBox1.SelectedIndex == 2)
                {
                    if (item.Kholuu.ToLower().CompareTo("Võ Văn Tần".ToLower()) == 0)
                    {
                        listSearch.Add(item);
                    }
                }
                else if (comboBox1.SelectedIndex == 3)
                {
                    if (item.Kholuu.ToLower().CompareTo("Mai Thị Lựu".ToLower()) == 0)
                    {
                        listSearch.Add(item);
                    }
                }
            }    
            dataGridView.DataSource = null;
            CreateColumnForDataGridView();
            dataGridView.DataSource = listSearch;
        }
        private void xuatfile(DataTable dataTable, string sheetName, string title)
        {
            //Tạo các đối tượng excel
            Microsoft.Office.Interop.Excel.Application oExcel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks oBooks;
            Microsoft.Office.Interop.Excel.Sheets oSheets;
            Microsoft.Office.Interop.Excel.Workbook oBook;
            Microsoft.Office.Interop.Excel.Worksheet oSheet;
            //Tạo mới một Excel Workbook
            oExcel.Visible = true;
            oExcel.DisplayAlerts = false;
            oExcel.Application.SheetsInNewWorkbook = 1;
            oBooks = oExcel.Workbooks;
            oBook = (Microsoft.Office.Interop.Excel.Workbook)(oExcel.Workbooks.Add(Type.Missing));
            oSheets = oBook.Worksheets;
            oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oSheets.get_Item(1);
            oSheet.Name = sheetName;
            //Tạo phần tiêu đề
            Microsoft.Office.Interop.Excel.Range head = oSheet.get_Range("A1", "G1");
            head.MergeCells = true;
            head.Value2 = title;
            head.Font.Bold = true;
            head.Font.Name = "Time New Roman";
            head.Font.Size = "20";
            head.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            //Tạo tiêu đề cột
            Microsoft.Office.Interop.Excel.Range cl1 = oSheet.get_Range("A3");
            cl1.Value2 = "Mã sách";
            cl1.ColumnWidth = 15.0;

            Microsoft.Office.Interop.Excel.Range cl2 = oSheet.get_Range("B3");
            cl2.Value2 = "Tên sách";
            cl2.ColumnWidth = 30.0;

            Microsoft.Office.Interop.Excel.Range cl3 = oSheet.get_Range("C3");
            cl3.Value2 = "Năm xuất bản";
            cl3.ColumnWidth = 15.0;

            Microsoft.Office.Interop.Excel.Range cl4 = oSheet.get_Range("D3");
            cl4.Value2 = "Tên tác giả";
            cl4.ColumnWidth = 30.0;

            Microsoft.Office.Interop.Excel.Range cl5 = oSheet.get_Range("E3");
            cl5.Value2 = "Kho lưu trữ";
            cl5.ColumnWidth = 15.0;

            Microsoft.Office.Interop.Excel.Range cl6 = oSheet.get_Range("F3");
            cl6.Value2 = "Số lượng";
            cl6.ColumnWidth = 15.0;
            //Tạo mảng theo datagridview
            object[,] arr = new object[dataTable.Rows.Count, dataTable.Columns.Count];
            //chuyển dữ liệu từ datagridview sang mảng đối tượng
            for (int row = 0; row < dataTable.Rows.Count; row++)
            {
                DataRow dataRow = dataTable.Rows[row];
                for (int col = 0; col < dataTable.Columns.Count; col++)
                {
                    arr[row, col] = dataRow[col];
                } 
                    
            }
            //thiết lập vùng điền dữ liệu
            int rowStart = 4;
            int colStart = 1;
            int rowEnd = rowStart + dataTable.Rows.Count - 1;
            int colEnd = dataTable.Columns.Count;
            //ô bắt đầu điền dữ liệu
            Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowStart, colStart];
            //ô kết thúc điền dữ liệu
            Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowEnd, colEnd];
            //lấy về vùng điền dữ liệu
            Microsoft.Office.Interop.Excel.Range range = oSheet.get_Range(c1, c2);
            //Điền dữ liệu vào vùng đã thiết lập
            range.Value2 = arr;
            //kẻ viền
            range.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            
        }
        private void exportExceḷ(string path)
        {
            Excel.Application application = new Excel.Application();
            application.Application.Workbooks.Add(Type.Missing);
            for (int i=0; i<dataGridView.Columns.Count; i++)
            {
                application.Cells[1, i + 1] = dataGridView.Columns[i].HeaderText;
            } 
            for (int i=0; i<dataGridView.Rows.Count;i++)
            {
                for (int j=0; j<dataGridView.Columns.Count;j++)
                {
                    application.Cells[i + 2f, j + 1] = dataGridView.Rows[i].Cells[j].Value; 
                }    
            }
            application.Columns.AutoFit();
            application.ActiveWorkbook.SaveCopyAs(path);
            application.ActiveWorkbook.Saved = true;

        }
        private void btXuat_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("Mã sách"));
                dt.Columns.Add(new DataColumn("Tên sách"));
                dt.Columns.Add(new DataColumn("Năm xuất bản"));
                dt.Columns.Add(new DataColumn("Tên tác giả"));
                dt.Columns.Add(new DataColumn("Kho lưu trữ"));
                dt.Columns.Add(new DataColumn("Số lượng"));

                foreach (DataGridViewRow dtgvrow in dataGridView.Rows)
                {
                    DataRow dtrow = dt.NewRow();
                    dtrow[0] = dtgvrow.Cells[0].Value;
                    dtrow[1] = dtgvrow.Cells[1].Value;
                    dtrow[2] = dtgvrow.Cells[2].Value;
                    dtrow[3] = dtgvrow.Cells[3].Value;
                    dtrow[4] = dtgvrow.Cells[4].Value;
                    dtrow[5] = dtgvrow.Cells[5].Value;
                    dt.Rows.Add(dtrow);
                }
                xuatfile(dt, "Danh sách", "THỐNG KÊ SÁCH");
            }
            else
            { 
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Xuất dữ liệu";
                saveFileDialog.Filter = "Excel(*.xlsx)|*.xlsx";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        exportExceḷ(saveFileDialog.FileName);
                        MessageBox.Show("Xuất file thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Xuất file thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

        }

        private void ThongKe_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }
    }
}
