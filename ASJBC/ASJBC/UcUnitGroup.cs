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
using ASJ.TOOLS;
using ASJ.ENTITY;
using ASJ.TOOLS.Basic;

namespace ASJ.BCOR
{
    /// <summary>
    /// 计量单位分组管理
    /// </summary>
    public partial class UcUnitGroup : BaseUserControl
    {
        BCORHelper BHelper = new BCORHelper();
        /// <summary>
        /// 计量单位分组实体
        /// </summary>
        private BCDF_UNIT_GRP unitgroup;

        /// <summary>
        /// 计量单位分组编辑
        /// </summary>
        public UcUnitGroup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成 
        /// </summary>
        /// <param name="_suppliergroup">计量单位实体</param>
        public UcUnitGroup(BCDF_UNIT_GRP _unitgroup) : this()
        {
            unitgroup = _unitgroup;

        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcUnitGroup_Load(object sender, EventArgs e)
        {
            BCORHelper Helper = new BCORHelper();
            txtUGRPCode.EditValue = unitgroup.UNIT_GRP_CODE?.ToString();//分组编码
            txtUGRPName.EditValue = unitgroup.UNIT_GRP_NAME?.ToString();//分组名称
            txtUGRPTkey.EditValue = unitgroup.F_UNITGRP_TKEY?.ToString();//上级分组KEY
            txtCMT.EditValue = unitgroup.CMT?.ToString();//备注
            BindUnitGRPTKey();//绑定节点下拉框的值

            //根节点 下拉框无法编辑
            if (txtUGRPCode.Text.ToUpper() == "ROOT")
            {
                txtUGRPTkey.Enabled = false;
            }

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns>返回一个计量单位分组实体</returns>
        public BCDF_UNIT_GRP UpdateUI()
        {
            unitgroup.UNIT_GRP_CODE = txtUGRPCode.EditValue?.ToString();
            unitgroup.UNIT_GRP_NAME = txtUGRPName.EditValue?.ToString();
            unitgroup.F_UNITGRP_TKEY = txtUGRPTkey.EditValue?.ToString();
            unitgroup.CMT = txtCMT.EditValue?.ToString();

            return unitgroup;
        }


        /// <summary>
        /// 绑定计量单位分组下拉框
        /// </summary>
        public void BindUnitGRPTKey()
        {
            string sql = @"SELECT TKEY,UNIT_GRP_NAME,UNIT_GRP_CODE FROM BCDF_UNIT_GRP WHERE FLAG = 1 ";
            BHelper.BindGridLookUpEdit(sql, txtUGRPTkey);
        }


    }
}
