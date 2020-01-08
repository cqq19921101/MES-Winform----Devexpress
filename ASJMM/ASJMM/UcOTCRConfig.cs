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
using ASJ.TOOLS;
using ASJ.TOOLS.Basic;
using ASJ.TOOLS.Data;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;

namespace ASJMM
{
    /// <summary>
    /// 物料单据类型采集路线配置
    /// </summary>
    public partial class UcOTCRConfig : BaseUserControl
    {
        //实例化帮助类
        MMSMMHelper MHelper = new MMSMMHelper();
        Result rs = new Result();

        //声明实体
        private MMSMM_ORDERTYPE_CLTROUTE otcrconfig;


        /// <summary>
        /// 控件加载
        /// </summary>
        public UcOTCRConfig()
        {
            InitializeComponent();
            MHelper.BindCustomDrawRowIndicator(GrvCltRoute);
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_otcrconfig"></param>
        public UcOTCRConfig(MMSMM_ORDERTYPE_CLTROUTE _otcrconfig) : this()
        {
            otcrconfig = _otcrconfig;
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcOTCRConfig_Load(object sender, EventArgs e)
        {
            txtORDERTYPE_CODE.EditValue = txtORDERTYPE_CODE.EditValue?.ToString();//单据类型编码
            txtCLTROUTE_CODE.EditValue = txtCLTROUTE_CODE.EditValue?.ToString();//采集路线编码
            txtCMT.EditValue = txtCMT.EditValue?.ToString();//备注
            MHelper.BindSysDict(txtBUSINESS_TYPE, "MMSMM_ORDERTYPE_BUSINESS_TYPE");//绑定业务场景下拉框的值 (系统数据字典)
            BindGridLookUpEdit();//绑定GridControl下拉框的值
            BindGridViewDataSource(txtCLTROUTE_CODE.EditValue?.ToString());//编辑时  绑定Datasource的数据源
        }

        /// <summary>
        /// 更新方法
        /// </summary>
        /// <returns></returns>
        public MMSMM_ORDERTYPE_CLTROUTE UpdateUI()
        {
            otcrconfig.ORDERTYPE_TKEY = txtORDERTYPE_CODE.EditValue?.ToString();
            otcrconfig.CLTROUTE_TKEY = txtCLTROUTE_CODE.EditValue?.ToString();
            otcrconfig.CMT = txtCMT.EditValue?.ToString();

            return otcrconfig;
        }

        /// <summary>
        /// 绑定GridView的数据源
        /// </summary>
        /// <param name="TKEY"></param>
        public void BindGridViewDataSource(string TKEY)
        {
            string SqlGridView = @"select T1.*,T2.TKEY,T2.CLTNODE_CODE,T2.CLTNODE_NAME from MMSMM_CLTROUTE_SEQ T1 
                                   left join MMSMM_CLTNODE_BASE T2 ON T1.CLTNODE_TKEY = T2.TKEY AND T1.FLAG = T2.FLAG
                                   WHERE T1.FLAG = 1 and T1.TKEY = " + "'" + TKEY + "'";
            MHelper.BindDataSourceForGridControl(GridItem, GrvCltRoute, MHelper.QueryBindGridView(SqlGridView).Ds.Tables[0]);//绑定GridControl
        }

        #region 下拉框 GridLookUpEdit 数据绑定
        private void BindGridLookUpEdit()
        {
            List<string> strsql = new List<string>();
            List<GridLookUpEdit> Control = new List<GridLookUpEdit>();
            strsql.Add("SELECT TKEY,ORDERTYPE_NAME,ORDERTYPE_code,BUSINESS_TYPE from MMSMM_ORDERTYPE where  FLAG = 1 ");
            strsql.Add("SELECT TKEY,CLTROUTE_NAME,CLTROUTE_CODE, CASE WHEN CLTROUTE_READ_FLAG = 1 THEN '完成'  END CLTROUTE_READ_FLAG  from MMSMM_CLTROUTE where  FLAG = 1 and  CLTROUTE_READ_FLAG = 1");
            Control.Add(txtORDERTYPE_CODE);
            Control.Add(txtCLTROUTE_CODE);
            MHelper.BindGridLookUpEdit(strsql, Control);
        }
        #endregion

        #region 控件触发事件

        //选择单据类型编码 带出单据类型名称和业务场景  MMSMM_ORDERTYPE
        private void txtORDERTYPE_CODE_EditValueChanged(object sender, EventArgs e)
        {
            string ORDERTYPE_CODE = txtORDERTYPE_CODE.EditValue?.ToString();
            rs = MHelper.Query("MMSMM_ORDERTYPE", ORDERTYPE_CODE);//MMSMM_ORDERTYPE数据 物料单据类型表
            if (rs.Ds.Tables[0].Rows.Count > 0)
            {
                txtORDERTYPE_NAME.EditValue = rs.Ds.Tables[0].Rows[0]["ORDERTYPE_NAME"].ToString();
                txtBUSINESS_TYPE.EditValue = rs.Ds.Tables[0].Rows[0]["BUSINESS_TYPE"].ToString();
            }
        }


        //选择采集路线编码 带出采集路线名称和GridView的数据源  MMSMM_CLTROUTE
        private void txtCLTROUTE_CODE_EditValueChanged(object sender, EventArgs e)
        {
            string CLTROUTE_CODE = txtCLTROUTE_CODE.EditValue?.ToString();
            rs = MHelper.Query("MMSMM_CLTROUTE", CLTROUTE_CODE);//MMSMM_CLTROUTE数据 物料采集路线主表
            if (rs.Ds.Tables[0].Rows.Count > 0)
            {
                txtCLTROUTE_NAME.EditValue = rs.Ds.Tables[0].Rows[0]["CLTROUTE_NAME"].ToString();
                string SqlGridView = @"select T1.*,T2.TKEY,T2.CLTNODE_CODE,T2.CLTNODE_NAME from MMSMM_CLTROUTE_SEQ T1 
                                   left join MMSMM_CLTNODE_BASE T2 ON T1.CLTNODE_TKEY = T2.TKEY AND T1.FLAG = T2.FLAG
                                   WHERE T1.FLAG = 1 and T1.TKEY = " + "'" + CLTROUTE_CODE + "'";
                MHelper.BindDataSourceForGridControl(GridItem, GrvCltRoute, MHelper.QueryBindGridView(SqlGridView).Ds.Tables[0]);//绑定GridView

            }
            else
            {
                XtraMessageBox.Show("无数据", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
            }

        }

        #region 多列模糊查询
        private void txtORDERTYPE_CODE_EditValueChanging(object sender, ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                MHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtCLTROUTE_CODE_EditValueChanging(object sender, ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                MHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtCLTROUTE_CODE_Popup(object sender, EventArgs e)
        {
            MHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtORDERTYPE_CODE_Popup(object sender, EventArgs e)
        {
            MHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        #endregion

        #endregion

    }
}
