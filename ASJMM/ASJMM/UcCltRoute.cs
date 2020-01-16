using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ASJ.BASE;
using ASJ.ENTITY;
using ASJ.TOOLS;
using ASJ.TOOLS.Basic;
using ASJ.TOOLS.Data;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;

namespace ASJMM
{
    /// <summary>
    /// 物料单据采集路线维护
    /// </summary>
    public partial class UcCltRoute : BaseUserControl
    {
        //实例化帮助类
        ASJMM_CLTROUTE MHelper = new ASJMM_CLTROUTE();
        
        //实体
        private MMSMM_CLTROUTE cltroute;

        private DataSet ds;
        private string cltroutetkey;//采集路线主表主键
        int errorReason = 777;
        bool IsError = true;
        Result rs = new Result();

        /// <summary>
        /// 控件加载
        /// </summary>
        public UcCltRoute()
        {
            InitializeComponent();
            MHelper.BindCustomDrawRowIndicator(GrvCltRoute);//GridView显示行号 宽度自适应

        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_cltroute"></param>
        public UcCltRoute(MMSMM_CLTROUTE _cltroute) : this()
        {
            cltroute = _cltroute;
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcCltRoute_Load(object sender, EventArgs e)
        {
            DataBinding(cltroute);
        }


        #region 数据绑定 数据处理
        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="cltroute"></param>
        public void DataBinding(MMSMM_CLTROUTE cltroute)
        {
            cltroutetkey = cltroute.TKEY == string.Empty ? Guid.NewGuid().ToString() : cltroute.TKEY;
            LoadData(cltroutetkey);//初始化加载

            BindReGridLookUpEdit();
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="Tkey"></param>
        public void LoadData(string Tkey)
        {
            ds = MHelper.CLTRouteLoad(Tkey);//FrmDataLoad

            //-----------------------------------------------------
            txtCLTROUTE_CODE.EditValue = ds.Tables["MMSMM_CLTROUTE"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_CLTROUTE"].Rows[0]["CLTROUTE_CODE"].ToString();//采集路线编码
            txtCLTROUTE_NAME.EditValue = ds.Tables["MMSMM_CLTROUTE"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_CLTROUTE"].Rows[0]["CLTROUTE_NAME"].ToString();//采集路线名称
            txtCMT.EditValue = ds.Tables["MMSMM_CLTROUTE"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_CLTROUTE"].Rows[0]["CMT"].ToString();//备注
            chCLTROUTE_READ_FLAG.EditValue = ds.Tables["MMSMM_CLTROUTE"].Rows.Count == 0 ? 0 : int.Parse(ds.Tables["MMSMM_CLTROUTE"].Rows[0]["CLTROUTE_READ_FLAG"].ToString());//配置完成标识
            BindGridViewDataSource(cltroutetkey);//绑定GridView的数据源
        }

        /// <summary>
        /// 保存方法
        /// </summary>
        public void SaveFunction()
        {
            //非空校验
            string ErrMsg = JudgeEmpty();
            if (ErrMsg.Length > 0)
            {
                XtraMessageBox.Show(ErrMsg, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
                return;
            }

            #region 控件值赋值到Dataset  
            ds.Tables["MMSMM_CLTROUTE"].Rows[0]["TKEY"] = cltroutetkey;
            ds.Tables["MMSMM_CLTROUTE"].Rows[0]["CLTROUTE_CODE"] = txtCLTROUTE_CODE.EditValue ?? txtCLTROUTE_CODE.EditValue.ToString();
            ds.Tables["MMSMM_CLTROUTE"].Rows[0]["CLTROUTE_NAME"] = txtCLTROUTE_NAME.EditValue ?? txtCLTROUTE_NAME.EditValue.ToString();
            ds.Tables["MMSMM_CLTROUTE"].Rows[0]["CMT"] = txtCMT.EditValue ?? txtCMT.EditValue.ToString();

            ds.Tables["MMSMM_CLTROUTE"].Rows[0]["CLTROUTE_READ_FLAG"] = chCLTROUTE_READ_FLAG.EditValue ?? chCLTROUTE_READ_FLAG.EditValue.ToString();

            #endregion

            #region GridView中的数据保存到Dataset中  
            DataTable dt = ((DataView)GrvCltRoute.DataSource).ToTable();
            dt.TableName = "MMSMM_CLTROUTE_SEQ";
            if (dt.Rows.Count > 0)
            {
                DataTable dtC = dt.Copy();
                //删除不属于该实体的Datatable列
                foreach (DataColumn dc in dt.Columns)
                {
                    switch (dc.ColumnName)
                    {
                        case "CLTNODE_CODE":
                        case "CLTNODE_NAME":
                            dtC.Columns.Remove(dc.ColumnName);
                            break;
                    }
                }
            }

            #endregion

            OracleHelper.UpdateDataSet(ds);//更新整个Dataset

        }

        #region 绑定下拉框的值
        public void BindReGridLookUpEdit()
        {
            List<RepositoryItemGridLookUpEdit> Control = new List<RepositoryItemGridLookUpEdit>();
            Control.Add(ReGridLookUpEdit);
            MHelper.BindReGridLookUpEdit_CLTRoute(Control);
        }
        #endregion

        /// <summary>
        /// 绑定GridView的数据源
        /// </summary>
        /// <param name="TKEY"></param>
        public void BindGridViewDataSource(string TKEY)
        {
            MHelper.BindDataSourceForGridControl(GridItem, GrvCltRoute, "MMSMM_CLTROUTE_SEQ", TKEY);
        }

        #endregion

        #region 特殊字段非空校验
        /// <summary>
        /// 非空校验
        /// </summary>
        public string JudgeEmpty()
        {
            StringBuilder sbErrmsg = new StringBuilder();
            string Errmsg = string.Empty;
            DataTable dt = ((DataView)GrvCltRoute.DataSource).ToTable();//GridView中的数据转成Datatable 方便检查是否为空
            //采集路线主表非空验证
            if (string.IsNullOrEmpty(txtCLTROUTE_CODE.EditValue.ToString())) sbErrmsg.Append("采集路线编码不能为空 \n");
            if (string.IsNullOrEmpty(txtCLTROUTE_NAME.EditValue.ToString())) sbErrmsg.Append("采集路线名称不能为空 \n");

            //采集路线明细 GridView验证非空
            if (dt.Rows.Count == 0) sbErrmsg.Append("需要新增采集路线明细 ");

            return sbErrmsg.ToString();
        }

        #endregion

        #region GridView 事件 (新增行 删除行 部分单元格不能编辑 显示行号)

        //新增行
        private void BarBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GrvCltRoute.AddNewRow();//新增行
            int RowHandle = GrvCltRoute.RowCount;//新增行后获取GridView中 共多少行数据
            this.GrvCltRoute.SetFocusedRowCellValue("TKEY", Guid.NewGuid().ToString());//单据采集路线明细表主键
            this.GrvCltRoute.SetFocusedRowCellValue("CLTROUTE_SEQ", RowHandle);//采集路线顺序
            this.GrvCltRoute.SetFocusedRowCellValue("CLTNODE_TKEY", string.Empty);//采集节点TKEY
            this.GrvCltRoute.SetFocusedRowCellValue("CLTNODE_CODE", string.Empty);//采集节点编码
            this.GrvCltRoute.SetFocusedRowCellValue("CLTNODE_NAME", string.Empty);//采集节点名称
            this.GrvCltRoute.SetFocusedRowCellValue("CMT",string.Empty);//备注

            GrvCltRoute.OptionsBehavior.Editable = true;//单元格可编辑

            //this.GrvCltRoute.ValidatingEditor += new CancelEventHandler(this.ReGridLookUpEdit_Validating);

        }

        //删除行
        private void BarBtnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GrvCltRoute.SelectedRowsCount == 0) { XtraMessageBox.Show("请先选中行再进行删除！", "删除提示框", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            //删除选中的行
            GrvCltRoute.DeleteSelectedRows();

        }

        //GridView中 部分单元格无法编辑 根据特定条件自动带出
        private void GrvCltRoute_ShowingEditor(object sender, CancelEventArgs e)
        {
            switch (GrvCltRoute.FocusedColumn.FieldName)
            {
                case "TKEY"://采集路线顺序主键
                case "CLTROUTE_SEQ"://采集路线顺序
                case "CLTNODE_NAME"://采集节点名称
                case "CLTNODE_TKEY"://采集节点TKEY
                    e.Cancel = true;
                    break;
            }
        }

        /// <summary>
        /// 绑定ReGridLookUpEdit的下拉框的值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="DisplayMember"></param>
        /// <param name="ValueMember"></param>
        /// <param name="upEdit"></param>
        private void BindRitemGridLookUpEdit(DataTable dt, string DisplayMember, string ValueMember, RepositoryItemGridLookUpEdit GridLookUpEdit)
        {
            GridLookUpEdit.ValueMember = ValueMember;
            GridLookUpEdit.DisplayMember = DisplayMember;
            GridLookUpEdit.DataSource = dt;
            GridLookUpEdit.ImmediatePopup = true;//在输入框按任一可见字符键时立即弹出下拉窗体  
            GridLookUpEdit.NullText = "[请选择]";
            GridLookUpEdit.TextEditStyle = TextEditStyles.Standard;//允许选择编辑 

            //-----------------------------------------------------

            GridColumn column = GridLookUpEdit.View.Columns.AddField("TKEY");
            column.Caption = "主键";
            column.Visible = false;

            column = GridLookUpEdit.View.Columns.AddField("CLTNODE_CODE");
            column.Caption = "采集编码";
            column.Visible = true;

            column = GridLookUpEdit.View.Columns.AddField("CLTNODE_NAME");
            column.Caption = "采集编码";
            column.Visible = true;

            column = GridLookUpEdit.View.Columns.AddField("CLTNODE_TYPE");
            column.Caption = "采集类型";
            column.Visible = true;
        }

        //显示行号 行号自增
        private void GrvCltRoute_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }

        }

        #endregion

        #region GridView 单元格输入验证

        /// <summary>
        /// GridControl单元格内输入数据验证  LookUpEdit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReLookUpEdit_Validating(object sender, CancelEventArgs e)
        {
            BaseEdit lookupedit = sender as BaseEdit;
            if (lookupedit.Text.Length == 0)
            {
                e.Cancel = true;
                errorReason = 0;
            }

        }

        /// <summary>
        /// GridControl单元格内输入数据验证  DateEdit 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReDateEdit_Validating(object sender, CancelEventArgs e)
        {
            BaseEdit dateedit = sender as BaseEdit;
            if (dateedit.Text.Length == 0)
            {
                e.Cancel = true;
                errorReason = 0;
            }

        }

        /// <summary>
        ///  GridControl单元格内输入数据验证  GridLookUpEdit 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReGridLookUpEdit_Validating(object sender, CancelEventArgs e)
        {
            BaseEdit gridlookupedit = sender as BaseEdit;
            if (gridlookupedit.Text.Length == 0)
            {
                e.Cancel = true;
                errorReason = 0;
            }
        }



        /// <summary>
        /// GridView单元格数据 异常信息提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrvCltRoute_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            if (errorReason == 0)
            {
                e.ErrorText = "不允许为空！";
            }
            else if (errorReason == 1)
            {
                e.ErrorText = "不允许为重复！";
            }
            else if (errorReason == 2)
            {
                e.ErrorText = "数量不能为0";
            }
            else
            {
                e.ErrorText = "404";
            }
            IsError = false;

        }

        #endregion

        #region 控件触发事件

        /// <summary>
        /// GridView ReGirdLookUpEdit触发事件 通过选择采集节点基础档案表中的采集编码 自动捞出节点名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReGridLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            BaseEdit edit = sender as BaseEdit;
            GridLookUpEdit gridlookupedit = sender as GridLookUpEdit;
            switch(GrvCltRoute.FocusedColumn.FieldName)
            {
                case "CLTNODE_CODE":
                    string CltRouteTKEY = gridlookupedit.EditValue?.ToString();//采集节点表主键
                    dt = MHelper.Query("MMSMM_CLTNODE_BASE", CltRouteTKEY).Ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        string Name = dt.Rows[0]["CLTNODE_NAME"].ToString();
                        GrvCltRoute.SetFocusedRowCellValue("CLTNODE_TKEY",dt.Rows[0]["TKEY"].ToString() == "" ? string.Empty : dt.Rows[0]["TKEY"].ToString());//采集路线明细表中 采集节点TKEY
                        GrvCltRoute.SetFocusedRowCellValue("CLTNODE_NAME",dt.Rows[0]["CLTNODE_NAME"].ToString() == "" ? string.Empty : dt.Rows[0]["CLTNODE_NAME"].ToString());//采集节点名称
                    }
                    break;
            }
        }

        #endregion

    }
}
