// ================================================== //
//
// ASJBCMA_Material.cs
//
// 物料管理（物料管理 物料分组 ）
//
// 涉及到的窗体 ： 
//      UcMaterial.cs -- 物料管理  
//      UcMaterialGRP.cs --  物料分组
//      
// ================================================== //

using ASJ.TOOLS.Basic;
using ASJ.TOOLS.Data;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ASJ.BCMA
{
    public  class ASJBCMA_Material : ASJBCMA_Base
    {
        Result rs = new Result();

        #region 物料管理
        /// <summary>
        /// 绑定GridLookUpEdit下拉框的值
        /// </summary>
        /// <param name="GridLookUpEdit"></param>
        public void BindGridLookUpEdit_Material(List<GridLookUpEdit> GridLookUpEdit)
        {
            List<string> lststrsql = new List<string>();

            lststrsql.Add(sqlunit);//基本计量单位
            lststrsql.Add(sqlunit);//辅助计量单位
            lststrsql.Add(sqlunit);//重量计量单位
            lststrsql.Add(sqlunit);//长度计量单位
            lststrsql.Add(sqlunit);//体积计量单位
            lststrsql.Add(sqlsupplier);//所属供应商
            lststrsql.Add(sqlemp);//默认管理员
            lststrsql.Add(sqlstock);//默认库房
            lststrsql.Add(sqlstocksite);//默认库位
            lststrsql.Add(sqlunit);//最大库存周期单位
            lststrsql.Add(sqlunit);//在库周期检验周期单位
            base.BindGridLookUpEdit(lststrsql, GridLookUpEdit);
        }
        /// <summary>
        /// LoadData
        /// </summary>
        /// <param name="TKEY"></param>
        /// <returns></returns>
        public DataSet MaterialLoad(string TKEY)
        {
            List<string> strsql = new List<string>();
            List<string> TableNames = new List<string>();
            string SqlMaster = $@" SELECT * FROM BCMA_MATERIAL WHERE FLAG = 1  AND TKEY = '{TKEY}' ";
            string SqlMPur = $@" SELECT * FROM BCMA_MATERIAL_PURCHASE WHERE FLAG = 1  AND MATERIAL_TKEY = '{TKEY}' ";
            string SqlMS = $@" SELECT * FROM BCMA_MATERIAL_STOCK WHERE FLAG = 1  AND MATERIAL_TKEY = '{TKEY}' ";
            string SqlMQ = $@" SELECT * FROM BCMA_MATERIAL_QUALITY WHERE FLAG = 1  AND MATERIAL_TKEY = '{TKEY}' ";
            string SqlMU = $@" SELECT * FROM BCMA_MATERIAL_USECONTROL WHERE FLAG = 1  AND MATERIAL_TKEY = '{TKEY}' ";
            string SqlMPro = $@" SELECT * FROM BCMA_MATERIAL_PRODUCE WHERE FLAG = 1  AND MATERIAL_TKEY = '{TKEY}' ";

            strsql.Add(SqlMaster);//主档
            strsql.Add(SqlMPur);//采购
            strsql.Add(SqlMS);//库存
            strsql.Add(SqlMQ);//质量
            strsql.Add(SqlMU);//业务
            strsql.Add(SqlMPro);//生产

            TableNames.Add("BCMA_MATERIAL");
            TableNames.Add("BCMA_MATERIAL_PURCHASE");
            TableNames.Add("BCMA_MATERIAL_STOCK");
            TableNames.Add("BCMA_MATERIAL_QUALITY");
            TableNames.Add("BCMA_MATERIAL_USECONTROL");
            TableNames.Add("BCMA_MATERIAL_PRODUCE");
            return base.FrmDataLoad(strsql, TableNames);
        }
        public string GetMappingTKEY(string DBNAME)
        {
            string strsql = $@"Select TKEY,MATERIAL_TKEY FROM {DBNAME} WHERE FLAG = 1";
            DataSet ds  = OracleHelper.Query(strsql);
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0] != null) return ds.Tables[0].Rows[0]["TKEY"].ToString();
            return null;
        }
        #endregion

        #region 物料分组
        public void BindGridLookUpEdit_MaterialGRP(GridLookUpEdit GridLookUpEdit)
        {
            base.BindGridLookUpEdit(sqlmaterialgrp, GridLookUpEdit);
        }
        #endregion
    }
}
