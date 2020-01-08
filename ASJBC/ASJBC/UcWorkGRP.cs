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
    public partial class UcWorkGRP : BaseUserControl
    {
        BCORHelper BHelper = new BCORHelper();
        Result rs = new Result();
        //声明实体
        private BCOR_WORKGRP workgrp;


        /// <summary>
        /// 加载
        /// </summary>
        public UcWorkGRP()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_workorg"></param>
        public UcWorkGRP(BCOR_WORKGRP _workgrp) : this()
        {
            workgrp = _workgrp;
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcWorkGRP_Load(object sender, EventArgs e)
        {
            txtWORKGRP_CODE.EditValue = workgrp.WORKGRP_CODE?.ToString();//编码
            txtWORKGRP_NAME.EditValue = workgrp.WORKGRP_NAME?.ToString();//名称
            txtORGANIZATION_TKEY.EditValue = workgrp.ORGANIZATION_TKEY?.ToString();//生产组织
            txtF_WORKGRP_TKEY.EditValue = workgrp.F_WORKGRP_TKEY?.ToString();//上级分组KEY
            txtDEPT_TKEY.EditValue = workgrp.DEPT_TKEY?.ToString();//部门
            txtGRPADMIN_EMPL_TKEY.EditValue = workgrp.GRPADMIN_EMPL_TKEY?.ToString();//班组负责人
            txtCMT.EditValue = workgrp.CMT?.ToString();//备注


            BindGridLookUpEdit();//绑定生产组织 部门负责人
            //根节点 下拉框无法编辑
            if (txtF_WORKGRP_TKEY.Text.ToUpper() == "ROOT")
            {
                txtF_WORKGRP_TKEY.Enabled = false;
            }
        }

        /// <summary>
        /// 更新方法
        /// </summary>
        /// <returns></returns>
        public BCOR_WORKGRP UpdateUI()
        {
            workgrp.WORKGRP_CODE = txtWORKGRP_CODE.EditValue?.ToString();
            workgrp.WORKGRP_NAME = txtWORKGRP_NAME.EditValue?.ToString();
            workgrp.ORGANIZATION_TKEY = txtORGANIZATION_TKEY.EditValue?.ToString();
            workgrp.F_WORKGRP_TKEY = txtF_WORKGRP_TKEY.EditValue?.ToString();
            workgrp.DEPT_TKEY = txtDEPT_TKEY.EditValue?.ToString();
            workgrp.GRPADMIN_EMPL_TKEY = txtGRPADMIN_EMPL_TKEY.EditValue?.ToString();
            workgrp.CMT = txtCMT.EditValue?.ToString();

            return workgrp;
        }

        ///// <summary>
        ///// 绑定分组下拉框
        ///// </summary>
        //public void BindLookUpEdit()
        //{
        //    BCORHelper Helper = new BCORHelper();
        //    Result rs = new Result();
        //    rs = Helper.QueryGroupTable("BCOR_WORKGRP");
        //    txtF_WORKGRP_TKEY.Properties.DataSource = rs.Ds.Tables[0];
        //    txtF_WORKGRP_TKEY.Properties.DisplayMember = "WORKGRP_CODE";
        //    txtF_WORKGRP_TKEY.Properties.ValueMember = "TKEY";
        //}


        /// <summary>
        /// 绑定下拉框的值
        /// </summary>
        public void BindGridLookUpEdit()
        {
            List<string> strsql = new List<string>();
            List<GridLookUpEdit> Control = new List<GridLookUpEdit>();
            strsql.Add("SELECT TKEY,WORKORGAN_NAME,WORKORGAN_CODE from BCOR_WORKORGANIZATION where  FLAG = 1 ");//生产组织
            strsql.Add("SELECT TKEY,DEPT_NAME,DEPT_CODE from BCOR_DEPT where  FLAG = 1 ");//部门
            strsql.Add("SELECT TKEY,EMPLOYEE_NAME,EMPLOYEE_CODE FROM BCOR_EMPLOYEE WHERE FLAG = 1 ");//负责人
            strsql.Add("SELECT TKEY,WORKGRP_NAME,WORKGRP_CODE FROM BCOR_WORKGRP WHERE FLAG = 1 order by CRE_TIME");//上级班组

            Control.Add(txtORGANIZATION_TKEY);//生产组织
            Control.Add(txtDEPT_TKEY);//部门
            Control.Add(txtGRPADMIN_EMPL_TKEY);//负责人
            Control.Add(txtF_WORKGRP_TKEY);//上级班组
            BHelper.BindGridLookUpEdit(strsql, Control);

        }
        #region 多列模糊查询
        private void txtF_WORKGRP_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));

        }

        private void txtF_WORKGRP_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtORGANIZATION_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));

        }

        private void txtORGANIZATION_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtDEPT_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));

        }

        private void txtDEPT_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtGRPADMIN_EMPL_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));

        }

        private void txtGRPADMIN_EMPL_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }
        #endregion
    }
}
