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
    /// 工位
    /// </summary>
    public partial class UcStation : BaseUserControl
    {
        ASJBCOR_ORG BHelper = new ASJBCOR_ORG();
        //声明实体
        private BCOR_STATION station;


        /// <summary>
        /// 加载
        /// </summary>
        public UcStation()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_workorg"></param>
        public UcStation(BCOR_STATION _station) : this()
        {
            station = _station;
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcStation_Load(object sender, EventArgs e)
        {
            txtSTATION_CODE.EditValue = station.STATION_CODE?.ToString();//代码
            txtSTATION_NAME.EditValue = station.STATION_NAME?.ToString();//名称
            txtORGANIZATION_TKEY.EditValue = station.ORGANIZATION_TKEY?.ToString();//生产组织
            txtSTATION_TYPE.EditValue = station.STATION_TYPE?.ToString();//工位类型
            txtCMT.EditValue = station.CMT?.ToString();//备注

            BindLookUpEdit();//初始化时加载分组节点
            BindGridLookUpEdit();//绑定生产组织 部门负责人
        }

        /// <summary>
        /// 更新方法
        /// </summary>
        /// <returns></returns>
        public BCOR_STATION UpdateUI()
        {
            station.STATION_CODE = txtSTATION_CODE.EditValue?.ToString();
            station.STATION_NAME = txtSTATION_NAME.EditValue?.ToString();
            station.ORGANIZATION_TKEY = txtORGANIZATION_TKEY.EditValue?.ToString();
            station.STATION_TYPE = txtSTATION_TYPE.EditValue?.ToString();
            station.CMT = txtCMT.EditValue?.ToString();

            return station;
        }

        /// <summary>
        /// 绑定下拉框
        /// </summary>
        public void BindLookUpEdit()
        {
            BHelper.BindSysDict(txtSTATION_TYPE, "BCOR_STATION_STATION_TYPE");
        }


        /// <summary>
        /// 绑定下拉框的值
        /// </summary>
        public void BindGridLookUpEdit()
        {
            BHelper.BindGridLookUpEdit_Station(txtORGANIZATION_TKEY);
        }

        //多列模糊查询
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
    }
}
