// ================================================== //
//
// ASJMM_Purchase.cs
//
// 仓库物料管理 （采购 请购）
//
// 涉及到的窗体 ： 
//      UcPurchase.cs -- 采购单管理  
//      UcPurchaseMAP.cs --  请购转采购 单据转换
//      FrmPurchaseMAP.cs -- 请购转采购 单据转换子窗体
//      UcPurchaseREQ.cs -- 请购单管理
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
    public class ASJMM_Purchase : ASJMM_Base
    {
        Result rs = new Result();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GridControl"></param>
        /// <param name="GridView"></param>
        /// <param name="DBNAME"></param>
        /// <param name="TKEY"></param>
        override public void BindDataSourceForGridControl(GridControl GridControl, GridView GridView, string TKEY)
        {
            string strsql = $@" Select T1.*,T2.SOURCE_ORDER_TKEY TKEY_REQ,T2.SOURCE_ORDER_NO REQUEST_NO,T2.SOURCE_ORDER_D_TKEY TKEY_REQ_D FROM
                                (SELECT T1.*,
                                T2.MATERIAL_CODE,T2.MATERIAL_NAME,T2.MAPID FROM
                                (SELECT T1.*,
                                T2.CONCESSION_RECEIVE_FLAG,
                                T2.PURCHASE_RETURN_FLAG,
                                T2.DELIVERY_ACTIVE_FLAG,
                                T2.IQC_FLAG,
                                T2.SUPPLIER_LOT_FLAG
                                FROM MMSMM_PURCHASE_D T1
                                LEFT JOIN MMSMM_PURCHASE_D_RULE T2 ON
                                T1.TKEY = T2.PURCHASE_D AND T1.FLAG = T2.FLAG
                                WHERE T1.FLAG = 1) T1
                                LEFT JOIN BCMA_MATERIAL T2 ON T1.MATERIAL_TKEY = T2.TKEY AND T1.FLAG = T2.FLAG
                                WHERE T1.FLAG = 1) T1
                                left join MMSMM_PURCHASE_MAP T2 ON T1.CKEY = T2.PURCHASE_TKEY 
                                WHERE T1.FLAG = 1 and T1.CKEY = '{TKEY}'";
            base.BindDataSourceForGridControl(GridControl, GridView, QueryBindGridView(strsql).Ds.Tables[0]);//绑定GridControl

        }

        #region 采购
        /// <summary>
        /// LoadData
        /// </summary>
        /// <param name="TKEY"></param>
        /// <returns></returns>
        public DataSet PurchaseLoad (string TKEY)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add($"SELECT * FROM MMSMM_PURCHASE WHERE FLAG  = 1 AND TKEY = '{TKEY}'");
            lsttablaname.Add("MMSMM_PURCHASE");
            return base.FrmDataLoad(lststrsql, lsttablaname);
        }

        /// <summary>
        /// QueryData 出现主从关系的两个表的时候使用  关联字段Ckey 存在多个条件时使用
        /// </summary>
        /// <param name="DBNAME"></param>
        /// <param name="TKEY"></param>
        /// <param name="ParaMeter">为了区分是3个参数 标明属于哪个单据</param>
        /// <returns></returns>
        public Result QueryForPurchase(string DBNAME, List<string> TKEY, string ParaMeter)
        {
            string sql = $@" 
                            SELECT
	                            T1.REQUEST_NO,
	                            T2.* 
                            FROM
	                            MMSMM_PURCHASE_REQ T1
	                            LEFT JOIN (
                            SELECT
	                            T1.*,
	                            T2.MATERIAL_CODE,
	                            T2.MATERIAL_NAME,
	                            T2.MAPID 
                            FROM
	                            MMSMM_PURCHASE_REQ_D T1
	                            LEFT JOIN BCMA_MATERIAL T2 ON T1.MATERIAL_TKEY = T2.TKEY 
	                            AND T1.FLAG = T2.FLAG 
                            WHERE
	                            T1.FLAG = 1 
	                            AND CKEY IN ( '{string.Join("','", TKEY.ToArray())}' ) 
                            ORDER BY
	                            T1.CRE_TIME 
	                            ) T2 ON T1.TKEY = T2.CKEY 
	                            AND T1.FLAG = T2.FLAG 
                            WHERE
	                            T1.FLAG = 1";
            DataSet ds = OracleHelper.Query(sql);
            rs.Ds = ds;
            rs.Msg = "Success";
            rs.Status = true;
            return rs;
        }

        /// <summary>
        /// 绑定GridLookUpEdit下拉框的值
        /// </summary>
        /// <param name="GridLookUpEdit"></param>
        public void BindGridLookUpEdit_Purchase(List<GridLookUpEdit> GridLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqldept);//采购部门
            lststrsql.Add(sqlemp);//采购职员
            lststrsql.Add(sqlsupplier);//供应商
            base.BindGridLookUpEdit(lststrsql, GridLookUpEdit);
        }

        /// <summary>
        /// 绑定RepositoryItemGridLookUpEdit下拉框的值
        /// </summary>
        /// <param name="RepositoryItemGridLookUpEdit"></param>
        public void BindReGridLookUpEdit_Purcahse(List<RepositoryItemGridLookUpEdit> RepositoryItemGridLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqlmaterial);//物料
            base.BindReGridLookUpEdit(lststrsql, RepositoryItemGridLookUpEdit);
        }

        /// <summary>
        /// 绑定RepositoryItemLookUpEdit下拉框的值
        /// </summary>
        /// <param name="RepositoryItemLookUpEdit"></param>
        public void BindReLookUpEdit_Purchase(List<RepositoryItemLookUpEdit> RepositoryItemLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqlunit);//计量单位
            base.BindReLookUpEdit(lststrsql, RepositoryItemLookUpEdit);
        }

        /// <summary>
        /// 子窗体Datatable转换后 绑定到GridControl
        /// </summary>
        /// <param name="dt"></param>
        public DataTable ConvertToGridControl(DataSet ds,DataTable dt,string PurChaseTKEY)
        {
            dt.Columns["REQUEST_QTY"].ColumnName = "REQUEST_QTY_REQ";//请购数量列重命名 两张表字段名一样 重命名区分
            dt.Columns["TKEY"].ColumnName = "TKEY_REQ_D";//请购单明细主键重命名
            dt.Columns["CKEY"].ColumnName = "TKEY_REQ";//请购单主表主键重命名

            dt.Columns.Add("TKEY", typeof(String));//采购单明细主键 
            dt.Columns.Add("CKEY", typeof(String));//采购单主表主键 
            dt.Columns.Add("REQUEST_QTY", typeof(int));//采购单主表主键 

            foreach (DataRow dr in dt.Rows)
            {
                dr["TKEY"] = Guid.NewGuid().ToString();//采购单明细主键
                dr["CKEY"] = ds.Tables["MMSMM_PURCHASE"].Rows.Count == 0 ? PurChaseTKEY : ds.Tables["MMSMM_PURCHASE"].Rows[0]["TKEY"].ToString();//采购单主表主键
                dr["PURCHASE_D_STATUS"] = 1;//明细状态
                dr["REQUEST_QTY"] = 0;//采购数量
                dr["CONCESSION_RECEIVE_FLAG"] = 0;//允许让步接收标识
                dr["PURCHASE_RETURN_FLAG"] = 0;//允许退料标识
                dr["DELIVERY_ACTIVE_FLAG"] = 0;//到货接收启用标识
                dr["IQC_FLAG"] = 0;//启用来料检验标识
                dr["SUPPLIER_LOT_FLAG"] = 0;//启用供应商批次标识

            }

            return dt;
        }

        /// <summary>
        /// 查询采购订单转换映射 
        /// </summary>
        /// <param name="PURCHASE_TKEY"></param>
        public DataTable QueryPurchaseMAP(string PURCHASE_TKEY)
        {
            string sql = $@"SELECT * FROM  MMSMM_PURCHASE_MAP  WHERE FLAG = 1  and PURCHASE_TKEY= '{PURCHASE_TKEY}'";
            return OracleHelper.Query(sql).Tables[0];
        }


        public DataTable QueryPurchaseRULE(string PURCHASE_TKEY)
        {
            string sql = $@"SELECT * FROM  MMSMM_PURCHASE_MAP  WHERE FLAG = 1  and PURCHASE_TKEY= '{PURCHASE_TKEY}'";
            return OracleHelper.Query(sql).Tables[0];
        }

        #endregion

        #region 请购
        /// <summary>
        /// LoadData
        /// </summary>
        /// <param name="TKEY"></param>
        /// <returns></returns>
        public DataSet PurchaseREQLoad(string TKEY)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add($"SELECT * FROM MMSMM_PURCHASE_REQ WHERE FLAG  = 1 AND TKEY = '{TKEY}'");
            lsttablaname.Add("MMSMM_PURCHASE_REQ");
            return base.FrmDataLoad(lststrsql, lsttablaname);
        }

        /// <summary>
        /// 绑定GridLookUpEdit下拉框的值
        /// </summary>
        /// <param name="GridLookUpEdit"></param>
        public void BindGridLookUpEdit_PurchaseREQ(List<GridLookUpEdit> GridLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqldept);//采购部门
            lststrsql.Add(sqlemp);//采购职员
            lststrsql.Add(sqlsupplier);//供应商
            base.BindGridLookUpEdit(lststrsql, GridLookUpEdit);
        }
        /// <summary>
        /// 绑定RepositoryItemGridLookUpEdit下拉框的值
        /// </summary>
        /// <param name="RepositoryItemGridLookUpEdit"></param>
        public void BindReGridLookUpEdit_PurcahseREQ(List<RepositoryItemGridLookUpEdit> RepositoryItemGridLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqlmaterial);//物料
            base.BindReGridLookUpEdit(lststrsql, RepositoryItemGridLookUpEdit);
        }

        /// <summary>
        /// 绑定RepositoryItemLookUpEdit下拉框的值
        /// </summary>
        /// <param name="RepositoryItemLookUpEdit"></param>
        public void BindReLookUpEdit_PurchaseREQ(List<RepositoryItemLookUpEdit> RepositoryItemLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqlunit);//计量单位
            base.BindReLookUpEdit(lststrsql, RepositoryItemLookUpEdit);
        }

        #endregion
    }
}
