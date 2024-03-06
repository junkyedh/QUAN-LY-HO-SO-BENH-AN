using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Model;
using Bunifu.UI.WinForms;

namespace DO_AN_CUA_HAN.View
{
    public partial class FormRoleDetail: Form
    {
        public Role roleDetail { get; set; }
        public String UserAction { get; set; }
        public FormRoleDetail()
        {
            InitializeComponent();
            DataTable dtSystemFunction = RoleFunction.GetListFunction();
            for (int i = 0; i < dtSystemFunction.Rows.Count; i++)
            {
                RoleFunction tempRF = new RoleFunction(int.Parse(dtSystemFunction.Rows[i][0].ToString()), dtSystemFunction.Rows[i][1].ToString(), dtSystemFunction.Rows[i][2].ToString());
                listBoxSystemFunction.Items.Add(tempRF.FucntionName);
                listBoxSystemFunctionID.Items.Add(tempRF.FunctionID);
            }
        }
        public FormRoleDetail(Role roledetail, String usertAction)
        {
            InitializeComponent();
            this.roleDetail = roledetail;
            this.UserAction = usertAction;
            if (usertAction.Equals("edit"))
                SetRoleDetail(roleDetail);
        }
        public void SetRoleDetail(Role roledetail)
        {
            this.roleDetail = roledetail;

            textBoxRoleID.Text = roleDetail.RoleID.ToString();
            textBoxRoleName.Text = roleDetail.RoleName;

            DataTable dtCurrentFunction = RoleDetail.GetListStaffFunction(roledetail.RoleID);
            for (int i = 0; i < dtCurrentFunction.Rows.Count; i++)
            {
                RoleFunction tempRF = new RoleFunction(int.Parse(dtCurrentFunction.Rows[i][0].ToString()), dtCurrentFunction.Rows[i][1].ToString(), "");
                listBoxCurrentFunction.Items.Add(tempRF.FucntionName);
                listBoxCurrentFunctionID.Items.Add(tempRF.FunctionID);
            }
            DataTable dtSystemFunction = RoleFunction.GetListFunction();
            for (int i = 0; i < dtSystemFunction.Rows.Count; i++)
            {
                RoleFunction tempRF = new RoleFunction(int.Parse(dtSystemFunction.Rows[i][0].ToString()), dtSystemFunction.Rows[i][1].ToString(), dtSystemFunction.Rows[i][2].ToString());
                listBoxSystemFunction.Items.Add(tempRF.FucntionName);
                listBoxSystemFunctionID.Items.Add(tempRF.FunctionID);
            }
            ListBox lstTemp = new ListBox();
            for (int i = 0; i < listBoxCurrentFunctionID.Items.Count; i++)
            {
                for (int j = 0; j < listBoxSystemFunctionID.Items.Count; j++)
                {
                    if (listBoxCurrentFunctionID.Items[i].ToString() == listBoxSystemFunctionID.Items[j].ToString())
                    {
                        listBoxSystemFunctionID.Items.RemoveAt(j);
                        listBoxSystemFunction.Items.RemoveAt(j);
                        break;
                    }
                }
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBoxRoleName.Text))
            {
                bunifuSnackbar1.Show(this, "Thiếu tên phân quyền", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }
            try
            {
                if ("edit".Equals(this.UserAction))
                {
                    DialogResult dialogResult = MessageBox.Show("Xác nhận cập nhập thông tin phân quyền", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Role editRole = new Role(int.Parse(textBoxRoleID.Text), textBoxRoleName.Text);

                        if (Role.UpdateRole(editRole) > 0)
                        {
                            RoleDetail.DeleteRoleDetail(roleDetail.RoleID);
                            for (int i = 0; i < listBoxCurrentFunctionID.Items.Count; i++)
                            {
                                RoleDetail newRD = new RoleDetail(roleDetail.RoleID, int.Parse(listBoxCurrentFunctionID.Items[i].ToString()));
                                RoleDetail.InsertRoleDetail(newRD);
                            }
                            bunifuSnackbar1.Show(this, "Cập nhật thông tin phân quyền thành công", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                            return;
                        }
                    }
                }
                else
                {
                    Role newRole = new Role(0, textBoxRoleName.Text);
                    if (Role.InsertRole(newRole) > 0)
                    {
                        int currentIdent = Role.GetCurrentIdentity();
                        for (int i = 0; i < listBoxCurrentFunctionID.Items.Count; i++)
                        {
                            RoleDetail newRD = new RoleDetail(currentIdent, int.Parse(listBoxCurrentFunctionID.Items[i].ToString()));
                            RoleDetail.InsertRoleDetail(newRD);
                        }
                        bunifuSnackbar1.Show(this, "Thêm thông tin phân quyền thành công", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                        return;
                    }
                }
            }
            catch
            {
                bunifuSnackbar1.Show(this, "Lỗi dữ liệu", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }

            this.Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonInsertFunction_Click(object sender, EventArgs e)
        {
            if (listBoxSystemFunction.Items.Count > 0)
            {
                int selectedItem = listBoxSystemFunction.SelectedIndex;
                if (selectedItem >= 0)
                {
                    listBoxCurrentFunction.Items.Add(listBoxSystemFunction.Items[selectedItem].ToString());
                    listBoxCurrentFunctionID.Items.Add(listBoxSystemFunctionID.Items[selectedItem].ToString());
                    listBoxSystemFunction.Items.RemoveAt(selectedItem);
                    listBoxSystemFunctionID.Items.RemoveAt(selectedItem);
                }
            }
        }

        private void buttonRemoveFunction_Click(object sender, EventArgs e)
        {
            if (listBoxCurrentFunction.Items.Count > 0)
            {
                int selectedItem = listBoxCurrentFunction.SelectedIndex;
                if (selectedItem >= 0)
                {
                    listBoxSystemFunction.Items.Add(listBoxCurrentFunction.Items[selectedItem].ToString());
                    listBoxSystemFunctionID.Items.Add(listBoxCurrentFunctionID.Items[selectedItem].ToString());
                    listBoxCurrentFunction.Items.RemoveAt(selectedItem);
                    listBoxCurrentFunctionID.Items.RemoveAt(selectedItem);
                }
            }
        }

        private void textBoxRoleName_KeyPress(object sender, KeyPressEventArgs e)
        {

            e.Handled = !((e.KeyChar >= 'A' && e.KeyChar <= 'Z') ||
              (e.KeyChar >= 'a' && e.KeyChar <= 'z') ||
              e.KeyChar == 8 || // Backspace
              e.KeyChar == 32 || // Space
              e.KeyChar == 16 || // Shift
              e.KeyChar == 46 || // Delete
              (e.KeyChar >= 'À' && e.KeyChar <= 'ỹ') || // Vietnamese characters and accented vowels
              (e.KeyChar >= 'à' && e.KeyChar <= 'ỹ'));
        }
    }
}
