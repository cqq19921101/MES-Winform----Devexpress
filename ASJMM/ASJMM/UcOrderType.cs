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

namespace ASJMM
{
    /// <summary>
    /// 物料管理模块 - 物料单据类型
    /// </summary>
    public partial class UcOrderType : BaseUserControl
    {
        //帮助类
        ASJMM_CLTROUTE MHelper = new ASJMM_CLTROUTE();

        //实体类
        private MMSMM_ORDERTYPE ordertype;

        /// <summary>
        /// 控件加载
        /// </summary>
        public UcOrderType()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_ordertype"></param>
        public UcOrderType(MMSMM_ORDERTYPE _ordertype) : this()
        {
            ordertype = _ordertype;
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         private void UcOrderType_Load(object sender, EventArgs e)
        {
            txtORDERTYPE_CODE.EditValue = ordertype.ORDERTYPE_CODE?.ToString();//单据类型编码
            txtORDERTYPE_NAME.EditValue = ordertype.ORDERTYPE_NAME?.ToString();//单据类型名称
            txtBUSINESS_TYPE.EditValue = ordertype.BUSINESS_TYPE?.ToString();//业务场景
            txtCMT.EditValue = ordertype.CMT?.ToString();//备注

            MHelper.BindSysDict(txtBUSINESS_TYPE, "MMSMM_ORDERTYPE_BUSINESS_TYPE");//绑定业务场景下拉框的值 (系统数据字典)

        }

        /// <summary>
        /// 更新方法
        /// </summary>
        /// <returns>返回实体</returns>
        public MMSMM_ORDERTYPE UpdateUI()
        {
            ordertype.ORDERTYPE_CODE = txtORDERTYPE_CODE.EditValue?.ToString();
            ordertype.ORDERTYPE_NAME = txtORDERTYPE_NAME.EditValue?.ToString();
            ordertype.BUSINESS_TYPE = txtBUSINESS_TYPE.EditValue?.ToString();
            ordertype.CMT = txtCMT.EditValue?.ToString();
            return ordertype;
        }



    }
}
