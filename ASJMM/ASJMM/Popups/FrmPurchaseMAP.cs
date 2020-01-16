using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ASJ.BASE;
using ASJ.ENTITY;
using ASJ.TOOLS;
using ASJ.TOOLS.Basic;
using ASJ.TOOLS.Data;
using DevExpress.XtraEditors.Repository;
using System.Data.SqlClient;
using System.Reflection;

namespace ASJMM
{
    /// <summary>
    /// 采购订单单据转换  - 请购单转采购单
    /// </summary>
    public partial class FrmPurchaseMAP : BaseForm
    {
        //帮助类
        ASJMM_Purchase MHelper = new ASJMM_Purchase();
        Result rs = new Result();

        //声明实体
        private MMSMM_PURCHASE purchase;
        private MMSMM_PURCHASE_D purchase_d;
        private MMSMM_PURCHASE_REQ_D purchasereq;

        //声明一个Datatable变量
        private DataTable _dt;

        public DataTable DT
        {
            get { return _dt; }
            set { _dt = value; }
        }

        /// <summary>
        /// 控件加载
        /// </summary>
        public FrmPurchaseMAP()
        {
            InitializeComponent();
            MHelper.BindCustomDrawRowIndicator(GrvPurREQMaster);//GridView新增序号栏位 自增长 宽度自适应
            MHelper.BindCustomDrawRowIndicator(GrvPurREQDetail);//GridView新增序号栏位 自增长 宽度自适应

        }

        public FrmPurchaseMAP(MMSMM_PURCHASE _purchase) : this()
        {
            purchase = _purchase;
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmPurchaseMAP_Load(object sender, EventArgs e)
        {
            BindGrvPurREQMaster();//初始化加载时,显示所有符合条件的请购单记录  单据状态是 4 - 已审核 过滤已审核单据中已经被转换过的单据
            BindLookUpEditForGrv();//·&ReSyscLookUpEdit下拉框的值

        }

        #region GridControl数据筛选 绑定 单据状态记录 变更
        /// <summary>
        /// 选中GridMaster行时 触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrvPurREQMaster_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            BindReLookUpEdit();
            int[] ids = GrvPurREQMaster.GetSelectedRows();//选中了几行
            if (ids.Length > 0)            //选中时 绑定到GridDetail数据源
            {
                List<string> lstKey = new List<string>();
                for (int i = 0; i < ids.Length; i++)
                {
                    DataRow dr = GrvPurREQMaster.GetDataRow(ids[i]);
                    string TKEY = dr["TKEY"].ToString();
                    lstKey.Add(TKEY);
                }

                BindDetailDataSource(lstKey);
            }
            else                        //没有选中时,清空GridDetail数据源
            {
                GridDetail.DataSource = null;
            }
        }

        /// <summary>
        /// 绑定GridDetail的数据源
        /// </summary>
        /// <param name="TKEY"></param>
        public void BindDetailDataSource(List<string> TKEY)
        {
            if (TKEY.Count > 0)
            {
                GridDetail.DataSource = null;
                MHelper.BindDataSourceForGridControl(GridDetail, GrvPurREQDetail, MHelper.QueryForPurchase("MMSMM_PURCHASE_REQ_D", TKEY, "Purchase").Ds.Tables[0]);
            }
            else
            {
                GridDetail.DataSource = null;
            }
        }

        /// <summary>
        /// 绑定请购单主表记录
        /// </summary>
        public void BindGrvPurREQMaster()
        {
            //暂未增加Teky条件   过滤已经转换过的请购单
            string strsql = @"  SELECT * FROM MMSMM_PURCHASE_REQ WHERE FLAG = 1 AND REQUEST_STATUS = 4 order by CRE_TIME ";
            MHelper.BindDataSourceForGridControl(GridMaster, GrvPurREQMaster, MHelper.QueryBindGridView(strsql).Ds.Tables[0]);//绑定GridControl
        }

        #endregion

        #region 绑定 ReLookUpEdit下拉框的值
        /// <summary>
        /// 绑定GrvPurchaseREQDetail下拉框的值
        /// </summary>
        public void BindLookUpEditForGrv()
        {
            BindReSyscLookUpEdit();//系统字典下拉框中的值
            BindReLookUpEdit();//特定表中的值
        }

        /// <summary>
        /// 系统字典表中的值
        /// </summary>
        public void BindReSyscLookUpEdit()
        {
            List<string> ParaMeter = new List<string>();
            List<RepositoryItemLookUpEdit> ReLookUpEdit = new List<RepositoryItemLookUpEdit>();
            //GrvPurchaseREQ_M
            ParaMeter.Add("MMSMM_PURCHASE_REQ_REQUEST_TYPE");//请购单类型
            ParaMeter.Add("MMSMM_PURCHASE_REQ_PQSOURCE_SYSTEM_TYPE");//转换来源类型
            ParaMeter.Add("MMSMM_PURCHASE_REQ_PQSOURCE_ORDER_TYPE");//转换单据类型
            ParaMeter.Add("MMSMM_PURCHASE_REQ_PQSOURCE_TRANS_TYPE");//转换方式类型
            ParaMeter.Add("MMSMM_PURCHASE_REQ_REQUEST_STATUS");//请购单状态

            ReLookUpEdit.Add(ReREQUEST_TYPE);//请购单类型
            ReLookUpEdit.Add(RePQSOURCE_SYSTEM_TYPE);//转换来源类型
            ReLookUpEdit.Add(RePQSOURCE_ORDER_TYPE);//转换单据类型
            ReLookUpEdit.Add(RePQSOURCE_TRANS_TYPE);//转换方式类型
            ReLookUpEdit.Add(ReREQUEST_STATUS);//请购单状态


            MHelper.BindReSyscLookUpEdit(ParaMeter, ReLookUpEdit);
        }

        /// <summary>
        /// 指定表中的值  (部门  职员 计量单位)
        /// </summary>
        public void BindReLookUpEdit()
        {
            List<string> strsqlM = new List<string>();
            List<RepositoryItemLookUpEdit> ReLookUpEdit = new List<RepositoryItemLookUpEdit>();
            strsqlM.Add("SELECT TKEY,DEPT_NAME from bcor_dept WHERE FLAG = 1");//部门
            strsqlM.Add("SELECT TKEY,EMPLOYEE_NAME FROM bcor_employee where FLAG = 1");//职员
            strsqlM.Add("SELECT TKEY,UNIT_NAME from BCDF_UNIT where  FLAG = 1 ");//计量单位

            ReLookUpEdit.Add(ReDEPT);//部门
            ReLookUpEdit.Add(ReEmployee);//职员
            ReLookUpEdit.Add(ReBaseUnit);//计量单位

            MHelper.BindReLookUpEdit(strsqlM, ReLookUpEdit);//绑定GrvPurChaseMaster下拉框的值
        }

        #endregion

        //-------------------------------------------------------

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            int[] count = GrvPurREQDetail.GetSelectedRows();
            if (count.Length > 0)
            {
                DataTable dt = CreateDatatable();
                for (int i = 0; i < count.Length; i++)
                {
                    DataRow dr = GrvPurREQDetail.GetDataRow(count[i]);
                    dt.ImportRow(dr);
                }
                if (dt != null)
                {
                    DT = dt;
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                     XtraMessageBox.Show("请选择需要转换的单据！", "提示框", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; 
                }

            }
            else
            {
                XtraMessageBox.Show("请选择需要转换的单据！", "提示框", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; 
            }
        }

        /// <summary>
        /// 创建Datatable结构
        /// </summary>
        public DataTable CreateDatatable()
        {
            //ModelHandler<MMSMM_PURCHASE_D> Func = new ModelHandler<MMSMM_PURCHASE_D>();
            //List<MMSMM_PURCHASE_D> REQ = new List<MMSMM_PURCHASE_D>();
            //REQ.Add(purchase_d);
            //DataTable dt = Func.FillDataTable(REQ);//实体类转成Datatable
            //dt.Columns.Add("MATERIAL_CODE", typeof(string));
            //dt.Columns.Add("MATERIAL_NAME", typeof(string));
            //dt.Columns.Add("MAPID", typeof(string));
            string strsql = @"  SELECT T1.*,T2.REQUEST_NO FROM 
                                (SELECT T1.*,
                                T2.MATERIAL_CODE,T2.MATERIAL_NAME,T2.MAPID FROM
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
                                WHERE T1.FLAG = 1 )T1
                                LEFT JOIN MMSMM_PURCHASE_REQ T2 
                                ON T1.CKEY = T2.TKEY
                                AND T1.FLAG = T2.FLAG
                                WHERE T1.FLAG = 1 AND T1.TKEY = ''";
            DataTable dt = MHelper.QueryBindGridView(strsql).Ds.Tables[0];
            return dt;
        }

        /// <summary>
        /// 更新单据转换后的请购单明细的状态
        /// </summary>
        private void UpdateDetailStatus()
        {

        }

        private void barBtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
    }
}
