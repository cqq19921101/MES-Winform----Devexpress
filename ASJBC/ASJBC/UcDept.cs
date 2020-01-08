using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ASJ.BASE;
using ASJ.ENTITY;
using ASJ.TOOLS.Basic;

namespace ASJ.BCOR
{
    public partial class UcDept : BaseUserControl
    {
        BCORHelper BHelper = new BCORHelper();
        //声明实体
        private BCOR_DEPT dept;


        /// <summary>
        /// 加载
        /// </summary>
        public UcDept()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_workorg"></param>
        public UcDept(BCOR_DEPT _dept) : this()
        {
            dept = _dept;
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcDept_Load(object sender, EventArgs e)
        {
            txtDEPT_CODE.EditValue = dept.DEPT_CODE?.ToString();//部门编码
            txtDEPT_NAME.EditValue = dept.DEPT_NAME?.ToString();//部门名称
            txtWORKORGAN_TKEY.EditValue = dept.WORKORGAN_TKEY?.ToString();//组织
            txtF_DEPT_TKEY.EditValue = dept.F_DEPT_TKEY?.ToString();//上级分组KEY
            txtDEPTADMIN_EMPL_TKEY.EditValue = dept.DEPTADMIN_EMPL_TKEY?.ToString();//部门负责人
            txtCMT.EditValue = dept.CMT?.ToString();//备注

            BindGridLookUpEdit();//绑定生产组织 部门负责人 上级部门
            //根节点 下拉框无法编辑
            if (txtF_DEPT_TKEY.Text.ToUpper() == "ROOT")
            {
                txtF_DEPT_TKEY.Enabled = false;
            }
        }

        /// <summary>
        /// 更新方法
        /// </summary>
        /// <returns></returns>
        public BCOR_DEPT UpdateUI()
        {
            dept.DEPT_CODE = txtDEPT_CODE.EditValue?.ToString();
            dept.DEPT_NAME = txtDEPT_NAME.EditValue?.ToString();
            dept.WORKORGAN_TKEY = txtWORKORGAN_TKEY.EditValue?.ToString();
            dept.F_DEPT_TKEY = txtF_DEPT_TKEY.EditValue?.ToString();
            dept.DEPTADMIN_EMPL_TKEY = txtDEPTADMIN_EMPL_TKEY.EditValue?.ToString();
            dept.CMT = txtCMT.EditValue?.ToString();

            return dept;
        }


        /// <summary>
        /// 绑定下拉框的值
        /// </summary>
        public void BindGridLookUpEdit()
        {
            List<string> strsql = new List<string>();
            List<GridLookUpEdit> Control = new List<GridLookUpEdit>();
            strsql.Add("SELECT TKEY,WORKORGAN_NAME,WORKORGAN_CODE from BCOR_WORKORGANIZATION where  FLAG = 1 ");//生产组织
            strsql.Add("SELECT TKEY,EMPLOYEE_NAME,EMPLOYEE_CODE FROM BCOR_EMPLOYEE WHERE FLAG = 1 ");//负责人
            strsql.Add("SELECT TKEY,DEPT_NAME,DEPT_CODE FROM BCOR_DEPT WHERE FLAG = 1 ");//上级部门

            Control.Add(txtWORKORGAN_TKEY);//生产组织
            Control.Add(txtDEPTADMIN_EMPL_TKEY);//负责人
            Control.Add(txtF_DEPT_TKEY);//上级部门
            BHelper.BindGridLookUpEdit(strsql, Control);

        }

        //多列模糊查询
        private void txtDEPTADMIN_EMPL_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtDEPTADMIN_EMPL_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtWORKORGAN_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtWORKORGAN_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }
    }
}
