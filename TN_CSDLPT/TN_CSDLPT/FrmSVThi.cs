using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TN_CSDLPT
{
    public partial class FrmSVThi : Form
    {
        public static Boolean checkThi = false;
        private Boolean isSinhVien = false;
        private int thoigianThi = 0;
        private int soCauThi = 0;
        public static CauHoiItem[] listCauHoi;
        public static ListViewItem baiThi;
        private float diem = -1;
        private DateTime ngayThi;

        public FrmSVThi()
        {
            InitializeComponent();
        }

        private void FrmSVThi_Load(object sender, EventArgs e)
        {

        }
    }
}
