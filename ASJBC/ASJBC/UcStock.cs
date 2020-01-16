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
using ASJ.FRAMEWORK.Basic;
using ASJ.FRAMEWORK.RecursionTable;

namespace ASJ.BCOR
{
    /// <summary>
    /// 工厂组织 - 库房管理 - 库房维护
    /// </summary>
    public partial class UcStock : BaseUserControl
    {
        //实例化帮助类
        ASJBCOR_Stock BHelper = new ASJBCOR_Stock();

        //声明实体
        private BCOR_STOCK stock;

        private string StockTkey;//主键

        /// <summary>
        /// 控件加载
        /// </summary>
        public UcStock()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_stock"></param>
        public UcStock(BCOR_STOCK _stock):this()
        {
            stock = _stock;
        }


        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcStock_Load(object sender, EventArgs e)
        {
            txtSTOCK_CODE.EditValue = stock.STOCK_CODE?.ToString();//库房代码:
            txtSTOCK_NAME.EditValue = stock.STOCK_NAME?.ToString();//库房名称
            txtSTOCK_TYPE.EditValue = stock.STOCK_TYPE?.ToString();//库房类型
            txtSTOCKGRP_TKEY.EditValue = stock.STOCKGRP_TKEY?.ToString();//库房组
            txtSTOCK_BELONGTO_TYPE.EditValue = stock.STOCK_BELONGTO_TYPE?.ToString();//库房归属标识
            txtWORKORGAN_TKEY.EditValue = stock.WORKORGAN_TKEY?.ToString();//所属生产组织:
            txtSUPPLIER_BELONG_TKEY.EditValue = stock.SUPPLIER_BELONG_TKEY?.ToString();//所属供应商:
            txtBELONG_CUSTOMER_TKEY.EditValue = stock.BELONG_CUSTOMER_TKEY?.ToString();//所属客户:
            txtADMIN_EMPL_TKEY.EditValue = stock.ADMIN_EMPL_TKEY?.ToString();//库房负责人:
            txtDEFAULTSITE_TKEY.EditValue = stock.DEFAULTSITE_TKEY?.ToString();//默认库位
            chJOIN_PICKING_FLAG.EditValue = stock.TKEY == null ? 1 : int.Parse(stock.JOIN_PICKING_FLAG.ToString());//参与拣货
            txtPICKING_SEQ.EditValue = stock.PICKING_SEQ == null ? string.Empty : stock.PICKING_SEQ.ToString();//拣货优先级
            txtADDRESS.EditValue = stock.ADDRESS?.ToString();//库房地址s

            chALLOW_PDA_FLSG.EditValue = stock.ALLOW_PDA_FLSG.ToString() == "" ? 0 : int.Parse(stock.ALLOW_PDA_FLSG.ToString());//允许PDA采集
            chALLOW_STOCKAREA_FLAG.EditValue = stock.ALLOW_STOCKAREA_FLAG.ToString() == "" ? 0 : int.Parse(stock.ALLOW_STOCKAREA_FLAG.ToString());//允许启用库区
            chALLOW_NAGASTOCK_FLAG.EditValue = stock.ALLOW_NAGASTOCK_FLAG.ToString() == "" ? 0 : int.Parse(stock.ALLOW_NAGASTOCK_FLAG.ToString());//允许负库存
            chALLOW_LOCK_FLAG.EditValue = stock.ALLOW_LOCK_FLAG.ToString() == "" ? 0 : int.Parse(stock.ALLOW_LOCK_FLAG.ToString());//允许锁库
            chALLOW_IQC_FLAG.EditValue = stock.ALLOW_IQC_FLAG.ToString() == "" ? 0 : int.Parse(stock.ALLOW_IQC_FLAG.ToString());//采购入库检验标识
            chALLOW_MRP_FLAG.EditValue = stock.ALLOW_MRP_FLAG.ToString() == "" ? 0 : int.Parse(stock.ALLOW_MRP_FLAG.ToString());//允许MRP
            chALLOW_OQC_FLAG.EditValue = stock.ALLOW_OQC_FLAG.ToString() == "" ? 0 : int.Parse(stock.ALLOW_OQC_FLAG.ToString());//销售出库检验启用标识
            chALLOW_EWARNING_FLAG.EditValue = stock.ALLOW_EWARNING_FLAG.ToString() == "" ? 0 : int.Parse(stock.ALLOW_EWARNING_FLAG.ToString());//允许预警
            chINITCHECK_FINISH_FLAG.EditValue = stock.INITCHECK_FINISH_FLAG.ToString() == "" ? 0 : int.Parse(stock.INITCHECK_FINISH_FLAG.ToString());//初始化盘库完成标识

            txtCMT.EditValue = stock.CMT?.ToString();//备注

            BindGrdLookUpEdit();//绑定下拉框的值 (所属生产组织，所属供应商，所属客户,库房组)
            BindLookUpEdit();//绑定下拉框的值 (系统数据字典)
        }

        /// <summary>
        /// 更新方法
        /// </summary>
        /// <returns></returns>
        public BCOR_STOCK UpdateUI()
        {

            stock.STOCK_CODE = txtSTOCK_CODE.EditValue?.ToString();
            stock.STOCK_NAME = txtSTOCK_NAME.EditValue?.ToString();
            stock.STOCK_TYPE = txtSTOCK_TYPE.EditValue?.ToString();
            stock.STOCKGRP_TKEY = txtSTOCKGRP_TKEY.EditValue?.ToString();
            stock.STOCK_BELONGTO_TYPE = txtSTOCK_BELONGTO_TYPE.EditValue?.ToString();
            stock.WORKORGAN_TKEY = txtWORKORGAN_TKEY.EditValue?.ToString();
            stock.SUPPLIER_BELONG_TKEY = txtSUPPLIER_BELONG_TKEY.EditValue?.ToString();
            stock.BELONG_CUSTOMER_TKEY = txtBELONG_CUSTOMER_TKEY.EditValue?.ToString();
            stock.ADMIN_EMPL_TKEY = txtADMIN_EMPL_TKEY.EditValue?.ToString();
            stock.DEFAULTSITE_TKEY = txtDEFAULTSITE_TKEY.EditValue?.ToString();
            stock.JOIN_PICKING_FLAG = decimal.Parse(chJOIN_PICKING_FLAG?.EditValue.ToString());
            stock.PICKING_SEQ = txtPICKING_SEQ.EditValue.ToString() == "" ? 0 : decimal.Parse(txtPICKING_SEQ.EditValue.ToString());
            stock.ADDRESS = txtADDRESS.EditValue?.ToString();

            stock.ALLOW_PDA_FLSG = decimal.Parse(chALLOW_PDA_FLSG?.EditValue.ToString());
            stock.ALLOW_STOCKAREA_FLAG = decimal.Parse(chALLOW_STOCKAREA_FLAG?.EditValue.ToString());
            stock.ALLOW_NAGASTOCK_FLAG = decimal.Parse(chALLOW_NAGASTOCK_FLAG?.EditValue.ToString());
            stock.ALLOW_LOCK_FLAG = decimal.Parse(chALLOW_LOCK_FLAG?.EditValue.ToString());
            stock.ALLOW_IQC_FLAG = decimal.Parse(chALLOW_IQC_FLAG?.EditValue.ToString());
            stock.ALLOW_MRP_FLAG = decimal.Parse(chALLOW_MRP_FLAG?.EditValue.ToString());
            stock.ALLOW_OQC_FLAG = decimal.Parse(chALLOW_OQC_FLAG?.EditValue.ToString());
            stock.ALLOW_EWARNING_FLAG = decimal.Parse(chALLOW_EWARNING_FLAG?.EditValue.ToString());
            stock.INITCHECK_FINISH_FLAG = decimal.Parse(chINITCHECK_FINISH_FLAG?.EditValue.ToString());

            stock.CMT = txtCMT.EditValue?.ToString();

            return stock;
        }

        #region 下拉框绑定
        /// <summary>
        /// 绑定GridLookUpEdit的数据源
        /// </summary>
        public void BindGrdLookUpEdit()
        {
            List<GridLookUpEdit> Control = new List<GridLookUpEdit>();
            Control.Add(txtWORKORGAN_TKEY);//生产组织
            Control.Add(txtSUPPLIER_BELONG_TKEY);//所属供应商
            Control.Add(txtBELONG_CUSTOMER_TKEY);//所属客户
            Control.Add(txtADMIN_EMPL_TKEY);//库房负责人
            Control.Add(txtDEFAULTSITE_TKEY);//默认库位
            Control.Add(txtSTOCKGRP_TKEY);//库房组
            BHelper.BindGridLookUpEdit_Stock(Control);
        }

        /// <summary>
        /// 绑定LookUpEdit的数据源
        /// </summary>
        public void BindLookUpEdit()
        {
            List<LookUpEdit> Control = new List<LookUpEdit>();
            List<string> Para = new List<string>();

            //库房类型
            Control.Add(txtSTOCK_TYPE);
            Para.Add("BCOR_STOCK_STOCK_TYPE");
            //库房归属标识
            Control.Add(txtSTOCK_BELONGTO_TYPE);
            Para.Add("BCOR_STOCK_STOCK_BELONGTO_TYPE");

            BHelper.BindLookUpEdit(Control, Para);
        }

        #endregion

        #region 控件触发

        /// <summary>
        /// 选择库房归属标识 判断必填项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSTOCK_BELONGTO_TYPE_EditValueChanged(object sender, EventArgs e)
        {
            string Flag = txtSTOCK_BELONGTO_TYPE.EditValue?.ToString();
            switch (Flag)
            {
                case "0":
                    txtWORKORGAN_TKEY.Enabled = true;
                    txtSUPPLIER_BELONG_TKEY.Enabled = false;
                    txtBELONG_CUSTOMER_TKEY.Enabled = false;

                    txtSUPPLIER_BELONG_TKEY.EditValue = string.Empty;
                    txtBELONG_CUSTOMER_TKEY.EditValue = string.Empty;
                    break;
                case "1":
                    txtWORKORGAN_TKEY.Enabled = false;
                    txtSUPPLIER_BELONG_TKEY.Enabled = true;
                    txtBELONG_CUSTOMER_TKEY.Enabled = false;

                    txtWORKORGAN_TKEY.EditValue = string.Empty;
                    txtBELONG_CUSTOMER_TKEY.EditValue = string.Empty;
                    break;
                case "2":
                    txtWORKORGAN_TKEY.Enabled = false;
                    txtSUPPLIER_BELONG_TKEY.Enabled = false;
                    txtBELONG_CUSTOMER_TKEY.Enabled = true;

                    txtWORKORGAN_TKEY.EditValue = string.Empty;
                    txtSUPPLIER_BELONG_TKEY.EditValue = string.Empty;
                    break;
                default:
                    txtWORKORGAN_TKEY.Enabled = false;
                    txtSUPPLIER_BELONG_TKEY.Enabled = false;
                    txtBELONG_CUSTOMER_TKEY.Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// 参与拣货按钮
        /// </summary>dd
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chJOIN_PICKING_FLAG_CheckedChanged(object sender, EventArgs e)
        {
            int Flag = int.Parse(chJOIN_PICKING_FLAG.EditValue?.ToString());
            string PickingSEQ = BHelper.GetPickingSEQ(stock.TKEY == null ? string.Empty : stock.TKEY.ToString());//优先级的值
            switch (Flag)
            {
                case 0:
                    txtPICKING_SEQ.EditValue = string.Empty;
                    txtPICKING_SEQ.Enabled = false;
                    break;

                case 1://选中参与拣货
                    txtPICKING_SEQ.Enabled = true;
                    txtPICKING_SEQ.EditValue = string.Empty;
                    txtPICKING_SEQ.Text = stock.TKEY == null ? "" : PickingSEQ;
                    break;

            }
        }

        /// <summary>
        /// 选择拣货优先级时 Check 同一库房组Key下优先级不能重复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPICKING_SEQ_Modified(object sender, EventArgs e)
        {
            string StockGRPTkey = txtSTOCKGRP_TKEY.EditValue.ToString();//库房组Teky
            int PickingSEQ = txtPICKING_SEQ.EditValue == null ? 0 : int.Parse(txtPICKING_SEQ.EditValue.ToString() == "" ? "0" : txtPICKING_SEQ.EditValue.ToString());//输入的拣货优先级

            //参与拣货
            if (chJOIN_PICKING_FLAG.EditValue.ToString() == "1" && txtPICKING_SEQ.Text.Length > 0)
            {
                if (txtSTOCKGRP_TKEY.EditValue?.ToString() == string.Empty || txtSTOCKGRP_TKEY.EditValue == null)
                {
                    XtraMessageBox.Show("请先选择库房组！", "提示框", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
                }
                else
                {
                    //检查同一库房组是否出现重复的优先级
                    if (!BHelper.CheckDuplicates(StockGRPTkey, PickingSEQ))
                    {
                        string StockName = txtSTOCKGRP_TKEY.Text;
                        XtraMessageBox.Show("库房组:" + StockName + " \n已存在优先级 : " + txtPICKING_SEQ.Text , "提示框", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtPICKING_SEQ.Text = stock.TKEY == null ? "" : BHelper.GetPickingSEQ(stock.TKEY == null ? string.Empty : stock.TKEY.ToString());
                        chJOIN_PICKING_FLAG.EditValue = 0;
                        return;
                    }
                }
            }

        }

        #region 多列模糊查询


        private void txtSTOCKGRP_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtSTOCKGRP_TKEY_Popup(object sender, EventArgs e)
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

        private void txtSUPPLIER_BELONG_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtSUPPLIER_BELONG_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtBELONG_CUSTOMER_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtBELONG_CUSTOMER_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtADMIN_EMPL_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtADMIN_EMPL_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtDEFAULTSITE_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtDEFAULTSITE_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        #endregion

        #endregion


        /*最后保存的时候需要再检查一次拣货部分是否符合规范*/

    }
}


