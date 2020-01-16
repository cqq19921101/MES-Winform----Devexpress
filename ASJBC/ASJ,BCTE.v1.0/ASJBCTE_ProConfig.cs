// ================================================== //
//
// ASJBCTE_ProConfig.cs
//
// 工艺配置（工序自定义参数管理 工序模板 工序模板分组 ）
//
// 涉及到的窗体 ： 
//      UcDIYParaMeter.cs -- 工序自定义参数管理  
//      UcProcessModel.cs --  工序模板
//      UcProModelGRP.cs --  工序模板分组
//      
// ================================================== //

using ASJ.BCTE;
using ASJ.TOOLS.Basic;
using ASJ.TOOLS.Data;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ASJ.BCTE
{
    public class ASJBCTE_ProConfig : ASJBCTE_Base
    {
        #region 工序自定义参数管理
        /// <summary>
        /// LoadData
        /// </summary>
        /// <param name="TKEY"></param>
        /// <returns></returns>
        public DataSet DIYParaMeterLoad(string TKEY)
        {
            List<string> strsql = new List<string>();
            List<string> TableNames = new List<string>();
            string SqlMaster = $@" SELECT * FROM BCTE_DIYPARAMETER WHERE FLAG = 1  AND TKEY = '{TKEY}' ";
            strsql.Add(SqlMaster);
            TableNames.Add("BCTE_DIYPARAMETER");
            return base.FrmDataLoad(strsql, TableNames);
        }

        #endregion

        #region  工序模板分组
        public void BindGridLookUpEdit_ProModelGRP(GridLookUpEdit GridLookUpEdit)
        {
            base.BindGridLookUpEdit(sqlpromodelgrp,GridLookUpEdit);
        }
        #endregion
    }
}
