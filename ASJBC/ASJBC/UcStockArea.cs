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
    /// 工厂组织 - 库房管理 - 库区维护
    /// </summary>
    public partial class UcStockArea : BaseUserControl
    {
        //实例化帮助类
        ASJBCOR_Stock BHelper = new ASJBCOR_Stock();

        //声明实体
        private BCOR_STOCK_AREA stockarea;

        /// <summary>
        /// 控件加载
        /// </summary>
        public UcStockArea()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_stockarea"></param>
        public UcStockArea(BCOR_STOCK_AREA _stockarea) :this()
        {
            stockarea = _stockarea;
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcStockArea_Load(object sender, EventArgs e)
        {
            txtSTOCK_AREA_CODE.EditValue = stockarea.STOCK_AREA_CODE?.ToString();//库区代码:
            txtSTOCK_AREA_NAME.EditValue = stockarea.STOCK_AREA_NAME?.ToString();//库区名称
            txtSTOCK_TKEY.EditValue = stockarea.STOCK_TKEY?.ToString();//所属库房
            txtSTOCKAREA_BELONGTO_TYPE.EditValue = stockarea.STOCKAREA_BELONGTO_TYPE?.ToString();//库房归属标识
            txtORGANIZATION_TKEY.EditValue = stockarea.ORGANIZATION_TKEY?.ToString();//所属生产组织:
            txtSUPPLIER_BELONG_TKEY.EditValue = stockarea.SUPPLIER_BELONG_TKEY?.ToString();//所属供应商:
            txtBELONG_CUSTOMER_TKEY.EditValue = stockarea.BELONG_CUSTOMER_TKEY?.ToString();//所属客户:
            txtADMIN_EMPL_TKEY.EditValue = stockarea.ADMIN_EMPL_TKEY?.ToString();//库区负责人:
            txtDEFAULTSITE_TKEY.EditValue = stockarea.DEFAULTSITE_TKEY?.ToString();//默认库位
            txtADDRESS.EditValue = stockarea.ADDRESS?.ToString();//库区地址

            chALLOW_LOCK_FLAG.EditValue = stockarea.ALLOW_LOCK_FLAG.ToString() == "" ? 0 : int.Parse(stockarea.ALLOW_LOCK_FLAG.ToString());//允许锁库
            chALLOW_NAGASTOCK_FLAG.EditValue = stockarea.ALLOW_NAGASTOCK_FLAG.ToString() == "" ? 0 : int.Parse(stockarea.ALLOW_NAGASTOCK_FLAG.ToString());//允许负库存
            chALLOW_EWARNING_FLAG.EditValue = stockarea.ALLOW_EWARNING_FLAG.ToString() == "" ? 0 : int.Parse(stockarea.ALLOW_EWARNING_FLAG.ToString());//允许预警


            txtCMT.EditValue = stockarea.CMT?.ToString();//备注

            BindGrdLookUpEdit();//绑定下拉框的值 (所属生产组织，所属供应商，所属客户,所属库房)
            BindLookUpEdit();//绑定下拉框的值 (系统数据字典)
        }

        /// <summary>
        /// 更新方法
        /// </summary>
        /// <returns></returns>
        public BCOR_STOCK_AREA UpdateUI()
        {
            stockarea.STOCK_AREA_CODE = txtSTOCK_AREA_CODE.EditValue?.ToString();
            stockarea.STOCK_AREA_NAME = txtSTOCK_AREA_NAME.EditValue?.ToString();
            stockarea.STOCK_TKEY = txtSTOCK_TKEY.EditValue?.ToString();
            stockarea.STOCKAREA_BELONGTO_TYPE = txtSTOCKAREA_BELONGTO_TYPE.EditValue?.ToString();
            stockarea.ORGANIZATION_TKEY = txtORGANIZATION_TKEY.EditValue?.ToString();
            stockarea.SUPPLIER_BELONG_TKEY = txtSUPPLIER_BELONG_TKEY.EditValue?.ToString();
            stockarea.BELONG_CUSTOMER_TKEY = txtBELONG_CUSTOMER_TKEY.EditValue?.ToString();
            stockarea.ADMIN_EMPL_TKEY = txtADMIN_EMPL_TKEY.EditValue?.ToString();
            stockarea.DEFAULTSITE_TKEY = txtDEFAULTSITE_TKEY.EditValue?.ToString();
            stockarea.ADDRESS = txtADDRESS.EditValue?.ToString();

            stockarea.ALLOW_LOCK_FLAG = int.Parse(chALLOW_LOCK_FLAG?.EditValue.ToString());
            stockarea.ALLOW_EWARNING_FLAG = int.Parse(chALLOW_EWARNING_FLAG?.EditValue.ToString());
            stockarea.ALLOW_NAGASTOCK_FLAG = int.Parse(chALLOW_NAGASTOCK_FLAG?.EditValue.ToString());

            stockarea.CMT = txtCMT.EditValue?.ToString();

            stockarea.LOCK_FLAG = 0;

            return stockarea;
        }

        #region 下拉框绑定
        /// <summary>
        /// 绑定GridLookUpEdit的数据源
        /// </summary>
        public void BindGrdLookUpEdit()
        {
            List<GridLookUpEdit> Control = new List<GridLookUpEdit>();
            Control.Add(txtORGANIZATION_TKEY);//生产组织
            Control.Add(txtSUPPLIER_BELONG_TKEY);//所属供应商
            Control.Add(txtBELONG_CUSTOMER_TKEY);//所属客户
            Control.Add(txtADMIN_EMPL_TKEY);//库区负责人
            Control.Add(txtSTOCK_TKEY);//所属库房
            Control.Add(txtDEFAULTSITE_TKEY);//默认库位
            BHelper.BindGridLookUpEdit_StockArea(Control);
        }

        /// <summary>
        /// 绑定LookUpEdit的数据源
        /// </summary>
        public void BindLookUpEdit()
        {
            List<LookUpEdit> Control = new List<LookUpEdit>();
            List<string> Para = new List<string>();

            //库区归属标识
            Control.Add(txtSTOCKAREA_BELONGTO_TYPE);
            Para.Add("BCOR_STOCK_AREA_STOCKAREA_BELONGTO_TYPE");

            BHelper.BindLookUpEdit(Control, Para);
        }
        #endregion

        #region 控件触发事件
        private void txtSTOCKAREA_BELONGTO_TYPE_EditValueChanged(object sender, EventArgs e)
        {
            string Flag = txtSTOCKAREA_BELONGTO_TYPE.EditValue?.ToString();
            switch (Flag)
            {
                case "0":
                    txtORGANIZATION_TKEY.Enabled = true;
                    txtSUPPLIER_BELONG_TKEY.Enabled = false;
                    txtBELONG_CUSTOMER_TKEY.Enabled = false;

                    txtSUPPLIER_BELONG_TKEY.EditValue = string.Empty;
                    txtBELONG_CUSTOMER_TKEY.EditValue = string.Empty;
                    break;
                case "1":
                    txtORGANIZATION_TKEY.Enabled = false;
                    txtSUPPLIER_BELONG_TKEY.Enabled = true;
                    txtBELONG_CUSTOMER_TKEY.Enabled = false;

                    txtORGANIZATION_TKEY.EditValue = string.Empty;
                    txtBELONG_CUSTOMER_TKEY.EditValue = string.Empty;
                    break;
                case "2":
                    txtORGANIZATION_TKEY.Enabled = false;
                    txtSUPPLIER_BELONG_TKEY.Enabled = false;
                    txtBELONG_CUSTOMER_TKEY.Enabled = true;

                    txtORGANIZATION_TKEY.EditValue = string.Empty;
                    txtSUPPLIER_BELONG_TKEY.EditValue = string.Empty;
                    break;
                default:
                    txtORGANIZATION_TKEY.Enabled = false;
                    txtSUPPLIER_BELONG_TKEY.Enabled = false;
                    txtBELONG_CUSTOMER_TKEY.Enabled = false;
                    break;
            }
        }

        #region 多列模糊查询

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
        #endregion

        #endregion

    }
}
