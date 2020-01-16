/*
 库房 库区 库位 在GridView中的多级联动问题
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

namespace ASJMM
{
    /// <summary>
    /// 物料管理模块 - 盘库管理
    /// </summary>
    public partial class UcInventory : BaseUserControl
    {

        //实例化帮助类
        ASJMM_Inventory MHelper = new ASJMM_Inventory();
        Result rs = new Result();

        private DataSet ds;//参数
        int errorReason = 777;
        bool IsError = true;

        //声明实体
        private MMSMM_INVENTORY inventory;//盘库主表
        private MMSMM_INVENTORY_D inventory_d;//盘库明细

        private string InventoryTKEY;//主表Tkey变量

        List<object> lstentity = new List<object>();
        

        /// <summary>
        /// 控件加载
        /// </summary>
        public UcInventory()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_purchase"></param>
        public UcInventory(MMSMM_INVENTORY _inventory) : this()
        {
            inventory = _inventory;
            MHelper.BindCustomDrawRowIndicator(GrvInventory);//GridView新增序号栏位 自增长 宽度自适应
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcInventory_Load(object sender, EventArgs e)
        {
            DataBinding(inventory);
        }

        #region 数据绑定 数据处理
        /// <summary>
        /// 数据绑定
        /// </summary>
        public void DataBinding(MMSMM_INVENTORY inventory)
        {
            InventoryTKEY = inventory.TKEY == null ? Guid.NewGuid().ToString() : inventory.TKEY;
            LoadData(InventoryTKEY);

        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        public void LoadData(string TKEY)
        {
            ds = MHelper.InventoryLoad(TKEY);//FrmDataLoad

            //------------------------------------------------

            txtINVENTORY_NO.EditValue = ds.Tables["MMSMM_INVENTORY"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INVENTORY"].Rows[0]["INVENTORY_NO"].ToString();//单号
            txtINVENTORY_TYPE.EditValue = ds.Tables["MMSMM_INVENTORY"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INVENTORY"].Rows[0]["INVENTORY_TYPE"].ToString();//单据类型
            txtCHECK_COLLECT_TYPE.EditValue = ds.Tables["MMSMM_INVENTORY"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INVENTORY"].Rows[0]["CHECK_COLLECT_TYPE"].ToString();//盘点采集类型
            txtSTOCK_TKEY.EditValue = ds.Tables["MMSMM_INVENTORY"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INVENTORY"].Rows[0]["STOCK_TKEY"].ToString();//库房名称
            txtOWNER_TYPE.EditValue = ds.Tables["MMSMM_INVENTORY"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INVENTORY"].Rows[0]["OWNER_TYPE"].ToString();//货主类型
            txtCOMPLETE_REAL_DATE.EditValue = ds.Tables["MMSMM_INVENTORY"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INVENTORY"].Rows[0]["COMPLETE_REAL_DATE"].ToString();//计划完工时间
            txtCOMPLETE_PLAN_DATE.EditValue = ds.Tables["MMSMM_INVENTORY"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INVENTORY"].Rows[0]["COMPLETE_PLAN_DATE"].ToString();//实际完工时间
            txtCMT.EditValue = ds.Tables["MMSMM_INVENTORY"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INVENTORY"].Rows[0]["CMT"].ToString();//备注

            BindGridViewDataSource(InventoryTKEY);//绑定GridView数据源

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
                }
                #region 控件内容赋值到Dataset
                if (ds.Tables["MMSMM_INVENTORY"].Rows.Count == 0) MHelper.InsertNewRowForDatatable(ds, "MMSMM_INVENTORY");
                ds.Tables["MMSMM_INVENTORY"].Rows[0]["TKEY"] = InventoryTKEY;
                ds.Tables["MMSMM_INVENTORY"].Rows[0]["INVENTORY_NO"] = txtINVENTORY_NO.EditValue ?? txtINVENTORY_NO.EditValue.ToString();
                ds.Tables["MMSMM_INVENTORY"].Rows[0]["INVENTORY_TYPE"] = txtINVENTORY_TYPE.EditValue ?? txtINVENTORY_TYPE.EditValue.ToString();
                ds.Tables["MMSMM_INVENTORY"].Rows[0]["CHECK_COLLECT_TYPE"] = txtCHECK_COLLECT_TYPE.EditValue ?? txtCHECK_COLLECT_TYPE.EditValue.ToString();
                ds.Tables["MMSMM_INVENTORY"].Rows[0]["STOCK_TKEY"] = txtSTOCK_TKEY.EditValue ?? txtSTOCK_TKEY.EditValue.ToString();
                ds.Tables["MMSMM_INVENTORY"].Rows[0]["OWNER_TYPE"] = txtOWNER_TYPE.EditValue ?? txtOWNER_TYPE.EditValue.ToString();
                ds.Tables["MMSMM_INVENTORY"].Rows[0]["COMPLETE_REAL_DATE"] = txtCOMPLETE_REAL_DATE.EditValue.ToString() == "" ? DBNull.Value : txtCOMPLETE_REAL_DATE.EditValue;
                ds.Tables["MMSMM_INVENTORY"].Rows[0]["COMPLETE_PLAN_DATE"] = txtCOMPLETE_PLAN_DATE.EditValue.ToString() == "" ? DBNull.Value : txtCOMPLETE_PLAN_DATE.EditValue;
                ds.Tables["MMSMM_INVENTORY"].Rows[0]["CMT"] = txtCMT.EditValue ?? txtCMT.EditValue.ToString();

                #endregion

                #region Gridview中有数据 则将GridView中的值存到Dataset中 一般都会有值 保存前Check GridView是否有数据
                DataTable dt = ((DataView)GrvInventory.DataSource).ToTable();//GridView数据源转成Datatable
                if (dt.Rows.Count > 0)
                {

                    //GridView中的数据赋值到对应的实体中
                    foreach (DataRow dr in dt.Rows)
                    {
                        //明细主表
                        inventory_d.TKEY = dr["TKEY"].ToString();
                        inventory_d.CKEY = dr["CKEY"].ToString();
                        inventory_d.MATERIAL_TKEY = dr["MATERIAL_TKEY"].ToString();
                        inventory_d.BASE_UNIT_TKEY = dr["BASE_UNIT_TKEY"].ToString();
                        inventory_d.STOCK_TKEY = dr["STOCK_TKEY"].ToString();
                        inventory_d.STOCK_AREA_KEY = dr["STOCK_AREA_KEY"].ToString();
                        inventory_d.STOCK_SITE_TKEY =dr["STOCK_SITE_TKEY"].ToString();
                        inventory_d.OWNER_TYPE = dr["OWNER_TYPE"].ToString();
                        inventory_d.MATERIAL_LOTID = dr["MATERIAL_LOTID"].ToString();
                        inventory_d.ORIGINAL_LOTID = dr["ORIGINAL_LOTID"].ToString();
                        inventory_d.STOCKLIST_QTY = decimal.Parse(dr["STOCKLIST_QTY"].ToString());
                        inventory_d.RESPONS_USERKEY = dr["RESPONS_USERKEY"].ToString();
                        inventory_d.COMPLETE_PLAN_DATE = DateTime.Parse(dr["COMPLETE_PLAN_DATE"].ToString());
                        inventory_d.COMPLETE_REAL_DATE = DateTime.Parse(dr["COMPLETE_REAL_DATE"].ToString()) ;
                        inventory_d.CMT = dr["CMT"].ToString();

                        inventory_d.CHECKLIST_D_STATUS = dr["CHECKLIST_D_STATUS"].ToString();
                        lstentity.Add(inventory_d);

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
            MHelper.BindDataSourceForGridControl(GridItem, GrvInventory, "MMSMM_INVENTORY_D", TKEY);
        }

        #region 绑定下拉框的值 
        public void BindGrdLookUpEdit()
        {
            List<GridLookUpEdit> Control = new List<GridLookUpEdit>();
            Control.Add(txtSTOCK_TKEY);//库房
            MHelper.BindGridLookUpEdit_Inventory(Control);
        }

        public void BindLookUpEdit()
        {
            MHelper.BindSysDict(txtINVENTORY_TYPE, "MMSMM_INVENTORY_INVENTORY_TYPE");
        }

        public void BindReLookUpEdit()
        {
            List<RepositoryItemLookUpEdit> Control = new List<RepositoryItemLookUpEdit>();
            Control.Add(ReLookUpEdit);//计量单位 GrvInStock
            MHelper.BindReLookUpEdit_Inventory(Control);
        }

        public void BindReGridLookUpEdit()
        {
            List<RepositoryItemGridLookUpEdit> Control = new List<RepositoryItemGridLookUpEdit>();
            Control.Add(ReGridLookUpEdit_Stock);//库房名
            Control.Add(ReGridLookUpEdit_User);//负责人
            Control.Add(ReGridLookUpEdit);//物料编码
            MHelper.BindReGridLookUpEdit_Inventory(Control);
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
            DataTable dt = ((DataView)GrvInventory.DataSource).ToTable();//GridView中的数据转成Datatable 方便检查是否为空
            //采购单主表 非空验证
            if (string.IsNullOrEmpty(txtINVENTORY_NO.EditValue.ToString())) sbErrMsg.Append("单号不能为空,\n");
            if (string.IsNullOrEmpty(txtINVENTORY_TYPE.EditValue.ToString())) sbErrMsg.Append("单据类型不能为空,\n");
            if (string.IsNullOrEmpty(txtCHECK_COLLECT_TYPE.EditValue.ToString())) sbErrMsg.Append("采集类型不能为空,\n");
            if (string.IsNullOrEmpty(txtSTOCK_TKEY.EditValue.ToString())) sbErrMsg.Append("库房名称不能为空,\n");
            if (string.IsNullOrEmpty(txtOWNER_TYPE.EditValue.ToString())) sbErrMsg.Append("货主类型不能为空,\n");
            if (string.IsNullOrEmpty(txtCOMPLETE_PLAN_DATE.EditValue?.ToString())) sbErrMsg.Append("计划完工时间不能为空,\n");
            if (string.IsNullOrEmpty(txtCOMPLETE_REAL_DATE.EditValue?.ToString())) sbErrMsg.Append("实际完工时间不能为空,\n");

            //采购单明细 GridView验证非空
            if (dt.Rows.Count == 0) sbErrMsg.Append("需新增明细资料 \n");

            return sbErrMsg.ToString();
        }
        #endregion

        #region GridView 事件  (新增行 删除行 特定单元格不能编辑 显示行号 日期校验)

        //新增行
        private void BarBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GrvInventory.AddNewRow();//在GridControl中新增一行
            this.GrvInventory.SetFocusedRowCellValue("TKEY", Guid.NewGuid().ToString());//请购单明细表主键
            this.GrvInventory.SetFocusedRowCellValue("CKEY", ds.Tables["MMSMM_INVENTORY"].Rows.Count == 0 ? InventoryTKEY : ds.Tables["MMSMM_INVENTORY"].Rows[0]["TKEY"].ToString());//主表关联主键
            this.GrvInventory.SetFocusedRowCellValue("MATERIAL_TKEY", string.Empty);//物料TKEY  明细表中
            this.GrvInventory.SetFocusedRowCellValue("MATERIAL_CODE", null);//物料编码 物料表 后续需删除列
            this.GrvInventory.SetFocusedRowCellValue("MAPID", string.Empty);//物料图号  物料表 后续需删除列
            this.GrvInventory.SetFocusedRowCellValue("MATERIAL_NAME", string.Empty);//物料名称  物料表 后续需删除列
            this.GrvInventory.SetFocusedRowCellValue("BASE_UNIT_TKEY", string.Empty);//基本计量单位名称  

            this.GrvInventory.SetFocusedRowCellValue("STOCK_TKEY", string.Empty);//库房KEY
            this.GrvInventory.SetFocusedRowCellValue("STOCK_AREA_KEY", string.Empty);//库区KEY
            this.GrvInventory.SetFocusedRowCellValue("STOCK_SITE_TKEY", string.Empty);//库位KEY
            this.GrvInventory.SetFocusedRowCellValue("OWNER_TYPE", string.Empty);//货主类型
            this.GrvInventory.SetFocusedRowCellValue("MATERIAL_LOTID", string.Empty);//物料批次号
            this.GrvInventory.SetFocusedRowCellValue("ORIGINAL_LOTID", string.Empty);//原始批次号
            this.GrvInventory.SetFocusedRowCellValue("STOCKLIST_QTY", 0);//库存数量
            this.GrvInventory.SetFocusedRowCellValue("RESPONS_USERKEY", string.Empty);//负责采集人KEY
            this.GrvInventory.SetFocusedRowCellValue("COMPLETE_PLAN_DATE", DateTime.Now);//计划完工日期
            this.GrvInventory.SetFocusedRowCellValue("COMPLETE_REAL_DATE", DateTime.Now.AddDays(1));//实际完工日期
            this.GrvInventory.SetFocusedRowCellValue("CMT", string.Empty);//备注

            this.GrvInventory.SetFocusedRowCellValue("CHECKLIST_D_STATUS", "1");//明细状态

            GrvInventory.OptionsBehavior.Editable = true;//栏位可编辑
        }

        //删除行
        private void BarBtnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GrvInventory.SelectedRowsCount == 0) { XtraMessageBox.Show("请先选中行再进行删除！", "删除提示框", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            //删除选中的行
            GrvInventory.DeleteSelectedRows();
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
            switch (GrvInventory.FocusedColumn.FieldName)
            {
                case "MAPID"://物料图号
                case "MATERIAL_NAME"://物料名称
                case "BASE_UNIT_TKEY"://基本计量单位名称
                case "CHECKLIST_D_STATUS"://明细状态
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
            BaseEdit edit = GrvInventory.ActiveEditor;
            GridLookUpEdit gridlookupedit = sender as GridLookUpEdit;
            MHelper.BindReGLE_BCMA_MATERIAL(GrvInventory, gridlookupedit, "BASE_UNIT_TKEY");
        }

        #region 多列模糊查询
        private void txtSTOCK_TKEY_EditValueChanging(object sender, ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                MHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtSTOCK_TKEY_Popup(object sender, EventArgs e)
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
