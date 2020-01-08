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

namespace ASJ.MMSMM
{
    /// <summary>
    /// 请购单管理
    /// </summary>
    public partial class UcPurchaseREQ : BaseUserControl
    {
        //实例化帮助类
        //MMSMMHelper Helper = new MMSMMHelper();
        MMSMMHelper Helper = new MMSMMHelper();
        Result rs = new Result();


        private DataSet ds;//请购单参数
        int errorReason = 777;
        bool IsError = true;

        //请购单主表主键
        private MMSMM_PURCHASE_REQ PurchaseREQ;

        private string PurChaseTKEY;//主表Tkey变量

        /// <summary>
        /// 控件加载
        /// </summary>
        private UcPurchaseREQ()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        public UcPurchaseREQ(MMSMM_PURCHASE_REQ _PurchaseREQ) : this()
        {
            PurchaseREQ = _PurchaseREQ;
        }
        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcPurchaseREQ_Load(object sender, EventArgs e)
        {
            DataBinding(PurchaseREQ);
        }

        #region 数据处理方法
        private void DataBinding(MMSMM_PURCHASE_REQ PurchaseREQ)
        {
            string Tkey = PurchaseREQ.TKEY == string.Empty ? Guid.NewGuid().ToString() : PurchaseREQ.TKEY;
            LoadData(Tkey);//初始化数据
            BindRitemGridLookUpEdit(Helper.Query("BCMA_MATERIAL").Ds.Tables[0], "MATERIAL_CODE", "TKEY", ReGridLookUpEdit);//绑定GridView中GridLookUpEdit的下拉框的值
            BindReLookUpEdit(Helper.Query("BCDF_UNIT").Ds.Tables[0],"UNIT_NAME", "TKEY", ReLookUpEdit);//绑定GridView中LookUpEdit的下拉框的值

            #region 绑定下拉框的值  系统数据字典 AND 其他特殊表(部门,人员)
            List<LookUpEdit> Control = new List<LookUpEdit>();
            List<string> Para = new List<string>();

            //请购单类型
            Control.Add(txtREQUEST_TYPE);
            Para.Add("MMSMM_PURCHASE_REQ_REQUEST_TYPE");
            //转换来源类型
            Control.Add(txtPQSOURCE_SYSTEM_TYPE);
            Para.Add("MMSMM_PURCHASE_REQ_PQSOURCE_SYSTEM_TYPE");
            //转换单据类型
            Control.Add(txtPQSOURCE_ORDER_TYPE);
            Para.Add("MMSMM_PURCHASE_REQ_PQSOURCE_ORDER_TYPE");
            //转换方式类型
            Control.Add(txtPQSOURCE_TRANS_TYPE);
            Para.Add("MMSMM_PURCHASE_REQ_PQSOURCE_TRANS_TYPE");

            Helper.BindLookUpEdit(Control, Para);//绑定下拉框的值 系统数据字典


            #endregion

        }

        /// <summary>
        /// 初始化加载数据
        /// </summary>
        /// <param name="TKEY">工序自定义参数表 TKEY</param>
        public void LoadData(string TKEY)
        {

            List<string> strsql = new List<string>();
            List<string> TableNames = new List<string>();
            string SqlMaster = @" SELECT * FROM MMSMM_PURCHASE_REQ WHERE FLAG = 1  AND TKEY = " + "'" + TKEY + "'";
            strsql.Add(SqlMaster);//主档
            TableNames.Add("MMSMM_PURCHASE_REQ");
            ds = OracleHelper.Get_DataSet(strsql, TableNames);

            //-----------------------------------------------------

            PurChaseTKEY = ds.Tables["MMSMM_PURCHASE_REQ"].Rows.Count == 0 ? string.Empty : Guid.NewGuid().ToString();//主键
            txtREQUEST_NO.EditValue = ds.Tables["MMSMM_PURCHASE_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_PURCHASE_REQ"].Rows[0]["REQUEST_NO"].ToString();//请购单号
            txtREQUEST_TYPE.EditValue = ds.Tables["MMSMM_PURCHASE_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_PURCHASE_REQ"].Rows[0]["REQUEST_TYPE"].ToString();//请购单类型
            txtPQSOURCE_SYSTEM_TYPE.EditValue = ds.Tables["MMSMM_PURCHASE_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_PURCHASE_REQ"].Rows[0]["PQSOURCE_SYSTEM_TYPE"].ToString();//转换来源系统类型:
            txtPQSOURCE_ORDER_TYPE.EditValue = ds.Tables["MMSMM_PURCHASE_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_PURCHASE_REQ"].Rows[0]["PQSOURCE_ORDER_TYPE"].ToString();//转换单据类型
            txtPQSOURCE_TRANS_TYPE.EditValue = ds.Tables["MMSMM_PURCHASE_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_PURCHASE_REQ"].Rows[0]["PQSOURCE_TRANS_TYPE"].ToString();//转换方式类型
            //txtREQUEST_DEPT_TKEY.EditValue = ds.Tables["MMSMM_PURCHASE_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_PURCHASE_REQ"].Rows[0]["REQUEST_DEPT_TKEY"].ToString();//请购部门
            //txtREQUEST_EMPLOYEE_TKEY.EditValue = ds.Tables["MMSMM_PURCHASE_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_PURCHASE_REQ"].Rows[0]["REQUEST_EMPLOYEE"].ToString();//请购职员
            txtREQUEST_REASON.EditValue = ds.Tables["MMSMM_PURCHASE_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_PURCHASE_REQ"].Rows[0]["REQUEST_REASON"].ToString();//请购原因
            txtREQUEST_DESC.EditValue = ds.Tables["MMSMM_PURCHASE_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_PURCHASE_REQ"].Rows[0]["REQUEST_DESC"].ToString();//请购说明
            txtCMT.EditValue = ds.Tables["MMSMM_PURCHASE_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_PURCHASE_REQ"].Rows[0]["CMT"].ToString();//备注

            //rs = Helper.Query("MMSMM_PURCHASE_REQ_D", TKEY, "CKEY");//根据关联的TKEY带出子表的数据 绑定GridControl数据源
            rs = Helper.QueryBindGridView();
            Helper.BindDataSourceForGridControl(GridItem, GrvPurDetail, rs.Ds.Tables[0]);//绑定GridControl
        }

        /// <summary>
        /// 保存方法
        /// </summary>
        public void SaveFunction()
        {
            //非空校验
            string ErrMsgText = string.Empty;
            ErrMsgText = JudgeEmpty();
            if (ErrMsgText.Length > 0)
            {
                XtraMessageBox.Show(ErrMsgText, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
                return;
            }

            #region 控件内容赋值到Dataset
            ds.Tables["MMSMM_PURCHASE_REQ"].Rows[0]["REQUEST_NO"] = txtREQUEST_NO.EditValue ?? txtREQUEST_NO.EditValue.ToString();
            ds.Tables["MMSMM_PURCHASE_REQ"].Rows[0]["REQUEST_TYPE"] = txtREQUEST_TYPE.EditValue ?? txtREQUEST_TYPE.EditValue.ToString();
            ds.Tables["MMSMM_PURCHASE_REQ"].Rows[0]["PQSOURCE_SYSTEM_TYPE"] = txtPQSOURCE_SYSTEM_TYPE.EditValue ?? txtPQSOURCE_SYSTEM_TYPE.EditValue.ToString();
            ds.Tables["MMSMM_PURCHASE_REQ"].Rows[0]["PQSOURCE_ORDER_TYPE"] = txtPQSOURCE_ORDER_TYPE.EditValue ?? txtPQSOURCE_ORDER_TYPE.EditValue.ToString();
            ds.Tables["MMSMM_PURCHASE_REQ"].Rows[0]["PQSOURCE_TRANS_TYPE"] = txtPQSOURCE_TRANS_TYPE.EditValue ?? txtPQSOURCE_TRANS_TYPE.EditValue.ToString();
            ds.Tables["MMSMM_PURCHASE_REQ"].Rows[0]["REQUEST_REASON"] = txtREQUEST_REASON.EditValue ?? txtREQUEST_REASON.EditValue.ToString();
            ds.Tables["MMSMM_PURCHASE_REQ"].Rows[0]["REQUEST_DESC"] = txtREQUEST_DESC.EditValue ?? txtREQUEST_DESC.EditValue.ToString();
            ds.Tables["MMSMM_PURCHASE_REQ"].Rows[0]["CMT"] = txtCMT.EditValue ?? txtCMT.EditValue.ToString();
            #endregion


            #region Gridview中有数据 则将GridView中的值存到Dataset中 一般都会有值 保存前Check GridView是否有数据
            DataTable dt = ((DataView)GrvPurDetail.DataSource).ToTable();
            dt.TableName = "MMSMM_PURCHASE_REQ_D";
            if (dt.Rows.Count > 0)
            {
                DataTable dtC = dt.Copy();
                //删除不属于该实体的Datatable列
                foreach (DataColumn dc in dt.Columns)
                {
                    switch (dc.ColumnName)
                    {
                        case "MATERIAL_CODE":
                        case "MATERIAL_NAME":
                        case "MAPID":
                            dtC.Columns.Remove(dc.ColumnName);
                            break;
                    }
                } 

                ds.Tables.Add(dtC);
            }
            #endregion

            OracleHelper.UpdateDataSet(ds);//更新整个Dataset

        }
        #endregion

        #region 特殊字段非空校验
        /// <summary>
        /// 字段非空校验 TextEdit
        /// </summary>
        /// <returns></returns>
        public string JudgeEmpty()
        {
            StringBuilder sbErrMsg = new StringBuilder();
            string ErrMsg = string.Empty;
            DataTable dt = ((DataView)GrvPurDetail.DataSource).ToTable();
            //请购单主表 非空验证
            if (string.IsNullOrEmpty(txtREQUEST_NO.EditValue.ToString())) sbErrMsg.Append("请购单号不能为空,\n");
            if (string.IsNullOrEmpty(txtREQUEST_TYPE.EditValue.ToString())) sbErrMsg.Append("请购单类型不能为空,\n");
            if (string.IsNullOrEmpty(txtPQSOURCE_SYSTEM_TYPE.EditValue.ToString())) sbErrMsg.Append("转换来源来类型不能为空,\n");
            if (string.IsNullOrEmpty(txtPQSOURCE_ORDER_TYPE.EditValue.ToString())) sbErrMsg.Append("转换单据类型不能为空,\n");
            if (string.IsNullOrEmpty(txtPQSOURCE_TRANS_TYPE.EditValue.ToString())) sbErrMsg.Append("转换方式类型不能为空,\n");
            if (string.IsNullOrEmpty(txtREQUEST_DEPT_TKEY.EditValue.ToString())) sbErrMsg.Append("请购部门不能为空,\n");
            if (string.IsNullOrEmpty(txtREQUEST_EMPLOYEE_TKEY.EditValue.ToString())) sbErrMsg.Append("请购职员不能为空,\n");
            if (string.IsNullOrEmpty(txtREQUEST_REASON.EditValue.ToString())) sbErrMsg.Append("请购原因不能为空,\n");
            if (string.IsNullOrEmpty(txtREQUEST_DESC.EditValue.ToString())) sbErrMsg.Append("请购说明不能为空,\n");

            //请购单明细 GridView验证非空
            if (dt.Rows.Count == 0) sbErrMsg.Append("需新增请购单明细资料 \n");

            return sbErrMsg.ToString();
        }

        #endregion

        #region GridView 事件  (新增行 删除行 特定单元格不能编辑)
        //新增行按钮 
        private void BarBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GrvPurDetail.AddNewRow();//在GridControl中新增一行
            this.GrvPurDetail.SetFocusedRowCellValue("TKEY", Guid.NewGuid().ToString());//请购单明细表主键
            this.GrvPurDetail.SetFocusedRowCellValue("CKEY", ds.Tables["MMSMM_PURCHASE_REQ"].Rows.Count == 0 ? PurChaseTKEY : ds.Tables["MMSMM_PURCHASE_REQ"].Rows[0]["TKEY"].ToString());//主表关联主键
            this.GrvPurDetail.SetFocusedRowCellValue("MATERIAL_TKEY", string.Empty);//物料TKEY  明细表中
            this.GrvPurDetail.SetFocusedRowCellValue("MATERIAL_CODE", null);//物料编码 物料表 后续需删除列
            this.GrvPurDetail.SetFocusedRowCellValue("MAPID", string.Empty);//物料图号  物料表 后续需删除列
            this.GrvPurDetail.SetFocusedRowCellValue("MATERIAL_NAME", string.Empty);//物料名称  物料表 后续需删除列
            this.GrvPurDetail.SetFocusedRowCellValue("BASE_UNIT_TKEY", string.Empty);//基本计量单位名称  
            this.GrvPurDetail.SetFocusedRowCellValue("REQUEST_QTY", 0);//请购数量
            this.GrvPurDetail.SetFocusedRowCellValue("REQ_D_STATUS", 0);//明细状态
            this.GrvPurDetail.SetFocusedRowCellValue("REQUEST_DATE", DateTime.Now);//需求日期
            this.GrvPurDetail.SetFocusedRowCellValue("PLAN_DELIVERY_DATE", DateTime.Now.AddDays(1));//计划交期
            this.GrvPurDetail.SetFocusedRowCellValue("CMT", "");//备注

            GrvPurDetail.OptionsBehavior.Editable = true;//栏位可编辑
        }

        //删除行按钮
        private void BarBtnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GrvPurDetail.SelectedRowsCount == 0) { XtraMessageBox.Show("请先选中行再进行删除！", "删除提示框", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            //删除选中的行
            GrvPurDetail.DeleteSelectedRows();

        }


        /// <summary>
        /// GridView 特定单元格不能编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrvPurDetail_ShowingEditor(object sender, CancelEventArgs e)
        {
            switch (GrvPurDetail.FocusedColumn.FieldName)
            {
                case "MAPID"://物料图号
                case "MATERIAL_NAME"://物料名称
                case "BASE_UNIT_TKEY"://基本计量单位名称
                case "REQ_D_STATUS"://明细状态
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
            column.Caption = "物料主键";
            column.Visible = true;

            column = GridLookUpEdit.View.Columns.AddField("MATERIAL_CODE");
            column.Caption = "物料编码";
            column.Visible = true;

            column = GridLookUpEdit.View.Columns.AddField("MATERIAL_NAME");
            column.Caption = "物料名称";
            column.Visible = true;

            column = GridLookUpEdit.View.Columns.AddField("MAPID");
            column.Caption = "图号";
            column.Visible = true;
        }

        /// <summary>
        /// 绑定ReLookUpEdit的下拉框的值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="DisplayMember"></param>
        /// <param name="ValueMember"></param>
        /// <param name="LookUpEdit"></param>
        private void BindReLookUpEdit(DataTable dt, string DisplayMember, string ValueMember, RepositoryItemLookUpEdit LookUpEdit)
        {
            LookUpEdit.ValueMember = ValueMember;
            LookUpEdit.DisplayMember = DisplayMember;
            LookUpEdit.DataSource = dt;

            LookUpEdit.NullText = null;

        }

        /// <summary>
        /// GridView 行内单元格编辑 - BCMA_MATERIAL表 选择物料编码 带出 图号 计量单位 物料名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReGridLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            BaseEdit edit = GrvPurDetail.ActiveEditor;
            GridLookUpEdit gridlookupedit = sender as GridLookUpEdit;
            switch (GrvPurDetail.FocusedColumn.FieldName)
            {
                case "MATERIAL_CODE":
                    string MaterialTkey = gridlookupedit.EditValue?.ToString();//物料表主键
                    dt = Helper.Query("BCMA_MATERIAL", MaterialTkey).Ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        GrvPurDetail.SetFocusedRowCellValue("MATERIAL_TKEY", MaterialTkey);//物料Key  MATERIAL_TKEY
                        GrvPurDetail.SetFocusedRowCellValue("MATERIAL_NAME", dt.Rows[0]["MATERIAL_NAME"].ToString() == "" ? string.Empty : dt.Rows[0]["MATERIAL_NAME"].ToString());//物料名称 MATERIAL_NAME
                        GrvPurDetail.SetFocusedRowCellValue("MAPID", dt.Rows[0]["MAPID"].ToString() == "" ? string.Empty : dt.Rows[0]["MAPID"].ToString());//图号 MAPID
                        GrvPurDetail.SetFocusedRowCellValue("BASE_UNIT_TKEY", dt.Rows[0]["BASE_UNIT_TKEY"].ToString() == "" ? string.Empty : dt.Rows[0]["BASE_UNIT_TKEY"].ToString());//基本计量单位TKEY BASE_UNIT_TKEY
                    }
                    break;
            }
        }


        #endregion

        #region GridView 单元格输入验证


        /// <summary>
        /// GridControl单元格内输入数据验证  NumberEdit --   请购数量允许重复但不能为0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReNumberEdit_Validating_1(object sender, CancelEventArgs e)
        {
            BaseEdit numberEdit = sender as BaseEdit;
            if (numberEdit.Text.Length == 0)
            {
                e.Cancel = true;
                errorReason = 0;
                return;
            }
            else if(numberEdit.Text == "0")
            {
                e.Cancel = true;
                errorReason = 2;
                return;
            }
        }

        /// <summary>
        /// GridControl单元格内输入数据验证  TextEdit 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReTextEdit_Validating(object sender, CancelEventArgs e)
        {
            BaseEdit textedit = sender as BaseEdit;
            if (textedit.Text.Length == 0)
            {
                e.Cancel = true;
                errorReason = 0;
                return;
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
                return;
            }
        }

        /// <summary>
        /// GridControl单元格内输入数据验证  LookUpEdit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReLookUpEdit_Validating(object sender, CancelEventArgs e)
        {
            BaseEdit dateedit = sender as BaseEdit;
            if (dateedit.Text.Length == 0)
            {
                e.Cancel = true;
                errorReason = 0;
                return;
            }
        }

        /// <summary>
        /// GridView 单元格数据验证  错误信息提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrvDIYPMT_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
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
        #endregion

    }
}
