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

namespace ASJ.BCTE
{
    #region 父类抽象类
    public abstract class BC_Standard
    {
        Result rs = new Result();
        public BC_Standard()
        {

        }

        #region QueryData
        /// <summary>
        /// QueryData
        /// </summary>
        /// <param name="DBNAME"></param>
        /// <returns></returns>
        public Result Query(string DBNAME)
        {
            string sql = $@"SELECT * FROM  {DBNAME} WHERE FLAG = 1 ";
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
            string sql = $@"SELECT * FROM   {DBNAME}  WHERE FLAG = 1 and TKEY = '{TKEY}'";
            DataSet ds = OracleHelper.Query(sql);
            rs.Ds = ds;
            rs.Msg = "Success";
            rs.Status = true;
            return rs;

        }

        /// <summary>
        /// QueryData 出现主从关系的两个表的时候使用 
        /// </summary>
        /// <param name="DBNAME"></param>
        /// <param name="TKEY"></param>
        /// <param name="ParaMeter">为了区分是3个参数</param>
        /// <returns></returns>
        public Result Query(string DBNAME, string TKEY, string ParaMeter)
        {
            string sql = $@"SELECT * FROM  {DBNAME} WHERE FLAG = 1 and CKEY = '{TKEY}'";
            DataSet ds = OracleHelper.Query(sql);
            rs.Ds = ds;
            rs.Msg = "Success";
            rs.Status = true;
            return rs;

        }

        /// <summary>
        /// 捞取分组表中的所有的数据
        /// </summary>
        /// <param name="DBNAME"></param>
        /// <param name="Node">表中节点字段名</param>
        /// <returns></returns>
        public Result QueryGroupTable(string DBNAME)
        {
            Result rs = new Result();
            string sql = $@"SELECT * FROM {DBNAME} WHERE FLAG = 1 ";
            DataSet ds = OracleHelper.Query(sql);
            rs.Ds = ds;
            rs.Msg = "Success";
            rs.Status = true;
            return rs;

        }

        /// <summary>
        /// 捞取分组表中的所有的数据 过滤根节点
        /// </summary>
        /// <param name="DBNAME"></param>
        /// <param name="Node">表中节点字段名</param>
        /// <returns></returns>
        public Result QueryGroupTable(string DBNAME, string Node)
        {
            Result rs = new Result();
            string sql = $@"SELECT * FROM  {DBNAME} WHERE FLAG = 1  and {Node} <> 0 ";
            DataSet ds = OracleHelper.Query(sql);
            rs.Ds = ds;
            rs.Msg = "Success";
            rs.Status = true;
            return rs;

        }

        #endregion

        #region GridControl GridView RepositoryItemLookUpEdit

        /// <summary>
        /// 绑定GridView中的数据源
        /// </summary>
        /// <param name="GridControl"></param>
        /// <param name="GridView"></param>
        /// <param name="dt"></param>
        public void BindDataSourceForGridControl(GridControl GridControl, GridView GridView, DataTable dt)
        {
            GridControl.DataSource = dt;//绑定GridControl数据源

            GridView.OptionsView.ColumnAutoWidth = false;//列宽自动
            GridView.OptionsBehavior.Editable = true;//允许编辑
            GridView.OptionsSelection.MultiSelect = true;//可以多选
            GridView.BestFitColumns();

            //显示水平滚动条
            GridView.ScrollStyle = ScrollStyleFlags.LiveHorzScroll | ScrollStyleFlags.LiveVertScroll;
            GridView.HorzScrollVisibility = ScrollVisibility.Always;

        }

        /// <summary>
        /// 绑定GridView中  repositoryItemLookUpEdit的下拉框的值 系统数据字典表 多个控件需要绑定时使用
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="strsql"></param>
        public void BindReSyscLookUpEdit(List<string> ParaMeter, List<RepositoryItemLookUpEdit> ReLookUpEdit)
        {
            for (int i = 0; i < ParaMeter.Count; i++)
            {
                string sql = $@"SELECT D.SYSDICT_CODE,D.SYSDICT_NAME,D.SYSDICT_VALUE FROM SYSC_SYSDICT_TYPE M, SYSC_SYSDICT D 
                           WHERE M.SYSDICT_TYPE_CODE = '{ParaMeter[i].ToString() }' AND M.TKEY = D.CKEY AND M.FLAG = 1 AND D.FLAG = 1";
                DataSet ds = OracleHelper.Query(sql);
                ReLookUpEdit[i].DataSource = ds.Tables[0];
                ReLookUpEdit[i].DisplayMember = "SYSDICT_VALUE";
                ReLookUpEdit[i].ValueMember = "SYSDICT_CODE";
            }
        }

        /// <summary>
        /// 绑定GridView中  repositoryItemLookUpEdit的下拉框的值 系统数据字典表
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="strsql"></param>
        public void BindReLookUpEdit(List<string> strsql, List<RepositoryItemLookUpEdit> ReLookUpEdit)
        {
            for (int i = 0; i < strsql.Count; i++)
            {
                DataSet ds = OracleHelper.Query(strsql[i].ToString());
                DataTable dt = ds.Tables[0];
                ReLookUpEdit[i].DisplayMember = dt.Columns[1].ToString();
                ReLookUpEdit[i].ValueMember = dt.Columns[0].ToString();
                ReLookUpEdit[i].DataSource = dt;
                ReLookUpEdit[i].NullText = null;
            }
        }

        /// <summary>
        /// 绑定GridView中  repositoryItemLookUpEdit的下拉框的值 单个ReLookUpEdit单元格需要绑定时使用
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="DisplayMember"></param>
        /// <param name="ValueMember"></param>
        /// <param name="LookUpEdit"></param>
        public void BindReLookUpEdit(DataTable dt, string DisplayMember, string ValueMember, RepositoryItemLookUpEdit ReLookUpEdit)
        {
            ReLookUpEdit.ValueMember = ValueMember;
            ReLookUpEdit.DisplayMember = DisplayMember;
            ReLookUpEdit.DataSource = dt;
            ReLookUpEdit.NullText = null;
        }

        /// <summary>
        /// 根据Parameter传入的参数 查询系统数据字典对应项 并绑定下拉框的值
        /// </summary>
        /// <param name="Control">LookUpEdit控件</param>
        /// <param name="Parameter">表名 + 字段名 如：BCMA_MATERIAL_MATERIAL_TYPE</param>
        public void BindSysDict(RepositoryItemLookUpEdit Control, string Parameter)
        {
            string sql = $@"SELECT D.SYSDICT_CODE,D.SYSDICT_NAME,D.SYSDICT_VALUE FROM SYSC_SYSDICT_TYPE M, 
                           SYSC_SYSDICT D WHERE M.SYSDICT_TYPE_CODE = '{Parameter}' AND M.TKEY = D.CKEY AND M.FLAG = 1 AND D.FLAG = 1";
            DataSet ds = OracleHelper.Query(sql);
            Control.DataSource = ds.Tables[0];
            Control.DisplayMember = "SYSDICT_VALUE";
            Control.ValueMember = "SYSDICT_CODE";

        }

        #endregion

        #region 系统数据字典 & 下拉框绑定 LookUpEdit

        /// <summary>
        /// 捞取数据字典中SysDict_Code对应的SYSDICT_VALUE的值 （如 SYSDICT_CODE = 0 ， SYSDICT_VALUE = 原材料）
        /// </summary>
        /// <param name="SysDict_Type_Code">表名 + 字段名 如 BCMA_MATERIAL_MATERIAL_TYPE</param>
        /// <param name="SysDict_Code">表中对应的值 (0,1,2)</param>
        /// <returns></returns>
        public string GetValueBySysDict(string SysDict_Type_Code, string SysDict_Code)
        {
            string sql = $@"SELECT D.SYSDICT_CODE,D.SYSDICT_NAME,D.SYSDICT_VALUE FROM SYSC_SYSDICT_TYPE M, SYSC_SYSDICT D 
                           WHERE M.SYSDICT_TYPE_CODE = '{SysDict_Type_Code}' AND M.TKEY = D.CKEYAND M.FLAG = 1 AND D.FLAG = 1 AND D.SYSDICT_CODE =  '{SysDict_Code}'";
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
        /// 绑定下拉框的值 资料来源系统数据字典 多个LookUpEdit需要绑定 例：0=MES;1=PLM;2=ERP
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
            string sql = $@"SELECT D.SYSDICT_CODE,D.SYSDICT_NAME,D.SYSDICT_VALUE FROM SYSC_SYSDICT_TYPE M, SYSC_SYSDICT D 
                           WHERE M.SYSDICT_TYPE_CODE = '{Parameter}' AND M.TKEY = D.CKEY AND M.FLAG = 1 AND D.FLAG = 1";
            DataSet ds = OracleHelper.Query(sql);
            Control.Properties.DataSource = ds.Tables[0];
            Control.Properties.DisplayMember = "SYSDICT_VALUE";
            Control.Properties.ValueMember = "SYSDICT_CODE";

        }

        /// <summary>
        /// 根据查询的数据 绑定LookUpEdit的值 一个窗体多个LookUpEdit需要绑定
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="strsql"></param>
        public void BindLookUpEditForQueryData(List<LookUpEdit> Control, List<string> strsql)
        {
            for (int i = 0; i < Control.Count; i++)
            {
                BindOtherTable(Control[i], strsql[i]);
            }
        }

        /// <summary>
        /// 根据查询的数据 绑定LookUpEdit的值 
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="strsql"></param>
        public void BindOtherTable(LookUpEdit Control, string strsql)
        {
            DataSet ds = OracleHelper.Query(strsql);
            DataTable dt = ds.Tables[0];
            Control.Properties.DataSource = dt;
            Control.Properties.DisplayMember = dt.Columns[1].ColumnName;
            Control.Properties.ValueMember = dt.Columns[0].ColumnName;

            Control.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

        }
        #endregion

        #region GridView 行头显示行号 宽度自适应
        /// <summary>
        /// GridView 行头显示行号 宽度自适应
        /// </summary>
        /// <param name="view"></param>
        public void BindCustomDrawRowIndicator(DevExpress.XtraGrid.Views.Grid.GridView view)
        {
            view.IndicatorWidth = CalcIndicatorDefaultWidth(view);
            view.CustomDrawRowIndicator += (s, e) =>
            {
                if (e.RowHandle >= 0)
                {
                    e.Info.DisplayText = (e.RowHandle + 1).ToString();
                }
            };
            view.TopRowChanged += (s, e) =>
            {
                int width = CalcIndicatorBestWidth(view);
                if ((view.IndicatorWidth - 4 < width || view.IndicatorWidth + 4 > width) && view.IndicatorWidth != width)
                {
                    view.IndicatorWidth = width;
                }
            };

        }
        /// <summary>
        /// 计算行头宽度
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        int CalcIndicatorBestWidth(DevExpress.XtraGrid.Views.Grid.GridView view)
        {
            Graphics graphics = new Control().CreateGraphics();
            SizeF sizeF = new SizeF();
            int count = view.TopRowIndex + ((DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo)view.GetViewInfo()).RowsInfo.Count;
            if (count == 0)
            {
                count = 30;
            }
            sizeF = graphics.MeasureString(count.ToString(), view.Appearance.Row.Font);
            return Convert.ToInt32(sizeF.Width) + 20;
        }
        /// <summary>
        /// 计算默认的宽度
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        int CalcIndicatorDefaultWidth(DevExpress.XtraGrid.Views.Grid.GridView view)
        {
            var grid = view.GridControl;
            Graphics graphics = new Control().CreateGraphics();
            SizeF sizeF = new SizeF();
            int rowHeight = 22;//22是Row的估计高度
            if (view.RowHeight > 0)
            {
                rowHeight = view.RowHeight;
            }
            int count = grid != null ? grid.Height / rowHeight : 30;
            sizeF = graphics.MeasureString(count.ToString(), view.Appearance.Row.Font);
            return Convert.ToInt32(sizeF.Width) + 20;
        }

        #endregion

        #region 绑定GridLookUpEdit下拉框的值

        /// <summary>
        /// 根据查询得到的数据源 绑定到GridLookUpEdit  窗体中出现多个下拉框需要绑定时使用
        /// </summary>
        /// <param name="strsql"></param>
        /// <param name="Control"></param>
        public void BindGridLookUpEdit(List<string> strsql, List<GridLookUpEdit> Control)
        {
            for (int i = 0; i < Control.Count; i++)
            {
                DataSet ds = OracleHelper.Query(strsql[i].ToString());
                DataTable dt = ds.Tables[0];
                Control[i].Properties.DataSource = dt;
                Control[i].Properties.DisplayMember = dt.Columns[1].ColumnName;
                Control[i].Properties.ValueMember = dt.Columns[0].ColumnName;

                Control[i].Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
                Control[i].Properties.View.BestFitColumns();
                Control[i].Properties.ShowFooter = false;
                Control[i].Properties.View.OptionsView.ShowAutoFilterRow = false; //显示不显示grid上第一个空行,也是用于检索的应用
                Control[i].Properties.AutoComplete = false;
                Control[i].Properties.ImmediatePopup = true;
                Control[i].Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                Control[i].Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            }

        }


        /// <summary>
        /// 根据查询得到的数据源 绑定到GridLookUpEdit 窗体中只有单个下拉框时绑定
        /// </summary>
        /// <param name="strsql"></param>
        /// <param name="Control"></param>
        public void BindGridLookUpEdit(string strsql, GridLookUpEdit Control)
        {
            DataSet ds = OracleHelper.Query(strsql);
            DataTable dt = ds.Tables[0];
            Control.Properties.DataSource = dt;
            Control.Properties.DisplayMember = dt.Columns[1].ColumnName;
            Control.Properties.ValueMember = dt.Columns[0].ColumnName;

            Control.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            Control.Properties.View.BestFitColumns();
            Control.Properties.ShowFooter = false;
            Control.Properties.View.OptionsView.ShowAutoFilterRow = false; //显示不显示grid上第一个空行,也是用于检索的应用
            Control.Properties.AutoComplete = false;
            Control.Properties.ImmediatePopup = true;
            Control.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            Control.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

        }

        /// <summary>
        /// 根据查询得到的数据源 绑定到GridLookUpEdit 窗体中只有单个下拉框时绑定
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Control"></param>
        public void BindGridLookUpEdit(DataTable dt, GridLookUpEdit Control)
        {
            Control.Properties.DataSource = dt;
            Control.Properties.DisplayMember = dt.Columns[1].ColumnName;
            Control.Properties.ValueMember = dt.Columns[0].ColumnName;

            Control.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            Control.Properties.View.BestFitColumns();
            Control.Properties.ShowFooter = false;
            Control.Properties.View.OptionsView.ShowAutoFilterRow = false; //显示不显示grid上第一个空行,也是用于检索的应用
            Control.Properties.AutoComplete = false;
            Control.Properties.ImmediatePopup = true;
            Control.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            Control.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

        }


        #endregion

        #region 绑定GridView下 ReGridLookUpEdit下拉框的值

        /// <summary>
        /// 绑定ReGridLookUpEdit下拉框的值 多个控件需要绑定时使用
        /// </summary>
        /// <param name="strsql">Sql语句集合</param>
        /// <param name="GridLookUpEdit">控件集合</param>
        public void BindReGridLookUpEdit(List<string> strsql, List<RepositoryItemGridLookUpEdit> GridLookUpEdit)
        {
            for (int i = 0; i < GridLookUpEdit.Count; i++)
            {
                DataSet ds = OracleHelper.Query(strsql[i].ToString());
                DataTable dt = ds.Tables[0];
                GridLookUpEdit[i].DataSource = dt;
                GridLookUpEdit[i].DisplayMember = dt.Columns[1].ColumnName;
                GridLookUpEdit[i].ValueMember = dt.Columns[0].ColumnName;
                GridLookUpEdit[i].NullText = "请选择";
                GridLookUpEdit[i].AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
                GridLookUpEdit[i].View.BestFitColumns();
                GridLookUpEdit[i].ShowFooter = false;
                GridLookUpEdit[i].View.OptionsView.ShowAutoFilterRow = false; //显示不显示grid上第一个空行,也是用于检索的应用
                GridLookUpEdit[i].AutoComplete = false;
                GridLookUpEdit[i].ImmediatePopup = true;
                GridLookUpEdit[i].PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                GridLookUpEdit[i].TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            }
        }

        /// <summary>
        /// 绑定ReGridLookUpEdit下拉框的值 单个控件需要绑定时使用
        /// </summary>
        /// <param name="strsql"></param>
        /// <param name="ReGridLookUpEdit"></param>
        public void BindReGridLookUpEdit(string strsql, RepositoryItemGridLookUpEdit ReGridLookUpEdit)
        {
            DataSet ds = OracleHelper.Query(strsql);
            DataTable dt = ds.Tables[0];
            ReGridLookUpEdit.DataSource = dt;
            ReGridLookUpEdit.DisplayMember = dt.Columns[1].ColumnName;
            ReGridLookUpEdit.ValueMember = dt.Columns[0].ColumnName;
            ReGridLookUpEdit.NullText = "请选择";
            ReGridLookUpEdit.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            ReGridLookUpEdit.View.BestFitColumns();
            ReGridLookUpEdit.ShowFooter = false;
            ReGridLookUpEdit.View.OptionsView.ShowAutoFilterRow = false; //显示不显示grid上第一个空行,也是用于检索的应用
            ReGridLookUpEdit.AutoComplete = false;
            ReGridLookUpEdit.ImmediatePopup = true;
            ReGridLookUpEdit.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            ReGridLookUpEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
        }

        #endregion

        #region GridLookUpEdit && ReGridLookUpEdit 多列模糊查询
        /// <summary>
        /// 设置GridLookUpEdit多列模糊查询
        /// </summary>
        /// <param name="sender"></param>
        public void SetGridLookUpEditMoreColumnFilter(object sender)
        {
            GridLookUpEdit edit = sender as GridLookUpEdit;
            GridView view = edit.Properties.View as GridView;
            FieldInfo extraFilter = view.GetType().GetField("extraFilter", BindingFlags.NonPublic | BindingFlags.Instance);
            List<CriteriaOperator> columnsOperators = new List<CriteriaOperator>();
            foreach (GridColumn col in view.VisibleColumns)
            {
                if (col.Visible && col.ColumnType == typeof(string))
                    columnsOperators.Add(new FunctionOperator(FunctionOperatorType.Contains,
                        new OperandProperty(col.FieldName),
                        new OperandValue(edit.Text)));
            }
            string filterCondition = new GroupOperator(GroupOperatorType.Or, columnsOperators).ToString();
            extraFilter.SetValue(view, filterCondition);
            //获取GriView中处理列过滤的私有方法
            MethodInfo ApplyColumnsFilterEx = view.GetType().GetMethod("ApplyColumnsFilterEx", BindingFlags.NonPublic | BindingFlags.Instance);
            ApplyColumnsFilterEx.Invoke(view, null);
        }

        #endregion

        #region Tool


        virtual public void InsertNewRowForDatatable(DataSet ds, string DBNAME)
        {
            ds.Tables[DBNAME].NewRow();
            ds.Tables[DBNAME].Rows.Add();
        }

        virtual public void InsertNewRowForDatatable(DataTable dt)
        {
            dt.NewRow();
            dt.Rows.Add();
        }

        ///<summary>   
        /// 将两个列不同的DataTable合并成一个新的DataTable   
        ///</summary>   
        ///<param name="dt1">原表</param>   
        ///<param name="dt2">合并的表</param>   
        ///<param name="primaryKey">需要排重列表（为空不排重）</param>   
        ///<param name="maxRows">合并后Table的最大行数</param>   
        ///<returns>合并后的datatable</returns>
        virtual public DataTable MergeDataTable(DataTable dt1, DataTable dt2, string primaryKey, int maxRows)
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


        #endregion

    }
    #endregion

    /*新增实体类与Datatable互相转换*/
    #region Entity To Datatable
    /// <summary>
    /// DataTable与实体类互相转换
    /// </summary>
    /// <typeparam name="T">实体类</typeparam>
    public class ModelHandler<T> where T : new()
    {
        #region DataTable转换成实体类

        /// <summary>
        /// 填充对象列表：用DataSet的第一个表填充实体类
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <returns></returns>
        public List<T> FillModel(DataSet ds)
        {
            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return FillModel(ds.Tables[0]);
            }
        }

        /// <summary>  
        /// 填充对象列表：用DataSet的第index个表填充实体类
        /// </summary>  
        public List<T> FillModel(DataSet ds, int index)
        {
            if (ds == null || ds.Tables.Count <= index || ds.Tables[index].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return FillModel(ds.Tables[index]);
            }
        }

        /// <summary>  
        /// 填充对象列表：用DataTable填充实体类
        /// </summary>  
        public List<T> FillModel(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            List<T> modelList = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                //T model = (T)Activator.CreateInstance(typeof(T));  
                T model = new T();
                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                {
                    model.GetType().GetProperty(propertyInfo.Name).SetValue(model, dr[propertyInfo.Name], null);
                }
                modelList.Add(model);
            }
            return modelList;
        }

        /// <summary>  
        /// 填充对象：用DataRow填充实体类
        /// </summary>  
        public T FillModel(DataRow dr)
        {
            if (dr == null)
            {
                return default(T);
            }

            //T model = (T)Activator.CreateInstance(typeof(T));  
            T model = new T();
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                model.GetType().GetProperty(propertyInfo.Name).SetValue(model, dr[propertyInfo.Name], null);
            }
            return model;
        }

        #endregion

        #region 实体类转换成DataTable

        /// <summary>
        /// 实体类转换成DataSet
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public DataSet FillDataSet(List<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            else
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(FillDataTable(modelList));
                return ds;
            }
        }

        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public DataTable FillDataTable(List<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            return CreateData(modelList[0]);
        }

        /// <summary>
        /// 根据实体类得到表结构
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        private DataTable CreateData(T model)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                Type colType = propertyInfo.PropertyType;
                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    colType = colType.GetGenericArguments()[0];
                }
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name, colType));
            }
            return dataTable;
        }

        #endregion
    }
    #endregion

}
