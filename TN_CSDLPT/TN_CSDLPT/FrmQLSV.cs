﻿using System;
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

    public partial class FrmQLSV : Form
    {
        private int loadCS = 0;
        private Boolean checkThem = false;
        private Boolean checkSua = false;
        public static Boolean checkSave = true;
        public String MASV_CU = "";
        public FrmQLSV()
        {
            InitializeComponent();
        }

        private void sINHVIENBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bds_SINHVIEN.EndEdit();
            this.tableAdapterManager.UpdateAll(this.tN_CSDLPTDataSet);

        }

        private void FrmQLSV_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            Program.connstrKhac = Program.connstr;
            tN_CSDLPTDataSet.EnforceConstraints = false;

            try
            {
                // Lấy kết danh sách phân mảnh đổ vào combobox
                cbbCOSO.DataSource = Program.bds_dspm.DataSource;
                cbbCOSO.DisplayMember = "TENCS";
                cbbCOSO.ValueMember = "TENSERVER";
                cbbCOSO.SelectedIndex = Program.mCoSo;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu cơ sở. \n" + ex.Message, "Lỗi", MessageBoxButtons.OK);
            }

            this.ta_KHOA.Connection.ConnectionString = Program.connstr;
            this.ta_KHOA.Fill(this.tN_CSDLPTDataSet.KHOA);

            this.ta_LOP.Connection.ConnectionString = Program.connstr;
            this.ta_LOP.Fill(this.tN_CSDLPTDataSet.LOP);

            this.ta_SINHVIEN.Connection.ConnectionString = Program.connstr;
            this.ta_SINHVIEN.Fill(this.tN_CSDLPTDataSet.SINHVIEN);

            if (cbbTENKHOA.SelectedValue != null)
            {
                cbbTENKHOA.SelectedIndex = 0;
                bds_LOP.Filter = "MAKH = '" + cbbTENKHOA.SelectedValue.ToString().Trim() + "'";

                if (cbbTENLOP.SelectedValue != null)
                {
                    cbbTENLOP.Enabled = true;
                    bds_SINHVIEN.Filter = "MALOP = '" + cbbTENLOP.SelectedValue.ToString().Trim() + "'";
                    SINHVIEN_GridControl.Refresh();
                    cbbTENLOP.SelectedIndex = 0;
                }
                else
                {
                    bds_SINHVIEN.Filter = "MALOP = ''";
                    SINHVIEN_GridControl.Refresh();
                    cbbTENLOP.Enabled = false;
                    MessageBox.Show("Khoa chưa có lớp", "", MessageBoxButtons.OK);
                    btnThem.Visibility = btnGhi.Visibility = btnSua.Visibility = btnXoa.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
            }

            if (Program.mGroup == "TRUONG")
            {
                cbbCOSO.Enabled = true;
                btnThem.Visibility = btnGhi.Visibility = btnSua.Visibility = btnXoa.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            }
            else if (Program.mGroup == "COSO")
            {
                cbbCOSO.Enabled = false;
                btnGhi.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                btnThem.Visibility = btnSua.Visibility = btnXoa.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                if (cbbTENLOP.SelectedValue == null)
                {
                    btnThem.Visibility = btnGhi.Visibility = btnSua.Visibility = btnXoa.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
            }

            groupControl4.Enabled = false;

            loadCS++;

        }

        private void cbbCOSO_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbbCOSO.SelectedValue != null && loadCS != 0)
                {
                    Program.servernameKhac = cbbCOSO.SelectedValue.ToString();
                    if (Program.KetNoiCosoKhac() == 0) return;
                    else
                    {
                        this.ta_KHOA.Connection.ConnectionString = Program.connstrKhac;
                        this.ta_KHOA.Fill(this.tN_CSDLPTDataSet.KHOA);

                        this.ta_LOP.Connection.ConnectionString = Program.connstrKhac;
                        this.ta_LOP.Fill(this.tN_CSDLPTDataSet.LOP);

                        this.ta_SINHVIEN.Connection.ConnectionString = Program.connstrKhac;
                        this.ta_SINHVIEN.Fill(this.tN_CSDLPTDataSet.SINHVIEN);

                        if (cbbTENKHOA.SelectedValue != null)
                        {
                            cbbTENKHOA.SelectedIndex = 0;
                            bds_LOP.Filter = "MAKH = '" + cbbTENKHOA.SelectedValue.ToString().Trim() + "'";

                            if (cbbTENLOP.SelectedValue != null)
                            {
                                cbbTENLOP.SelectedIndex = 0;
                                cbbTENLOP.Enabled = true;
                                if (Program.mGroup == "COSO")
                                {
                                    btnThem.Visibility = btnSua.Visibility = btnXoa.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                                }
                                bds_SINHVIEN.Filter = "MALOP = '" + cbbTENLOP.SelectedValue.ToString().Trim() + "'";
                                SINHVIEN_GridControl.Refresh();
                            }
                            else
                            {
                                cbbTENLOP.Enabled = false;
                                btnThem.Visibility = btnSua.Visibility = btnXoa.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                                bds_SINHVIEN.Filter = "MALOP = ''";
                                SINHVIEN_GridControl.Refresh();
                            }
                        }

                    }

                }
            }
            catch (Exception) { };
        }

        private void cbbTENKHOA_SelectedIndexChanged(object sender, EventArgs e)
        {
            int vt = cbbTENKHOA.SelectedIndex;
            if (cbbTENKHOA.SelectedValue != null)
            {
                cbbTENKHOA.SelectedIndex = vt;
                bds_LOP.Filter = "MAKH = '" + cbbTENKHOA.SelectedValue.ToString().Trim() + "'";

                if (cbbTENLOP.SelectedValue != null)
                {
                    cbbTENLOP.SelectedIndex = 0;
                    cbbTENLOP.Enabled = true;
                    if (Program.mGroup == "COSO")
                    {
                        btnThem.Visibility = btnSua.Visibility = btnXoa.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    }

                    bds_SINHVIEN.Filter = "MALOP = '" + cbbTENLOP.SelectedValue.ToString().Trim() + "'";
                    SINHVIEN_GridControl.Refresh();
                }
                else
                {
                    bds_SINHVIEN.Filter = "MALOP = ''";
                    SINHVIEN_GridControl.Refresh();

                    cbbTENLOP.Enabled = false;
                    btnThem.Visibility = btnSua.Visibility = btnXoa.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
            }

        }

        private void cbbTENLOP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbTENLOP.SelectedValue != null)
            {
                if (Program.mGroup == "COSO")
                {
                    btnThem.Visibility = btnSua.Visibility = btnXoa.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }
                bds_SINHVIEN.Filter = "MALOP = '" + cbbTENLOP.SelectedValue.ToString().Trim() + "'";
                SINHVIEN_GridControl.Refresh();
            }
            else
            {
                btnThem.Visibility = btnSua.Visibility = btnXoa.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                bds_SINHVIEN.Filter = "MALOP = ''";
                SINHVIEN_GridControl.Refresh();
            }


        }
        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                btnGhi.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                btnThem.Enabled = btnXoa.Enabled = btnTaiLai.Enabled = btnSua.Enabled = false;
                groupControl4.Enabled = true;
                SINHVIEN_GridControl.Enabled = true;

                bds_SINHVIEN.AddNew();

                checkThem = true;
                checkSave = false;
                edtMASV.Focus();

                edtMALOP.Text = cbbTENLOP.SelectedValue.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm SV " + ex.Message, "", MessageBoxButtons.OK);
            }
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (bds_SINHVIEN.Count == 0)
            {
                MessageBox.Show("Không có SV để sửa!", "THÔNG BÁO", MessageBoxButtons.OK);
            }
            else
            {
                btnGhi.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                groupControl3.Enabled = false;
                groupControl4.Enabled = true;

                checkSua = true;
                btnThem.Enabled = btnXoa.Enabled = btnTaiLai.Enabled = btnSua.Enabled = false;
                edtMASV.Focus();
                SINHVIEN_GridControl.Enabled = false;
                checkSave = false;

                MASV_CU = edtMASV.Text;
            }
        }


        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (bds_SINHVIEN.Count == 0)
            {
                MessageBox.Show("Không có SINH VIEN để xóa!", "THÔNG BÁO", MessageBoxButtons.OK);
                return;
            }
            else
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa " +
                    ((DataRowView)this.bds_SINHVIEN.Current).Row["HO"].ToString() + ((DataRowView)this.bds_SINHVIEN.Current).Row["TEN"].ToString() + "?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        //phải chạy lệnh del from where mới chính xác
                        bds_SINHVIEN.RemoveCurrent();
                        //đẩy dữ liệu về adapter
                        this.ta_SINHVIEN.Update(this.tN_CSDLPTDataSet.SINHVIEN);

                        this.ta_SINHVIEN.Connection.ConnectionString = Program.connstrKhac;
                        this.ta_SINHVIEN.Fill(this.tN_CSDLPTDataSet.SINHVIEN);

                        bds_SINHVIEN.Filter = "MALOP = '" + edtMALOP.Text.ToString().Trim() + "'";
                        SINHVIEN_GridControl.Refresh();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xóa GV" + ex.Message, "", MessageBoxButtons.OK);
                    }
                }
            }
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (checkThem == true)
            {
                if (edtMASV.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Mã SV không được rỗng : ", "", MessageBoxButtons.OK);
                    edtMASV.Focus();
                    return;
                }
                if (edtHO.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Họ SV không được rỗng: ", "", MessageBoxButtons.OK);
                    edtHO.Focus();
                    return;
                }
                if (edtTEN.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Tên SV không được rỗng: ", "", MessageBoxButtons.OK);
                    edtTEN.Focus();
                    return;
                }
                if (date_NGAYSINH.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Ngày sinh không được rỗng", "", MessageBoxButtons.OK);
                    date_NGAYSINH.Focus();
                    return;
                }
                if (edt_DIACHI.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Địa chỉ không được rỗng", "", MessageBoxButtons.OK);
                    edt_DIACHI.Focus();
                    return;
                }

                String sql = "EXEC SP_KTSV_TONTAI '" + edtMASV.Text.Trim() + "'";

                int kq = Program.ExecSqlNonQuery(sql);
                if (kq == 1)
                {
                    edtMASV.Focus();
                    return;
                }
                else
                {
                    ghiSV();
                    checkThem = false;
                }
            }
            else if (checkSua == true)
            {
                if (edtMASV.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Mã SV không được rỗng", "", MessageBoxButtons.OK);
                    edtMASV.Focus();
                    return;
                }
                if (edtHO.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Họ SV không được rỗng", "", MessageBoxButtons.OK);
                    edtHO.Focus();
                    return;
                }
                if (edtTEN.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Tên SV không được rỗng", "", MessageBoxButtons.OK);
                    edtTEN.Focus();
                    return;
                }
                if (date_NGAYSINH.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Ngày sinh không được rỗng", "", MessageBoxButtons.OK);
                    date_NGAYSINH.Focus();
                    return;
                }
                if (edt_DIACHI.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Địa chỉ không được rỗng", "", MessageBoxButtons.OK);
                    edt_DIACHI.Focus();
                    return;
                }
                if (MASV_CU.Trim() != edtMASV.Text.Trim())
                {
                    String sql = "EXEC SP_KTSV_TONTAI '" + edtMASV.Text.Trim() + "'";

                    int kq = Program.ExecSqlNonQuery(sql);
                    if (kq == 1)
                    {
                        edtMASV.Focus();
                        return;
                    }
                    else
                    {
                        ghiSV();
                        checkThem = false;
                    }
                }
                else
                {
                    ghiSV();
                    checkThem = false;
                }

            }
            else
            {
                ghiSV();
            }
        }

        private int ghiSV()
        {
            try
            {
                btnThem.Enabled = btnXoa.Enabled = btnTaiLai.Enabled = btnSua.Enabled = true;
                btnGhi.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                SINHVIEN_GridControl.Enabled = true;
                groupControl4.Enabled = false;

                bds_SINHVIEN.EndEdit();
                bds_SINHVIEN.ResetCurrentItem();

                this.ta_SINHVIEN.Update(this.tN_CSDLPTDataSet.SINHVIEN);

                this.ta_SINHVIEN.Connection.ConnectionString = Program.connstr;
                this.ta_SINHVIEN.Fill(this.tN_CSDLPTDataSet.SINHVIEN);

                bds_SINHVIEN.Filter = "MALOP = '" + edtMALOP.Text.ToString().Trim() + "'";
                SINHVIEN_GridControl.Refresh();

                checkThem = false;
                checkSave = true;

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                checkSave = false;
                MessageBox.Show("Lỗi ghi SINHVIEN: " + ex, "", MessageBoxButtons.OK);
                return -1;
            }
        }

        private void btnTaiLai_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (cbbCOSO.SelectedValue != null && loadCS != 0)
                {
                    Program.servernameKhac = cbbCOSO.SelectedValue.ToString();
                    if (Program.KetNoiCosoKhac() == 0) return;
                    else
                    {
                        this.ta_KHOA.Connection.ConnectionString = Program.connstrKhac;
                        this.ta_KHOA.Fill(this.tN_CSDLPTDataSet.KHOA);

                        this.ta_LOP.Connection.ConnectionString = Program.connstrKhac;
                        this.ta_LOP.Fill(this.tN_CSDLPTDataSet.LOP);

                        this.ta_SINHVIEN.Connection.ConnectionString = Program.connstrKhac;
                        this.ta_SINHVIEN.Fill(this.tN_CSDLPTDataSet.SINHVIEN);

                        if (cbbTENKHOA.SelectedValue != null)
                        {
                            cbbTENKHOA.SelectedIndex = 0;


                            if (cbbTENLOP.SelectedValue != null)
                            {
                                cbbTENLOP.Enabled = true;
                                if (Program.mGroup == "COSO")
                                {
                                    btnThem.Visibility = btnSua.Visibility = btnXoa.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                                }
                                bds_SINHVIEN.Filter = "MALOP = '" + cbbTENLOP.SelectedValue.ToString().Trim() + "'";
                                SINHVIEN_GridControl.Refresh();
                            }
                            else
                            {
                                cbbTENLOP.Enabled = false;
                                btnThem.Visibility = btnSua.Visibility = btnXoa.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                                bds_SINHVIEN.Filter = "MALOP = ''";
                                SINHVIEN_GridControl.Refresh();
                            }
                        }

                    }

                }
            }
            catch (Exception) { };
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (checkThem == true)
                {
                    if (MessageBox.Show("Bạn đang tạo mới sinh viên, bạn có muốn ghi thông tin này?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        btnGhi_ItemClick(sender, e);
                        if (checkSave == true)
                            this.Close();
                        else
                            return;
                    }
                    else
                    {
                        checkSave = true;
                        Close();
                    }
                }
                else if (checkSua == true)
                {
                    if (MessageBox.Show("Bạn đang sửa sinh viên, bạn có muốn ghi thông tin này?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        btnGhi_ItemClick(sender, e);
                        if (checkSave == true)
                            this.Close();
                        else
                            return;
                    }
                    else
                    {
                        checkSave = true;
                        Close();
                    }
                }
                else
                {
                    checkSave = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK);

            }
        }
    }
}
