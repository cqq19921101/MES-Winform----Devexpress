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
using ASJ.TOOLS.Data;
using ASJ.TOOLS.Basic;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;

namespace ASJ.BCTE
{
    /// <summary>
    /// 工艺配置 - 工序自定义参数委会
    /// </summary>
    public partial class UcDIYParaMeter : BaseUserControl
    {
        //实例化帮助类
        BCTEHelper Helper = new BCTEHelper();
        Result rs = new Result();


        private DataSet ds;//工艺参数
        int errorReason = 777;
        bool IsError = true;


        //工序模板主键
        private BCTE_DIYPARAMETER DIYParaMeter;

        /// <summary>
        /// 控件加载
        /// </summary>
        public UcDIYParaMeter()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        public UcDIYParaMeter(BCTE_DIYPARAMETER _DIYParaMeter) :this()
        {
            DIYParaMeter = _DIYParaMeter;
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcDIYParaMeter_Load(object sender, EventArgs e)
        {

            LoadData(DIYParaMeter);//初始化加载

            #region 绑定下拉框的值 (系统数据字典表&&其他表)
            BindLookUpEdit();
            #endregion

        }

        #region 数据处理方法 数据绑定
        /// <summary>
        /// 初始化加载数据
        /// </summary>
        /// <param name="TKEY">工序自定义参数表 TKEY</param>
        public void LoadData(BCTE_DIYPARAMETER DIYParaMeter)
        {
            string TKEY = DIYParaMeter.TKEY == string.Empty ? Guid.NewGuid().ToString() : DIYParaMeter.TKEY;

            List<string> strsql = new List<string>();
            List<string> TableNames = new List<string>();

            string SqlMaster = @" SELECT * FROM BCTE_DIYPARAMETER WHERE FLAG = 1  AND TKEY = " + "'" + TKEY + "'";
            //string Sql = @" SELECT * FROM BCTE_DIYPMT_ITEM WHERE FLAG = 1  AND DIYPMT_TKEY = " + "'" + TKEY + "'";


            strsql.Add(SqlMaster);//主档
            TableNames.Add("BCTE_DIYPARAMETER");
            //TableNames.Add("BCTE_DIYPMT_ITEM");

            ds = OracleHelper.Get_DataSet(strsql, TableNames);

            #region 控件内容赋值到Dataset
            txtDMCODE.EditValue = ds.Tables["BCTE_DIYPARAMETER"].Rows.Count == 0 ? string.Empty : ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["DIYPMT_CODE"].ToString();//自定义参数编码
            txtDMNAME.EditValue = ds.Tables["BCTE_DIYPARAMETER"].Rows.Count == 0 ? string.Empty : ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["DIYPMT_NAME"].ToString();//自定义参数名称
            txtSTANDARD_VALUES.EditValue = ds.Tables["BCTE_DIYPARAMETER"].Rows.Count == 0 ? string.Empty : ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["STANDARD_VALUES"].ToString();//标准值:
            txtUPPER_LIMIT.EditValue = ds.Tables["BCTE_DIYPARAMETER"].Rows.Count == 0 ? string.Empty : ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["UPPER_LIMIT"].ToString();//标准上限值
            txtLOWER_LIMIT.EditValue = ds.Tables["BCTE_DIYPARAMETER"].Rows.Count == 0 ? string.Empty : ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["LOWER_LIMIT"].ToString();//标准下限值
            txtATTSOURCE_TKEY.EditValue = ds.Tables["BCTE_DIYPARAMETER"].Rows.Count == 0 ? string.Empty : ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["ATTSOURCE_TKEY"].ToString();//关联附件
            txtCMT.EditValue = ds.Tables["BCTE_DIYPARAMETER"].Rows.Count == 0 ? string.Empty : ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["CMT"].ToString();//备注


            txtDMCHECKTYPE.EditValue = ds.Tables["BCTE_DIYPARAMETER"].Rows.Count == 0 ? string.Empty : ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["DIYPMT_CHECKTYPE"].ToString();//自定义参数方式
            txtSOURCE.EditValue = ds.Tables["BCTE_DIYPARAMETER"].Rows.Count == 0 ? string.Empty : ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["SOURCE"].ToString();//数据源

            chISNEED.EditValue = int.Parse(ds.Tables["BCTE_DIYPARAMETER"].Rows.Count == 0 ? "0" : ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["ISNEED"].ToString());//是否必填
            chBBEYOND_WARING.EditValue = int.Parse(ds.Tables["BCTE_DIYPARAMETER"].Rows.Count == 0 ? "0" : ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["BEYOND_WARING"].ToString());//超标预警
            chBEYOND_RESTRICT.EditValue = int.Parse(ds.Tables["BCTE_DIYPARAMETER"].Rows.Count == 0 ? "0" : ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["BEYOND_RESTRICT"].ToString());//超标限制

            #endregion

            rs = Helper.Query("BCTE_DIYPMT_ITEM", TKEY, "DIYPMT_TKEY");//根据关联的TKEY带出子表的数据 并绑定到GridControl
            Helper.BindDataSourceForGridControl(GridItem, GrvDIYPMT, rs.Ds.Tables[0]);//绑定View
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

            #region 保存工序自定义参数主表资料
            ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["DIYPMT_CODE"] = txtDMCODE.EditValue ?? txtDMCODE.EditValue.ToString();
            ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["DIYPMT_NAME"] = txtDMNAME.EditValue ?? txtDMNAME.EditValue.ToString();
            ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["STANDARD_VALUES"] = txtSTANDARD_VALUES.EditValue ?? txtSTANDARD_VALUES.EditValue.ToString();
            ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["UPPER_LIMIT"] = txtUPPER_LIMIT.EditValue ?? txtUPPER_LIMIT.EditValue.ToString();
            ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["LOWER_LIMIT"] = txtLOWER_LIMIT.EditValue ?? txtLOWER_LIMIT.EditValue.ToString();
            ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["ATTSOURCE_TKEY"] = txtATTSOURCE_TKEY.EditValue ?? txtATTSOURCE_TKEY.EditValue.ToString();
            ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["CMT"] = txtCMT.EditValue ?? txtCMT.EditValue.ToString();

            ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["ISNEED"] = chISNEED.EditValue ?? chISNEED.EditValue.ToString();
            ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["BEYOND_WARING"] = chBBEYOND_WARING.EditValue ?? chBBEYOND_WARING.EditValue.ToString();
            ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["BEYOND_RESTRICT"] = chBEYOND_RESTRICT.EditValue ?? chBEYOND_RESTRICT.EditValue.ToString();


            ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["DIYPMT_CHECKTYPE"] = txtDMCHECKTYPE.EditValue ?? txtDMCHECKTYPE.EditValue.ToString();
            ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["SOURCE"] = txtSOURCE.EditValue ?? txtSOURCE.EditValue.ToString();
            #endregion

            #region 如果数据源是选项选填 且 Gridview中有数据 则将GridView中的值存到Dataset中
            DataTable dt = ((DataView)GrvDIYPMT.DataSource).ToTable();
            dt.TableName = "BCTE_DIYPMT_ITEM";
            if (int.Parse(txtSOURCE.EditValue.ToString()) == 3 && dt.Rows.Count > 0)
            {
                ds.Tables.Add(dt);
            }
            #endregion

            OracleHelper.UpdateDataSet(ds);//更新整个Dataset
        }

        #region 绑定下拉框的值
        public void BindLookUpEdit()
        {
            List<LookUpEdit> Control = new List<LookUpEdit>();
            List<string> Para = new List<string>();

            //自定义参数方式
            Control.Add(txtDMCHECKTYPE);
            Para.Add("BCTE_DIYPARAMETER_DIYPMT_CHECKTYPE");
            //数据源
            Control.Add(txtSOURCE);
            Para.Add("BCTE_DIYPARAMETER_SOURCE");

            Helper.BindLookUpEdit(Control, Para);//绑定下拉框的值 (系统数据字典)

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
            int StandardValue = int.Parse(txtSTANDARD_VALUES.EditValue.ToString() == "" ? "0" : txtSTANDARD_VALUES.EditValue.ToString());
            int UpperValue = int.Parse(txtUPPER_LIMIT.EditValue.ToString() == "" ? "0" : txtUPPER_LIMIT.EditValue.ToString());
            int LowerValue = int.Parse(txtLOWER_LIMIT.EditValue.ToString() == "" ? "0" : txtLOWER_LIMIT.EditValue.ToString());
            DataTable dtItem = ((DataView)GrvDIYPMT.DataSource).ToTable();
            //工序自定义数`
            if (string.IsNullOrEmpty(txtDMCODE.EditValue.ToString())) sbErrMsg.Append("自定义参数编码不能为空,\n");
            if (string.IsNullOrEmpty(txtDMNAME.EditValue.ToString())) sbErrMsg.Append("自定义参数名称不能为空,\n");
            if (string.IsNullOrEmpty(txtDMCHECKTYPE.EditValue.ToString())) sbErrMsg.Append("自定义参数方式不能为空,\n");

            if(int.Parse(txtDMCHECKTYPE.EditValue.ToString()) == 2 &&
                (string.IsNullOrEmpty(txtSTANDARD_VALUES.EditValue.ToString()) || string.IsNullOrEmpty(txtUPPER_LIMIT.EditValue.ToString())
                || string.IsNullOrEmpty(txtLOWER_LIMIT.EditValue.ToString()))) sbErrMsg.Append("方式选择定量时,标准值、标准上限值、标准下限值不能为空 \n");
            if (StandardValue < LowerValue || StandardValue > UpperValue || UpperValue < LowerValue)
                sbErrMsg.Append("请确认标准值,标准上限值，标准下限值的逻辑关系 \n");

            //工序自定义参数值
            if (int.Parse(txtSOURCE.EditValue.ToString()) == 3 && dtItem.Rows.Count == 0) sbErrMsg.Append("数据源栏位是选项选填时,必须新增工序自定义参数值 \n");
            return sbErrMsg.ToString();
        }

        #endregion

        #region GridControl 事件  (新增行 删除行)
        //新增行按钮 
        private void BarBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GrvDIYPMT.AddNewRow();//在GridControl中新增一行
            this.GrvDIYPMT.SetFocusedRowCellValue("TKEY", Guid.NewGuid().ToString());//工艺自定义参数值 表主键
            this.GrvDIYPMT.SetFocusedRowCellValue("DIYPMT_TKEY", ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["TKEY"].ToString());//关联参数表的主键
            this.GrvDIYPMT.SetFocusedRowCellValue("VALUE1", 0);//参数值1
            this.GrvDIYPMT.SetFocusedRowCellValue("VALUE2", 0);//参数值2
            this.GrvDIYPMT.SetFocusedRowCellValue("VALUE3", 0);//参数值3
            this.GrvDIYPMT.SetFocusedRowCellValue("ATTSOURCE_TKEY", "");//关联附件TKEY
            this.GrvDIYPMT.SetFocusedRowCellValue("CMT", "");//备注

            GrvDIYPMT.OptionsBehavior.Editable = true;
        }

        //删除行按钮
        private void BarBtnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GrvDIYPMT.SelectedRowsCount == 0) { XtraMessageBox.Show("请先选中行再进行删除！", "删除提示框", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            //删除选中的行
            GrvDIYPMT.DeleteSelectedRows();

            ////DataTable dt = (DataTable)GridItem.DataSource;
            //List<string> lstDelKey = new List<string>();
            //foreach(int index in GridDITPMT.GetSelectedRows())
            //{
            //    string delKey = GridDITPMT.GetDataRow(index)["tkey"].ToString();
            //    lstDelKey.Add(delKey);
            //}

        }

        #endregion

        #region GridView 单元格输入验证


        /// <summary>
        /// GridControl单元格内输入数据验证  NumberEdit  数字的输入允许重复
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
            else
            {
                e.ErrorText = "404";
            }
            IsError = false;
        }

        #endregion

        #region 控件触发事件
        //自定义参数方式下拉框触发  选中"定量"时(Value = 2),标准值3个控件必填
        private void txtDMCHECKTYPE_EditValueChanged(object sender, EventArgs e)
        {
            int value = int.Parse(txtDMCHECKTYPE.EditValue.ToString() == "" ? "0" : txtDMCHECKTYPE.EditValue.ToString());
            if (value == 2)
            {
                txtSTANDARD_VALUES.Enabled = true;
                txtUPPER_LIMIT.Enabled = true;
                txtLOWER_LIMIT.Enabled = true;

                //如果该Tkey中定量方式下有标准值 则赋值到前台显示
                txtSTANDARD_VALUES.EditValue = ds.Tables["BCTE_DIYPARAMETER"].Rows.Count == 0 ? string.Empty : ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["STANDARD_VALUES"].ToString();//标准值:
                txtUPPER_LIMIT.EditValue = ds.Tables["BCTE_DIYPARAMETER"].Rows.Count == 0 ? string.Empty : ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["UPPER_LIMIT"].ToString();//标准上限值
                txtLOWER_LIMIT.EditValue = ds.Tables["BCTE_DIYPARAMETER"].Rows.Count == 0 ? string.Empty : ds.Tables["BCTE_DIYPARAMETER"].Rows[0]["LOWER_LIMIT"].ToString();//标准下限值

            }
            else
            {
                txtSTANDARD_VALUES.Enabled = false;
                txtUPPER_LIMIT.Enabled = false;
                txtLOWER_LIMIT.Enabled = false;

                txtSTANDARD_VALUES.EditValue = string.Empty;
                txtUPPER_LIMIT.EditValue = string.Empty;
                txtLOWER_LIMIT.EditValue = string.Empty;
            }
        }


        //数据源下拉框触发  选中"选项选填"时(Value=3),显示自定义参数明细 
        private void txtSOURCE_EditValueChanged(object sender, EventArgs e)
        {
            int value = int.Parse(txtSOURCE.EditValue.ToString() == "" ? "0" : txtSOURCE.EditValue.ToString());
            if (value == 3)
            {
                GridItem.Visible = true;
                BarGrid.Visible = true;
            }
            else
            {
                GridItem.Visible = false;
                BarGrid.Visible = false;
            }
        }

        #endregion


        //测试用
        private void button1_Click(object sender, EventArgs e)
        {
            string ErrMsgText = string.Empty;
            ErrMsgText = JudgeEmpty();
            if (ErrMsgText.Length > 0)
            {
                XtraMessageBox.Show(ErrMsgText, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
                return;
            }
        }

        private DataTable GetMaterialData()
        {
            string sql = @"SELECT * FROM  BCMA_MATERIAL where FLAG = 1";
            DataSet ds = OracleHelper.Query(sql);
            rs.Ds = ds;
            rs.Msg = "Success";
            rs.Status = true;
            return rs.Ds.Tables[0];
        }



    }
}
