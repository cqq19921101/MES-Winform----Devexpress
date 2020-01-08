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
    /// 物料管理 - 单据采集节点基础档案
    /// </summary>
    public partial class UcCltNodeBase : BaseUserControl
    {
        //帮助类
        MMSMMHelper MHelper = new MMSMMHelper();

        //实体类
        private MMSMM_CLTNODE_BASE cltnodebase;


        /// <summary>
        /// 控件加载
        /// </summary>
        public UcCltNodeBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_cltnodebase"></param>
        public UcCltNodeBase(MMSMM_CLTNODE_BASE _cltnodebase) : this()
        {
            cltnodebase = _cltnodebase;
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcCltNodeBase_Load(object sender, EventArgs e)
        {
            txtCLTNODE_CODE.EditValue = cltnodebase.CLTNODE_CODE?.ToString();//采集节点编码
            txtCLTNODE_NAME.EditValue = cltnodebase.CLTNODE_NAME?.ToString();//采集节点名称
            txtCLTNODE_TYPE.EditValue = cltnodebase.CLTNODE_TYPE?.ToString();//采集节点类型
            txtCMT.EditValue = cltnodebase.CMT?.ToString();//备注

            #region 绑定下拉框的值 (系统数据字典表)
            BindLookUpEdit();
            #endregion
        }


        /// <summary>
        /// 更新方法
        /// </summary>
        /// <returns></returns>
        public MMSMM_CLTNODE_BASE UpdateUI()
        {
            cltnodebase.CLTNODE_CODE = txtCLTNODE_CODE.EditValue?.ToString();
            cltnodebase.CLTNODE_NAME = txtCLTNODE_NAME.EditValue?.ToString();
            cltnodebase.CLTNODE_TYPE = txtCLTNODE_TYPE.EditValue?.ToString();
            cltnodebase.CMT = txtCMT.EditValue?.ToString();

            return cltnodebase;
        }

        public void BindLookUpEdit()
        {
            List<LookUpEdit> Control = new List<LookUpEdit>();
            List<string> Para = new List<string>();

            //采集节点类型
            Control.Add(txtCLTNODE_TYPE);
            Para.Add("MMSMM_CLTNODE_BASE_CLTNODE_TYPE");

            MHelper.BindLookUpEdit(Control, Para);//绑定业务场景下拉框的值 (系统数据字典)

        }
    }
}
