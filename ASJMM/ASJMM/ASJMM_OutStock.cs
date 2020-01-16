// ================================================== //
//
// ASJMM_OutStock.cs
//
// 材料出库 （出库单 出库申请单）
//
// 涉及到的窗体 ： 
//      UcOutStock.cs -- 出库单管理  
//      UcOutStockREQ.cs --  出库申请单管理
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
    public class ASJMM_OutStock : ASJMM_Base
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GridControl"></param>
        /// <param name="GridView"></param>
        /// <param name="DBNAME"></param>
        /// <param name="TKEY"></param>
        override public void BindDataSourceForGridControl(GridControl GridControl, GridView GridView, string DBNAME, string TKEY)
        {
            base.BindDataSourceForGridControl(GridControl, GridView, DBNAME, TKEY);
        }

        #region 出库单管理
        /// <summary>
        /// LoadData
        /// </summary>
        /// <param name="TKEY"></param>
        /// <returns></returns>
        public DataSet OutStockLoad(string TKEY)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();
            lststrsql.Add($"SELECT * FROM MMSMM_OUTSTOCK WHERE FLAG  = 1 AND TKEY = '{TKEY}'");
            lsttablaname.Add("MMSMM_OUTSTOCK");
            return base.FrmDataLoad(lststrsql, lsttablaname);
        }

        /// <summary>
        /// 绑定GridLookUpEdit下拉框的值
        /// </summary>
        /// <param name="GridLookUpEdit"></param>
        public void BindGridLookUpEdit_OutStock(List<GridLookUpEdit> GridLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqlworg);//生产组织
            lststrsql.Add(sqlsupplier);//所属供应商
            lststrsql.Add(sqlcustomer);//所属客户
            lststrsql.Add(sqldept);//来源部门
            lststrsql.Add(sqlemp);//来源发起人
            lststrsql.Add(sqlstock);//来源库房
            lststrsql.Add(sqlstock);//目标库房
            base.BindGridLookUpEdit(lststrsql, GridLookUpEdit);
        }

        /// <summary>
        /// 绑定RepositoryItemGridLookUpEdit下拉框的值
        /// </summary>
        /// <param name="RepositoryItemGridLookUpEdit"></param>
        public void BindReGridLookUpEdit_OutStock(List<RepositoryItemGridLookUpEdit> RepositoryItemGridLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqlmaterial);//物料
            lststrsql.Add(sqlstock);//目标库房
            lststrsql.Add(sqlstockstatus);//库存状态
            lststrsql.Add(sqlstockstatus);//库存状态
            base.BindReGridLookUpEdit(lststrsql, RepositoryItemGridLookUpEdit);
        }

        /// <summary>
        /// 绑定RepositoryItemLookUpEdit下拉框的值
        /// </summary>
        /// <param name="RepositoryItemLookUpEdit"></param>
        public void BindReLookUpEdit_OutStock(List<RepositoryItemLookUpEdit> RepositoryItemLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqlunit);//计量单位
            lststrsql.Add(sqlunit);//计量单位
            lststrsql.Add(sqlstock);//目标库房
            base.BindReLookUpEdit(lststrsql, RepositoryItemLookUpEdit);
        }
        #endregion

        #region 出库申请单管理
        /// <summary>
        /// LoadData
        /// </summary>
        /// <param name="TKEY"></param>
        /// <returns></returns>
        public DataSet OutStockLoadREQ(string TKEY)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();
            lststrsql.Add($"SELECT * FROM MMSMM_OUTSTOCK_REQ WHERE FLAG  = 1 AND TKEY = '{TKEY}'");
            lsttablaname.Add("MMSMM_OUTSTOCK_REQ");
            return base.FrmDataLoad(lststrsql, lsttablaname);
        }
        #endregion

        #region 创建临时的Datatable

        /// <summary>
        /// 创建Datatable结构  MMSMM_OUTSTOCK_DLOT
        /// </summary>
        public DataTable CreateTempForGrvOutStock()
        {
            DataTable dt = new DataTable("MMSMM_OUTSTOCK_DLOT");
            dt.Columns.Add(new DataColumn("OUTSTOCK_TKEY", typeof(String)));//主表KEY
            dt.Columns.Add(new DataColumn("OUTSTOCK_D_TKEY", typeof(String)));//当前选中行的明细Tkey
            dt.Columns.Add(new DataColumn("MATERIAL_TKEY", typeof(String)));//物料KEY
            dt.Columns.Add(new DataColumn("BASE_UNIT_KEY", typeof(String)));//计量单位
            dt.Columns.Add(new DataColumn("TO_STOCK_KEY", typeof(String)));//目标库房


            return dt;
        }

        /// <summary>
        /// 创建Datatable结构  MMSMM_OUTSTOCK_REQ_DLOT
        /// </summary>
        public DataTable CreateTempForGrvOutStockREQ()
        {
            DataTable dt = new DataTable("MMSMM_OUTSTOCK_REQ_DLOT");
            dt.Columns.Add(new DataColumn("OUTSTOCK_REQ_TKEY", typeof(String)));//主表KEY
            dt.Columns.Add(new DataColumn("OUTSTOCK_REQ_D_TKEY", typeof(String)));//当前选中行的明细Tkey
            dt.Columns.Add(new DataColumn("MATERIAL_TKEY", typeof(String)));//物料KEY
            dt.Columns.Add(new DataColumn("BASE_UNIT_KEY", typeof(String)));//计量单位
            dt.Columns.Add(new DataColumn("TO_STOCK_KEY", typeof(String)));//目标库房


            return dt;
        }

        #endregion

    }
}
