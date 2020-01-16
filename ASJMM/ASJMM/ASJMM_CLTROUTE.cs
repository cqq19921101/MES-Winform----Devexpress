// ================================================== //
//
// ASJMM_CLTROUTE.cs
//
// 单据路线配置模块
//
// 涉及到的窗体 ： 
//      UcCltNodeBase.cs -- 单据采集节点基础档案  
//      UcCltRoute.cs -- 物料单据采集路线维护 
//      UcOrderType.cs -- 物料单据类型管理
//      UcOTCRConfig.cs -- 物料单据类型采集路线配置
//
// ================================================== //

using ASJ.TOOLS.Basic;
using ASJ.TOOLS.Data;
using DevExpress.Data.Filtering;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ASJMM
{
    public class ASJMM_CLTROUTE : ASJMM_Base
    {

        Result rs = new Result();

        /// <summary>
        /// 绑定GridView的DataSource
        /// </summary>
        /// <param name="GridControl"></param>
        /// <param name="GridView"></param>
        /// <param name="DBNAME"></param>
        /// <param name="TKEY"></param>
        override public void BindDataSourceForGridControl(GridControl GridControl, GridView GridView, string DBNAME, string TKEY)
        {
            string strsql = $@"select T1.*,T2.TKEY,T2.CLTNODE_CODE,T2.CLTNODE_NAME from {DBNAME} T1 
                                   left join MMSMM_CLTNODE_BASE T2 ON T1.CLTNODE_TKEY = T2.TKEY AND T1.FLAG = T2.FLAG
                                   WHERE T1.FLAG = 1 and T1.TKEY = '{TKEY}'";
            GridControl.DataSource = OracleHelper.Query(strsql);//绑定GridControl数据源
            GridView.OptionsView.ColumnAutoWidth = false;//列宽自动
            GridView.OptionsBehavior.Editable = true;//允许编辑
            GridView.OptionsSelection.MultiSelect = true;//可以多选
            GridView.BestFitColumns();

            //显示水平滚动条
            GridView.ScrollStyle = ScrollStyleFlags.LiveHorzScroll | ScrollStyleFlags.LiveVertScroll;
            GridView.HorzScrollVisibility = ScrollVisibility.Always;
        }


        #region 物料单据类型采集路线配置  UcOTCRConfig

        /// <summary>
        /// 选择采集路线编码 带出采集路线名称和GridView的数据源 
        /// </summary>
        /// <param name="GridItem"></param>
        /// <param name="GridView"></param>
        /// <param name="gridlookupedit"></param>
        /// <param name="textedit"></param>
        /// <param name="CLTROUTE_CODE"></param>
        public void BindReGLE_MMSMM_CLTROUTE(GridControl GridItem, GridView GridView, TextEdit txtCLTROUTE_NAME, string CLTROUTE_CODE)
        {
            rs = Query("MMSMM_CLTROUTE", CLTROUTE_CODE);//MMSMM_CLTROUTE数据 物料采集路线主表
            if (rs.Ds.Tables[0].Rows.Count > 0)
            {
                txtCLTROUTE_NAME.EditValue = rs.Ds.Tables[0].Rows[0]["CLTROUTE_NAME"].ToString();
                BindDataSourceForGridControl(GridItem, GridView, "MMSMM_CLTROUTE_SEQ", CLTROUTE_CODE);//绑定GridView

            }

        }


        /// <summary>
        /// 绑定GridLookUpEdit下拉框的值
        /// </summary>
        /// <param name="GridLookUpEdit"></param>
        public void BindGridLookUpEdit_OTCRConfig(List<GridLookUpEdit> GridLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqlordertype);//物料单据类型
            lststrsql.Add(_sqlcltroute);//物料单据采集节点基础档案
            base.BindGridLookUpEdit(lststrsql, GridLookUpEdit);
        }

        #endregion

        #region 物料单据采集路线维护
        /// <summary>
        /// LoadData
        /// </summary>
        /// <param name="TKEY"></param>
        /// <returns></returns>
        public DataSet CLTRouteLoad(string TKEY)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();
            lststrsql.Add($"SELECT * FROM MMSMM_CLTROUTE WHERE FLAG  = 1 AND TKEY = '{TKEY}'");
            lsttablaname.Add("MMSMM_CLTROUTE");
            return base.FrmDataLoad(lststrsql, lsttablaname);
        }

        /// <summary>
        /// 绑定RepositoryItemGridLookUpEdit下拉框的值
        /// </summary>
        /// <param name="RepositoryItemGridLookUpEdit"></param>
        public void BindReGridLookUpEdit_CLTRoute(List<RepositoryItemGridLookUpEdit> RepositoryItemGridLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqlcltroute);//物料单据采集节点基础档案
            base.BindReGridLookUpEdit(lststrsql, RepositoryItemGridLookUpEdit);
        }
        #endregion
    }
}
