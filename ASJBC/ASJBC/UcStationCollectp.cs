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
using DevExpress.Utils;

namespace ASJ.BCOR
{
    /// <summary>
    /// 工位采集节点
    /// </summary>
    public partial class UcStationCollectp : BaseUserControl
    {
        ASJBCOR_ORG BHelper = new ASJBCOR_ORG();
        //声明实体
        private BCOR_STATION_COLLECTP stationcollectp;


        /// <summary>
        /// 加载
        /// </summary>
        public UcStationCollectp()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_workorg"></param>
        public UcStationCollectp(BCOR_STATION_COLLECTP _stationcollectp) : this()
        {
            stationcollectp = _stationcollectp;
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcStationCollectp_Load(object sender, EventArgs e)
        {
            txtCOLLECTP_CODE.EditValue = stationcollectp.COLLECTP_CODE?.ToString();//编码
            txtCOLLECTP_NAME.EditValue = stationcollectp.COLLECTP_NAME?.ToString();//名称
            txtSTATION_TKEY.EditValue = stationcollectp.STATION_TKEY?.ToString();//所属工位
            txtCOLLECTP_IP.EditValue = stationcollectp.COLLECTP_IP?.ToString();//IP地址
            txtCOLLECTP_MAC.EditValue = stationcollectp.COLLECTP_MAC?.ToString();//MAC地址
            txtCMT.EditValue = stationcollectp.CMT?.ToString();//备注

            BindGridLookUpEdit();//所属工位
        }

        /// <summary>
        /// 更新方法
        /// </summary>
        /// <returns></returns>
        public BCOR_STATION_COLLECTP UpdateUI()
        {
            stationcollectp.COLLECTP_CODE = txtCOLLECTP_CODE.EditValue?.ToString();
            stationcollectp.COLLECTP_NAME = txtCOLLECTP_NAME.EditValue?.ToString();
            stationcollectp.STATION_TKEY = txtSTATION_TKEY.EditValue?.ToString();
            stationcollectp.COLLECTP_IP = txtCOLLECTP_IP.EditValue?.ToString();
            stationcollectp.COLLECTP_MAC = txtCOLLECTP_MAC.EditValue?.ToString();
            stationcollectp.CMT = txtCMT.EditValue?.ToString();

            return stationcollectp;
        }

        public void BindGridLookUpEdit()
        {
            BHelper.BindGridLookUpEdit_StationCollectp(txtSTATION_TKEY);
        }

        //多列模糊查询
        private void txtSTATION_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtSTATION_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }
    }
}
