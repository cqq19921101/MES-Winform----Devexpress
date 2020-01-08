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
using ASJ.BCTE;
using ASJ.TOOLS.Basic;

namespace ASJ.BCTE
{
    /// <summary>
    /// 工艺配置 - 工序模板分组
    /// </summary>
    public partial class UcProModelGRP : BaseUserControl
    {
        /// <summary>
        /// 工序模板分组实体
        /// </summary>
        private BCTE_PROMODEL_GRP promodelgrp;

        /// <summary>
        /// 分组编辑
        /// </summary>
        public UcProModelGRP()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_promodelgrp"></param>
        public UcProModelGRP(BCTE_PROMODEL_GRP _promodelgrp) : this()
        {
            promodelgrp = _promodelgrp;
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcProModelGRP_Load(object sender, EventArgs e)
        {
            txtPMGRPCode.EditValue = promodelgrp.PROMODELGRP_CODE?.ToString();//分组编码
            txtPMGRPName.EditValue = promodelgrp.PROMODELGRP_NAME?.ToString();//分组名称
            txtPMGRPTkey.EditValue = promodelgrp.F_PROMODELGRP_TKEY?.ToString();//上级分组KEY
            txtCMT.EditValue = promodelgrp.CMT?.ToString();//备注
            BindDIYPMTGRPTKey();//绑定节点下拉框的值

            //根节点 下拉框无法编辑
            if (txtPMGRPCode.Text.ToUpper() == "ROOT")
            {
                txtPMGRPTkey.Enabled = false;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns>返回一个工序模板分组实体</returns>
        public BCTE_PROMODEL_GRP UpdateUI()
        {
            promodelgrp.PROMODELGRP_CODE = txtPMGRPCode.EditValue?.ToString();
            promodelgrp.PROMODELGRP_NAME = txtPMGRPName.EditValue?.ToString();
            promodelgrp.F_PROMODELGRP_TKEY = txtPMGRPTkey.EditValue?.ToString();
            promodelgrp.CMT = txtCMT.EditValue?.ToString();

            return promodelgrp;
        }


        /// <summary>
        /// 绑定工序模板分组下拉框
        /// </summary>
        public void BindDIYPMTGRPTKey()
        {
            BCTEHelper Helper = new BCTEHelper();
            Result rs = new Result();
            rs = Helper.QueryGroupTable("BCTE_PROMODEL_GRP");
            txtPMGRPTkey.Properties.DataSource = rs.Ds.Tables[0];
            txtPMGRPTkey.Properties.DisplayMember = "PROMODELGRP_CODE";
            txtPMGRPTkey.Properties.ValueMember = "TKEY";
        }

    }
}
