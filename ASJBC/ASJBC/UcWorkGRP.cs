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
    /// <summary>
    /// 工作班组
    /// </summary>
    public partial class UcWorkGRP : BaseUserControl
    {
        ASJBCOR_ORG BHelper = new ASJBCOR_ORG();
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

        /// <summary>
        /// 绑定下拉框的值
        /// </summary>
        public void BindGridLookUpEdit()
        {
            List<GridLookUpEdit> Control = new List<GridLookUpEdit>();
            Control.Add(txtORGANIZATION_TKEY);//生产组织
            Control.Add(txtDEPT_TKEY);//部门
            Control.Add(txtGRPADMIN_EMPL_TKEY);//负责人
            Control.Add(txtF_WORKGRP_TKEY);//上级班组
            BHelper.BindGridLookUpEdit_WorkGRP(Control);
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
