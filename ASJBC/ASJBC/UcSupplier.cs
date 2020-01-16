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
    /// 工厂组织 - 供应链 - 供应商管理
    /// </summary>
    public partial class UcSupplier : BaseUserControl
    {
        ASJBCOR_ORG BHelper = new ASJBCOR_ORG();
        Result rs = new Result();

        /// <summary>
        /// 供应商实体
        /// </summary>
        private BCOR_SUPPLIER supplier;

        /// <summary>
        /// 供应商编辑
        /// </summary>
        public UcSupplier()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_supplier">供应商实体</param>
        public UcSupplier(BCOR_SUPPLIER _supplier):this()
        {
            supplier = _supplier;
        }

        /// <summary>
        ///  初始化加载
        /// </summary>
        private void UcSupplier_Load(object sender, EventArgs e)
        {
            txtSupplierCode.EditValue = supplier.SUPPLIER_CODE?.ToString();//供应商编码
            txtSupplierName.EditValue = supplier.SUPPLIER_NAME?.ToString();//供应商名称
            txtSupplierGroup.EditValue = supplier.SUPPGRP_TKEY?.ToString();//供应商分组
            txtSupplierSubName.EditValue = supplier.SUPPLIER_SHORTNAME?.ToString();//供应商简称
            txtContactPepole.EditValue = supplier.CONTACT_PEOPLE?.ToString();//联系人
            txtTelphone.EditValue = supplier.TELPHONE?.ToString();//联系人
            txtEmail.EditValue = supplier.EMAIL?.ToString();//电子邮件
            txtFax.EditValue = supplier.FAX?.ToString();//传真
            txtZIPCODE.EditValue = supplier.ZIPCODE?.ToString();//邮编
            txtADDRESS.EditValue = supplier.ADDRESS?.ToString();//联系地址
            txtWebSite.EditValue = supplier.WebSIte?.ToString();//官网
            txtCMT.EditValue = supplier.CMT?.ToString();//备注

            BindSupplierTKey();//供应商分组
        }

        /// <summary>
        /// 更新方法
        /// </summary>
        /// <returns>返回一个供应商实体</returns>
        public BCOR_SUPPLIER UpdateUI()
        {
            supplier.SUPPLIER_CODE = txtSupplierCode.EditValue?.ToString();
            supplier.SUPPLIER_NAME = txtSupplierName.EditValue?.ToString();
            supplier.SUPPGRP_TKEY = txtSupplierGroup.EditValue?.ToString();
            supplier.SUPPLIER_SHORTNAME = txtSupplierSubName.EditValue?.ToString();

            supplier.CONTACT_PEOPLE = txtContactPepole.EditValue?.ToString();
            supplier.TELPHONE = txtTelphone.EditValue?.ToString();
            supplier.EMAIL = txtEmail.EditValue?.ToString();
            supplier.FAX = txtFax.EditValue?.ToString();
            supplier.ZIPCODE = txtZIPCODE.EditValue?.ToString();
            supplier.ADDRESS = txtADDRESS.EditValue?.ToString();
            supplier.WebSIte = txtWebSite.EditValue?.ToString();

            supplier.CMT = txtCMT.EditValue?.ToString();

            
            return supplier;
        }

        /// <summary>
        /// 绑定供应商分组下拉框
        /// </summary>
        public void BindSupplierTKey()
        {
            BHelper.BindGridLookUpEdit_Supplier(txtSupplierGroup);
        }

        //多列模糊查询
        private void txtSupplierGroup_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));

        }

        private void txtSupplierGroup_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }
    }
}
