using ASJ.TOOLS.Basic;
using ASJ.TOOLS.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ASJ.MMSMM
{
    public abstract class BC_Standard
    {
        Result rs = new Result();
        public BC_Standard()
        {

        }

        #region Query
        /// <summary>
        /// QueryData
        /// </summary>
        /// <param name="DBNAME"></param>
        /// <returns></returns>
        public Result Query(string DBNAME)
        {
            string sql = @"SELECT * FROM  " + DBNAME + " WHERE FLAG = 1 ";
            DataSet ds = OracleHelper.Query(sql);
            rs.Ds = ds;
            rs.Msg = "Success";
            rs.Status = true;
            return rs;

        }


        /// <summary>
        /// QueryData
        /// </summary>
        /// <param name="DBNAME"></param>
        /// <returns></returns>
        public Result Query(string DBNAME, string TKEY)
        {
            string sql = @"SELECT * FROM  " + DBNAME + " WHERE FLAG = 1 and TKEY = " + "'" + TKEY + "'";
            DataSet ds = OracleHelper.Query(sql);
            rs.Ds = ds;
            rs.Msg = "Success";
            rs.Status = true;
            return rs;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DBNAME"></param>
        /// <param name="TKEY"></param>
        /// <param name="ParaMeter"></param>
        /// <returns></returns>
        public Result Query(string DBNAME, string TKEY, string ParaMeter)
        {
            string sql = @"SELECT * FROM  " + DBNAME + " WHERE FLAG = 1 and CKEY = " + "'" + TKEY + "'";
            DataSet ds = OracleHelper.Query(sql);
            rs.Ds = ds;
            rs.Msg = "Success";
            rs.Status = true;
            return rs;

        }
        #endregion

        #region GridControl

        /// <summary>
        /// 绑定GridControl 
        /// </summary>
        /// <param name="GridControl"></param>
        /// <param name="GridView"></param>
        /// <param name="dt"></param>
        public void BindDataSourceForGridControl(GridControl GridControl, GridView GridView,DataTable dt)
        {
            GridControl.DataSource = dt;//绑定GridControl数据源

            GridView.OptionsView.ColumnAutoWidth = true;//列宽自动
            GridView.OptionsBehavior.Editable = true;//允许编辑
            GridView.OptionsSelection.MultiSelect = true;//可以多选
        }

        #endregion

        #region 系统数据字典 逻辑处理  绑定

        /// <summary>
        /// 捞取数据字典中SysDict_Code对应的SYSDICT_VALUE的值 （如 SYSDICT_CODE = 0 ， SYSDICT_VALUE = 原材料）
        /// </summary>
        /// <param name="SysDict_Type_Code">表名 + 字段名 如 BCMA_MATERIAL_MATERIAL_TYPE</param>
        /// <param name="SysDict_Code">表中对应的值 (0,1,2)</param>
        /// <returns></returns>
        public string GetValueBySysDict(string SysDict_Type_Code, string SysDict_Code)
        {
            string sql = @"SELECT D.SYSDICT_CODE,D.SYSDICT_NAME,D.SYSDICT_VALUE FROM SYSC_SYSDICT_TYPE M, SYSC_SYSDICT D 
                           WHERE M.SYSDICT_TYPE_CODE = " + "'" + SysDict_Type_Code + "'" + "AND M.TKEY = D.CKEYAND M.FLAG = 1 AND D.FLAG = 1 AND D.SYSDICT_CODE =  " + "'" + SysDict_Code + "'" ;
            DataSet ds = OracleHelper.Query(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["SYSDICT_VALUE"].ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 绑定下拉框的值 资料来源系统数据字典 例：0=MES;1=PLM;2=ERP
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="Parameter"></param>
        public void BindLookUpEdit(List<LookUpEdit> Control, List<string> Parameter)
        {
            for (int i = 0; i < Control.Count; i++)
            {
                BindSysDict(Control[i], Parameter[i]);
            }

        }

        /// <summary>
        /// 绑定下拉框控件的值 枚举节点 LookUpEdit   
        /// </summary>
        /// <param name="Control">控件名</param>
        /// <param name="enumtype">枚举</param>
        /// <param name="DisplayMember">DisplayMember</param>
        /// <param name="ValueMember">ValueMember</param>
        public void BindLookUpEdit(DevExpress.XtraEditors.LookUpEdit Control, Type enumtype, string DisplayMember, string ValueMember)
        {
            DataTable dtNode = ASJ.TOOLS.Data.DataHelper.EnumToDataTable(enumtype, DisplayMember, ValueMember);
            Control.Properties.DataSource = dtNode;
            Control.Properties.DisplayMember = DisplayMember;
            Control.Properties.ValueMember = ValueMember;
        }

        /// <summary>
        /// 根据Parameter传入的参数 查询系统数据字典对应项 并绑定下拉框的值
        /// </summary>
        /// <param name="Control">LookUpEdit控件</param>
        /// <param name="Parameter">表名 + 字段名 如：BCMA_MATERIAL_MATERIAL_TYPE</param>
        public void BindSysDict(DevExpress.XtraEditors.LookUpEdit Control, string Parameter)
        {
            string sql = @"SELECT D.SYSDICT_CODE,D.SYSDICT_NAME,D.SYSDICT_VALUE FROM SYSC_SYSDICT_TYPE M, SYSC_SYSDICT D 
                           WHERE M.SYSDICT_TYPE_CODE = " +"'" + Parameter + "'" + "AND M.TKEY = D.CKEY AND M.FLAG = 1 AND D.FLAG = 1";
            DataSet ds = OracleHelper.Query(sql);
            Control.Properties.DataSource = ds.Tables[0];
            Control.Properties.DisplayMember = "SYSDICT_VALUE";
            Control.Properties.ValueMember = "SYSDICT_CODE";

        }

        #endregion




    }
}
