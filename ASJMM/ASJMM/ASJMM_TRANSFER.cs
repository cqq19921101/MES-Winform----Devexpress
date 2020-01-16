// ================================================== //
//
// ASJMM_TRANSFER.cs
//
// 调拨模块 （调拨单）
//
// 涉及到的窗体 ： 
//      UcTransfer.cs -- 调拨单管理  
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
    public class ASJMM_TRANSFER : ASJMM_Base
    {
        Result rs = new Result();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GridControl"></param>
        /// <param name="GridView"></param>
        /// <param name="DBNAME"></param>
        /// <param name="TKEY"></param>
        override public void BindDataSourceForGridControl(GridControl GridControl, GridView GridView, string DBNAME, string TKEY)
        {
            base.BindDataSourceForGridControl(GridControl, GridView, DBNAME,TKEY);
        }

        #region 调拨
        /// <summary>
        /// LoadData
        /// </summary>
        /// <param name="TKEY"></param>
        /// <returns></returns>
        public DataSet TransferLoad(string TKEY)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();
            lststrsql.Add($"SELECT * FROM MMSMM_TRANSFER WHERE FLAG  = 1 AND TKEY = '{TKEY}'");
            lsttablaname.Add("MMSMM_TRANSFER");
            return base.FrmDataLoad(lststrsql, lsttablaname);
        }

        /// <summary>
        /// 绑定GridLookUpEdit下拉框的值
        /// </summary>
        /// <param name="GridLookUpEdit"></param>
        public void BindGridLookUpEdit_Transfer(List<GridLookUpEdit> GridLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqlstock);//转出库房
            lststrsql.Add(sqlstock);//接收库房
            lststrsql.Add(sqlemp);//申请人
            lststrsql.Add(sqlemp);//接收人
            base.BindGridLookUpEdit(lststrsql, GridLookUpEdit);
        }

        /// <summary>
        /// 绑定RepositoryItemGridLookUpEdit下拉框的值
        /// </summary>
        /// <param name="RepositoryItemGridLookUpEdit"></param>
        public void BindReGridLookUpEdit_Transfer(List<RepositoryItemGridLookUpEdit> RepositoryItemGridLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqlstock);//转出库房 & 接收库房
            lststrsql.Add(sqlmaterial);//物料
            base.BindReGridLookUpEdit(lststrsql, RepositoryItemGridLookUpEdit);
        }

        /// <summary>
        /// 绑定RepositoryItemLookUpEdit下拉框的值
        /// </summary>
        /// <param name="RepositoryItemLookUpEdit"></param>
        public void BindReLookUpEdit_Transfer(List<RepositoryItemLookUpEdit> RepositoryItemLookUpEdit)
        {
            List<string> lststrsql = new List<string>();
            List<string> lsttablaname = new List<string>();

            lststrsql.Add(sqlunit);//计量单位
            base.BindReLookUpEdit(lststrsql, RepositoryItemLookUpEdit);
        }

        #endregion
    }
}
