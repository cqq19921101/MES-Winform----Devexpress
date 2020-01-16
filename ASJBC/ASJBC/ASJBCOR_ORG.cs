// ================================================== //
//
// ASJBCOR_ORG.cs
//
// 组织布局/人力资源 （供应商 供应商分组 客户 客户分组 计量单位 计量单位分组 生产组织 部门 工位 工作班次 工作班组 工位采集节点 ）
//
// 涉及到的窗体 ： 
//      UcSupplier.cs -- 供应商  
//      UcSupplierGRP.cs --  供应商分组
//      UcCustomer.cs -- 客户
//      UcCustomerGRP.cs -- 客户分组  
//      UcUnit.cs --  计量单位
//      UcUnitGroup.cs -- 计量单位分组
//      UcWorkORG.cs --  生产组织
//      UcDept.cs -- 部门
//      UcStation.cs -- 工位
//      UcWorkShift.cs -- 工作班次
//      UcWorkGRP.cs -- 工作班组
//      UcStationCollectp.cs -- 工位采集节点
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
    public class ASJBCOR_ORG : ASJBCOR_Base
    {
        #region const sql
        //const string sqlsuppliernode  = 
        #endregion

        #region  供应商
        public void BindGridLookUpEdit_Supplier(GridLookUpEdit GridLookUpEdit)
        {
            base.BindGridLookUpEdit(sqlsuppliergrpnode, GridLookUpEdit);
        }
        #endregion

        #region 供应商分组
        public void BindGridLookUpEdit_SupplierGRP(GridLookUpEdit GridLookUpEdit)
        {
            base.BindGridLookUpEdit(sqlsuppliergrp, GridLookUpEdit);
        }

        #endregion

        #region 客户
        public void BindGridLookUpEdit_Customer(GridLookUpEdit GridLookUpEdit)
        {
            base.BindGridLookUpEdit(sqlcustomergrpnode, GridLookUpEdit);
        }
        #endregion

        #region 客户分组
        public void BindGridLookUpEdit_CustomerGRP(GridLookUpEdit GridLookUpEdit)
        {
            base.BindGridLookUpEdit(sqlcustomergrp, GridLookUpEdit);
        }
        #endregion

        #region 计量单位  UcUnit 
        public void BindGridLookUpEdit_Unit(GridLookUpEdit GridLookUpEdit)
        {
            base.BindGridLookUpEdit(sqlunitgrpnode, GridLookUpEdit);
        }

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

        #region 计量单位分组
        public void BindGridLookUpEdit_UnitGRP(GridLookUpEdit GridLookUpEdit)
        {
            base.BindGridLookUpEdit(sqlunitgrp, GridLookUpEdit);
        }

        #endregion

        #region 生产组织
        public void BindGridLookUpEdit_WorkORG(GridLookUpEdit GridLookUpEdit)
        {
            base.BindGridLookUpEdit(sqlemp, GridLookUpEdit);
        }
        #endregion

        #region 部门
        public void BindGridLookUpEdit_Dept(List<GridLookUpEdit> GridLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqlworg);//生产组织
            lststrsql.Add(sqlemp);//负责人
            lststrsql.Add(sqldept);//上级部门
            base.BindGridLookUpEdit(lststrsql, GridLookUpEdit);
        }
        #endregion

        #region 工位
        public void BindGridLookUpEdit_Station(GridLookUpEdit GridLookUpEdit)
        {
            base.BindGridLookUpEdit(sqlworg, GridLookUpEdit);
        }
        #endregion

        #region 工作班组
        public void BindGridLookUpEdit_WorkGRP(List<GridLookUpEdit> GridLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqlworg);//生产组织
            lststrsql.Add(sqlemp);//负责人
            lststrsql.Add(sqldept);//上级部门
            lststrsql.Add(sqlworkgrp);//上级班组
            base.BindGridLookUpEdit(lststrsql, GridLookUpEdit);
        }
        #endregion

        #region 工位采集节点
        public void BindGridLookUpEdit_StationCollectp(GridLookUpEdit GridLookUpEdit)
        {
            base.BindGridLookUpEdit(sqlworkgrp, GridLookUpEdit);
        }
        #endregion
    }
}
