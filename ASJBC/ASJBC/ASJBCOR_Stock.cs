// ================================================== //
//
// ASJBCOR_Stock.cs
//
// 库房管理 （库房 库区 库位 库存状态 库房分组 库存状态分组）
//
// 涉及到的窗体 ： 
//      UcStock.cs -- 库房  
//      UcStockArea.cs --  库区
//      UcStockSite.cs -- 库位
//      UcStockStatus.cs -- 库存状态  
//      UcStockGRP.cs --  库房分组
//      UcStockStatusGRP.cs -- 库存状态分组
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

namespace ASJ.BCOR
{
    public class ASJBCOR_Stock : ASJBCOR_Base
    {
        #region 库房
        public void BindGridLookUpEdit_Stock(List<GridLookUpEdit> GridLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqlworg);//生产组织
            lststrsql.Add(sqlsupplier);//所属供应商
            lststrsql.Add(sqlcustomer);//所属客户
            lststrsql.Add(sqlemp);//库房负责人
            lststrsql.Add(sqlstocksite);//默认库位
            lststrsql.Add(sqlstockgrpnode);//库房组
            base.BindGridLookUpEdit(lststrsql, GridLookUpEdit);
        }

        /// <summary>
        /// 检查同一库房组是否出现重复的优先级
        /// </summary>
        /// <param name="StockGRPTkey">库房组</param>
        /// <param name="PickingSEQ">优先级</param>
        /// <returns></returns>
        public bool CheckDuplicates(string StockGRPTkey, int PickingSEQ)
        {
            string sql = $@"Select TKEY,STOCKGRP_TKEY,STOCK_CODE,STOCK_NAME from BCOR_STOCK where FLAG = 1
                            AND STOCKGRP_TKEY =  '{StockGRPTkey}' and PICKING_SEQ = {PickingSEQ} and JOIN_PICKING_FLAG = 1 ";
            DataTable dt = OracleHelper.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return false;//出现重复
            }
            return true;
        }

        /// <summary>
        /// 抓取当前主键的拣货优先级
        /// </summary>
        /// <param name="TKEY"></param>
        /// <returns></returns>
        public string GetPickingSEQ(string TKEY)
        {
            string sql = $@"Select TKEY,STOCKGRP_TKEY,STOCK_CODE,STOCK_NAME ,PICKING_SEQ from BCOR_STOCK where FLAG = 1
                            AND TKEY = '{TKEY}'";
            DataTable dt = OracleHelper.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["PICKING_SEQ"].ToString();
            }
            return "";
        }

        /// <summary>
        /// GridView DataSource查询绑定  连接查询 
        /// </summary>
        /// <returns></returns>
        override public Result QueryBindGridView(string Sql)
        {
            Result rs = new Result();
            DataSet ds = OracleHelper.Query(Sql);
            rs.Ds = ds;
            rs.Msg = "Success";
            rs.Status = true;
            return rs;
        }

        /// <summary>
        /// 抓取当前库房组下最后的拣货优先级 +1
        /// </summary>
        /// <param name="StockGRPTkey"></param>
        /// <returns></returns>
        public int InsertPickingSEQ(string StockGRPTkey, string Type)
        {
            string sql = $@"Select TKEY,STOCKGRP_TKEY,STOCK_CODE,STOCK_NAME,PICKING_SEQ from BCOR_STOCK where FLAG = 1 AND STOCKGRP_TKEY =  '{StockGRPTkey}' and JOIN_PICKING_FLAG = 1 order by PICKING_SEQ desc ";
            DataTable dt = OracleHelper.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                switch (Type)
                {
                    case "ADD":
                        return int.Parse(dr["PICKING_SEQ"].ToString()) + 1;
                    case "Update":
                        return int.Parse(dr["PICKING_SEQ"].ToString());
                }
            }
            return 0;
        }

        #endregion

        #region 库区
        public void BindGridLookUpEdit_StockArea(List<GridLookUpEdit> GridLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqlworg);//生产组织
            lststrsql.Add(sqlsupplier);//所属供应商
            lststrsql.Add(sqlcustomer);//所属客户
            lststrsql.Add(sqlemp);//库房负责人
            lststrsql.Add(sqlstockallow);//所属库房
            lststrsql.Add(sqlstocksite);//默认库位
            base.BindGridLookUpEdit(lststrsql, GridLookUpEdit);
        }
        #endregion

        #region 库位
        public void BindGridLookUpEdit_StockSite(List<GridLookUpEdit> GridLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqlworg);//生产组织
            lststrsql.Add(sqlsupplier);//所属供应商
            lststrsql.Add(sqlcustomer);//所属客户
            lststrsql.Add(sqlemp);//库房负责人
            lststrsql.Add(sqlstock);//库房
            base.BindGridLookUpEdit(lststrsql, GridLookUpEdit);
        }
        #endregion

        #region 库存状态
        public void BindGridLookUpEdit_StockStatus(GridLookUpEdit GridLookUpEdit)
        {
            base.BindGridLookUpEdit(sqlstockstatusgrpnode, GridLookUpEdit);
        }
        #endregion

        #region 库房分组
        public void BindGridLookUpEdit_StockGRP(List<GridLookUpEdit> GridLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqlstockgrp);//库房组
            lststrsql.Add(sqlemp);//库房负责人
            base.BindGridLookUpEdit(lststrsql, GridLookUpEdit);
        }

        #endregion

        #region 库存状态分组
        public void BindGridLookUpEdit_StockStatusGRP(GridLookUpEdit GridLookUpEdit)
        {
            base.BindGridLookUpEdit(sqlstockstatus, GridLookUpEdit);
        }
        #endregion
    }
}
