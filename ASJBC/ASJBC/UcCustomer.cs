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
    /// 工厂组织 - 供应链 - 客户编辑管理
    /// </summary>
    public partial class UcCustomer : BaseUserControl
    {
        ASJBCOR_ORG BHelper = new ASJBCOR_ORG();
        Result rs = new Result();

        /// <summary>
        /// 客户实体
        /// </summary>
        private BCOR_CUSTOMER Customer;

        /// <summary>
        /// 客户编辑
        /// </summary>
        public UcCustomer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_Customer">客户实体</param>
        public UcCustomer(BCOR_CUSTOMER _Customer) : this()
        {
            Customer = _Customer;
        }

        /// <summary>
        ///  初始化加载
        /// </summary>
        private void UcCustomer_Load(object sender, EventArgs e)
        {
            txtCustomerCode.EditValue = Customer.CUSTOMER_CODE?.ToString();//客户编码
            txtCustomerName.EditValue = Customer.CUSTOMER_NAME?.ToString();//客户名称
            txtCustomerGroup.EditValue = Customer.CUSTOMER_TKEY?.ToString();//客户分组
            txtCustomerSubName.EditValue = Customer.CUSTOMER_SHORTNAME?.ToString();//客户简称
            txtContactPepole.EditValue = Customer.CONTACT_PEOPLE?.ToString();//联系电话
            txtEmail.EditValue = Customer.EMAIL?.ToString();//电子邮件
            txtFax.EditValue = Customer.FAX?.ToString();//传真
            txtZIPCODE.EditValue = Customer.ZIPCODE?.ToString();//邮编
            txtADDRESS.EditValue = Customer.ADDRESS?.ToString();//联系地址
            txtWebSite.EditValue = Customer.WebSIte?.ToString();//官网
            txtCMT.EditValue = Customer.CMT?.ToString();//备注

            BindCustomerTKey();
        }

        /// <summary>
        /// 更新方法
        /// </summary>
        /// <returns>返回一个客户实体</returns>
        public BCOR_CUSTOMER UpdateUI()
        {
            Customer.CUSTOMER_CODE = txtCustomerCode.EditValue?.ToString();
            Customer.CUSTOMER_NAME = txtCustomerName.EditValue?.ToString();
            Customer.CUSTOMER_TKEY = txtCustomerGroup.EditValue?.ToString();
            Customer.CUSTOMER_SHORTNAME = txtCustomerSubName.EditValue?.ToString();

            Customer.CONTACT_PEOPLE = txtContactPepole.EditValue?.ToString();
            Customer.EMAIL = txtEmail.EditValue?.ToString();
            Customer.FAX = txtFax.EditValue?.ToString();
            Customer.ZIPCODE = txtZIPCODE.EditValue?.ToString();
            Customer.ADDRESS = txtADDRESS.EditValue?.ToString();
            Customer.WebSIte = txtWebSite.EditValue?.ToString();

            Customer.CMT = txtCMT.EditValue?.ToString();

            return Customer;
        }


        /// <summary>
        /// 绑定客户分组下拉框
        /// </summary>
        public void BindCustomerTKey()
        {
            BHelper.BindGridLookUpEdit_Customer(txtCustomerGroup);
        }

        //多列模糊查询
        private void txtCustomerGroup_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtCustomerGroup_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }
    }
}
