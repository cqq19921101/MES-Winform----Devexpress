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
using DevExpress.XtraGrid.Views.Base;

namespace ASJMM
{
    /// <summary>
    /// 物料管理模块 - 采购订单管理新建
    /// </summary>
    public partial class UcPurchase : BaseUserControl
    {

        //实例化帮助类
        ASJMM_Purchase MHelper = new ASJMM_Purchase();
        Result rs = new Result();

        private DataSet ds;//请购单参数
        int errorReason = 777;
        bool IsError = true;

        //声明实体
        private MMSMM_PURCHASE purchase;//采购单主表实体
        private MMSMM_PURCHASE_D purchase_d;//采购明细表实体
        private MMSMM_PURCHASE_D_RULE purchase_d_rule;//采购明细执行策略实体

        private string PurChaseTKEY;//主表Tkey变量

        List<object> lstentity = new List<object>();
        

        /// <summary>
        /// 控件加载
        /// </summary>
        public UcPurchase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_purchase"></param>
        public UcPurchase(MMSMM_PURCHASE _purchase) : this()
        {
            purchase = _purchase;//采购单主表
            MHelper.BindCustomDrawRowIndicator(GrvPurDetail);//GridView新增序号栏位 自增长 宽度自适应

        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcPurchase_Load(object sender, EventArgs e)
        {
            DataBinding(purchase);
        }

        #region 数据绑定 数据处理
        /// <summary>
        /// 数据绑定
        /// </summary>
        public void DataBinding(MMSMM_PURCHASE purchase)
        {
            PurChaseTKEY = purchase.TKEY == null ? Guid.NewGuid().ToString() : purchase.TKEY;
            LoadData(PurChaseTKEY);

            #region 绑定各个下拉框的值
            BindGridLookUpEdit();//绑定采购部门 采购职员 供应商 下拉框的值
            BindLookUpEdit();//Sysc
            BindReGridLookUpEdit();//物料编码
            BindReLookUpEdit();//计量单位
            #endregion

        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        public void LoadData(string TKEY)
        {
            ds = MHelper.PurchaseLoad(TKEY);//FrmDataLoad

            //------------------------------------------------

            //PurChaseTKEY = ds.Tables["MMSMM_PURCHASE"].Rows.Count == 0 ? Guid.NewGuid().ToString() : ds.Tables["MMSMM_PURCHASE"].Rows[0]["TKEY"].ToString();//主键
            txtPURCHASE_NO.EditValue = ds.Tables["MMSMM_PURCHASE"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_PURCHASE"].Rows[0]["PURCHASE_NO"].ToString();//采购单号
            txtPURCHASE_TYPE.EditValue = ds.Tables["MMSMM_PURCHASE"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_PURCHASE"].Rows[0]["PURCHASE_TYPE"].ToString();//采购订单类型
            txtPURCHASE_DEPT_TKEY.EditValue = ds.Tables["MMSMM_PURCHASE"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_PURCHASE"].Rows[0]["PURCHASE_DEPT_TKEY"].ToString();//采购部门
            txtPURCHASE_EMPLOYEE_TKEY.EditValue = ds.Tables["MMSMM_PURCHASE"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_PURCHASE"].Rows[0]["PURCHASE_EMPLOYEE_TKEY"].ToString();//采购职员
            txtPURCHASE_REASON.EditValue = ds.Tables["MMSMM_PURCHASE"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_PURCHASE"].Rows[0]["PURCHASE_REASON"].ToString();//采购原因
            txtPURCHASE_DESC.EditValue = ds.Tables["MMSMM_PURCHASE"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_PURCHASE"].Rows[0]["PURCHASE_DESC"].ToString();//采购说明
            txtSUPPLIER_TKEY.EditValue = ds.Tables["MMSMM_PURCHASE"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_PURCHASE"].Rows[0]["SUPPLIER_TKEY"].ToString();//所属供应商
            txtCONTRACT_NO.EditValue = ds.Tables["MMSMM_PURCHASE"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_PURCHASE"].Rows[0]["CONTRACT_NO"].ToString();//合同号
            txtCMT.EditValue = ds.Tables["MMSMM_PURCHASE"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_PURCHASE"].Rows[0]["CMT"].ToString();//备注

            BindGridViewDataSource(PurChaseTKEY);//绑定GridView数据源


        }

        /// <summary>
        /// 保存方法
        /// </summary>
        public DataSet SaveFunction() 
        {
            List<object> lstentity = new List<object>();

            try
            {
                //非空校验
                string ErrMsgText = string.Empty;
                ErrMsgText = JudgeEmpty();
                if (ErrMsgText.Length > 0)
                {
                    XtraMessageBox.Show(ErrMsgText, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                #region 控件内容赋值到Dataset
                if (ds.Tables["MMSMM_PURCHASE"].Rows.Count == 0) MHelper.InsertNewRowForDatatable(ds, "MMSMM_PURCHASE");
                ds.Tables["MMSMM_PURCHASE"].Rows[0]["TKEY"] = PurChaseTKEY;
                ds.Tables["MMSMM_PURCHASE"].Rows[0]["PURCHASE_NO"] = txtPURCHASE_NO.EditValue ?? txtPURCHASE_NO.EditValue.ToString();
                ds.Tables["MMSMM_PURCHASE"].Rows[0]["PURCHASE_NO"] = txtPURCHASE_NO.EditValue ?? txtPURCHASE_NO.EditValue.ToString();
                ds.Tables["MMSMM_PURCHASE"].Rows[0]["PURCHASE_TYPE"] = txtPURCHASE_TYPE.EditValue ?? txtPURCHASE_TYPE.EditValue.ToString();
                ds.Tables["MMSMM_PURCHASE"].Rows[0]["PURCHASE_DEPT_TKEY"] = txtPURCHASE_DEPT_TKEY.EditValue ?? txtPURCHASE_DEPT_TKEY.EditValue.ToString();
                ds.Tables["MMSMM_PURCHASE"].Rows[0]["PURCHASE_EMPLOYEE_TKEY"] = txtPURCHASE_EMPLOYEE_TKEY.EditValue ?? txtPURCHASE_EMPLOYEE_TKEY.EditValue.ToString();
                ds.Tables["MMSMM_PURCHASE"].Rows[0]["PURCHASE_REASON"] = txtPURCHASE_REASON.EditValue ?? txtPURCHASE_REASON.EditValue.ToString();
                ds.Tables["MMSMM_PURCHASE"].Rows[0]["PURCHASE_DESC"] = txtPURCHASE_DESC.EditValue ?? txtPURCHASE_DESC.EditValue.ToString();
                ds.Tables["MMSMM_PURCHASE"].Rows[0]["SUPPLIER_TKEY"] = txtSUPPLIER_TKEY.EditValue ?? txtSUPPLIER_TKEY.EditValue.ToString();
                ds.Tables["MMSMM_PURCHASE"].Rows[0]["CONTRACT_NO"] = txtCONTRACT_NO.EditValue ?? txtCONTRACT_NO.EditValue.ToString();
                ds.Tables["MMSMM_PURCHASE"].Rows[0]["CMT"] = txtCMT.EditValue ?? txtCMT.EditValue.ToString();

                #endregion

                #region Gridview中有数据 则将GridView中的值存到Dataset中 一般都会有值 保存前Check GridView是否有数据
                DataTable dt = ((DataView)GrvPurDetail.DataSource).ToTable();//GridView数据源转成Datatable
                if (dt.Rows.Count > 0)
                {

                    //GridView中的数据赋值到对应的实体中
                    foreach (DataRow dr in dt.Rows)
                    {
                        //采购明细主表
                        purchase_d.TKEY = dr["TKEY"].ToString();
                        purchase_d.CKEY = dr["CKEY"].ToString();
                        purchase_d.MATERIAL_TKEY = dr["MATERIAL_TKEY"].ToString();
                        purchase_d.BASE_UNIT_TKEY = dr["BASE_UNIT_TKEY"].ToString();
                        purchase_d.REQUEST_QTY = decimal.Parse(dr["REQUEST_QTY"].ToString());
                        purchase_d.PURCHASE_D_STATUS = dr["PURCHASE_D_STATUS"].ToString();
                        purchase_d.REQUEST_DATE = DateTime.Parse(dr["REQUEST_DATE"].ToString());
                        purchase_d.PLAN_DELIVERY_DATE = DateTime.Parse(dr["PLAN_DELIVERY_DATE"].ToString());
                        purchase_d.SUPPLIER_TKEY = dr["SUPPLIER_TKEY"].ToString();
                        purchase_d.CMT = dr["CMT"].ToString();

                        //采购订单明细执行策略
                        purchase_d_rule.TKEY = Guid.NewGuid().ToString();
                        purchase_d_rule.PURCHASE_D = dr["TKEY"].ToString();
                        purchase_d_rule.CONCESSION_RECEIVE_FLAG = int.Parse(dr["CONCESSION_RECEIVE_FLAG"].ToString());
                        purchase_d_rule.PURCHASE_RETURN_FLAG = int.Parse(dr["PURCHASE_RETURN_FLAG"].ToString());
                        purchase_d_rule.DELIVERY_ACTIVE_FLAG = int.Parse(dr["DELIVERY_ACTIVE_FLAG"].ToString());
                        purchase_d_rule.IQC_FLAG = int.Parse(dr["IQC_FLAG"].ToString());
                        purchase_d_rule.SUPPLIER_LOT_FLAG = int.Parse(dr["SUPPLIER_LOT_FLAG"].ToString());
                        purchase_d_rule.CMT = dr["CMT"].ToString();

                        lstentity.Add(purchase);
                        lstentity.Add(purchase_d_rule);

                    }


                }
                #endregion

                return ds;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return  null;
                return null ;
            }


        }

        #region 绑定下拉框的值
        /// <summary>
        /// 绑定GirdView数据源
        /// </summary>
        public void BindGridViewDataSource(string TKEY)
        {
            MHelper.BindDataSourceForGridControl(GridItem, GrvPurDetail, TKEY);//绑定GridControl
        }

        /// <summary>
        /// 绑定GridView中 ReGridLookUpEdit下拉框的值
        /// </summary>
        public void BindReGridLookUpEdit()
        {
            List<RepositoryItemGridLookUpEdit> Control = new List<RepositoryItemGridLookUpEdit>();
            Control.Add(ReGridLookUpEdit);//物料编码
            MHelper.BindReGridLookUpEdit_Purcahse(Control);
        }

        public void BindReLookUpEdit()
        {
            List<RepositoryItemLookUpEdit> Control = new List<RepositoryItemLookUpEdit>();
            Control.Add(ReLookUpEdit);//计量单位 
            MHelper.BindReLookUpEdit_Purchase(Control);
        }

        /// <summary>
        /// 绑定请购部门，请购职员 下拉框的值
        /// </summary>
        public void BindGridLookUpEdit()
        {
            List<GridLookUpEdit> Control = new List<GridLookUpEdit>();
            Control.Add(txtPURCHASE_DEPT_TKEY);//采购部门
            Control.Add(txtPURCHASE_EMPLOYEE_TKEY);//采购职员
            Control.Add(txtSUPPLIER_TKEY);//供应商
            MHelper.BindGridLookUpEdit_Purchase(Control);
        }

        public void BindLookUpEdit()
        {
            MHelper.BindSysDict(txtPURCHASE_TYPE, "MMSMM_PURCHASE__PURCHASE_TYPE");//采购订单类型绑定
        }

        #endregion
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
            DataTable dt = ((DataView)GrvPurDetail.DataSource).ToTable();//GridView中的数据转成Datatable 方便检查是否为空
            //采购单主表 非空验证
            if (string.IsNullOrEmpty(txtPURCHASE_NO.EditValue.ToString())) sbErrMsg.Append("采购单号不能为空,\n");
            if (string.IsNullOrEmpty(txtPURCHASE_TYPE.EditValue.ToString())) sbErrMsg.Append("采购订单类型不能为空,\n");
            if (string.IsNullOrEmpty(txtPURCHASE_DEPT_TKEY.EditValue.ToString())) sbErrMsg.Append("采购部门不能为空,\n");
            if (string.IsNullOrEmpty(txtPURCHASE_EMPLOYEE_TKEY.EditValue.ToString())) sbErrMsg.Append("采购人员不能为空,\n");
            if (string.IsNullOrEmpty(txtPURCHASE_REASON.EditValue.ToString())) sbErrMsg.Append("采购原因不能为空,\n");
            if (string.IsNullOrEmpty(txtPURCHASE_DESC.EditValue.ToString())) sbErrMsg.Append("采购说明不能为空,\n");
            if (string.IsNullOrEmpty(txtSUPPLIER_TKEY.EditValue.ToString())) sbErrMsg.Append("供应商不能为空,\n");
            if (string.IsNullOrEmpty(txtCONTRACT_NO.EditValue.ToString())) sbErrMsg.Append("合同号不能为空,\n");

            //采购单明细 GridView验证非空
            if (dt.Rows.Count == 0) sbErrMsg.Append("需新增采购单明细资料 \n");

            return sbErrMsg.ToString();
        }
        #endregion

        #region GridView 事件  (新增行 删除行 特定单元格不能编辑 显示行号 日期校验)

        //新增行
        private void BarBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GrvPurDetail.AddNewRow();//在GridControl中新增一行
            this.GrvPurDetail.SetFocusedRowCellValue("TKEY", Guid.NewGuid().ToString());//请购单明细表主键
            this.GrvPurDetail.SetFocusedRowCellValue("CKEY", ds.Tables["MMSMM_PURCHASE"].Rows.Count == 0 ? PurChaseTKEY : ds.Tables["MMSMM_PURCHASE"].Rows[0]["TKEY"].ToString());//主表关联主键
            this.GrvPurDetail.SetFocusedRowCellValue("MATERIAL_TKEY", string.Empty);//物料TKEY  明细表中
            this.GrvPurDetail.SetFocusedRowCellValue("MATERIAL_CODE", null);//物料编码 物料表 后续需删除列
            this.GrvPurDetail.SetFocusedRowCellValue("MAPID", string.Empty);//物料图号  物料表 后续需删除列
            this.GrvPurDetail.SetFocusedRowCellValue("MATERIAL_NAME", string.Empty);//物料名称  物料表 后续需删除列
            this.GrvPurDetail.SetFocusedRowCellValue("BASE_UNIT_TKEY", string.Empty);//基本计量单位名称  
            this.GrvPurDetail.SetFocusedRowCellValue("REQUEST_QTY", 0);//请购数量
            this.GrvPurDetail.SetFocusedRowCellValue("PURCHASE_D_STATUS", "0");//明细状态
            this.GrvPurDetail.SetFocusedRowCellValue("REQUEST_DATE", DateTime.Now);//需求日期
            this.GrvPurDetail.SetFocusedRowCellValue("PLAN_DELIVERY_DATE", DateTime.Now.AddDays(1));//计划交期

            this.GrvPurDetail.SetFocusedRowCellValue("CONCESSION_RECEIVE_FLAG", 0);//允许让步接收标识
            this.GrvPurDetail.SetFocusedRowCellValue("PURCHASE_RETURN_FLAG", 0);//允许退料标识
            this.GrvPurDetail.SetFocusedRowCellValue("DELIVERY_ACTIVE_FLAG", 0);//到货接收启用标识
            this.GrvPurDetail.SetFocusedRowCellValue("IQC_FLAG", 0);//启用来料检验标识
            this.GrvPurDetail.SetFocusedRowCellValue("SUPPLIER_LOT_FLAG", 0);//启用供应商批次标识
            this.GrvPurDetail.SetFocusedRowCellValue("CMT", string.Empty);//备注
            this.GrvPurDetail.SetFocusedRowCellValue("PURCHASE_STATUS", "0");//采购订单状态

            GrvPurDetail.OptionsBehavior.Editable = true;//栏位可编辑
        }

        //删除行
        private void BarBtnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GrvPurDetail.SelectedRowsCount == 0) { XtraMessageBox.Show("请先选中行再进行删除！", "删除提示框", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            //删除选中的行
            GrvPurDetail.DeleteSelectedRows();
        }

        //显示行号 行号自增
        private void GrvPurDetail_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
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

        #endregion

        #region GridView 单元格输入验证
        /// <summary>
        /// GridControl单元格内输入数据验证  DateEdit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void ReDateEdit_Validating(object sender, CancelEventArgs e)
        {

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
        /// GridControl单元格内输入数据验证  NumberEdit  采购数量允许重复但不能为0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void ReNumberEdit_Validating(object sender, CancelEventArgs e)
        {
            BaseEdit numberEdit = sender as BaseEdit;
            if (numberEdit.Text.Length == 0)
            {
                e.Cancel = true;
                errorReason = 0;
                return;
            }
            else if (numberEdit.Text == "0")
            {
                e.Cancel = true;
                errorReason = 2;
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
            BaseEdit lookupedit = sender as BaseEdit;
            if (lookupedit.Text.Length == 0)
            {
                e.Cancel = true;
                errorReason = 0;
                return;
            }

        }

        /// <summary>
        /// GridControl单元格内输入数据验证  GridLookUpEdit
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
                return;
            }
        }

        /// <summary>
        /// GridView 单元格数据验证  错误信息提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrvPurDetail_InvalidValueException(object sender, InvalidValueExceptionEventArgs e)
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

        /// <summary>
        /// 日期校验
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrvPurDetail_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            ColumnView view = sender as ColumnView;
            GridColumn StarDate = view.Columns["REQUEST_DATE"];
            GridColumn EndDate = view.Columns["PLAN_DELIVERY_DATE"];
            //Get the value of the first column
            DateTime StartTime = (DateTime)view.GetRowCellValue(e.RowHandle, StarDate);
            //Get the value of the second column
            DateTime EndTime = (DateTime)view.GetRowCellValue(e.RowHandle, EndDate);
            //Validity criterion
            if (StartTime >= EndTime)
            {
                e.Valid = false;
                //Set errors with specific descriptions for the columns
                view.SetColumnError(StarDate, "需求日期必须小于计划交期");
                view.SetColumnError(EndDate, "计划交期必须大于需求日期");
            }
        }

        #endregion

        #region 控件触发事件
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
            MHelper.BindReGLE_BCMA_MATERIAL(GrvPurDetail, gridlookupedit, "BASE_UNIT_TKEY");
        }

        #region GridLookUpEdit ReGridLookUpEdit 多列模糊查询
        private void txtPURCHASE_EMPLOYEE_TKEY_EditValueChanging(object sender, ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                MHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtPURCHASE_EMPLOYEE_TKEY_Popup(object sender, EventArgs e)
        {
            MHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtPURCHASE_DEPT_TKEY_EditValueChanging(object sender, ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                MHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtPURCHASE_DEPT_TKEY_Popup(object sender, EventArgs e)
        {
            MHelper.SetGridLookUpEditMoreColumnFilter(sender);

        }

        private void txtSUPPLIER_TKEY_EditValueChanging(object sender, ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                MHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtSUPPLIER_TKEY_Popup(object sender, EventArgs e)
        {
            MHelper.SetGridLookUpEditMoreColumnFilter(sender);

        }

        private void ReGridLookUpEdit_EditValueChanging(object sender, ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                MHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));

        }

        private void ReGridLookUpEdit_Popup(object sender, EventArgs e)
        {
            MHelper.SetGridLookUpEditMoreColumnFilter(sender);

        }

        #endregion  
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFunction();
        }


    }
}
