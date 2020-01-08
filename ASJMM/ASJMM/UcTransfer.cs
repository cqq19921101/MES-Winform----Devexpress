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
    /// 物料管理模块 - 调拨单管理
    /// </summary>
    public partial class UcTransfer : BaseUserControl
    {

        //实例化帮助类
        MMSMMHelper MHelper = new MMSMMHelper();
        Result rs = new Result();

        private DataSet ds;//参数
        int errorReason = 777;
        bool IsError = true;

        //声明实体
        private MMSMM_TRANSFER transfer;//调拨主表
        private MMSMM_TRANSFER_D transfer_d;//调拨明细

        private string TransferTKEY;//主表Tkey变量

        List<object> lstentity = new List<object>();
        

        /// <summary>
        /// 控件加载
        /// </summary>
        public UcTransfer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_purchase"></param>
        public UcTransfer(MMSMM_TRANSFER _transfer) : this()
        {
            transfer = _transfer;
            MHelper.BindCustomDrawRowIndicator(GrvTransfer);//GridView新增序号栏位 自增长 宽度自适应
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcTransfer_Load(object sender, EventArgs e)
        {
            DataBinding(transfer);
        }

        #region 数据绑定 数据处理
        /// <summary>
        /// 数据绑定
        /// </summary>
        public void DataBinding(MMSMM_TRANSFER TransferTKEY)
        {
            string Tkey = TransferTKEY.TKEY == string.Empty ? Guid.NewGuid().ToString() : TransferTKEY.TKEY;
            LoadData(Tkey);

        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        public void LoadData(string TKEY)
        {
            List<string> strsql = new List<string>();
            List<string> TableNames = new List<string>();
            string SqlMaster = @" SELECT * FROM MMSMM_TRANSFER WHERE FLAG = 1  AND TKEY = " + "'" + TKEY + "'";
            strsql.Add(SqlMaster);//主档
            TableNames.Add("MMSMM_TRANSFER");
            ds = OracleHelper.Get_DataSet(strsql, TableNames);

            //------------------------------------------------

            TransferTKEY = ds.Tables["MMSMM_TRANSFER"].Rows.Count == 0 ? Guid.NewGuid().ToString() : ds.Tables["MMSMM_TRANSFER"].Rows[0]["TKEY"].ToString();//主键

            txtTRANSFER_NO.EditValue = ds.Tables["MMSMM_TRANSFER"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_TRANSFER"].Rows[0]["TRANSFER_NO"].ToString();//单号
            txtTRANSFER_TYPE.EditValue = ds.Tables["MMSMM_TRANSFER"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_TRANSFER"].Rows[0]["TRANSFER_TYPE"].ToString();//单据类型
            txtTRANS_BUSINESS_TYPE.EditValue = ds.Tables["MMSMM_TRANSFER"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_TRANSFER"].Rows[0]["TRANS_BUSINESS_TYPE"].ToString();//业务类型

            txtFROM_STOCK_TKEY.EditValue = ds.Tables["MMSMM_TRANSFER"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_TRANSFER"].Rows[0]["FROM_STOCK_TKEY"].ToString();//转出库房
            txtAPPLY_USERKEY.EditValue = ds.Tables["MMSMM_TRANSFER"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_TRANSFER"].Rows[0]["APPLY_USERKEY"].ToString();//申请人
            txtAPPLY_TIME.EditValue = ds.Tables["MMSMM_TRANSFER"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_TRANSFER"].Rows[0]["APPLY_TIME"].ToString();//申请时间
            txtAPPLY_DESC.EditValue = ds.Tables["MMSMM_TRANSFER"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_TRANSFER"].Rows[0]["APPLY_DESC"].ToString();//申请说明

            txtTO_STOCK_TKEY.EditValue = ds.Tables["MMSMM_TRANSFER"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_TRANSFER"].Rows[0]["TO_STOCK_TKEY"].ToString();//接收库房
            txtRECEIVE_USERKEY.EditValue = ds.Tables["MMSMM_TRANSFER"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_TRANSFER"].Rows[0]["RECEIVE_USERKEY"].ToString();//接收人
            txtRECEIVE_TIME.EditValue = ds.Tables["MMSMM_TRANSFER"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_TRANSFER"].Rows[0]["RECEIVE_TIME"].ToString();//接收时间
            txtRECEIVE_DESC.EditValue = ds.Tables["MMSMM_TRANSFER"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_TRANSFER"].Rows[0]["RECEIVE_DESC"].ToString();//接收说明


            txtAGENT_USER.EditValue = ds.Tables["MMSMM_TRANSFER"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_TRANSFER"].Rows[0]["AGENT_USER"].ToString();//经办人
            txtCMT.EditValue = ds.Tables["MMSMM_TRANSFER"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_TRANSFER"].Rows[0]["CMT"].ToString();//备注

            BindGridViewDataSource(TransferTKEY);//绑定GridView数据源

            #region 绑定各个下拉框的值
            BindGrdLookUpEdit();
            BindLookUpEdit();
            BindReLookUpEdit();
            BindReGridLookUpEdit();
            #endregion

        }

        /// <summary>
        /// 保存方法
        /// </summary>
        public List<object> SaveFunction() 
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
                if (transfer.TKEY == null)
                {
                    ds.Tables["MMSMM_TRANSFER"].NewRow();
                    ds.Tables["MMSMM_TRANSFER"].Rows.Add();
                }

                ds.Tables["MMSMM_TRANSFER"].Rows[0]["TRANSFER_NO"] = txtTRANSFER_NO.EditValue ?? txtTRANSFER_NO.EditValue.ToString();
                ds.Tables["MMSMM_TRANSFER"].Rows[0]["TRANSFER_TYPE"] = txtTRANSFER_TYPE.EditValue ?? txtTRANSFER_TYPE.EditValue.ToString();
                ds.Tables["MMSMM_TRANSFER"].Rows[0]["TRANS_BUSINESS_TYPE"] = txtTRANS_BUSINESS_TYPE.EditValue ?? txtTRANS_BUSINESS_TYPE.EditValue.ToString();
                ds.Tables["MMSMM_TRANSFER"].Rows[0]["FROM_STOCK_TKEY"] = txtFROM_STOCK_TKEY.EditValue ?? txtFROM_STOCK_TKEY.EditValue.ToString();
                ds.Tables["MMSMM_TRANSFER"].Rows[0]["APPLY_USERKEY"] = txtAPPLY_USERKEY.EditValue ?? txtAPPLY_USERKEY.EditValue.ToString();
                ds.Tables["MMSMM_TRANSFER"].Rows[0]["APPLY_TIME"] = txtAPPLY_TIME.EditValue.ToString() == "" ? DBNull.Value : txtAPPLY_TIME.EditValue;
                ds.Tables["MMSMM_TRANSFER"].Rows[0]["APPLY_DESC"] = txtAPPLY_DESC.EditValue ?? txtAPPLY_DESC.EditValue.ToString();

                ds.Tables["MMSMM_TRANSFER"].Rows[0]["TO_STOCK_TKEY"] = txtTO_STOCK_TKEY.EditValue ?? txtTO_STOCK_TKEY.EditValue.ToString();
                ds.Tables["MMSMM_TRANSFER"].Rows[0]["RECEIVE_USERKEY"] = txtRECEIVE_USERKEY.EditValue ?? txtRECEIVE_USERKEY.EditValue.ToString();
                ds.Tables["MMSMM_TRANSFER"].Rows[0]["RECEIVE_TIME"] = txtRECEIVE_TIME.EditValue.ToString() == "" ? DBNull.Value : txtRECEIVE_TIME.EditValue;
                ds.Tables["MMSMM_TRANSFER"].Rows[0]["RECEIVE_DESC"] = txtRECEIVE_DESC.EditValue ?? txtRECEIVE_DESC.EditValue.ToString();

                ds.Tables["MMSMM_TRANSFER"].Rows[0]["AGENT_USER"] = txtAGENT_USER.EditValue ?? txtAGENT_USER.EditValue.ToString();
                ds.Tables["MMSMM_TRANSFER"].Rows[0]["CMT"] = txtCMT.EditValue ?? txtCMT.EditValue.ToString();

                #endregion

                #region Gridview中有数据 则将GridView中的值存到Dataset中 一般都会有值 保存前Check GridView是否有数据
                DataTable dt = ((DataView)GrvTransfer.DataSource).ToTable();//GridView数据源转成Datatable
                if (dt.Rows.Count > 0)
                {

                    //GridView中的数据赋值到对应的实体中
                    foreach (DataRow dr in dt.Rows)
                    {
                        //明细主表
                        transfer_d.TKEY = dr["TKEY"].ToString();
                        transfer_d.CKEY = dr["CKEY"].ToString();
                        transfer_d.TRANS_BUSINESS_TYPE = dr["CKEY"].ToString();
                        transfer_d.MATERIAL_TKEY = dr["MATERIAL_TKEY"].ToString();
                        transfer_d.BASE_UNIT_TKEY = dr["BASE_UNIT_TKEY"].ToString();
                        transfer_d.FROM_STOCK_TKEY = dr["FROM_STOCK_TKEY"].ToString();
                        transfer_d.FROM_STOCK_AREA_TKEY = dr["FROM_STOCK_AREA_TKEY"].ToString();
                        transfer_d.FROM_STOCK_SITE_TKEY = dr["FROM_STOCK_SITE_TKEY"].ToString();

                        transfer_d.TO_STOCK_TKEY = dr["TO_STOCK_TKEY"].ToString();
                        transfer_d.TO_STOCK_AREA_TKEY = dr["TO_STOCK_AREA_TKEY"].ToString();
                        transfer_d.TO_STOCK_SITE_TKEY = dr["TO_STOCK_SITE_TKEY"].ToString();

                        transfer_d.MATERIAL_LOTID = dr["MATERIAL_LOTID"].ToString();
                        transfer_d.ORIGINAL_LOTID = dr["ORIGINAL_LOTID"].ToString();
                        transfer_d.TASKQTY = decimal.Parse(dr["TASKQTY"].ToString());
                        transfer_d.COMPLETEQTY = decimal.Parse(dr["COMPLETEQTY"].ToString());
                        transfer_d.COMPLETE_PLAN_DATE = DateTime.Parse(dr["COMPLETE_PLAN_DATE"].ToString());
                        transfer_d.COMPLETE_REAL_DATE = DateTime.Parse(dr["COMPLETE_REAL_DATE"].ToString()) ;
                        transfer_d.CMT = dr["CMT"].ToString();

                        transfer_d.TRANSFER_D_STATUS = dr["TRANSFER_D_STATUS"].ToString();
                        lstentity.Add(transfer_d);

                    }
                }
                #endregion

                return lstentity;


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return  null;
                return null ;
            }


        }

        /// <summary>
        /// 绑定GirdView数据源
        /// </summary>
        public void BindGridViewDataSource(string TKEY)
        {
            string SqlGridView = string.Format(@"select T1.*,T2.TKEY,T2.MATERIAL_CODE,T2.MATERIAL_NAME,T2.MAPID,T2.BASE_UNIT_TKEY from MMSMM_TRANSFER_D T1 
                                   left join BCMA_MATERIAL T2 ON T1.MATERIAL_TKEY = T2.TKEY AND T1.FLAG = T2.FLAG
                                   WHERE T1.FLAG = 1 and T1.TKEY = '{0}' ", TKEY);
            MHelper.BindDataSourceForGridControl(GridItem, GrvTransfer, MHelper.QueryBindGridView(SqlGridView).Ds.Tables[0]);//绑定GridControl
        }

        #region 绑定下拉框的值 
        public void BindGrdLookUpEdit()
        {
            List<string> strsql = new List<string>();
            List<GridLookUpEdit> Control = new List<GridLookUpEdit>();
            strsql.Add("SELECT TKEY,STOCK_NAME,STOCK_CODE from BCOR_STOCK where  FLAG = 1");//转出库房
            strsql.Add("SELECT TKEY,STOCK_NAME,STOCK_CODE from BCOR_STOCK where  FLAG = 1");//接收库房
            strsql.Add("SELECT TKEY,EMPLOYEE_NAME,EMPLOYEE_CODE from BCOR_EMPLOYEE where  FLAG = 1");//申请人
            strsql.Add("SELECT TKEY,EMPLOYEE_NAME,EMPLOYEE_CODE from BCOR_EMPLOYEE where  FLAG = 1");//接收人

            Control.Add(txtFROM_STOCK_TKEY);//转出库房
            Control.Add(txtTO_STOCK_TKEY);//接收库房
            Control.Add(txtAPPLY_USERKEY);//申请人
            Control.Add(txtRECEIVE_USERKEY);//接收人
            MHelper.BindGridLookUpEdit(strsql, Control);
        }

        public void BindLookUpEdit()
        {
            List<string> Para = new List<string>();
            List<LookUpEdit> Control = new List<LookUpEdit>();
            Control.Add(txtTRANSFER_TYPE);
            Control.Add(txtTRANS_BUSINESS_TYPE);
            Para.Add("MMSMM_TRANSFER_TRANSFER_TYPE");
            Para.Add("MMSMM_TRANSFER_TRANS_BUSINESS_TYPE");

            MHelper.BindLookUpEdit(Control, Para);
        }

        public void BindReLookUpEdit()
        {
            List<string> strsql = new List<string>();
            List<RepositoryItemLookUpEdit> Control = new List<RepositoryItemLookUpEdit>();
            strsql.Add("Select TKEY,UNIT_NAME,UNIT_CODE from BCDF_UNIT WHERE FLAG = 1");

            Control.Add(ReLookUpEdit);//计量单位 

            MHelper.BindReLookUpEdit(strsql, Control);

        }

        public void BindReGridLookUpEdit()
        {
            List<string> strsql = new List<string>();
            List<RepositoryItemGridLookUpEdit> Control = new List<RepositoryItemGridLookUpEdit>();
            strsql.Add("Select TKEY,STOCK_NAME,STOCK_CODE FROM BCOR_STOCK WHERE FLAG = 1");
            strsql.Add("SELECT TKEY , MATERIAL_CODE,MATERIAL_NAME FROM BCMA_MATERIAL WHERE FLAG = 1 ");

            Control.Add(ReGridLookUpEdit_Stock);//转出库房 & 接收库房
            Control.Add(ReGridLookUpEdit);//物料编码

            MHelper.BindReGridLookUpEdit(strsql, Control);
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
            DataTable dt = ((DataView)GrvTransfer.DataSource).ToTable();//GridView中的数据转成Datatable 方便检查是否为空
            //采购单主表 非空验证
            if (string.IsNullOrEmpty(txtTRANSFER_NO.EditValue?.ToString())) sbErrMsg.Append("单号不能为空,\n");
            if (string.IsNullOrEmpty(txtTRANSFER_TYPE.EditValue?.ToString())) sbErrMsg.Append("单据类型不能为空,\n");
            if (string.IsNullOrEmpty(txtTRANS_BUSINESS_TYPE.EditValue?.ToString())) sbErrMsg.Append("业务类型不能为空,\n");

            if (string.IsNullOrEmpty(txtFROM_STOCK_TKEY.EditValue?.ToString())) sbErrMsg.Append("转出库房不能为空,\n");
            if (string.IsNullOrEmpty(txtAPPLY_USERKEY.EditValue.ToString())) sbErrMsg.Append("申请人不能为空,\n");
            if (string.IsNullOrEmpty(txtAPPLY_TIME.EditValue?.ToString())) sbErrMsg.Append("申请时间不能为空,\n");
            if (string.IsNullOrEmpty(txtAPPLY_DESC.EditValue?.ToString())) sbErrMsg.Append("申请说明不能为空,\n");

            if (string.IsNullOrEmpty(txtTO_STOCK_TKEY.EditValue?.ToString())) sbErrMsg.Append("接收库房不能为空,\n");
            if (string.IsNullOrEmpty(txtRECEIVE_USERKEY.EditValue?.ToString())) sbErrMsg.Append("接收人不能为空,\n");
            if (string.IsNullOrEmpty(txtRECEIVE_TIME.EditValue?.ToString())) sbErrMsg.Append("接收时间不能为空,\n");
            if (string.IsNullOrEmpty(txtRECEIVE_DESC.EditValue?.ToString())) sbErrMsg.Append("接收说明不能为空,\n");

            if (string.IsNullOrEmpty(txtAGENT_USER.EditValue?.ToString())) sbErrMsg.Append("经办人不能为空,\n");

            //明细 GridView验证非空
            if (dt.Rows.Count == 0) sbErrMsg.Append("需新增明细资料 \n");

            return sbErrMsg.ToString();
        }
        #endregion

        #region GridView 事件  (新增行 删除行 特定单元格不能编辑 显示行号 日期校验)

        //新增行
        private void BarBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GrvTransfer.AddNewRow();//在GridControl中新增一行
            this.GrvTransfer.SetFocusedRowCellValue("TKEY", Guid.NewGuid().ToString());//明细表主键
            this.GrvTransfer.SetFocusedRowCellValue("CKEY", ds.Tables["MMSMM_TRANSFER"].Rows.Count == 0 ? TransferTKEY : ds.Tables["MMSMM_TRANSFER"].Rows[0]["TKEY"].ToString());//主表关联主键
            this.GrvTransfer.SetFocusedRowCellValue("MATERIAL_TKEY", string.Empty);//物料TKEY  明细表中
            this.GrvTransfer.SetFocusedRowCellValue("MATERIAL_CODE", null);//物料编码 物料表 后续需删除列
            this.GrvTransfer.SetFocusedRowCellValue("MAPID", string.Empty);//物料图号  物料表 后续需删除列
            this.GrvTransfer.SetFocusedRowCellValue("MATERIAL_NAME", string.Empty);//物料名称  物料表 后续需删除列
            this.GrvTransfer.SetFocusedRowCellValue("BASE_UNIT_TKEY", string.Empty);//基本计量单位名称  

            this.GrvTransfer.SetFocusedRowCellValue("FROM_STOCK_TKEY", string.Empty);//转出库房KEY
            this.GrvTransfer.SetFocusedRowCellValue("FROM_STOCK_AREA_TKEY", string.Empty);//转出库区KEY
            this.GrvTransfer.SetFocusedRowCellValue("FROM_STOCK_SITE_TKEY", string.Empty);//转出库位KEY
            this.GrvTransfer.SetFocusedRowCellValue("TO_STOCK_TKEY", string.Empty);//接收库房KEY
            this.GrvTransfer.SetFocusedRowCellValue("TO_STOCK_AREA_TKEY", string.Empty);//接收库区KEY
            this.GrvTransfer.SetFocusedRowCellValue("TO_STOCK_SITE_TKEY", string.Empty);//接收库位

            this.GrvTransfer.SetFocusedRowCellValue("MATERIAL_LOTID", string.Empty);//物料批次号
            this.GrvTransfer.SetFocusedRowCellValue("ORIGINAL_LOTID", string.Empty);//原始批次号
            this.GrvTransfer.SetFocusedRowCellValue("TASKQTY", 0);//任务数量
            this.GrvTransfer.SetFocusedRowCellValue("COMPLETEQTY", 0);//完成数量


            this.GrvTransfer.SetFocusedRowCellValue("COMPLETE_PLAN_DATE", DateTime.Now);//计划完工日期
            this.GrvTransfer.SetFocusedRowCellValue("COMPLETE_REAL_DATE", DateTime.Now.AddDays(1));//实际完工日期
            this.GrvTransfer.SetFocusedRowCellValue("CMT", string.Empty);//备注

            this.GrvTransfer.SetFocusedRowCellValue("TRANSFER_D_STATUS", "1");//明细状态

            GrvTransfer.OptionsBehavior.Editable = true;//栏位可编辑
        }

        //删除行
        private void BarBtnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GrvTransfer.SelectedRowsCount == 0) { XtraMessageBox.Show("请先选中行再进行删除！", "删除提示框", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            //删除选中的行
            GrvTransfer.DeleteSelectedRows();
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
            switch (GrvTransfer.FocusedColumn.FieldName)
            {
                case "MAPID"://物料图号
                case "MATERIAL_NAME"://物料名称
                case "BASE_UNIT_TKEY"://基本计量单位名称
                case "TRANSFER_D_STATUS"://明细状态
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
            ColumnView view = sender as ColumnView;
            GridColumn StarDate = view.Columns["COMPLETE_PLAN_DATE"];
            GridColumn EndDate = view.Columns["COMPLETE_REAL_DATE"];
            DateTime StartTime = (DateTime)view.GetRowCellValue(e.RowHandle, StarDate);
            DateTime EndTime = (DateTime)view.GetRowCellValue(e.RowHandle, EndDate);
            //Validity criterion
            if (StartTime >= EndTime)
            {
                e.Valid = false;
                //Set errors with specific descriptions for the columns
                view.SetColumnError(StarDate, "计划日期必须小于实际交期");
                view.SetColumnError(EndDate, "实际交期必须大于计划日期");
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
            BaseEdit edit = GrvTransfer.ActiveEditor;
            GridLookUpEdit gridlookupedit = sender as GridLookUpEdit;
            MHelper.BindReGLE_BCMA_MATERIAL(GrvTransfer, gridlookupedit, "BASE_UNIT_TKEY");
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFunction();
        }

        /// <summary>
        /// 编辑列触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrvInventory_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {

        }

    }
}
