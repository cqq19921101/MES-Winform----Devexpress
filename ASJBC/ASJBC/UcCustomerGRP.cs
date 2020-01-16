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
    /// 工厂组织 - 供应链 - 客户分组管理
    /// </summary>
    public partial class UcCustomerGRP : BaseUserControl
    {
        ASJBCOR_ORG BHelper = new ASJBCOR_ORG();
        Result rs = new Result();

        /// <summary>
        /// 客户分组实体
        /// </summary>
        private BCOR_CUSTOMERGRP customergroup;

        /// <summary>
        /// 客户分组编辑
        /// </summary>
        public UcCustomerGRP()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成 
        /// </summary>
        /// <param name="_suppliergroup">客户分组实体</param>
        public UcCustomerGRP(BCOR_CUSTOMERGRP _customergroup) : this()
        {
            customergroup = _customergroup;
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcCustomerGRP_Load(object sender, EventArgs e)
        {
            txtCGRPCode.EditValue = customergroup.CUSTOMERGRP_CODE?.ToString();//分组编码
            txtCGRPName.EditValue = customergroup.CUSTOMERGRP_NAME?.ToString();//分组名称
            txtCGRPTkey.EditValue = customergroup.F_CUST_TKEY?.ToString();//上级分组KEY
            txtCMT.EditValue = customergroup.CMT?.ToString();//备注

            BindCustomerNode();//初始化时加载客户分组节点

            //根节点 下拉框无法编辑
            if (txtCGRPCode.Text.ToUpper() == "ROOT" )
            {
                txtCGRPTkey.Enabled = false;
            }

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns>返回一个客户分组实体</returns>
        public BCOR_CUSTOMERGRP UpdateUI()
        {
            customergroup.CUSTOMERGRP_CODE = txtCGRPCode.EditValue?.ToString();
            customergroup.CUSTOMERGRP_NAME = txtCGRPName.EditValue?.ToString();
            customergroup.F_CUST_TKEY = txtCGRPTkey.EditValue?.ToString();
            customergroup.CMT = txtCMT.EditValue?.ToString();

            return customergroup;
        }


        /// <summary>
        /// 绑定上级客户分组下拉框
        /// </summary>
        public void BindCustomerNode()
        {
            BHelper.BindGridLookUpEdit_CustomerGRP(txtCGRPTkey);
        }

        //多列模糊查询
        private void txtCGRPTkey_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtCGRPTkey_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }
    }
}
