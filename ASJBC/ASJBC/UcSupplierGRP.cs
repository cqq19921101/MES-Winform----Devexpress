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
using ASJ.TOOLS;
using ASJ.TOOLS.Basic;


namespace ASJ.BCOR
{
    /// <summary>
    /// 工厂组织 - 供应链 - 供应商分组管理
    /// </summary>
    public partial class UcSupplierGRP : BaseUserControl
    {

        ASJBCOR_ORG BHelper = new ASJBCOR_ORG();
        Result rs = new Result();
        /// <summary>
        /// 供应商分组实体
        /// </summary>
        private BCOR_SUPPLIERGRP suppliergroup;

        /// <summary>
        /// 供应商分组编辑
        /// </summary>
        public UcSupplierGRP()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成 
        /// </summary>
        /// <param name="_suppliergroup">供应商实体</param>
        public UcSupplierGRP(BCOR_SUPPLIERGRP _suppliergroup) : this()
        {
            suppliergroup = _suppliergroup;

        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcSupplierGRP_Load(object sender, EventArgs e)
        {
            txtSGRPCode.EditValue = suppliergroup.SUPPLIERGRP_CODE?.ToString();//分组编码
            txtSGRPName.EditValue = suppliergroup.SUPPLIERGRP_NAME?.ToString();//分组名称
            txtMGRPTkey.EditValue = suppliergroup.F_SUPPGRP_TKEY?.ToString();//上级分组KEY
            txtCMT.EditValue = suppliergroup.CMT?.ToString();//备注
            BindSupplierTKey();//绑定节点下拉框的值

            //根节点 下拉框无法编辑
            if (txtSGRPCode.Text.ToUpper() == "ROOT")
            {
                txtMGRPTkey.Enabled = false;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns>返回一个客户分组实体</returns>
        public BCOR_SUPPLIERGRP UpdateUI()
        {
            suppliergroup.SUPPLIERGRP_CODE = txtSGRPCode.EditValue?.ToString();
            suppliergroup.SUPPLIERGRP_NAME = txtSGRPName.EditValue?.ToString();
            suppliergroup.F_SUPPGRP_TKEY = txtMGRPTkey.EditValue?.ToString();
            suppliergroup.CMT = txtCMT.EditValue?.ToString();

            return suppliergroup;
        }

        /// <summary>
        /// 绑定供应商分组下拉框
        /// </summary>
        public void BindSupplierTKey()
        {
            BHelper.BindGridLookUpEdit_SupplierGRP(txtMGRPTkey);
        }

        //多列模糊查询
        private void txtMGRPTkey_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));

        }

        private void txtMGRPTkey_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);

        }
    }
}
