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
    /// 工厂组织 - 库房管理 - 库存状态维护
    /// </summary>
    public partial class UcStockStatus : BaseUserControl
    {
        ASJBCOR_Stock BHelper = new ASJBCOR_Stock();

        //库存状态实体
        private BCOR_STOCKSTATUS stockstatus;


        /// <summary>
        /// 控件加载
        /// </summary>
        public UcStockStatus()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_stockstatus"></param>
        public UcStockStatus(BCOR_STOCKSTATUS _stockstatus) : this()
        {
            stockstatus = _stockstatus;
        }


        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcStockStatus_Load(object sender, EventArgs e)
        {
            txtSTOCKSTATUS_CODE.EditValue = stockstatus.STOCKSTATUS_CODE?.ToString();//库存状态编码
            txtSTOCKSTATUS_NAME.EditValue = stockstatus.STOCKSTATUS_NAME?.ToString();//库存状态名称
            txtSTOCKSTATUSGRP_TKEY.EditValue = stockstatus.STOCKSTATUSGRP_TKEY?.ToString();//所属库存状态组
            txtCMT.EditValue = stockstatus.CMT?.ToString();//备注

            chALLOWMRP_FLAG.EditValue = stockstatus.ALLOWMRP_FLAG.ToString() == "" ? 0 : int.Parse(stockstatus.ALLOWMRP_FLAG.ToString());//MRP可用标识
            chALLOWUSE_FLAG.EditValue = stockstatus.ALLOWUSE_FLAG.ToString() == "" ? 0 : int.Parse(stockstatus.ALLOWUSE_FLAG.ToString());//可使用标识
            chALLOWSALE_FLAG.EditValue = stockstatus.ALLOWSALE_FLAG.ToString() == "" ? 0 : int.Parse(stockstatus.ALLOWSALE_FLAG.ToString());//可销售标识
            chALLOWPURCHASE_FLAG.EditValue = stockstatus.ALLOWPURCHASE_FLAG.ToString() == "" ? 0 : int.Parse(stockstatus.ALLOWPURCHASE_FLAG.ToString());//可生产领用标识
            chALLOWLOCK_FLAG.EditValue = stockstatus.ALLOWLOCK_FLAG.ToString() == "" ? 0 : int.Parse(stockstatus.ALLOWLOCK_FLAG.ToString());//可锁定标识
            chALLOWWARNING_FLAG.EditValue = stockstatus.ALLOWWARNING_FLAG.ToString() == "" ? 0 : int.Parse(stockstatus.ALLOWWARNING_FLAG.ToString());//可预警标识

            BindStockStatusTKEY();
        }

        /// <summary>
        /// 更新方法
        /// </summary>
        /// <returns></returns>
        public BCOR_STOCKSTATUS UpdateUI()
        {
            stockstatus.STOCKSTATUS_CODE = txtSTOCKSTATUS_CODE.EditValue?.ToString();
            stockstatus.STOCKSTATUS_NAME = txtSTOCKSTATUS_NAME.EditValue?.ToString();
            stockstatus.STOCKSTATUSGRP_TKEY = txtSTOCKSTATUSGRP_TKEY.EditValue?.ToString();
            stockstatus.CMT = txtCMT.EditValue?.ToString();


            stockstatus.ALLOWMRP_FLAG = int.Parse(chALLOWMRP_FLAG.EditValue.ToString());
            stockstatus.ALLOWUSE_FLAG = int.Parse(chALLOWUSE_FLAG.EditValue.ToString());
            stockstatus.ALLOWSALE_FLAG = int.Parse(chALLOWSALE_FLAG.EditValue.ToString());
            stockstatus.ALLOWPURCHASE_FLAG = int.Parse(chALLOWPURCHASE_FLAG.EditValue.ToString());
            stockstatus.ALLOWLOCK_FLAG = int.Parse(chALLOWLOCK_FLAG.EditValue.ToString());
            stockstatus.ALLOWWARNING_FLAG = int.Parse(chALLOWWARNING_FLAG.EditValue.ToString());

            return stockstatus; 
        }


        /// <summary>
        /// 绑定库存状态分组下拉框
        /// </summary>
        public void BindStockStatusTKEY()
        {
            BHelper.BindGridLookUpEdit_StockStatus(txtSTOCKSTATUSGRP_TKEY);

        }

        //多列模糊处查询
        private void txtSTOCKSTATUSGRP_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtSTOCKSTATUSGRP_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

    }
}
