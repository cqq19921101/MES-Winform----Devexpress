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
using ASJ.TOOLS.Data;

namespace ASJ.BCMA
{
    /// <summary>
    /// 物料配置 - 物料主档
    /// </summary>
    public partial class UcMaterial : BaseUserControl
    {
        /// <summary>
        /// 实例化帮助类
        /// </summary>
        BCMAHelper BHelper = new BCMAHelper();

        private BCMA_MATERIAL material;

        private string materialTkey;//主档 基础资料
        private string materialTkey_Pur;//采购
        private string materialTkey_Sto;//库存
        private string materialTkey_Qua;//质量
        private string materialTkey_Use;//业务
        private string materialTkey_Pro;//生产

        private DataSet ds;


        /// <summary>
        /// 物料主档编辑
        /// </summary>
        public UcMaterial()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_supplier">物料主档实体</param>
        public UcMaterial(BCMA_MATERIAL _material) : this()
        {

            material = _material;

        }

        /// <summary>
        ///  初始化加载
        /// </summary>
        private void UcMaterial_Load(object sender, EventArgs e)
        {
            materialTkey = material.TKEY == null ? Guid.NewGuid().ToString() : material.TKEY;
            materialTkey_Pur = material.TKEY == null ? Guid.NewGuid().ToString() : BHelper.GetMappingTKEY("BCMA_MATERIAL_PURCHASE");//采购
            materialTkey_Sto = material.TKEY == null ? Guid.NewGuid().ToString() : BHelper.GetMappingTKEY("BCMA_MATERIAL_STOCK");//库存
            materialTkey_Qua = material.TKEY == null ? Guid.NewGuid().ToString() : BHelper.GetMappingTKEY("BCMA_MATERIAL_QUALITY");//质量
            materialTkey_Use = material.TKEY == null ? Guid.NewGuid().ToString() : BHelper.GetMappingTKEY("BCMA_MATERIAL_USECONTROL");//业务
            materialTkey_Pro = material.TKEY == null ? Guid.NewGuid().ToString() : BHelper.GetMappingTKEY("BCMA_MATERIAL_PRODUCE");//生产
            LoadData(materialTkey);//初始化加载物料资料
        }


        #region 数据处理 数据绑定
        /// <summary>
        /// 物料多张表放到Dataset中
        /// </summary>
        /// <returns></returns>
        private void LoadData(string Tkey)
        {
            QueryDataToDataset(Tkey);

            #region 初始化赋值 将Dataset的值赋到控件中

            #region 物料主档    BCMA_MATERIAL
            txtMATERIAL_NAME.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["MATERIAL_CODE"].ToString();//物料编码
            txtMATERIAL_CODE.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["MATERIAL_NAME"].ToString();//物料名称
            txtMAPID.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["MAPID"].ToString();//图号
            txtSPEC.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["SPEC"].ToString();//规格型号
            txtVERSION.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["VERSION"].ToString();//物料版本

            txtMATERIAL_TYPE.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["MATERIAL_TYPE"].ToString();//物料类型 
            txtMATERIAL_SOURCE_TYPE.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["MATERIAL_SOURCE_TYPE"].ToString();//物料来源类型

            txtBASE_UNIT_TKEY.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["BASE_UNIT_TKEY"].ToString();//基本计量单位
            txtASSIST_UNIT_TKEY.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["ASSIST_UNIT_TKEY"].ToString();//辅助计量单位
            #endregion

            #region 物料基础属性   BCMA_MATERIAL
            txtMATERIAL_INSIDE_CODE.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["MATERIAL_INSIDE_CODE"].ToString();//内部物料编码
            txtMATERIAL_SUPPLIER_CODE.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["MATERIAL_SUPPLIER_CODE"].ToString();//供应商物料编码
            txtMATERIAL_CUSTOMER_CODE.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["MATERIAL_CUSTOMER_CODE"].ToString();//客户物料编码
            txtBARCODE.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["BARCODE"].ToString();//商品编码
            txtMIN_PACKAGE_QTY.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["MIN_PACKAGE_QTY"].ToString();//最小包装数量
            txtNET_WEIGHT.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["NET_WEIGHT"].ToString();//净重
            txtGROSS_WEIGHT.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["GROSS_WEIGHT"].ToString();//毛重
            txtLENGTH.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["LENGTH"].ToString();//长
            txtWIDTH.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["WIDTH"].ToString();//宽
            txtHEIGHT.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["HEIGHT"].ToString();//高
            txtVOLUME.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["VOLUME"].ToString();//体积

            txtLENGTH_UNIT_TKEY.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["LENGTH_UNIT_TKEY"].ToString();//长度计量单位
            txtWEIGHT_UNIT_TKEY.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["WEIGHT_UNIT_TKEY"].ToString();//重量计量单位
            txtVOLUME_UNIT_TKEY.EditValue = ds.Tables["BCMA_MATERIAL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL"].Rows[0]["VOLUME_UNIT_TKEY"].ToString();//体积计量单位
            #endregion

            #region 物料采购属性   BCMA_MATERIAL_PURCHASE
            txtPURCHASE_CONTROL_TYPE.EditValue = ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows[0]["PURCHASE_CONTROL_TYPE"].ToString(); ;//来料组批类型
            txtPURCHASE_CONTROL_LOTQTY.EditValue = ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows[0]["PURCHASE_CONTROL_LOTQTY"].ToString();//来料标准组批数量
            txtDEFAULT_SUPPLIER_KEY.EditValue = ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows[0]["DEFAULT_SUPPLIER_KEY"].ToString();//供应商Key  供应商表（BCOR_SUPPLIER）的数据TKEY

            chMUST_DEFAULTSUPP_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows[0]["MUST_DEFAULTSUPP_FLAG"].ToString());//强制使用默认供应商标识
            chSUPPLIER_LOT_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows[0]["SUPPLIER_LOT_FLAG"].ToString());//启用供应商批次标识
            chDELIVERY_ACTIVE_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows[0]["DELIVERY_ACTIVE_FLAG"].ToString());//到货接收启用标识
            chPURCHASE_REQUEST_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows[0]["PURCHASE_REQUEST_FLAG"].ToString());//强制请购申请标识

            chMUST_DEFAULTSUPP_FLAG.EditValue = ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows[0]["PURCHASE_CONTROL_LOTQTY"].ToString();//备注

            #endregion

            #region 物料库存属性   BCMA_MATERIAL_STOCK  
            txtDEFAULT_USER_TKEY.EditValue = ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["DEFAULT_USER_TKEY"].ToString();//库房管理员
            txtDEFAULT_SITE_TKEY.EditValue = ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["DEFAULT_SITE_TKEY"].ToString();//库位
            txtDEFAULT_STOCK_TKEY.EditValue = ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["DEFAULT_STOCK_TKEY"].ToString();//库房
            txtLOCK_TYPE.EditValue = ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["LOCK_TYPE"].ToString();//锁定类型
            txtLOCK_TIME.EditValue = ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["LOCK_TIME"].ToString();//锁定时间
            txtLOCK_CMT.EditValue = ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["LOCK_CMT"].ToString();//锁定备注
            txtMAX_STOCK_TIME.EditValue = ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["MAX_STOCK_TIME"].ToString();//最大库存周期
            txtMAX_STOCK_UNIT_TKEY.EditValue = ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["MAX_STOCK_UNIT_TKEY"].ToString();//最大库存周期单位
            txtMAX_STOCK_QTY.EditValue = ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["MAX_STOCK_QTY"].ToString();//最大库存数量
            txtMIN_STOCK_QTY.EditValue = ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["MIN_STOCK_QTY"].ToString();//最小库存数量
            txtSAFE_STOCK_QTY.EditValue = ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["SAFE_STOCK_QTY"].ToString();//安全库存数量
            txtMSCMT.EditValue = ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["CMT"].ToString();//备注

            chMUST_DEFAULTSTOCK_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["MUST_DEFAULTSTOCK_FLAG"].ToString());//入库默认库房标识
            chFIFO_FORCE_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["FIFO_FORCE_FLAG"].ToString());//强制先进先出标识
            chLOCK_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["LOCK_FLAG"].ToString()); ;//锁定标识
            chEXPIRE_WARNING_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["EXPIRE_WARNING_FLAG"].ToString()); ;//库存有效期预警标识
            chSAFETY_STOCK_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["SAFETY_STOCK_FLAG"].ToString()); ;//安全预警标识
            chINST_LOTCLT_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["INST_LOTCLT_FLAG"].ToString()); ;//入库强制采集标识
            chOQC_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["OQC_FLAG"].ToString()); ;//出库检验启用标识
            chSTOCKSITE_CLT_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["STOCKSITE_CLT_FLAG"].ToString()); ;//库位强制采集标识
            chOUTSTOCK_REQ_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["OUTSTOCK_REQ_FLAG"].ToString()); ;//出库申请启用标识
            chSTOCKLIST_RESERVE_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["STOCKLIST_RESERVE_FLAG"].ToString()); ;//分配库存启用标识
            chSTOCKLIST_LOCK_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["STOCKLIST_LOCK_FLAG"].ToString()); ;//分配库存锁定标识

            #endregion

            #region 物料质量属性   BCMA_MATERIAL_QUALITY  
            txtIQC_TYPE.EditValue = ds.Tables["BCMA_MATERIAL_QUALITY"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["IQC_TYPE"].ToString();//来料检验抽样类型
            txtINSTQC_TYPE.EditValue = ds.Tables["BCMA_MATERIAL_QUALITY"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["INSTQC_TYPE"].ToString();//在库检验抽样类型
            txtINST_CYQC_UNIT_TKEY.EditValue = ds.Tables["BCMA_MATERIAL_QUALITY"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["INST_CYQC_UNIT_TKEY"].ToString();//在库周期检验周期单位
            txtFQC_TYPE.EditValue = ds.Tables["BCMA_MATERIAL_QUALITY"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["FQC_TYPE"].ToString();//成品完工检验类型
            txtOQC_TYPE.EditValue = ds.Tables["BCMA_MATERIAL_QUALITY"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["OQC_TYPE"].ToString();//发货检验抽样类型

            txtIQC_MIN_PERCENT.EditValue = ds.Tables["BCMA_MATERIAL_QUALITY"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["IQC_MIN_PERCENT"].ToString();//来料检验最小抽样比例
            txtINSTQC_MIN_PERCENT.EditValue = ds.Tables["BCMA_MATERIAL_QUALITY"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["INSTQC_MIN_PERCENT"].ToString();//在库检验最小抽样比例
            txtINST_CYQC_TIME.EditValue = ds.Tables["BCMA_MATERIAL_QUALITY"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["INST_CYQC_TIME"].ToString();//在库周期检验时间
            txtINST_CYQC_WARN_DAYS.EditValue = ds.Tables["BCMA_MATERIAL_QUALITY"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["INST_CYQC_WARN_DAYS"].ToString();//在库周期检验预警天数
            txtFQC_MIN_PERCENT.EditValue = ds.Tables["BCMA_MATERIAL_QUALITY"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["FQC_MIN_PERCENT"].ToString();//成品完工检验抽样比例
            txtOQC_MIN_PERCENT.EditValue = ds.Tables["BCMA_MATERIAL_QUALITY"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["OQC_MIN_PERCENT"].ToString();//发货检验最小抽样比例
            txtQualityCMT.EditValue = ds.Tables["BCMA_MATERIAL_QUALITY"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["CMT"].ToString();//备注

            chIQC_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_QUALITY"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["IQC_FLAG"].ToString());//启用来料检验标识
            chINSTQC_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_QUALITY"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["INSTQC_FLAG"].ToString());//启用在库检验标识
            chINST_CYQC_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_QUALITY"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["INST_CYQC_FLAG"].ToString());//启用在库周期检验标识
            chINST_CYQC_WARN_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_QUALITY"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["INST_CYQC_WARN_FLAG"].ToString());//启用在库周期检验预警标识
            chFQC_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_QUALITY"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["FQC_FLAG"].ToString());//成品完工检验标识
            chOQC_FLAG_Q.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_QUALITY"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["OQC_FLAG"].ToString());//启用发货检验标识
            #endregion

            #region 物料业务属性   BCMA_MATERIAL_USECONTROL
            chPURCHASE_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows[0]["PURCHASE_FLAG"].ToString());//可采购标识
            chPRODUCE_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows[0]["PRODUCE_FLAG"].ToString());//可生产标识
            chSALE_FLAGag.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows[0]["SALE_FLAG"].ToString());//可销售标识
            chOUTPRODUCE_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows[0]["OUTPRODUCE_FLAG"].ToString());//可外协标识
            chPICK_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows[0]["PICK_FLAG"].ToString());//可领料标识
            chVMI_FLAG.EditValue = int.Parse(ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows.Count == 0 ? "0" : ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows[0]["VMI_FLAG"].ToString());//允许VMI标识

            txtBSCMT.EditValue = ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows[0]["CMT"].ToString();//备注
            #endregion

            #region 物料生产属性   BCMA_MATERIAL_PRODUCE   
            txtPRODUCE_CONTROL_TYPE.EditValue = ds.Tables["BCMA_MATERIAL_PRODUCE"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_PRODUCE"].Rows[0]["PRODUCE_CONTROL_TYPE"].ToString();//加工过程管控类型
            txtPRODUCE_CONTROL_LOTQTY.EditValue = ds.Tables["BCMA_MATERIAL_PRODUCE"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_PRODUCE"].Rows[0]["PURCHASE_FLAG"].ToString();//加工过程标准组批数量
            txtPCCMT.EditValue = ds.Tables["BCMA_MATERIAL_PRODUCE"].Rows.Count == 0 ? string.Empty : ds.Tables["BCMA_MATERIAL_PRODUCE"].Rows[0]["CMT"].ToString();//备注
            #endregion

            #endregion

            #region 绑定下拉框的值
            BindLookUpEdit();
            BindGridLookUpEdit();
            #endregion
        }

        /// <summary>
        /// Save方法
        /// </summary>
        public void SaveFunction()
        {
            //非空校验
            //string ErrMsgText = string.Empty;
            //string ErrMsgCheckEdit = string.Empty;
            //ErrMsgText = JudgeEmptyForTextEdit();//Text 非空验证
            //ErrMsgCheckEdit = JudgeEmptyForCheckEdit();//CheckEdit 非空验证
            //if (ErrMsgText.Length > 0)
            //{
            //    XtraMessageBox.Show(ErrMsgText, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
            //    return;
            //}

            //if (ErrMsgCheckEdit.Length > 0)
            //{
            //    XtraMessageBox.Show(ErrMsgCheckEdit, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
            //    return;
            //}

            #region 物料主档 && 物料基础属性
            if (ds.Tables["BCMA_MATERIAL"].Rows.Count == 0) BHelper.InsertNewRow(ds, "BCMA_MATERIAL");
            ds.Tables["BCMA_MATERIAL"].Rows[0]["TKEY"] = materialTkey;
            ds.Tables["BCMA_MATERIAL"].Rows[0]["MATERIAL_CODE"] =  txtMATERIAL_NAME.EditValue ?? txtMATERIAL_NAME.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL"].Rows[0]["MATERIAL_NAME"] = txtMATERIAL_CODE.EditValue ?? txtMATERIAL_CODE.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL"].Rows[0]["MAPID"] = txtMAPID.EditValue ?? txtMAPID.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL"].Rows[0]["SPEC"] = txtSPEC.EditValue ?? txtSPEC.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL"].Rows[0]["VERSION"] = txtVERSION.EditValue ?? txtVERSION.EditValue.ToString();

            ds.Tables["BCMA_MATERIAL"].Rows[0]["MATERIAL_SOURCE_TYPE"] = txtMATERIAL_SOURCE_TYPE.EditValue ?? txtMATERIAL_SOURCE_TYPE.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL"].Rows[0]["BASE_UNIT_TKEY"] = txtBASE_UNIT_TKEY.EditValue ?? txtBASE_UNIT_TKEY.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL"].Rows[0]["ASSIST_UNIT_TKEY"] = txtASSIST_UNIT_TKEY.EditValue ?? txtASSIST_UNIT_TKEY.EditValue.ToString();

            ds.Tables["BCMA_MATERIAL"].Rows[0]["MATERIAL_INSIDE_CODE"] = txtMATERIAL_INSIDE_CODE.EditValue ?? txtMATERIAL_INSIDE_CODE.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL"].Rows[0]["MATERIAL_SUPPLIER_CODE"] = txtMATERIAL_SUPPLIER_CODE.EditValue ?? txtMATERIAL_SUPPLIER_CODE.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL"].Rows[0]["MATERIAL_CUSTOMER_CODE"] = txtMATERIAL_CUSTOMER_CODE.EditValue ?? txtMATERIAL_CUSTOMER_CODE.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL"].Rows[0]["BARCODE"] = txtBARCODE.EditValue ?? txtBARCODE.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL"].Rows[0]["MIN_PACKAGE_QTY"] = txtMIN_PACKAGE_QTY.EditValue.ToString() == "" ? 0 : txtMIN_PACKAGE_QTY.EditValue;
            ds.Tables["BCMA_MATERIAL"].Rows[0]["NET_WEIGHT"] = txtNET_WEIGHT.EditValue.ToString() == "" ? 0 : txtNET_WEIGHT.EditValue;
            ds.Tables["BCMA_MATERIAL"].Rows[0]["GROSS_WEIGHT"] = txtGROSS_WEIGHT.EditValue.ToString() == "" ? 0 : txtGROSS_WEIGHT.EditValue;
            ds.Tables["BCMA_MATERIAL"].Rows[0]["WEIGHT_UNIT_TKEY"] = txtWEIGHT_UNIT_TKEY.EditValue ?? txtWEIGHT_UNIT_TKEY.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL"].Rows[0]["LENGTH"] = txtLENGTH.EditValue.ToString() == "" ? 0 : txtLENGTH.EditValue;
            ds.Tables["BCMA_MATERIAL"].Rows[0]["WIDTH"] = txtWIDTH.EditValue.ToString() == "" ? 0 : txtWIDTH.EditValue;
            ds.Tables["BCMA_MATERIAL"].Rows[0]["HEIGHT"] = txtHEIGHT.EditValue.ToString() == "" ? 0 : txtHEIGHT.EditValue;
            ds.Tables["BCMA_MATERIAL"].Rows[0]["LENGTH_UNIT_TKEY"] = txtLENGTH_UNIT_TKEY.EditValue ?? txtLENGTH_UNIT_TKEY.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL"].Rows[0]["VOLUME"] = txtVOLUME.EditValue.ToString() == "" ? 0 : txtVOLUME.EditValue;
            ds.Tables["BCMA_MATERIAL"].Rows[0]["VOLUME_UNIT_TKEY"] = txtVOLUME_UNIT_TKEY.EditValue ?? txtVOLUME_UNIT_TKEY.EditValue.ToString();
            #endregion

            #region 物料采购属性
            if (ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows.Count == 0) BHelper.InsertNewRow(ds, "BCMA_MATERIAL_PURCHASE");
            ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows[0]["TKEY"] = materialTkey_Pur == null ? Guid.NewGuid().ToString() : materialTkey_Pur;
            ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows[0]["MATERIAL_TKEY"] = materialTkey;
            ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows[0]["PURCHASE_CONTROL_TYPE"] = txtPURCHASE_CONTROL_TYPE.EditValue.ToString() ?? txtPURCHASE_CONTROL_TYPE.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows[0]["PURCHASE_CONTROL_LOTQTY"] = txtPURCHASE_CONTROL_LOTQTY.EditValue.ToString() == "" ? 0 : txtPURCHASE_CONTROL_LOTQTY.EditValue;
            ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows[0]["DEFAULT_SUPPLIER_KEY"] = txtDEFAULT_SUPPLIER_KEY.EditValue ?? txtDEFAULT_SUPPLIER_KEY.EditValue.ToString();

            ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows[0]["MUST_DEFAULTSUPP_FLAG"] = chMUST_DEFAULTSUPP_FLAG.EditValue.ToString() == "" ? 0 : chMUST_DEFAULTSUPP_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows[0]["SUPPLIER_LOT_FLAG"] = chSUPPLIER_LOT_FLAG.EditValue.ToString() == "" ? 0 : chSUPPLIER_LOT_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows[0]["DELIVERY_ACTIVE_FLAG"] = chDELIVERY_ACTIVE_FLAG.EditValue.ToString() == "" ? 0 : chDELIVERY_ACTIVE_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows[0]["PURCHASE_REQUEST_FLAG"] = chPURCHASE_REQUEST_FLAG.EditValue.ToString() == "" ? 0 : chPURCHASE_REQUEST_FLAG.EditValue;

            ds.Tables["BCMA_MATERIAL_PURCHASE"].Rows[0]["MUST_DEFAULTSUPP_FLAG"] = chMUST_DEFAULTSUPP_FLAG.EditValue.ToString() == "" ? 0 : chMUST_DEFAULTSUPP_FLAG.EditValue;
            #endregion

            #region 物料库存属性   
            if (ds.Tables["BCMA_MATERIAL_STOCK"].Rows.Count == 0) BHelper.InsertNewRow(ds, "BCMA_MATERIAL_STOCK");
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["TKEY"] = materialTkey_Sto == null ? Guid.NewGuid().ToString() : materialTkey_Sto;
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["MATERIAL_TKEY"] = materialTkey;
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["DEFAULT_USER_TKEY"] = txtDEFAULT_USER_TKEY.EditValue ?? txtDEFAULT_USER_TKEY.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["DEFAULT_SITE_TKEY"] = txtDEFAULT_SITE_TKEY.EditValue ?? txtDEFAULT_SITE_TKEY.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["DEFAULT_STOCK_TKEY"] = txtDEFAULT_STOCK_TKEY.EditValue ?? txtDEFAULT_STOCK_TKEY.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["LOCK_TYPE"] = txtLOCK_TYPE.EditValue ?? txtLOCK_TYPE.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["LOCK_TIME"] = txtLOCK_TIME.EditValue.ToString() == "" ? DBNull.Value : txtLOCK_TIME.EditValue;
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["LOCK_CMT"] = txtLOCK_CMT.EditValue ?? txtLOCK_CMT.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["MAX_STOCK_TIME"] = txtMAX_STOCK_TIME.EditValue.ToString() == "" ? DBNull.Value : txtMAX_STOCK_TIME.EditValue;
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["MAX_STOCK_UNIT_TKEY"] = txtMAX_STOCK_UNIT_TKEY.EditValue ?? txtMAX_STOCK_UNIT_TKEY.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["MAX_STOCK_QTY"] = txtMAX_STOCK_QTY.EditValue.ToString() == "" ? 0 : txtMAX_STOCK_QTY.EditValue;
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["MIN_STOCK_QTY"] = txtMIN_STOCK_QTY.EditValue.ToString() == "" ? 0 : txtMIN_STOCK_QTY.EditValue;
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["SAFE_STOCK_QTY"] = txtSAFE_STOCK_QTY.EditValue.ToString() == "" ? 0 : txtSAFE_STOCK_QTY.EditValue;
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["CMT"] = txtMSCMT.EditValue ?? txtMSCMT.EditValue.ToString();

            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["MUST_DEFAULTSTOCK_FLAG"] = chMUST_DEFAULTSTOCK_FLAG.EditValue.ToString() == "" ? 0 : chMUST_DEFAULTSTOCK_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["FIFO_FORCE_FLAG"] = chFIFO_FORCE_FLAG.EditValue.ToString() == "" ? 0 : chFIFO_FORCE_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["LOCK_FLAG"] = chLOCK_FLAG.EditValue.ToString() == "" ? 0 : chLOCK_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["EXPIRE_WARNING_FLAG"] = chEXPIRE_WARNING_FLAG.EditValue.ToString() == "" ? 0 : chEXPIRE_WARNING_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["SAFETY_STOCK_FLAG"] = chSAFETY_STOCK_FLAG.EditValue.ToString() == "" ? 0 : chSAFETY_STOCK_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["INST_LOTCLT_FLAG"] = chINST_LOTCLT_FLAG.EditValue.ToString() == "" ? 0 : chINST_LOTCLT_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["OQC_FLAG"] = chOQC_FLAG.EditValue.ToString() == "" ? 0 : chOQC_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["STOCKSITE_CLT_FLAG"] = chSTOCKSITE_CLT_FLAG.EditValue.ToString() == "" ? 0 : chSTOCKSITE_CLT_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["OUTSTOCK_REQ_FLAG"] = chOUTSTOCK_REQ_FLAG.EditValue.ToString() == "" ? 0 : chOUTSTOCK_REQ_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["STOCKLIST_RESERVE_FLAG"] = chSTOCKLIST_RESERVE_FLAG.EditValue.ToString() == "" ? 0 : chSTOCKLIST_RESERVE_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_STOCK"].Rows[0]["STOCKLIST_LOCK_FLAG"] = chSTOCKLIST_LOCK_FLAG.EditValue.ToString() == "" ? 0 : chSTOCKLIST_LOCK_FLAG.EditValue;
            #endregion

            #region 物料质量属性
            if (ds.Tables["BCMA_MATERIAL_QUALITY"].Rows.Count == 0)  BHelper.InsertNewRow(ds, "BCMA_MATERIAL_QUALITY");
            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["TKEY"] = materialTkey_Qua == null ? Guid.NewGuid().ToString() : materialTkey_Qua;
            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["MATERIAL_TKEY"] = materialTkey;
            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["IQC_TYPE"] = txtIQC_TYPE.EditValue ?? txtIQC_TYPE.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["INSTQC_TYPE"] = txtINSTQC_TYPE.EditValue ?? txtINSTQC_TYPE.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["INST_CYQC_UNIT_TKEY"] = txtINST_CYQC_UNIT_TKEY.EditValue ?? txtINST_CYQC_UNIT_TKEY.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["FQC_TYPE"] = txtFQC_TYPE.EditValue ?? txtFQC_TYPE.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["OQC_TYPE"] = txtOQC_TYPE.EditValue ?? txtOQC_TYPE.EditValue.ToString();

            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["IQC_MIN_PERCENT"] = txtIQC_MIN_PERCENT.EditValue.ToString() == "" ? 0 : txtIQC_MIN_PERCENT.EditValue;
            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["INSTQC_MIN_PERCENT"] = txtINSTQC_MIN_PERCENT.EditValue.ToString() == "" ? 0 : txtINSTQC_MIN_PERCENT.EditValue;
            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["INST_CYQC_TIME"] = txtINST_CYQC_TIME.EditValue.ToString() == "" ? DBNull.Value : txtMAX_STOCK_TIME.EditValue;
            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["INST_CYQC_WARN_DAYS"] = txtINST_CYQC_WARN_DAYS.EditValue.ToString() == "" ? 0 : txtINST_CYQC_WARN_DAYS.EditValue;
            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["FQC_MIN_PERCENT"] = txtFQC_MIN_PERCENT.EditValue.ToString() == "" ? 0 : txtFQC_MIN_PERCENT.EditValue;
            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["OQC_MIN_PERCENT"] = txtOQC_MIN_PERCENT.EditValue.ToString() == "" ? 0 : txtOQC_MIN_PERCENT.EditValue;
            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["CMT"] = txtQualityCMT.EditValue ?? txtQualityCMT.EditValue.ToString();

            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["IQC_FLAG"] = chIQC_FLAG.EditValue.ToString() == "" ? 0 : chIQC_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["INSTQC_FLAG"] = chINSTQC_FLAG.EditValue.ToString() == "" ? 0 : chINSTQC_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["INST_CYQC_FLAG"] = chINST_CYQC_FLAG.EditValue.ToString() == "" ? 0 : chINST_CYQC_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["INST_CYQC_WARN_FLAG"] = chINST_CYQC_WARN_FLAG.EditValue.ToString() == "" ? 0 : chINST_CYQC_WARN_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["FQC_FLAG"] = chFQC_FLAG.EditValue.ToString() == "" ? 0 : chFQC_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_QUALITY"].Rows[0]["OQC_FLAG"] = chOQC_FLAG_Q.EditValue.ToString() == "" ? 0 : chOQC_FLAG_Q.EditValue;

            #endregion

            #region 物料业务属性   BCMA_MATERIAL_USECONTROL
            if (ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows.Count == 0) BHelper.InsertNewRow(ds, "BCMA_MATERIAL_USECONTROL");
            ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows[0]["TKEY"] = materialTkey_Use == null ? Guid.NewGuid().ToString() : materialTkey_Use;
            ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows[0]["MATERIAL_TKEY"] = materialTkey;
            ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows[0]["PURCHASE_FLAG"] = chPURCHASE_FLAG.EditValue.ToString() == "" ? 0 : chPURCHASE_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows[0]["PRODUCE_FLAG"] = chPRODUCE_FLAG.EditValue.ToString() == "" ? 0 : chPRODUCE_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows[0]["SALE_FLAG"] = chSALE_FLAGag.EditValue.ToString() == "" ? 0 : chSALE_FLAGag.EditValue;
            ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows[0]["OUTPRODUCE_FLAG"] = chOUTPRODUCE_FLAG.EditValue.ToString() == "" ? 0 : chOUTPRODUCE_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows[0]["PICK_FLAG"] = chPICK_FLAG.EditValue.ToString() == "" ? 0 : chPICK_FLAG.EditValue;
            ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows[0]["VMI_FLAG"] = chVMI_FLAG.EditValue.ToString() == "" ? 0 : chVMI_FLAG.EditValue;

            ds.Tables["BCMA_MATERIAL_USECONTROL"].Rows[0]["CMT"] = txtBSCMT.EditValue ?? txtBSCMT.EditValue.ToString();
            #endregion

            #region 物料生产属性  
            if (ds.Tables["BCMA_MATERIAL_PRODUCE"].Rows.Count == 0) BHelper.InsertNewRow(ds, "BCMA_MATERIAL_PRODUCE");
            ds.Tables["BCMA_MATERIAL_PRODUCE"].Rows[0]["TKEY"] = materialTkey_Pro == null ? Guid.NewGuid().ToString() : materialTkey_Pro;
            ds.Tables["BCMA_MATERIAL_PRODUCE"].Rows[0]["MATERIAL_TKEY"] = materialTkey;
            ds.Tables["BCMA_MATERIAL_PRODUCE"].Rows[0]["PRODUCE_CONTROL_TYPE"] = txtPRODUCE_CONTROL_TYPE.EditValue.ToString() ?? txtPRODUCE_CONTROL_TYPE.EditValue.ToString();
            ds.Tables["BCMA_MATERIAL_PRODUCE"].Rows[0]["PRODUCE_CONTROL_LOTQTY"] = txtPRODUCE_CONTROL_LOTQTY.EditValue.ToString() == "" ? 0 : txtPRODUCE_CONTROL_LOTQTY.EditValue;
            ds.Tables["BCMA_MATERIAL_PRODUCE"].Rows[0]["CMT"] = txtPCCMT.EditValue ?? txtPCCMT.EditValue.ToString();
            #endregion

            try
            {
                OracleHelper.UpdateDataSet(ds);//更新整个Dataset

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);return;
            }
        }

        /// <summary>
        /// 下拉框绑定 LookUpEDit
        /// </summary>
        public void BindLookUpEdit()
        {
            List<string> ParaMeter = new List<string>();
            List<LookUpEdit> Control = new List<LookUpEdit>();
            //基础
            Control.Add(txtMATERIAL_SOURCE_TYPE);//物料来源类型
            Control.Add(txtMATERIAL_TYPE);//物料类型
            //采购
            Control.Add(txtPURCHASE_CONTROL_TYPE);//来料组批类型
            //库存
            Control.Add(txtLOCK_TYPE);//锁定类型
            //质量
            Control.Add(txtIQC_TYPE);//来料检验抽样类型
            Control.Add(txtINSTQC_TYPE);//在库检验抽样类型
            Control.Add(txtFQC_TYPE);//成品完工检验类型
            Control.Add(txtOQC_TYPE);//发货检验抽样类型
            //生产
            Control.Add(txtPRODUCE_CONTROL_TYPE);//加工管控类型


            ParaMeter.Add("BCMA_MATERIAL_MATERIAL_SOURCE_TYPE");
            ParaMeter.Add("BCMA_MATERIAL_MATERIAL_TYPE");
            ParaMeter.Add("BCMA_MATERIAL_PURCHASE_PURCHASE_CONTROL_TYPE");
            ParaMeter.Add("BCMA_MATERIAL_STOCK_LOCK_TYPE");
            ParaMeter.Add("BCMA_MATERIAL_QUALITY_IQC_TYPE");
            ParaMeter.Add("BCMA_MATERIAL_QUALITY_INSTQC_TYPE");
            ParaMeter.Add("BCMA_MATERIAL_QUALITY_FQC_TYPE");
            ParaMeter.Add("BCMA_MATERIAL_QUALITY_OQC_TYPE");
            ParaMeter.Add("BCMA_MATERIAL_PRODUCE_PRODUCE_CONTROL_TYPE");

            BHelper.BindLookUpEdit(Control,ParaMeter);
        }

        /// <summary>
        /// 下拉框绑定 GridLookUpEdit
        /// </summary>
        public void BindGridLookUpEdit()
        {
            List<string> strsql = new List<string>();
            List<GridLookUpEdit> Control = new List<GridLookUpEdit>();
            string sqlunit = "SELECT TKEY, UNIT_NAME, UNIT_CODE FROM BCDF_UNIT WHERE FLAG = 1";
            strsql.Add(sqlunit);//基本计量单位
            strsql.Add(sqlunit);//辅助计量单位
            strsql.Add(sqlunit);//重量计量单位
            strsql.Add(sqlunit);//长度计量单位
            strsql.Add(sqlunit);//体积计量单位
            strsql.Add("SELECT TKEY,SUPPLIER_NAME,SUPPLIER_CODE from BCOR_SUPPLIER where  FLAG = 1 ");//所属供应商
            strsql.Add("SELECT TKEY,EMPLOYEE_NAME,EMPLOYEE_CODE FROM BCOR_EMPLOYEE WHERE FLAG = 1");//默认管理员
            strsql.Add("SELECT TKEY,STOCK_NAME,STOCK_CODE FROM BCOR_STOCK WHERE FLAG = 1 ");//默认库房
            strsql.Add("SELECT TKEY,STOCK_SITE_NAME,STOCK_SITE_CODE FROM BCOR_STOCK_SITE WHERE FLAG = 1 ");//默认库位
            strsql.Add(sqlunit);//最大库存周期单位
            strsql.Add(sqlunit);//在库周期检验周期单位

            Control.Add(txtBASE_UNIT_TKEY);
            Control.Add(txtASSIST_UNIT_TKEY);
            Control.Add(txtWEIGHT_UNIT_TKEY);
            Control.Add(txtLENGTH_UNIT_TKEY);
            Control.Add(txtVOLUME_UNIT_TKEY);
            Control.Add(txtDEFAULT_SUPPLIER_KEY);
            Control.Add(txtDEFAULT_USER_TKEY);
            Control.Add(txtDEFAULT_STOCK_TKEY);
            Control.Add(txtDEFAULT_SITE_TKEY);
            Control.Add(txtMAX_STOCK_UNIT_TKEY);
            Control.Add(txtINST_CYQC_UNIT_TKEY);
            BHelper.BindGridLookUpEdit(strsql,Control);
        }
        /// <summary>
        /// 查询的数据赋值到Dataset中
        /// </summary>
        /// <param name="Tkey"></param>
        public void QueryDataToDataset(string Tkey)
        {
            List<string> strsql = new List<string>();
            List<string> TableNames = new List<string>();
            string SqlMaster = $@" SELECT * FROM BCMA_MATERIAL WHERE FLAG = 1  AND TKEY = '{Tkey}' ";
            string SqlMPur = $@" SELECT * FROM BCMA_MATERIAL_PURCHASE WHERE FLAG = 1  AND MATERIAL_TKEY = '{Tkey}' ";
            string SqlMS = $@" SELECT * FROM BCMA_MATERIAL_STOCK WHERE FLAG = 1  AND MATERIAL_TKEY = '{Tkey}' ";
            string SqlMQ = $@" SELECT * FROM BCMA_MATERIAL_QUALITY WHERE FLAG = 1  AND MATERIAL_TKEY = '{Tkey}' ";
            string SqlMU = $@" SELECT * FROM BCMA_MATERIAL_USECONTROL WHERE FLAG = 1  AND MATERIAL_TKEY = '{Tkey}' ";
            string SqlMPro = $@" SELECT * FROM BCMA_MATERIAL_PRODUCE WHERE FLAG = 1  AND MATERIAL_TKEY = '{Tkey}' ";

            strsql.Add(SqlMaster);//主档
            strsql.Add(SqlMPur);//采购
            strsql.Add(SqlMS);//库存
            strsql.Add(SqlMQ);//质量
            strsql.Add(SqlMU);//业务
            strsql.Add(SqlMPro);//生产

            TableNames.Add("BCMA_MATERIAL");
            TableNames.Add("BCMA_MATERIAL_PURCHASE");
            TableNames.Add("BCMA_MATERIAL_STOCK");
            TableNames.Add("BCMA_MATERIAL_QUALITY");
            TableNames.Add("BCMA_MATERIAL_USECONTROL");
            TableNames.Add("BCMA_MATERIAL_PRODUCE");

            ds = OracleHelper.Get_DataSet(strsql, TableNames);

        }
        #endregion

        #region 物料采购属性触发事件

        #endregion

        #region 物料库存属性触发事件
        //锁定标识触发事件
        private void CELockFlag_CheckedChanged(object sender, EventArgs e)
        {
            switch (chLOCK_FLAG.EditValue)
            {
                case 0:
                    txtLOCK_TYPE.Enabled = false;
                    txtLOCK_TIME.Enabled = false;
                    txtLOCK_CMT.Enabled = false;

                    txtLOCK_TYPE.EditValue = string.Empty;
                    txtLOCK_TIME.EditValue = string.Empty;
                    txtLOCK_CMT.EditValue = string.Empty;
                    break;
                case 1:
                    txtLOCK_TYPE.Enabled = true;
                    txtLOCK_TIME.Enabled = true;
                    txtLOCK_CMT.Enabled = true;
                    break;
            }

        }

        //安全预警标识
        private void CESafetyStockFlag_CheckedChanged(object sender, EventArgs e)
        {

            txtSAFE_STOCK_QTY.Enabled = int.Parse(chSAFETY_STOCK_FLAG.EditValue.ToString()) == 1 ? true : false; 
            
        }

        #endregion

        #region 物料质量属性触发事件
        //启用来料检验标识
        private void CEIQCFlag_CheckedChanged(object sender, EventArgs e)
        {
            switch (chIQC_FLAG.EditValue)
            {
                case 0:
                    txtIQC_TYPE.Enabled = false;
                    txtIQC_MIN_PERCENT.Enabled = false;

                    txtIQC_TYPE.EditValue = string.Empty;
                    txtIQC_MIN_PERCENT.EditValue = string.Empty;
                    break;
                case 1:
                    txtIQC_TYPE.Enabled = true;
                    txtIQC_MIN_PERCENT.Enabled = true;
                    break;
            }
        }

        //启用在库检验标识
        private void CEInstocFlag_CheckedChanged(object sender, EventArgs e)
        {
            switch (chINSTQC_FLAG.EditValue)
            {
                case 0:
                    txtINSTQC_TYPE.Enabled = false;
                    txtINSTQC_MIN_PERCENT.Enabled = false;

                    txtINSTQC_TYPE.EditValue = string.Empty;
                    txtINSTQC_MIN_PERCENT.EditValue = string.Empty;
                    break;
                case 1:
                    txtINSTQC_TYPE.Enabled = true;
                    txtINSTQC_MIN_PERCENT.Enabled = true;
                    break;
            }
        }

        //启用在库周期检验标识
        private void CEInstCyocFlag_CheckedChanged(object sender, EventArgs e)
        {
            switch (chINST_CYQC_FLAG.EditValue)
            {
                case 0:
                    txtINST_CYQC_TIME.Enabled = false;
                    txtINST_CYQC_UNIT_TKEY.Enabled = false;

                    txtINST_CYQC_TIME.EditValue = string.Empty;
                    txtINST_CYQC_UNIT_TKEY.EditValue = string.Empty;
                    break;
                case 1:
                    txtINST_CYQC_TIME.Enabled = true;
                    txtINST_CYQC_UNIT_TKEY.Enabled = true;
                    break;
            }
        }

        //启用在库周期检验预警标识
        private void CEInstCyocWarnFlag_CheckedChanged(object sender, EventArgs e)
        {
            switch (chINST_CYQC_WARN_FLAG.EditValue)
            {
                case 0:
                    txtINST_CYQC_WARN_DAYS.Enabled = false;

                    txtINST_CYQC_WARN_DAYS.EditValue = string.Empty;
                    break;
                case 1:
                    txtINST_CYQC_WARN_DAYS.Enabled = true;
                    break;
            }
        }

        //成品完工检验标识
        private void CEFQCFlag_CheckedChanged(object sender, EventArgs e)
        {
            switch (chFQC_FLAG.EditValue)
            {
                case 0:
                    txtFQC_TYPE.Enabled = false;
                    txtFQC_MIN_PERCENT.Enabled = false;

                    txtFQC_TYPE.EditValue = string.Empty;
                    txtFQC_MIN_PERCENT.EditValue = string.Empty;
                    break;
                case 1:
                    txtFQC_TYPE.Enabled = true;
                    txtFQC_MIN_PERCENT.Enabled = true;
                    break;
            }
        }

        //启用发货检验标识
        private void CEOQCFlag_CheckedChanged(object sender, EventArgs e)
        {
            switch (chOQC_FLAG_Q.EditValue)
            {
                case 0:
                    txtOQC_TYPE.Enabled = false;
                    txtOQC_MIN_PERCENT.Enabled = false;

                    txtOQC_TYPE.EditValue = string.Empty;
                    txtOQC_MIN_PERCENT.EditValue = string.Empty;
                    break;
                case 1:
                    txtOQC_TYPE.Enabled = true;
                    txtOQC_MIN_PERCENT.Enabled = true;
                    break;
            }
        }

        #endregion

        #region 特殊字段非空校验
        /// <summary>
        /// 字段非空校验 TextEdit
        /// </summary>
        /// <returns></returns>
        public string JudgeEmptyForTextEdit()
        {
            StringBuilder sbErrMsg = new StringBuilder();
            string ErrMsg = string.Empty;
            //物料主档
            if (string.IsNullOrEmpty(txtMATERIAL_NAME.EditValue.ToString())) sbErrMsg.Append("物料编码,");
            if (string.IsNullOrEmpty(txtMATERIAL_CODE.EditValue.ToString())) sbErrMsg.Append("物料名称,");
            if (string.IsNullOrEmpty(txtVERSION.EditValue.ToString())) sbErrMsg.Append("物料版本,");
            if (string.IsNullOrEmpty(txtMATERIAL_TYPE.EditValue.ToString())) sbErrMsg.Append("物料类型 ");

            return sbErrMsg.ToString() + " 不能为空！";
        }

        /// <summary>
        /// 选中多选框后 关联Text的非空验证
        /// </summary>
        /// <returns></returns>
        public string JudgeEmptyForCheckEdit()
        {
            StringBuilder sbErrMsg = new StringBuilder();
            //物料采购
            if (chSUPPLIER_LOT_FLAG.Checked == true && (string.IsNullOrEmpty(txtDEFAULT_SUPPLIER_KEY.EditValue.ToString()))) sbErrMsg.Append("启用供应商批次标识,供应商不能为空 \n");

            //物料库存
            int MaxStockQTY = int.Parse(txtMAX_STOCK_QTY.EditValue.ToString()== "" ? "0" : txtMAX_STOCK_QTY.EditValue.ToString());//最大库存数量
            int MinStockQTY = int.Parse(txtMIN_STOCK_QTY.EditValue.ToString() == "" ? "0" : txtMIN_STOCK_QTY.EditValue.ToString());//最小库存数量
            if (chLOCK_FLAG.Checked == true && (string.IsNullOrEmpty(txtLOCK_TYPE.EditValue.ToString()) || string.IsNullOrEmpty(txtLOCK_TIME.EditValue.ToString()))) sbErrMsg.Append("启用锁定标识,锁定类型,锁定时间不能为空 \n");
            if (chSAFETY_STOCK_FLAG.Checked == true && (string.IsNullOrEmpty(txtSAFE_STOCK_QTY.EditValue.ToString()))) sbErrMsg.Append("启用安全预警标识,安全库存数量,不能为空 \n,");
            if (MaxStockQTY < MinStockQTY) sbErrMsg.Append("最大库存数量必须大于最小库存数量 \n");

            //物料质量
            if (chIQC_FLAG.Checked == true && (string.IsNullOrEmpty(txtIQC_TYPE.EditValue.ToString()) || string.IsNullOrEmpty(txtIQC_MIN_PERCENT.EditValue.ToString()))) sbErrMsg.Append("启用来料检验标识,来料检验抽样类型,来料检验最小抽样比例不能为空 \n");
            if (chINSTQC_FLAG.Checked == true && (string.IsNullOrEmpty(txtINSTQC_TYPE.EditValue.ToString()) || string.IsNullOrEmpty(txtINSTQC_MIN_PERCENT.EditValue.ToString()))) sbErrMsg.Append("启用在库检验标识,在库检验抽样类型,在库检验最小抽样比例不能为空 \n,");
            if (chINST_CYQC_FLAG.Checked == true && (string.IsNullOrEmpty(txtINST_CYQC_TIME.EditValue.ToString()) || string.IsNullOrEmpty(txtINST_CYQC_UNIT_TKEY.EditValue.ToString()))) sbErrMsg.Append("启用在库周期检验标识,在库周期检验时间,在库周期检验周期单位不能为空 \n,");
            if (chINST_CYQC_WARN_FLAG.Checked == true && (string.IsNullOrEmpty(txtINST_CYQC_WARN_DAYS.EditValue.ToString()))) sbErrMsg.Append("启用在库周期检验预警标识,在库周期检验预警天数不能为空 \n,");
            if (chFQC_FLAG.Checked == true && (string.IsNullOrEmpty(txtFQC_TYPE.EditValue.ToString()) || string.IsNullOrEmpty(txtFQC_MIN_PERCENT.EditValue.ToString()))) sbErrMsg.Append("成品完工检验标识,成品完工检验类型,成品完工检验抽样比例不能为空 \n,");
            if (chOQC_FLAG_Q.Checked == true && (string.IsNullOrEmpty(txtOQC_TYPE.EditValue.ToString()) || string.IsNullOrEmpty(txtOQC_MIN_PERCENT.EditValue.ToString()))) sbErrMsg.Append("启用发货检验标识,发货检验抽样类型,发货检验最小抽样比例不能为空 \n");


            return sbErrMsg.ToString();
        }
        #endregion

        #region 多列模糊查询
        private void txtBASE_UNIT_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtBASE_UNIT_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtASSIST_UNIT_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtASSIST_UNIT_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtWEIGHT_UNIT_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtWEIGHT_UNIT_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtLENGTH_UNIT_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtLENGTH_UNIT_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtVOLUME_UNIT_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtVOLUME_UNIT_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtDEFAULT_SUPPLIER_KEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtDEFAULT_SUPPLIER_KEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtDEFAULT_SITE_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtDEFAULT_SITE_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtDEFAULT_STOCK_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtDEFAULT_STOCK_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtDEFAULT_USER_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtDEFAULT_USER_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtMAX_STOCK_UNIT_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtMAX_STOCK_UNIT_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFunction();
        }

    }
}
