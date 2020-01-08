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
    /// 工厂组织 - 库房管理 - 库房状态组管理
    /// </summary>
    public partial class UcStockStatusGRP : BaseUserControl
    {
        BCORHelper BHelper = new BCORHelper();
        //库房状态组实体
        private BCOR_STOCKSTATUSGRP stockstatusgrp;

        /// <summary>
        /// 控件加载
        /// </summary>
        public UcStockStatusGRP()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_stockstatucgrp"></param>
        public UcStockStatusGRP(BCOR_STOCKSTATUSGRP _stockstatusgrp) : this()
        {
            stockstatusgrp = _stockstatusgrp;
        }


        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcStockStatusGRP_Load(object sender, EventArgs e)
        {
            txtSTOCKSTATUS_CODE.EditValue = stockstatusgrp.STOCKSTATUS_CODE?.ToString();//库存状态编码
            txtSTOCKSTATUS_NAME.EditValue = stockstatusgrp.STOCKSTATUS_NAME?.ToString();//库存状态名称
            txtF_STATUSGRP_TKEY.EditValue = stockstatusgrp.F_STATUSGRP_TKEY?.ToString();//上级库存状态分组
            txtCMT.EditValue = stockstatusgrp.CMT?.ToString();//备注
            BindStockStatus();//绑定下拉框的值
        }

        /// <summary>
        /// 更新方法
        /// </summary>
        /// <returns></returns>
        public BCOR_STOCKSTATUSGRP UpdateUI()
        {
            stockstatusgrp.STOCKSTATUS_CODE = txtSTOCKSTATUS_CODE.EditValue?.ToString();
            stockstatusgrp.STOCKSTATUS_NAME = txtSTOCKSTATUS_NAME.EditValue?.ToString();
            stockstatusgrp.F_STATUSGRP_TKEY = txtF_STATUSGRP_TKEY.EditValue?.ToString();
            stockstatusgrp.CMT = txtCMT.EditValue?.ToString();

            return stockstatusgrp;
        }

        /// <summary>
        /// 绑定上级库存状态分组下拉框
        /// </summary>
        public void BindStockStatus()
        {
            string sql = @"SELECT TKEY,STOCKSTATUS_NAME,STOCKSTATUS_CODE FROM BCOR_STOCKSTATUSGRP WHERE FLAG = 1 ";
            BHelper.BindGridLookUpEdit(sql, txtF_STATUSGRP_TKEY);
        }

        //多列模糊查询
        private void txtF_STATUSGRP_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));

        }

        private void txtF_STATUSGRP_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }
    }
}
