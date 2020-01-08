using ASJ.TOOLS.Basic;
using ASJ.TOOLS.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ASJMM
{
    public class MMSMMHelper : BC_Standard
    {
        Result rs = new Result();

        #region 物料管理
        /// <summary>
        /// 捞取分组表中的所有的数据
        /// </summary>
        /// <param name="DBNAME"></param>
        /// <returns></returns>
        override public Result QueryGroupTable(string DBNAME)
        {
            Result rs = new Result();
            string sql = $@"SELECT * FROM {DBNAME} WHERE FLAG = 1";
            DataSet ds = OracleHelper.Query(sql);
            rs.Ds = ds;
            rs.Msg = "Success";
            rs.Status = true;

            return rs;

        }

        /// <summary>
        /// GridView DataSource查询绑定 BCMA_MATERIAL MMSMM_PURCHASE_REQ_D 连接查询
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
        /// MMSMM_PURCHASE_REQ_D BCMA_MATERIAL 请购明细和物料主表连接查询
        /// </summary>
        /// <returns></returns>
        public Result QueryMaterialMaster()
        {
            Result rs = new Result();
            string sql = @"select T1.*,T2.TKEY,T2.MATERIAL_CODE,T2.MATERIAL_NAME,T2.MAPID,T2.BASE_UNIT_TKEY from MMSMM_PURCHASE_REQ_D T1 
                            left join BCMA_MATERIAL T2 ON T1.MATERIAL_TKEY = T2.TKEY AND T1.FLAG = T2.FLAG
                            WHERE T1.FLAG = 1";
            DataSet ds = OracleHelper.Query(sql);
            rs.Ds = ds;
            rs.Msg = "Success";
            rs.Status = true;
            return rs;
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
            string sql = @" 
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
	                            AND CKEY IN ( '{0}' ) 
                            ORDER BY
	                            T1.CRE_TIME 
	                            ) T2 ON T1.TKEY = T2.CKEY 
	                            AND T1.FLAG = T2.FLAG 
                            WHERE
	                            T1.FLAG = 1";
            sql = string.Format(sql, string.Join("','", TKEY.ToArray()));
            DataSet ds = OracleHelper.Query(sql);
            rs.Ds = ds;
            rs.Msg = "Success";
            rs.Status = true;
            return rs;
        }

        ///<summary>   
        /// 将两个列不同的DataTable合并成一个新的DataTable   
        ///</summary>   
        ///<param name="dt1">原表</param>   
        ///<param name="dt2">合并的表</param>   
        ///<param name="primaryKey">需要排重列表（为空不排重）</param>   
        ///<param name="maxRows">合并后Table的最大行数</param>   
        ///<returns>合并后的datatable</returns>
        override public DataTable MergeDataTable(DataTable dt1, DataTable dt2, string primaryKey, int maxRows)
        {
            //判断是否需要合并
            if (dt1 == null && dt2 == null)
            {
                return null;
            }
            if (dt1 == null && dt2 != null)
            {
                return dt2.Copy();
            }
            else if (dt1 != null && dt2 == null)
            {
                return dt1.Copy();
            }
            //复制dt1的数据
            DataTable dt = dt1.Copy();
            //补充dt2的结构（dt1中没有的列）到dt中
            for (int i = 0; i < dt2.Columns.Count; i++)
            {
                string cName = dt2.Columns[i].ColumnName;
                if (!dt.Columns.Contains(cName))
                {
                    dt.Columns.Add(new DataColumn(cName));
                }
            }
            //复制dt2的数据
            if (dt2.Rows.Count > 0)
            {
                Type t = dt2.Rows[0][primaryKey].GetType();
                bool isNeedFilter = string.IsNullOrEmpty(primaryKey) ? false : true;
                bool isNeedQuotes = t.Name == "String" ? true : false;
                int mergeTableNum = dt.Rows.Count;
                for (int i = 0; i < dt2.Rows.Count && mergeTableNum < maxRows; i++)
                {
                    bool isNeedAdd = true;
                    //如果需要排重时，判断是否需要添加当前行
                    if (isNeedFilter)
                    {
                        string primaryValue = dt2.Rows[i][primaryKey].ToString();
                        string fileter = primaryKey + "=" + primaryValue;
                        if (isNeedQuotes)
                        {
                            fileter = primaryKey + "='" + primaryValue + "'";
                        }
                        DataRow[] drs = dt.Select(fileter);
                        if (drs != null && drs.Length > 0)//Tkey已存在 不需要添加当前行
                        {
                            isNeedAdd = false;
                            //部分栏位可能修改 原表更新当前行
                            dt.Rows.Remove(drs[0]);
                            dt.ImportRow(dt2.Rows[i]);
                        }
                    }
                    //添加数据
                    if (isNeedAdd)
                    {
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string cName = dt.Columns[j].ColumnName;
                            if (dt2.Columns.Contains(cName))
                            {
                                //防止因同一字段不同类型赋值出错
                                if (dt2.Rows[i][cName] != null && dt2.Rows[i][cName] != DBNull.Value && dt2.Rows[i][cName].ToString() != "")
                                {
                                    dr[cName] = dt2.Rows[i][cName];
                                }
                            }
                        }
                        dt.Rows.Add(dr);
                        mergeTableNum++;
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 选择物料编码 带出 图号 计量单位 物料名称并绑定到
        /// </summary>
        /// <param name="GridView"></param>
        /// <param name="gridlookupedit"></param>
        /// <param name="BASE_UNIT">表中的计量单位TKEY字段名 出现过不同</param>
        public void BindReGLE_BCMA_MATERIAL(GridView GridView, GridLookUpEdit gridlookupedit, string BASE_UNIT)
        {
            DataTable dt = new DataTable();

            switch (GridView.FocusedColumn.FieldName)
            {
                case "MATERIAL_CODE":
                    string MaterialTkey = gridlookupedit.EditValue?.ToString();//物料表主键
                    dt = Query("BCMA_MATERIAL", MaterialTkey).Ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        GridView.SetFocusedRowCellValue("MATERIAL_TKEY", MaterialTkey);//物料Key  MATERIAL_TKEY
                        GridView.SetFocusedRowCellValue("MATERIAL_NAME", dt.Rows[0]["MATERIAL_NAME"].ToString() == "" ? string.Empty : dt.Rows[0]["MATERIAL_NAME"].ToString());//物料名称 MATERIAL_NAME
                        GridView.SetFocusedRowCellValue("MAPID", dt.Rows[0]["MAPID"].ToString() == "" ? string.Empty : dt.Rows[0]["MAPID"].ToString());//图号 MAPID
                        GridView.SetFocusedRowCellValue(BASE_UNIT, dt.Rows[0]["BASE_UNIT_TKEY"].ToString() == "" ? string.Empty : dt.Rows[0]["BASE_UNIT_TKEY"].ToString());//基本计量单位TKEY BASE_UNIT_TKEY
                    }
                    break;
            }

        }
        #endregion

        #region 临时表Datatable创建 入库 / 入库申请 / 出库 / 出库申请 / 到货接收
        /// <summary>
        /// 创建Datatable结构  MMSMM_INSTOCK_DLOT
        /// </summary>
        public DataTable CreateTempForGrvInStock()
        {
            DataTable dt = new DataTable("MMSMM_INSTOCK_DLOT");
            dt.Columns.Add(new DataColumn("INSTOCK_TKEY", typeof(String)));//入库单主表KEY
            dt.Columns.Add(new DataColumn("INSTOCK_D_TKEY", typeof(String)));//当前选中行的入库单明细Tkey
            dt.Columns.Add(new DataColumn("MATERIAL_TKEY", typeof(String)));//物料KEY
            dt.Columns.Add(new DataColumn("BASE_UNIT_KEY", typeof(String)));//计量单位
            dt.Columns.Add(new DataColumn("TO_STOCK_KEY", typeof(String)));//目标库房


            return dt;
        }

        /// <summary>
        /// 创建Datatable结构  MMSMM_INSTOCK_REQ_DLOT
        /// </summary>
        public DataTable CreateTempForGrvInStockREQ()
        {
            DataTable dt = new DataTable("MMSMM_INSTOCK_REQ_DLOT");
            dt.Columns.Add(new DataColumn("INSTOCK_REQ_TKEY", typeof(String)));//入库申请单主表KEY
            dt.Columns.Add(new DataColumn("INSTOCK_REQ_D_TKEY", typeof(String)));//当前选中行的入库申请单明细Tkey
            dt.Columns.Add(new DataColumn("MATERIAL_TKEY", typeof(String)));//物料KEY
            dt.Columns.Add(new DataColumn("BASE_UNIT_KEY", typeof(String)));//计量单位
            dt.Columns.Add(new DataColumn("TO_STOCK_KEY", typeof(String)));//目标库房


            return dt;
        }

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

        /// <summary>
        /// 创建Datatable结构  MMSMM_INSTOCK_REC_DLOT
        /// </summary>
        public DataTable CreateTempForGrvOutStockREC()
        {
            DataTable dt = new DataTable("MMSMM_INSTOCK_REC_DLOT");
            dt.Columns.Add(new DataColumn("INSTOCK_REQ_TKEY", typeof(String)));//主表KEY
            dt.Columns.Add(new DataColumn("INSTOCK_REQ_D_TKEY", typeof(String)));//当前选中行的明细Tkey
            dt.Columns.Add(new DataColumn("MATERIAL_TKEY", typeof(String)));//物料KEY
            dt.Columns.Add(new DataColumn("BASE_UNIT_KEY", typeof(String)));//计量单位
            dt.Columns.Add(new DataColumn("TO_STOCK_KEY", typeof(String)));//目标库房


            return dt;
        }


        #endregion

        #region Datatable创建 请购→采购（单据转换） 

        /// <summary>
        /// 创建Datatable结构  MMSMM_PURCHASE_MAP 
        /// </summary>
        override public DataTable QueryToDatatable(string DBNAME)
        {
            string sql = $@"Select * from {DBNAME} WHERE FLAG  = 1";
            return OracleHelper.GetTable(sql);
        }
        #endregion
    }
}
