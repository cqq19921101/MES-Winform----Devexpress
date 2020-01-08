/*遗留问题
 最后的数据处理 返回实体的问题
 */
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
using DevExpress.XtraGrid.Views.Grid;
using System.Reflection;
using DevExpress.Data.Filtering;

namespace ASJMM
{
    /// <summary>
    /// 物料管理模块 - 采购订单管理单据转换
    /// </summary>
    public partial class UcPurchaseMAP : BaseUserControl
    {

        //实例化帮助类
        MMSMMHelper MHelper = new MMSMMHelper();
        Result rs = new Result();

        private DataSet ds;//Dataset变量
        int errorReason = 777;
        bool IsError = true;

        //采购单主表主键
        private MMSMM_PURCHASE purchase;

        private string PurChaseTKEY;//主表Tkey变量



        /// <summary>
        /// 控件加载
        /// </summary>
        public UcPurchaseMAP()
        {
            InitializeComponent();

        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_purchase"></param>
        public UcPurchaseMAP(MMSMM_PURCHASE _purchase) : this()
        {
            purchase = _purchase;
            MHelper.BindCustomDrawRowIndicator(GrvPurDetail);//GridView新增序号栏位 自增长 宽度自适应
            //SetGridLookUpEditMoreColumnFilter();
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcPurchaseMAP_Load(object sender, EventArgs e)
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
            #region 绑定Gridview中的下拉框的值

            BindGridLookUpEdit();//绑定采购部门 采购职员 供应商 下拉框的值
            BindLookUpEdit();//类型
            BindReLookUpEdit();//计量单位
            BindReGridLookUpEdit();//物料编码
            #endregion

        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        public void LoadData(string TKEY)
        {
            List<string> strsql = new List<string>();
            List<string> TableNames = new List<string>();
            string SqlMaster = $@" SELECT * FROM MMSMM_PURCHASE WHERE FLAG = 1  AND TKEY = '{TKEY}'";
            strsql.Add(SqlMaster);//主档
            TableNames.Add("MMSMM_PURCHASE");
            ds = OracleHelper.Get_DataSet(strsql, TableNames);

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
            //非空校验
            //string ErrMsgText = string.Empty;
            //ErrMsgText = JudgeEmpty();
            //if (ErrMsgText.Length > 0)
            //{
            //    XtraMessageBox.Show(ErrMsgText, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
            //    return;
            //}

            #region 控件内容赋值到Dataset
            if (purchase.TKEY == null)
            {
                ds.Tables["MMSMM_PURCHASE"].NewRow();
                ds.Tables["MMSMM_PURCHASE"].Rows.Add();
            }

            ds.Tables["MMSMM_PURCHASE"].Rows[0]["TKEY"] = PurChaseTKEY;
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

            DataTable dtD = ((DataView)GrvPurDetail.DataSource).ToTable();//明细
            DataTable dtRULE = MHelper.QueryToDatatable("MMSMM_PURCHASE_D_RULE");//执行策略
            DataTable dtMAP = MHelper.QueryToDatatable("MMSMM_PURCHASE_MAP");//转换映射

            if (dtD.Rows.Count > 0 && dtD != null)
            {
                for (int i = 0; i < dtD.Rows.Count; i++)
                {
                    DataRow drD = dtD.Rows[i];
                    MHelper.InsertNewRowForDatatable(dtRULE);//插入行
                    MHelper.InsertNewRowForDatatable(dtMAP);//插入行
                    //执行策略表
                    dtRULE.Rows[i]["TKEY"] = drD["TKEY"].ToString(); //采购订单明细执行策略主键
                    dtRULE.Rows[i]["PURCHASE_D"] = drD["TKEY"].ToString(); //采购订单明细KEY
                    dtRULE.Rows[i]["CONCESSION_RECEIVE_FLAG"] = drD["CONCESSION_RECEIVE_FLAG"].ToString(); //允许让步接收标识
                    dtRULE.Rows[i]["PURCHASE_RETURN_FLAG"] = drD["PURCHASE_RETURN_FLAG"].ToString(); //允许退料标识
                    dtRULE.Rows[i]["DELIVERY_ACTIVE_FLAG"] = drD["DELIVERY_ACTIVE_FLAG"].ToString(); //到货接收启用标识
                    dtRULE.Rows[i]["IQC_FLAG"] = drD["IQC_FLAG"].ToString(); //启用来料检验标识
                    dtRULE.Rows[i]["SUPPLIER_LOT_FLAG"] = drD["SUPPLIER_LOT_FLAG"].ToString(); //启用供应商批次标识
                    //转换映射表
                    dtMAP.Rows[i]["TKEY"] = Guid.NewGuid().ToString(); //主键
                    dtMAP.Rows[i]["SOURCE_SYSTEM_TYPE"] = string.Empty; //转换来源系统类型
                    dtMAP.Rows[i]["SOURCE_TRANS_TYPE"] = string.Empty; //转换方式类型
                    dtMAP.Rows[i]["SOURCE_ORDER_TYPE"] = string.Empty; //转换单据类型
                    dtMAP.Rows[i]["PURCHASE_D"] = drD["TKEY_REQ"].ToString(); //转换单KEY -- 请购单主表TKEY
                    dtMAP.Rows[i]["SOURCE_ORDER_NO"] = drD["REQUEST_NO"].ToString(); //转换单号-- 请购单单号
                    dtMAP.Rows[i]["SOURCE_ORDER_D_TKEY"] = drD["TKEY_REQ_D"].ToString(); //转换单明细KEY -- 请购单
                    dtMAP.Rows[i]["PURCHASE_TKEY"] = drD["CKEY"].ToString(); //采购订单KEY
                    dtMAP.Rows[i]["PURCHASE_D_TKEY"] = drD["TKEY"].ToString(); //采购订单明细KEY
                    dtMAP.Rows[i]["TRANS_QTY"] = drD["REQUEST_QTY"].ToString(); //转换数量
                }
            }

            dtD.TableName = "MMSMM_PURCHASE_D";
            dtMAP.TableName = "MMSMM_PURCHASE_MAP";
            dtRULE.TableName = "MMSMM_PURCHASE_D_RULE";

            ds.Tables.Add(dtD);
            ds.Tables.Add(dtMAP);
            ds.Tables.Add(dtRULE);

            return ds;
        }

        /// <summary>
        /// 绑定GirdView数据源
        /// </summary>
        public void BindGridViewDataSource(string TKEY)
        {
            string strsql = $@"  SELECT T1.*,
                                T2.TKEY,T2.MATERIAL_CODE,T2.MATERIAL_NAME,T2.MAPID,T2.BASE_UNIT_TKEY FROM
                                (SELECT T1.*,
                                T2.CONCESSION_RECEIVE_FLAG,
                                T2.PURCHASE_RETURN_FLAG,
                                T2.DELIVERY_ACTIVE_FLAG,
                                T2.IQC_FLAG,
                                T2.SUPPLIER_LOT_FLAG
                                FROM MMSMM_PURCHASE_D T1
                                LEFT JOIN MMSMM_PURCHASE_D_RULE T2 ON
                                T1.TKEY = T2.PURCHASE_D AND T1.FLAG = T2.FLAG
                                WHERE T1.FLAG = 1) T1
                                LEFT JOIN BCMA_MATERIAL T2 ON T1.MATERIAL_TKEY = T2.TKEY AND T1.FLAG = T2.FLAG
                                WHERE T1.FLAG = 1 and T1.TKEY = '{TKEY}'";
            MHelper.BindDataSourceForGridControl(GridItem, GrvPurDetail, MHelper.QueryBindGridView(strsql).Ds.Tables[0]);//绑定GridControl
        }

        #region 绑定下拉框的值
        /// <summary>
        /// 绑定请购部门，请购职员 下拉框的值
        /// </summary>
        public void BindGridLookUpEdit()
        {
            List<string> strsql = new List<string>();
            List<GridLookUpEdit> Control = new List<GridLookUpEdit>();
            strsql.Add("SELECT TKEY,DEPT_NAME,DEPT_CODE from bcor_dept WHERE FLAG = 1");//采购部门
            strsql.Add("SELECT TKEY,EMPLOYEE_NAME,EMPLOYEE_CODE FROM bcor_employee where FLAG = 1 ");//采购职员
            strsql.Add("SELECT TKEY,SUPPLIER_NAME,SUPPLIER_CODE from BCOR_SUPPLIER where  FLAG = 1");//供应商

            Control.Add(txtPURCHASE_DEPT_TKEY);//采购部门
            Control.Add(txtPURCHASE_EMPLOYEE_TKEY);//采购职员
            Control.Add(txtSUPPLIER_TKEY);//供应商
            MHelper.BindGridLookUpEdit(strsql, Control);
        }

        public void BindReGridLookUpEdit()
        {
            List<string> strsql = new List<string>();
            List<RepositoryItemGridLookUpEdit> Control = new List<RepositoryItemGridLookUpEdit>();
            strsql.Add("SELECT TKEY , MATERIAL_CODE,MATERIAL_NAME FROM BCMA_MATERIAL WHERE FLAG = 1 ");

            Control.Add(ReGridLookUpEdit);//物料编码

            MHelper.BindReGridLookUpEdit(strsql, Control);

        }

        public void BindReLookUpEdit()
        {
            List<string> strsql = new List<string>();
            List<RepositoryItemLookUpEdit> Control = new List<RepositoryItemLookUpEdit>();
            strsql.Add("Select TKEY,UNIT_NAME,UNIT_CODE from BCDF_UNIT WHERE FLAG = 1");

            Control.Add(ReLookUpEdit);//计量单位 

            MHelper.BindReLookUpEdit(strsql, Control);

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
            if (dt.Rows.Count == 0) sbErrMsg.Append("需新增明细资料 \n");

            return sbErrMsg.ToString();
        }
        #endregion

        #region GridView 事件  (单据转换 删除行 特定单元格不能编辑 显示行号)

        //单据转换
        private void BarBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmPurchaseMAP FormMAP = new FrmPurchaseMAP();
            FormMAP.ShowDialog();//弹出单据转换窗体
            GridItem.DataSource = null;//清空GridControl的数据源

            if (FormMAP.DialogResult == DialogResult.OK)
            {
                DataTable dt = FormMAP.DT.Copy();//子窗体选中的数据源 传递到 当前窗体的GridControl中
                GridItem.DataSource = ConvertToGridControl(dt);
            }
        }

        /// <summary>
        /// 子窗体Datatable转换后 绑定到GridControl
        /// </summary>
        /// <param name="dt"></param>
        public DataTable ConvertToGridControl(DataTable dt)
        {
            dt.Columns["REQUEST_QTY"].ColumnName = "REQUEST_QTY_REQ";//请购数量列重命名 两张表字段名一样 重命名区分
            dt.Columns["TKEY"].ColumnName = "TKEY_REQ_D";//请购单明细主键重命名
            dt.Columns["CKEY"].ColumnName = "TKEY_REQ";//请购单主表主键重命名

            dt.Columns.Add("TKEY", typeof(String));//采购单明细主键 
            dt.Columns.Add("CKEY", typeof(String));//采购单主表主键 
            dt.Columns.Add("REQUEST_QTY", typeof(int));//采购单主表主键 

            foreach (DataRow dr in dt.Rows)
            {
                dr["TKEY"] = Guid.NewGuid().ToString();//采购单明细主键
                dr["CKEY"] = ds.Tables["MMSMM_PURCHASE"].Rows.Count == 0 ? PurChaseTKEY : ds.Tables["MMSMM_PURCHASE"].Rows[0]["TKEY"].ToString();//采购单主表主键
                dr["PURCHASE_D_STATUS"] = 1;//明细状态
                dr["REQUEST_QTY"] = 0;//采购数量
                dr["CONCESSION_RECEIVE_FLAG"] = 0;//允许让步接收标识
                dr["PURCHASE_RETURN_FLAG"] = 0;//允许退料标识
                dr["DELIVERY_ACTIVE_FLAG"] = 0;//到货接收启用标识
                dr["IQC_FLAG"] = 0;//启用来料检验标识
                dr["SUPPLIER_LOT_FLAG"] = 0;//启用供应商批次标识

            }

            return dt;
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
                case "MATERIAL_CODE"://物料编码
                case "REQUEST_QTY_REQ"://请购数量
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
            BaseEdit dateedit = sender as BaseEdit;
            if (dateedit.Text.Length == 0)
            {
                e.Cancel = true;
                errorReason = 0;
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

            ////DataRow FieldName = GrvPurDetail.GetFocusedDataRow();
            ColumnView view = sender as ColumnView;
            GridColumn StarDate = view.Columns["REQUEST_DATE"];//需求日期
            GridColumn EndDate = view.Columns["PLAN_DELIVERY_DATE"];//计划交期
            DateTime StartTime = (DateTime)view.GetRowCellValue(e.RowHandle, StarDate);
            DateTime EndTime = (DateTime)view.GetRowCellValue(e.RowHandle, EndDate);

            GridColumn REQQTY = view.Columns["REQUEST_QTY_REQ"];//请购数量
            GridColumn QTY = view.Columns["REQUEST_QTY"];//采购数量
            int REQUEST_QTY_REQ = int.Parse(view.GetRowCellValue(e.RowHandle, REQQTY).ToString() == string.Empty ? "0" : view.GetRowCellValue(e.RowHandle, REQQTY).ToString());
            int REQUEST_QTY = int.Parse(view.GetRowCellValue(e.RowHandle, QTY).ToString() == string.Empty ? "0" : view.GetRowCellValue(e.RowHandle, QTY).ToString());

            //验证
            if (StartTime >= EndTime)
            {
                e.Valid = false;
                view.SetColumnError(StarDate, "需求日期必须小于计划交期");
                view.SetColumnError(EndDate, "计划交期必须大于需求日期");
            }

            if (REQUEST_QTY_REQ < REQUEST_QTY)
            {
                e.Valid = false;
                view.SetColumnError(REQQTY, "采购数量不能超过请购数量");
            }
        }

        #endregion

        #region GridLookUpEdit ReGridLookUpEdit 多列模糊查询
        private void txtPURCHASE_EMPLOYEE_TKEY_Popup(object sender, EventArgs e)
        {
            MHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }
        private void txtPURCHASE_DEPT_TKEY_Popup(object sender, EventArgs e)
        {
            MHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }
        private void txtSUPPLIER_TKEY_Popup(object sender, EventArgs e)
        {
            MHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }
        private void txtPURCHASE_EMPLOYEE_TKEY_EditValueChanging(object sender, ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                MHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }
        private void txtSUPPLIER_TKEY_EditValueChanging(object sender, ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                MHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }
        private void txtPURCHASE_DEPT_TKEY_EditValueChanging(object sender, ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                MHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFunction();
        }


    }
}
