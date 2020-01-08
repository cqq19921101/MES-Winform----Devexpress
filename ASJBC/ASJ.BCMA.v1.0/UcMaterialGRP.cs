using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ASJ.ENTITY;
using ASJ.BASE;
using ASJ.TOOLS.Basic;

namespace ASJ.BCMA
{
    /// <summary>
    /// 物料配置 - 物料组别管理
    /// </summary>
    public partial class UcMaterialGRP : BaseUserControl
    {
        BCMAHelper BHelper = new BCMAHelper();
        /// <summary>
        ///计量单位分组实体
        /// </summary>
        private BCMA_MATERIALGRP materialgroup;

        /// <summary>
        /// 物料组别编辑
        /// </summary>
        public UcMaterialGRP()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成 
        /// </summary>
        /// <param name="materialgroup">物料组别实体</param>
        public UcMaterialGRP(BCMA_MATERIALGRP _materialgroup) : this()
        {
            materialgroup = _materialgroup;

        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcUcUnitGRP_Load(object sender, EventArgs e)
        {
            txtMGRPCode.EditValue = materialgroup.MATERIALGRP_CODE?.ToString();//分组编码
            txtMGRPName.EditValue = materialgroup.MATERIALGRP_NAME?.ToString();//分组名称
            txtMGRPTkey.EditValue = materialgroup.F_MATERIALGRP_TKEY?.ToString();//上级分组KEY
            txtCMT.EditValue = materialgroup.CMT?.ToString();//备注
            BindMaterGRPTKey();//绑定节点下拉框的值
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns>返回一个物料组别实体</returns>
        public BCMA_MATERIALGRP UpdateUI()
        {
            materialgroup.MATERIALGRP_CODE = txtMGRPCode.EditValue?.ToString();
            materialgroup.MATERIALGRP_NAME = txtMGRPName.EditValue?.ToString();
            materialgroup.F_MATERIALGRP_TKEY = txtMGRPTkey.EditValue?.ToString();
            materialgroup.CMT = txtCMT.EditValue?.ToString();

            return materialgroup;
        }

        /// <summary>
        /// 绑定物料组别下拉框
        /// </summary>
        public void BindMaterGRPTKey()
        {
            string sql = @"SELECT TKEY,MATERIALGRP_NAME,MATERIALGRP_CODE FROM BCMA_MATERIALGRP WHERE FLAG = 1 ";
            BHelper.BindGridLookUpEdit(sql, txtMGRPTkey);
        }

        //多列模糊查询
        private void txtMGRPTkey_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtMGRPTkey_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }
    }
}
