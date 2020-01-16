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
using ASJ.TOOLS.Data;

namespace ASJ.BCOR
{
    /// <summary>
    /// 工厂组织 - 库房管理 - 库位维护
    /// </summary>
    public partial class UcStockSite : BaseUserControl
    {
        //实例化帮助类
        ASJBCOR_Stock BHelper = new ASJBCOR_Stock();

        //声明实体
        private BCOR_STOCK_SITE stocksite;


        /// <summary>
        /// 控件加载
        /// </summary>
        public UcStockSite()
        {
            InitializeComponent();
        }

        public UcStockSite(BCOR_STOCK_SITE _stocksite) : this()
        {
            stocksite = _stocksite;
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcStockSite_Load(object sender, EventArgs e)
        {
            txtSTOCK_SITE_CODE.EditValue = stocksite.STOCK_SITE_CODE?.ToString();//库区代码:
            txtSTOCK_SITE_NAME.EditValue = stocksite.STOCK_SITE_NAME?.ToString();//库区名称
            txtSTOCK_TKEY.EditValue = stocksite.STOCK_TKEY?.ToString();//所属库房
            txtSTCOK_AREA_TKEY.EditValue = stocksite.STCOK_AREA_TKEY?.ToString();//所属库区
            txtSTOCKSITE_BELONGTO_TYPE.EditValue = stocksite.STOCKSITE_BELONGTO_TYPE?.ToString();//库位归属标识
            txtORGANIZATION_TKEY.EditValue = stocksite.ORGANIZATION_TKEY?.ToString();//所属生产组织:
            txtSUPPLIER_BELONG_TKEY.EditValue = stocksite.SUPPLIER_BELONG_TKEY?.ToString();//所属供应商:
            txtBELONG_CUSTOMER_TKEY.EditValue = stocksite.BELONG_CUSTOMER_TKEY?.ToString();//所属客户:
            txtADMIN_EMPL_TKEY.EditValue = stocksite.ADMIN_EMPL_TKEY?.ToString();//库位负责人:
            txtADDRESS.EditValue = stocksite.ADDRESS?.ToString();//库位地址
            txtMAX_QTY.EditValue = stocksite.MAX_QTY.ToString() == "" ? 0 : stocksite.MAX_QTY;//最大库存容量



            chALLOW_LOCK_FLAG.EditValue = stocksite.ALLOW_LOCK_FLAG.ToString() == "" ? 0 : int.Parse(stocksite.ALLOW_LOCK_FLAG.ToString());//允许锁库
            chALLOW_NAGASTOCK_FLAG.EditValue = stocksite.ALLOW_NAGASTOCK_FLAG.ToString() == "" ? 0 : int.Parse(stocksite.ALLOW_NAGASTOCK_FLAG.ToString());//允许负库存
            chALLOW_EWARNING_FLAG.EditValue = stocksite.ALLOW_EWARNING_FLAG.ToString() == "" ? 0 : int.Parse(stocksite.ALLOW_EWARNING_FLAG.ToString());//允许预警


            txtCMT.EditValue = stocksite.CMT?.ToString();//备注


            BindGrdLookUpEdit();//绑定下拉框的值 (所属生产组织，所属供应商，所属客户,所属库区)
            BindLookUpEdit();//绑定下拉框的值 (系统数据字典)

        }


        /// <summary>
        /// 更新方法
        /// </summary>
        /// <returns></returns>
        public BCOR_STOCK_SITE UpdateUI()
        {
            stocksite.STOCK_SITE_CODE = txtSTOCK_SITE_CODE.EditValue?.ToString();
            stocksite.STOCK_SITE_NAME = txtSTOCK_SITE_NAME.EditValue?.ToString();
            stocksite.STOCK_TKEY = txtSTOCK_TKEY.EditValue?.ToString();
            stocksite.STCOK_AREA_TKEY = txtSTCOK_AREA_TKEY.EditValue?.ToString();
            stocksite.STOCKSITE_BELONGTO_TYPE = txtSTOCKSITE_BELONGTO_TYPE.EditValue?.ToString();
            stocksite.ORGANIZATION_TKEY = txtORGANIZATION_TKEY.EditValue?.ToString();
            stocksite.SUPPLIER_BELONG_TKEY = txtSUPPLIER_BELONG_TKEY.EditValue?.ToString();
            stocksite.BELONG_CUSTOMER_TKEY = txtBELONG_CUSTOMER_TKEY.EditValue?.ToString();
            stocksite.ADMIN_EMPL_TKEY = txtADMIN_EMPL_TKEY.EditValue?.ToString();
            stocksite.ADDRESS = txtADDRESS.EditValue?.ToString();
            //stocksite.MAX_QTY = int.Parse(txtMAX_QTY.EditValue.ToString() == "" ? "0" : txtMAX_QTY.EditValue.ToString());
            stocksite.MAX_QTY = int.Parse(txtMAX_QTY.EditValue?.ToString());

            stocksite.ALLOW_LOCK_FLAG = int.Parse(chALLOW_LOCK_FLAG?.EditValue.ToString());
            stocksite.ALLOW_EWARNING_FLAG = int.Parse(chALLOW_EWARNING_FLAG?.EditValue.ToString());
            stocksite.ALLOW_NAGASTOCK_FLAG = int.Parse(chALLOW_NAGASTOCK_FLAG?.EditValue.ToString());
            stocksite.CMT = txtCMT.EditValue?.ToString();

            stocksite.LOCK_FLAG = 0;

            return stocksite;
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
            Control.Add(txtBELONG_CUSTOMER_TKEY);//所属客户看
            Control.Add(txtADMIN_EMPL_TKEY);//库位负责人
            Control.Add(txtSTOCK_TKEY);//库房
            BHelper.BindGridLookUpEdit_StockSite(Control);
        }

        /// <summary>
        /// 绑定LookUpEdit的数据源
        /// </summary>
        public void BindLookUpEdit()
        {
            List<LookUpEdit> Control = new List<LookUpEdit>();
            List<string> Para = new List<string>();

            //库位归属标识
            Para.Add("BCOR_STOCK_SITE_STOCKSITE_BELONGTO_TYPE");
            Control.Add(txtSTOCKSITE_BELONGTO_TYPE);

            BHelper.BindLookUpEdit(Control, Para);
        }
        #endregion

        #region 控件触发事件
        private void txtSTOCKAREA_BELONGTO_EditValueChanged(object sender, EventArgs e)
        {
            string Flag = txtSTOCKSITE_BELONGTO_TYPE.EditValue?.ToString();
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
        /// <summary>
        /// 选择库房触发所属库区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSTOCK_TKEY_EditValueChanged(object sender, EventArgs e)
        {
            string StockTkey = txtSTOCK_TKEY.EditValue?.ToString();
            string sql = string.Format("SELECT TKEY,STOCK_AREA_NAME,STOCK_AREA_CODE from BCOR_STOCK_AREA where  FLAG = 1 and STOCK_TKEY = '{0}'", StockTkey);
            DataSet ds = OracleHelper.Query(sql);//指定库房下所属库区
            if (ds.Tables[0].Rows.Count > 0)
            {
                BHelper.BindGridLookUpEdit(sql, txtSTCOK_AREA_TKEY);//绑定库区
            }
            //else
            //{
            //    XtraMessageBox.Show("此库房还未维护库区！", "提示框", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    txtSTCOK_AREA_TKEY.Properties.DataSource = null;
            //    return;
            //}

        }

        #region 多列模糊查询
        private void txtSTOCK_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtSTOCK_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtSTCOK_AREA_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtSTCOK_AREA_TKEY_Popup(object sender, EventArgs e)
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

        #endregion

        #endregion


    }
}
