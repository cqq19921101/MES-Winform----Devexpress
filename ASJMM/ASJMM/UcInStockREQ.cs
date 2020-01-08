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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;

namespace ASJMM
{
    /// <summary>
    /// 物料管理模块 - 入库单申请管理
    /// </summary>
    public partial class UcInStockREQ : BaseUserControl
    {
        //实例化帮助类
        //MMSMMHelper Helper = new MMSMMHelper();
        MMSMMHelper MHelper = new MMSMMHelper();
        Result rs = new Result();


        private DataSet ds;//声明一个Dataset变量
        private DataTable Tempdt;//声明Datatable临时表 GrvInStock
        private DataTable dtDetail;//声明Datatable临时表 GrvInStockREQDLOT

        int errorReason = 777;
        bool IsError = true;

        
        private MMSMM_INSTOCK_REQ instockreq;//入库申请单实体
        private string InStockREQTkey;//主表Tkey变量


        /// <summary>
        /// 控件加载
        /// </summary>
        public UcInStockREQ()
        {
            InitializeComponent();
            MHelper.BindCustomDrawRowIndicator(GrvInStockREQ);//GridView显示行号 宽度自适应
            MHelper.BindCustomDrawRowIndicator(GrvInStockREQDLOT);//GridView显示行号 宽度自适应
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        public UcInStockREQ(MMSMM_INSTOCK_REQ _instockreq) : this()
        {
            instockreq = _instockreq;
        }
        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcInStockREQ_Load(object sender, EventArgs e)
        {
            DataBinding(instockreq);//DataBind
        }

        #region  数据绑定  数据处理
        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="PurchaseREQ"></param>
        private void DataBinding(MMSMM_INSTOCK_REQ instockreq)
        {
            InStockREQTkey = instockreq.TKEY == null ? Guid.NewGuid().ToString() : instockreq.TKEY;
            LoadData(InStockREQTkey);//初始化数据

            #region 绑定下拉框的值
            BindGrdLookUpEdit();
            BindLookUpEdit();
            BindReGridLookUpEdit();//GridView中 ReGridLookUpEdit下拉框的值
            BindReLookUpEdit() ;//绑定GridView中LookUpEdit的下拉框的值
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
            string SqlMaster = $@" SELECT * FROM MMSMM_INSTOCK_REQ WHERE FLAG = 1  AND TKEY = '{TKEY}'";
            strsql.Add(SqlMaster);//主档
            TableNames.Add("MMSMM_INSTOCK_REQ");
            ds = OracleHelper.Get_DataSet(strsql, TableNames);

            //-----------------------------------------------------

            txtINSTOCK_REQ_NO.EditValue = ds.Tables["MMSMM_INSTOCK_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["INSTOCK_REQ_NO"].ToString();//入库申请单号
            txtINSTOCK_REQ_TYPE.EditValue = ds.Tables["MMSMM_INSTOCK_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["INSTOCK_REQ_TYPE"].ToString();//入库申请单类型
            txtAGENT_USER.EditValue = ds.Tables["MMSMM_INSTOCK_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["AGENT_USER"].ToString();//经办人
            txtSOURCE_SUPPLIER_TKEY.EditValue = ds.Tables["MMSMM_INSTOCK_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["SOURCE_SUPPLIER_TKEY"].ToString();//供应商
            txtSOURCE_CUSTOMER_TKEY.EditValue = ds.Tables["MMSMM_INSTOCK_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["SOURCE_CUSTOMER_TKEY"].ToString();//客户
            txtSOURCE_WKOG_TKEY.EditValue = ds.Tables["MMSMM_INSTOCK_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["SOURCE_WKOG_TKEY"].ToString();//生产组织
            txtSOURCE_DEPT_TKEY.EditValue = ds.Tables["MMSMM_INSTOCK_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["SOURCE_DEPT_TKEY"].ToString();//部门
            txtSOURCE_PERSON.EditValue = ds.Tables["MMSMM_INSTOCK_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["SOURCE_PERSON"].ToString();//职员
            txtSOURCE_STOCK_TKEY.EditValue = ds.Tables["MMSMM_INSTOCK_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["SOURCE_STOCK_TKEY"].ToString();//来源库房
            txtTO_STOCK_TKEY.EditValue = ds.Tables["MMSMM_INSTOCK_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["TO_STOCK_TKEY"].ToString();//目标库房
            txtINSTOCK_PLAN_TIME.EditValue = ds.Tables["MMSMM_INSTOCK_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["INSTOCK_PLAN_TIME"].ToString();//计划入库时间
            txtCMT.EditValue = ds.Tables["MMSMM_INSTOCK_REQ"].Rows.Count == 0 ? string.Empty : ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["CMT"].ToString();//备注

            BindGridViewDataSource(InStockREQTkey);//绑定GridView数据源 GrvInStock
        }

        /// <summary>
        /// 保存方法
        /// </summary>
        public DataSet SaveFunction()
        {
            //非空校验
            string ErrMsgText = string.Empty;
            ErrMsgText = JudgeEmpty();
            if (ErrMsgText.Length > 0)
            {
                XtraMessageBox.Show(ErrMsgText, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
                return null ;
            }
            #region 控件内容赋值到Dataset
            if (instockreq.TKEY == null)
            {
                ds.Tables["MMSMM_INSTOCK_REQ"].NewRow();
                ds.Tables["MMSMM_INSTOCK_REQ"].Rows.Add();
            }
            ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["TKEY"] = InStockREQTkey;
            ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["INSTOCK_REQ_NO"] = txtINSTOCK_REQ_NO.EditValue ?? txtINSTOCK_REQ_NO.EditValue.ToString();
            ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["INSTOCK_REQ_TYPE"] = txtINSTOCK_REQ_TYPE.EditValue ?? txtINSTOCK_REQ_TYPE.EditValue.ToString();
            ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["AGENT_USER"] = txtAGENT_USER.EditValue ?? txtAGENT_USER.EditValue.ToString();
            ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["SOURCE_SUPPLIER_TKEY"] = txtSOURCE_SUPPLIER_TKEY.EditValue ?? txtSOURCE_SUPPLIER_TKEY.EditValue.ToString();
            ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["SOURCE_DEPT_TKEY"] = txtSOURCE_DEPT_TKEY.EditValue ?? txtSOURCE_DEPT_TKEY.EditValue.ToString();
            ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["SOURCE_PERSON"] = txtSOURCE_PERSON.EditValue ?? txtSOURCE_PERSON.EditValue.ToString();
            ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["SOURCE_CUSTOMER_TKEY"] = txtSOURCE_CUSTOMER_TKEY.EditValue ?? txtSOURCE_CUSTOMER_TKEY.EditValue.ToString();
            ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["SOURCE_WKOG_TKEY"] = txtSOURCE_WKOG_TKEY.EditValue ?? txtSOURCE_WKOG_TKEY.EditValue.ToString();
            ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["SOURCE_STOCK_TKEY"] = txtSOURCE_STOCK_TKEY.EditValue ?? txtSOURCE_STOCK_TKEY.EditValue.ToString();
            ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["TO_STOCK_TKEY"] = txtTO_STOCK_TKEY.EditValue ?? txtTO_STOCK_TKEY.EditValue.ToString();
            ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["INSTOCK_PLAN_TIME"] = txtINSTOCK_PLAN_TIME.EditValue.ToString() == "" ? DBNull.Value : txtINSTOCK_PLAN_TIME.EditValue;
            ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["CMT"] = txtCMT.EditValue ?? txtCMT.EditValue.ToString();

            #endregion


            DataTable dtD = ((DataView)GrvInStockREQ.DataSource).ToTable();//明细
            DataTable dtDLOT = dtDetail;//明细批次
            dtD.TableName = "MMSMM_INSTOCK_REQ_D";
            dtDLOT.TableName = "MMSMM_INSTOCK_REQ_DLOT";

            ds.Tables.Add(dtD);
            ds.Tables.Add(dtDLOT);

            return ds;

        }

        #region 下拉框值绑定
        /// <summary>
        /// 绑定GridView的数据源 Master
        /// </summary>
        public void BindGridViewDataSource(string TKEY)
        {
            string SqlGridView = string.Format(@"select T1.*,T2.TKEY,T2.MATERIAL_CODE,T2.MATERIAL_NAME,T2.MAPID,T2.BASE_UNIT_TKEY from MMSMM_INSTOCK_REQ_D T1 
                                   left join BCMA_MATERIAL T2 ON T1.MATERIAL_TKEY = T2.TKEY AND T1.FLAG = T2.FLAG
                                   WHERE T1.FLAG = 1 and T1.TKEY = '{0}' ", TKEY);
            MHelper.BindDataSourceForGridControl(GridItem, GrvInStockREQ, MHelper.QueryBindGridView(SqlGridView).Ds.Tables[0]);//绑定GridControl

        }

        /// <summary>
        /// 绑定GridView的数据源 Detail
        /// </summary>
        /// <param name="dtDetail"></param>
        /// <param name="INSTOCK_D_TKEY">入库单明细TKEY</param>
        public void BindGridViewDataSource_D(DataTable dtDetail, string INSTOCK_REQ_D_TKEY)
        {
            string SqlGridView = string.Format("SELECT * FROM MMSMM_INSTOCK_REQ_DLOT WHERE FLAG = 1 and INSTOCK_REQ_D_TKEY = '{0}'", "");

            if (dtDetail != null)//批次表存在数据时 Select deDetail
            {
                DataRow[] drItem = dtDetail.Select(string.Format("INSTOCK_REQ_D_TKEY =  '{0}'", INSTOCK_REQ_D_TKEY));
                if (drItem.Length > 0)
                {
                    DataTable tempT = new DataTable();
                    tempT = drItem[0].Table.Clone();
                    DataSet tempDs = new DataSet();
                    tempDs.Tables.Add(tempT);
                    tempDs.Merge(drItem);
                    DataTable dtItem = tempDs.Tables[0];
                    MHelper.BindDataSourceForGridControl(GridDetail, GrvInStockREQDLOT, dtItem);
                }
                else
                {
                    MHelper.BindDataSourceForGridControl(GridDetail, GrvInStockREQDLOT, MHelper.QueryBindGridView(SqlGridView).Ds.Tables[0]);//GridView绑定空数据源
                }
            }
            else
            {
                MHelper.BindDataSourceForGridControl(GridDetail, GrvInStockREQDLOT, MHelper.QueryBindGridView(SqlGridView).Ds.Tables[0]);//GridView绑定空数据源
            }
        }

        /// <summary>
        /// 绑定GridLookUpEdit的数据源 指定表
        /// </summary>
        public void BindGrdLookUpEdit()
        {
            List<string> strsql = new List<string>();
            List<GridLookUpEdit> Control = new List<GridLookUpEdit>();
            strsql.Add("SELECT TKEY,WORKORGAN_NAME,WORKORGAN_CODE from BCOR_WORKORGANIZATION where  FLAG = 1 ");//生产组织
            strsql.Add("SELECT TKEY,SUPPLIER_NAME,SUPPLIER_CODE from BCOR_SUPPLIER where  FLAG = 1 ");//所属供应商
            strsql.Add("SELECT TKEY,CUSTOMER_NAME,CUSTOMER_CODE  from BCOR_CUSTOMER where  FLAG = 1");//所属客户
            strsql.Add("SELECT TKEY,DEPT_NAME,DEPT_CODE from BCOR_DEPT WHERE FLAG = 1");//来源部门
            strsql.Add("SELECT TKEY,EMPLOYEE_NAME,EMPLOYEE_CODE  from BCOR_EMPLOYEE where  FLAG = 1");//来源发起人
            strsql.Add("SELECT TKEY,STOCK_NAME,STOCK_CODE  from BCOR_STOCK where  FLAG = 1");//来源库房
            strsql.Add("SELECT TKEY,STOCK_NAME,STOCK_CODE  from BCOR_STOCK where  FLAG = 1");//目标库房

            Control.Add(txtSOURCE_WKOG_TKEY);//生产组织
            Control.Add(txtSOURCE_SUPPLIER_TKEY);//所属供应商
            Control.Add(txtSOURCE_CUSTOMER_TKEY);//所属客户
            Control.Add(txtSOURCE_DEPT_TKEY);//来源部门
            Control.Add(txtSOURCE_PERSON);//来源发起人
            Control.Add(txtSOURCE_STOCK_TKEY);//来源库房
            Control.Add(txtTO_STOCK_TKEY);//目标库房
            MHelper.BindGridLookUpEdit(strsql, Control);
        }

        /// <summary>
        /// 绑定LookUpEdit的数据源 系统数据字典
        /// </summary>
        public void BindLookUpEdit()
        {
            MHelper.BindSysDict(txtINSTOCK_REQ_TYPE, "MMSMM_INSTOCK_REQ_INSTOCK_REQ_TYPE");
        }

        /// <summary>
        /// 绑定GridView中 ReGridLookUpEdit下拉框的值
        /// </summary>
        public void BindReGridLookUpEdit()
        {
            List<string> strsql = new List<string>();
            List<RepositoryItemGridLookUpEdit> Control = new List<RepositoryItemGridLookUpEdit>();
            strsql.Add("SELECT TKEY , MATERIAL_CODE,MATERIAL_NAME FROM BCMA_MATERIAL WHERE FLAG = 1 ");
            strsql.Add("SELECT TKEY, STOCK_NAME, STOCK_CODE FROM BCOR_STOCK WHERE FLAG = 1");
            strsql.Add("SELECT TKEY, STOCKSTATUS_NAME, STOCKSTATUS_CODE FROM BCOR_STOCKSTATUS WHERE FLAG = 1");
            strsql.Add("SELECT TKEY, STOCKSTATUS_NAME, STOCKSTATUS_CODE FROM BCOR_STOCKSTATUS WHERE FLAG = 1");

            Control.Add(ReGridLookUpEdit);//物料编码
            Control.Add(ReGridLookUpEditStock);//目标库房
            Control.Add(ReGridLookUpEdit_SS);//库存状态
            Control.Add(ReGridLookUpEdit_D);//库存状态

            MHelper.BindReGridLookUpEdit(strsql, Control);
        }

        /// <summary>
        /// 绑定ReLookUpEdit下拉框的值
        /// </summary>
        public void BindReLookUpEdit()
        {
            List<string> strsql = new List<string>();
            List<RepositoryItemLookUpEdit> Control = new List<RepositoryItemLookUpEdit>();
            strsql.Add("Select TKEY,UNIT_NAME,UNIT_CODE from BCDF_UNIT WHERE FLAG = 1");
            strsql.Add("Select TKEY,UNIT_NAME,UNIT_CODE from BCDF_UNIT WHERE FLAG = 1");
            strsql.Add("SELECT TKEY,STOCK_NAME,STOCK_CODE  from BCOR_STOCK where  FLAG = 1");

            Control.Add(ReLookUpEdit);//计量单位 GrvInStock
            Control.Add(ReLookUpEdit_Unit);//计量单位 GrvInStock
            Control.Add(ReLookUpEdit_Stock);//目标库房 GrvInStockREQDLOT

            MHelper.BindReLookUpEdit(strsql, Control);
        }

        /// <summary>
        /// 绑定系统数据字典下拉框的值
        /// </summary>
        public void BindReLookUpEdit_Sys()
        {
            MHelper.BindSysDict(ReLookUpEdit_DS, "MMSMM_INSTOCK_DLOT_INSTOCK_COLLECT_NODE");
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
            DataTable dt = ((DataView)GrvInStockREQ.DataSource).ToTable();//GridView中的数据转成Datatable 方便检查是否为空
            //入库单主表 非空验证
            if (string.IsNullOrEmpty(txtINSTOCK_REQ_NO.EditValue.ToString())) sbErrMsg.Append("入库申请单号不能为空,\n");
            if (string.IsNullOrEmpty(txtINSTOCK_REQ_TYPE.EditValue.ToString())) sbErrMsg.Append("入库申请单类型不能为空,\n");
            if (string.IsNullOrEmpty(txtAGENT_USER.EditValue.ToString())) sbErrMsg.Append("经办人不能为空,\n");
            if (string.IsNullOrEmpty(txtSOURCE_SUPPLIER_TKEY.EditValue.ToString())) sbErrMsg.Append("来源供应商不能为空,\n");
            if (string.IsNullOrEmpty(txtSOURCE_CUSTOMER_TKEY.EditValue.ToString())) sbErrMsg.Append("来源客户不能为空,\n");
            if (string.IsNullOrEmpty(txtSOURCE_WKOG_TKEY.EditValue.ToString())) sbErrMsg.Append("来源生产组织不能为空,\n");
            if (string.IsNullOrEmpty(txtSOURCE_DEPT_TKEY.EditValue.ToString())) sbErrMsg.Append("来源部门不能为空,\n");
            if (string.IsNullOrEmpty(txtSOURCE_PERSON.EditValue.ToString())) sbErrMsg.Append("来源发起人不能为空,\n");
            if (string.IsNullOrEmpty(txtSOURCE_STOCK_TKEY.EditValue.ToString())) sbErrMsg.Append("来源库房不能为空,\n");
            if (string.IsNullOrEmpty(txtTO_STOCK_TKEY.EditValue.ToString())) sbErrMsg.Append("目标库房不能为空,\n");
            if (string.IsNullOrEmpty(txtINSTOCK_PLAN_TIME.EditValue.ToString())) sbErrMsg.Append("计划入库时间不能为空,\n");

            //明细 GridView验证非空
            if (dt.Rows.Count == 0)
            {
                sbErrMsg.Append("需新增明细资料 \n");
            }
            else
            {
                if (dtDetail != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        //循环检查临时表中是否存在至少一笔批次资料  
                        DataRow[] drItem = dtDetail.Select(string.Format("INSTOCK_REQ_D_TKEY =  '{0}'", dr["TKEY"].ToString()));
                        if (drItem.Length == 0)
                        {
                            sbErrMsg.Append("物料编码:" + dr["MATERIAL_CODE"].ToString() + "未新建明细批次资料\n");
                        }
                    }
                }
                else
                {
                    sbErrMsg.Append("明细批次资料不能为空 \n");
                }
            }
            return sbErrMsg.ToString();
        }
        #endregion

        #region GridView 事件  (新增行 删除行 编辑明细批次 特定单元格不能编辑 显示行号)  ----- GrvInStock
        //新增行按钮 
        private void BarBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GrvInStockREQ.AddNewRow();//在GridControl中新增一行

            //GrvInStock
            string Tkey = Guid.NewGuid().ToString();
            this.GrvInStockREQ.SetFocusedRowCellValue("TKEY", Tkey);//入库申请单明细表主键
            this.GrvInStockREQ.SetFocusedRowCellValue("CKEY", ds.Tables["MMSMM_INSTOCK_REQ"].Rows.Count == 0 ? InStockREQTkey : ds.Tables["MMSMM_INSTOCK_REQ"].Rows[0]["TKEY"].ToString());//主表关联主键
            this.GrvInStockREQ.SetFocusedRowCellValue("MATERIAL_TKEY", string.Empty);//物料TKEY  明细表中
            this.GrvInStockREQ.SetFocusedRowCellValue("MATERIAL_CODE", null);//物料编码 物料表 后续需删除列
            this.GrvInStockREQ.SetFocusedRowCellValue("MAPID", string.Empty);//物料图号  物料表 后续需删除列
            this.GrvInStockREQ.SetFocusedRowCellValue("MATERIAL_NAME", string.Empty);//物料名称  物料表 后续需删除列
            this.GrvInStockREQ.SetFocusedRowCellValue("BASE_UNIT_KEY", string.Empty);//基本计量单位名称  
            this.GrvInStockREQ.SetFocusedRowCellValue("INSTOCK_REQ_D_STATUS", "1");//明细状态
            this.GrvInStockREQ.SetFocusedRowCellValue("TO_STOCK_KEY", string.Empty);//目标库房
            this.GrvInStockREQ.SetFocusedRowCellValue("TOTAL_QTY", 0);//入库申请数量
            this.GrvInStockREQ.SetFocusedRowCellValue("ALLOWIN_QTY", 0);//允许入库数量
            this.GrvInStockREQ.SetFocusedRowCellValue("IQC_NEED_FLAG", 0);//是否需要IQC
            this.GrvInStockREQ.SetFocusedRowCellValue("IQC_QTY", 0);//送检数量
            this.GrvInStockREQ.SetFocusedRowCellValue("MATERIAL_LEVEL", 0);//物料等级
            this.GrvInStockREQ.SetFocusedRowCellValue("STOCKSTATUS_CODE", 0);//库存状态编码
            this.GrvInStockREQ.SetFocusedRowCellValue("CMT", "");//备注
            GrvInStockREQ.OptionsBehavior.Editable = true;//栏位可编辑


        }

        //删除行按钮
        private void BarBtnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GrvInStockREQ.SelectedRowsCount == 0) { XtraMessageBox.Show("请先选中行再进行删除！", "删除提示框", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            //删除选中的行
            GrvInStockREQ.DeleteSelectedRows();

        }

        /// <summary>
        /// 检查GridView中当前选中行某些行是否为空
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string  CheckCellIsEmpty(DataRow dr)
        {
            StringBuilder sbErrMsg = new StringBuilder();
            if (string.IsNullOrEmpty(dr["MATERIAL_TKEY"].ToString()) || string.IsNullOrEmpty(dr["BASE_UNIT_KEY"].ToString())) sbErrMsg.Append("请先选择物料编码！");
            if (string.IsNullOrEmpty(dr["TO_STOCK_KEY"].ToString())) sbErrMsg.Append("请选择目标库房！");
            return sbErrMsg.ToString();
        }


        /// <summary>
        /// GridView 特定单元格不能编辑   GrvInStock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrvInStock_ShowingEditor(object sender, CancelEventArgs e)
        {
            switch (GrvInStockREQ.FocusedColumn.FieldName)
            {
                case "MAPID"://物料图号
                case "MATERIAL_NAME"://物料名称
                case "BASE_UNIT_TKEY"://基本计量单位名称
                case "REQ_D_STATUS"://明细状态
                    e.Cancel = true;
                    break;
            }
        }


        //显示行号 行号自增
        private void GrvPurDetail_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }


        #endregion

        #region GridView 事件  (新增行 删除行)  ----- GrvInStockREQDLOT

        //新增行
        private void BarBtnAdd_DLOT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraTabControl tabcontrol = sender as XtraTabControl;
            if (Tempdt != null)
            {
                //GridDetail.DataSource = null;//初始化GridDetail
                DataRow dr = Tempdt.Rows[0];//临时表第一行
                GrvInStockREQDLOT.AddNewRow();//在GridControl中新增一行

                string Tkey = Guid.NewGuid().ToString();
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("TKEY", Guid.NewGuid().ToString());//入库申请单明细批次主键
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("INSTOCK_REQ_TKEY", dr["INSTOCK_REQ_TKEY"].ToString());//入库申请单KEY
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("INSTOCK_REQ_D_TKEY", dr["INSTOCK_REQ_D_TKEY"].ToString());//入库申请单明细KEY
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("MATERIAL_TKEY", dr["MATERIAL_TKEY"].ToString());//物料TKEY 
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("MATERIAL_LOTID", string.Empty);//物料批次
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("ORIGINAL_LOTID", string.Empty);//原始批次
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("TO_STOCK_KEY", dr["TO_STOCK_KEY"].ToString());//目标仓库KEY
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("BASE_UNIT_KEY", dr["BASE_UNIT_KEY"].ToString());//基本计量单位名称  
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("TOTAL_QTY", 0);//入库申请数量
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("ALLOWIN_QTY", 0);//允许入库数量
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("IQC_NEED_FLAG", 0);//是否需要IQC
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("IQC_QTY", 0);//送检数量
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("REGULAR_QTY", 0);//判定合格数量
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("DEFECT_QTY", 0);//判定让步接收数量
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("REJECT_QTY", 0);//拒收数量
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("INFACTORY_TIME", DateTime.Now);//入厂时间
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("PRODUCE_TIME", DateTime.Now.AddDays(1));//生产日期
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("MATERIAL_LEVEL", string.Empty);//申请入库物料等级
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("STOCKSTATUS_CODE", string.Empty);//申请入库库存状态编码
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("INSTOCK_STOCK_QTY", 0);//下推入库单数量
                this.GrvInStockREQDLOT.SetFocusedRowCellValue("CMT", "");//备注


                GrvInStockREQDLOT.OptionsBehavior.Editable = true;//栏位可编辑
            }
            else
            {
                XtraMessageBox.Show("请先选择明细数据！", "提示框", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tabcontrol.SelectedTabPage = tabPageMaster;
                return;
            }


        }

        //删除行
        private void BarBtnDel_DLOT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GrvInStockREQDLOT.SelectedRowsCount == 0) { XtraMessageBox.Show("请先选中行再进行删除！", "删除提示框", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            try
            {
                DeletenRow();//删除选中的行 临时的Datatable删除对应的Row

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }
        }

        /// <summary>
        /// 删除行 同时删除临时Datatable中对应的行
        /// </summary>
        public void DeletenRow()
        {
            if (GrvInStockREQDLOT.DataSource != null)
            {
                //存到临时Datatable
                DataTable dtD = ((DataView)GrvInStockREQDLOT.DataSource).ToTable();
                dtDetail = MHelper.MergeDataTable(dtDetail, dtD, "TKEY", 30);//合并Datatable

            }

            int[] ids = GrvInStockREQDLOT.GetSelectedRows();
            if (ids.Length > 0)
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    DataRow dr = GrvInStockREQDLOT.GetDataRow(ids[i]);
                    string TKEY = dr["TKEY"].ToString();
                    DataRow[] drArray = dtDetail.Select(string.Format("TKEY = '{0}'", TKEY));
                    dtDetail.Rows.Remove(drArray[0]);
                }
            }
            dtDetail.AcceptChanges();
            GrvInStockREQDLOT.DeleteSelectedRows();//删除GridView中的行
        }


        /// <summary>
        /// GridView 特定单元格不能编辑  GrvInStockREQDLOT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrvInStockREQDLOT_ShowingEditor(object sender, CancelEventArgs e)
        {
            switch (GrvInStockREQ.FocusedColumn.FieldName)
            {
                case "TO_STOCK_KEY"://目标仓库
                case "BASE_UNIT_KEY"://基本计量单位
                    e.Cancel = true;
                    break;
            }
        }


        #endregion

        #region GridView 单元格输入验证

        #region GrvInStock
        /// <summary>
        /// GridControl单元格内输入数据验证  NumberEdit --   数量允许重复但不能为0
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
        private void GrvInStockREQ_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
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

        #region GrvInStockREQDLOT
        private void ReDateEdit_D_Validating(object sender, CancelEventArgs e)
        {
            BaseEdit DateEdit = sender as BaseEdit;
            if (DateEdit.Text.Length == 0)
            {
                e.Cancel = true;
                errorReason = 0;
                return;
            }
        }

        private void ReTextEdit_D_Validating(object sender, CancelEventArgs e)
        {
            BaseEdit textedit = sender as BaseEdit;
            if (textedit.Text.Length == 0)
            {
                e.Cancel = true;
                errorReason = 0;
                return;
            }

        }

        private void ReLookUpEdit_D_Validating(object sender, CancelEventArgs e)
        {
            BaseEdit LookUpEdit = sender as BaseEdit;
            if (LookUpEdit.Text.Length == 0)
            {
                e.Cancel = true;
                errorReason = 0;
                return;
            }

        }

        private void ReGridLookUpEdit_D_Validating(object sender, CancelEventArgs e)
        {
            BaseEdit GridLookUpEdit = sender as BaseEdit;
            if (GridLookUpEdit.Text.Length == 0)
            {
                e.Cancel = true;
                errorReason = 0;
                return;
            }

        }

        private void ReLookUpEdit_DS_Validating(object sender, CancelEventArgs e)
        {
            BaseEdit LookUpEdit_DS = sender as BaseEdit;
            if (LookUpEdit_DS.Text.Length == 0)
            {
                e.Cancel = true;
                errorReason = 0;
                return;
            }

        }

        private void ReNumberEdit_D_Validating(object sender, CancelEventArgs e)
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

        private void GrvInStockREQDLOT_InvalidValueException(object sender, InvalidValueExceptionEventArgs e)
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

        #endregion

        #region 控件触发事件
        /// <summary>
        /// GridView 行内单元格编辑 - BCMA_MATERIAL表 选择物料编码 带出 图号 计量单位 物料名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReGridLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            BaseEdit edit = GrvInStockREQ.ActiveEditor;
            GridLookUpEdit gridlookupedit = sender as GridLookUpEdit;
            MHelper.BindReGLE_BCMA_MATERIAL(GrvInStockREQ, gridlookupedit, "BASE_UNIT_KEY");
        }

        /// <summary>
        /// xtraTabPage切换触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            string Errmsg = string.Empty;
            XtraTabControl tabcontrol = sender as XtraTabControl;//TabControl
            //GridView gridview = sender as GridView;//GridView

            XtraTabPage tabpage = tabcontrol.SelectedTabPage;//当前选中的TabPage
            string SelectName = tabpage.Name.ToString(); //选中的TabPage名
            switch (SelectName)
            {
                case "tabPageMaster"://明细
                    //切换回明细时 如果批次有数据则存到临时的Datatable dtDetail
                    if (GrvInStockREQDLOT.DataSource != null)
                    {
                        //存到临时Datatable
                        DataTable dtD = ((DataView)GrvInStockREQDLOT.DataSource).ToTable();//批次下显示的GridView
                        dtDetail = MHelper.MergeDataTable(dtDetail, dtD, "TKEY", 30);//合并Datatable

                    }
                    GridDetail.DataSource = null;//切换回TabpageMaster时 清空批次Gridview的值
                    break;
                case "tabPageDLOT"://入库单明细批次新建
                    if (GrvInStockREQ.SelectedRowsCount == 0) { XtraMessageBox.Show("请先勾选行再进行编辑！", "提示框", MessageBoxButtons.OK, MessageBoxIcon.Warning); tabcontrol.SelectedTabPage = tabPageMaster; return; }
                    if (GrvInStockREQ.SelectedRowsCount > 1) { XtraMessageBox.Show("只能选中一行进行新建批次！", "提示框", MessageBoxButtons.OK, MessageBoxIcon.Warning); tabcontrol.SelectedTabPage = tabPageMaster; return; }
                    

                    int ids = GrvInStockREQ.GetSelectedRows()[0];//选中的一行
                    DataRow dr = GrvInStockREQ.GetDataRow(ids);
                    Errmsg = CheckCellIsEmpty(dr);//错误提示框
                    if (Errmsg.Length == 0)//无错误 对应dr的值赋值到临时的Datatable
                    {
                        Tempdt = MHelper.CreateTempForGrvInStockREQ();//创建临时表Datatable结构
                        DataRow drnew;
                        drnew = Tempdt.NewRow();
                        drnew["INSTOCK_REQ_TKEY"] = dr["CKEY"].ToString();//主表KEY
                        drnew["INSTOCK_REQ_D_TKEY"] = dr["TKEY"].ToString(); //当前选中行的明细Tkey
                        drnew["MATERIAL_TKEY"] = dr["MATERIAL_TKEY"].ToString();//物料KEY
                        drnew["BASE_UNIT_KEY"] = dr["BASE_UNIT_KEY"].ToString();//计量单位
                        drnew["TO_STOCK_KEY"] = dr["TO_STOCK_KEY"].ToString();//目标库房

                        Tempdt.Rows.Add(drnew);
                    }
                    else
                    {
                        XtraMessageBox.Show(Errmsg, "提示框", MessageBoxButtons.OK, MessageBoxIcon.Warning); tabcontrol.SelectedTabPage = tabPageMaster;
                        //BindGridViewDataSource_D(dtDetail, dr["TKEY"].ToString());//绑定
                        return;
                    }

                    BindGridViewDataSource_D(dtDetail, dr["TKEY"].ToString());//绑定
                    break;
            }
        }

        #region 多列模糊查询
        private void txtSOURCE_WKOG_TKEY_EditValueChanging(object sender, ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                MHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtSOURCE_WKOG_TKEY_Popup(object sender, EventArgs e)
        {
            MHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtSOURCE_SUPPLIER_TKEY_EditValueChanging(object sender, ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                MHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtSOURCE_SUPPLIER_TKEY_Popup(object sender, EventArgs e)
        {
            MHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtSOURCE_CUSTOMER_TKEY_EditValueChanging(object sender, ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                MHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtSOURCE_CUSTOMER_TKEY_Popup(object sender, EventArgs e)
        {
            MHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtSOURCE_PERSON_EditValueChanging(object sender, ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                MHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtSOURCE_PERSON_Popup(object sender, EventArgs e)
        {
            MHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtSOURCE_DEPT_TKEY_EditValueChanging(object sender, ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                MHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtSOURCE_DEPT_TKEY_Popup(object sender, EventArgs e)
        {
            MHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtSOURCE_STOCK_TKEY_EditValueChanging(object sender, ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                MHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtSOURCE_STOCK_TKEY_Popup(object sender, EventArgs e)
        {
            MHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtTO_STOCK_TKEY_EditValueChanging(object sender, ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                MHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtTO_STOCK_TKEY_Popup(object sender, EventArgs e)
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
