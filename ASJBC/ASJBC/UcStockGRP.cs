﻿using System;
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
    /// 工厂组织 - 库房管理 - 库房分组
    /// </summary>
    public partial class UcStockGRP : BaseUserControl
    {
        //实例化帮助类
        ASJBCOR_Stock BHelper = new ASJBCOR_Stock();

        //库存状态实体
        private BCOR_STOCKGRP stockgrp;


        /// <summary>
        /// 控件加载
        /// </summary>
        public UcStockGRP()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_stockgrp"></param>
        public UcStockGRP(BCOR_STOCKGRP _stockgrp) : this()
        {
            stockgrp = _stockgrp;
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcStockGRP_Load(object sender, EventArgs e)
        {
            txtSTOCK_CODE.EditValue = stockgrp.STOCKGRP_CODE?.ToString();//库房组代码
            txtSTOCK_NAME.EditValue = stockgrp.STOCKGRP_NAME?.ToString();//库房组名称
            txtGRPADMIN_EMPL_TKEY.EditValue = stockgrp.GRPADMIN_EMPL_TKEY?.ToString();//库房负责人TKEY
            txtF_STOCKGRP_TKEY.EditValue = stockgrp.F_STOCKGRP_TKEY?.ToString();//上级库房组名称
            txtCMT.EditValue = stockgrp.CMT?.ToString();//备注

            BindGridLookUpEdit();//绑定下拉框的值
        }

        /// <summary>
        /// 更新方法
        /// </summary>
        /// <returns></returns>
        public BCOR_STOCKGRP UpdateUI()
        {
            stockgrp.STOCKGRP_CODE = txtSTOCK_CODE.EditValue?.ToString();
            stockgrp.STOCKGRP_NAME = txtSTOCK_NAME.EditValue?.ToString();
            stockgrp.GRPADMIN_EMPL_TKEY = txtGRPADMIN_EMPL_TKEY.EditValue?.ToString();
            stockgrp.F_STOCKGRP_TKEY = txtF_STOCKGRP_TKEY.EditValue?.ToString();
            stockgrp.CMT = txtCMT.EditValue?.ToString();

            return stockgrp;
        }

        /// <summary>
        /// 绑定下拉框
        /// </summary>
        public void BindGridLookUpEdit()
        {
            List<GridLookUpEdit> Control = new List<GridLookUpEdit>();
            Control.Add(txtF_STOCKGRP_TKEY);//库房组
            Control.Add(txtGRPADMIN_EMPL_TKEY);//库房负责人
            BHelper.BindGridLookUpEdit_StockGRP(Control);
        }
        #region 多列模糊查询
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

        private void txtF_STOCKGRP_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtF_STOCKGRP_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }
        #endregion
    }
}
