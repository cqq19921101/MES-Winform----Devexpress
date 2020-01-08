using ASJ.BCOR;
using ASJ.TOOLS.Basic;
using ASJ.TOOLS.Data;
using DevExpress.XtraEditors;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ASJ.BCOR
{
    public class BCORHelper : BC_Standard
    {
        Result rs = new Result();

        #region 计量单位  UcUnit 

        /// <summary>
        /// 根据计量单位组KEY 查找是否存在该组的计量单位数据 （一个计量单位分组只有一个基准单位）
        /// </summary>
        /// <param name="GRPTKEY">计量单位组Key UNIT_GRP_TKEY </param>
        /// <returns> true :此组别下无基准单位 基准单位框默认选中 反灰    false : 此组别下已存在基准单位 复选框不选中 反灰   </returns>
        public bool CheckBaseUnit(string GRPTKEY)
        {
            string Sql = $@"SELECT * FROM BCDF_UNIT WHERE FLAG = 1 AND UNIT_GRP_TKEY = '{GRPTKEY}' ";
            DataSet ds = OracleHelper.Query(Sql);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check该组下是否是
        /// </summary>
        /// <param name="GRPTKEY">计量单位分组Tkey</param>
        /// <returns></returns>
        public bool CheckBASE_UNIT_FLAG(string GRPTKEY)
        {
            string Sql = $@"SELECT * FROM BCDF_UNIT WHERE FLAG = 1 AND UNIT_GRP_TKEY = '{GRPTKEY}' and BASE_UNIT_FLAG = 1";
            DataSet ds = OracleHelper.Query(Sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GRPTKEY"></param>
        /// <returns></returns>
        public DataTable GetBaseUnitFlag(string GRPTKEY)
        {
            string Sql = $@"SELECT * FROM BCDF_UNIT WHERE FLAG = 1 AND UNIT_GRP_TKEY = '{GRPTKEY}' and BASE_UNIT_FLAG = 1";
            DataSet ds = OracleHelper.Query(Sql);
            return ds.Tables[0];
        }


        /// <summary>
        /// Check是否是根节点
        /// </summary>
        /// <param name="GRPTKEY">计量单位组Key UNIT_GRP_TKEY </param>
        /// <returns> true :没有数据 基准单位框选中 反灰    false : 有数据 复选框不选中 反灰  </returns>
        public bool CheckUnitGRPNODE(string GRPTKEY)
        {
            string Sql = $@"SELECT * FROM BCDF_UNIT_GRP WHERE FLAG = 1 AND TKEY = '{GRPTKEY}' AND UNIT_GRP_NODE = 0";
            DataSet ds = OracleHelper.Query(Sql);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 捞出所属组别的基本计量单位名称
        /// </summary>
        /// <param name="GRPTKEY"></param>
        /// <returns></returns>
        public string GetBaseUnit(string GRPTKEY)
        {
            string Sql = @"SELECT * FROM BCDF_UNIT WHERE FLAG = 1 AND UNIT_GRP_TKEY = " + "'" + GRPTKEY + "'" + " AND BASE_UNIT_FLAG = 1";
            DataSet ds = OracleHelper.Query(Sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["BASE_UNIT_TKEY"].ToString();
            }
            else
            {
                return "";
            }
        }

        #endregion

        #region 库房 
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
        public Result QueryBindGridView(string Sql)
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
        public int InsertPickingSEQ(string StockGRPTkey,string Type)
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

    }
}
