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

namespace ASJ.BCOR
{
    /// <summary>
    /// 计量单位管理
    /// </summary>
    public partial class UcUnit : BaseUserControl
    {
        //实例化帮助类
        ASJBCOR_ORG BHelper = new ASJBCOR_ORG();
        Result rs = new Result();


        /// <summary>
        /// 计量单位实体
        /// </summary>
        private BCDF_UNIT unit;

        /// <summary>
        /// 供应商编辑
        /// </summary>
        public UcUnit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_supplier">计量单位实体</param>
        public UcUnit(BCDF_UNIT _unit) : this()
        {
            unit = _unit;
        }

        /// <summary>
        ///  初始化加载
        /// </summary>
        private void UcUnit_Load(object sender, EventArgs e)
        {
            txtUnitCode.EditValue = unit.UNIT_CODE?.ToString();//计量单位编码
            txtUnitName.EditValue = unit.UNIT_NAME?.ToString();//计量单位名称
            txtUnitGrpTkey.EditValue = unit.UNIT_GRP_TKEY?.ToString();//计量单位所属组别
            chBaseUnitFlag.EditValue = unit.BASE_UNIT_FLAG.ToString() == "" ? 0 : int.Parse(unit.BASE_UNIT_FLAG.ToString());//是否基准单位
            txtBaseUnitName.EditValue = unit.BASE_UNIT_TKEY?.ToString();//基准单位
            if (BHelper.CheckBASE_UNIT_FLAG(txtUnitGrpTkey.EditValue?.ToString()))
            {
                txtCONVERT_NUMERATOR.EditValue = "1";
                txtCONVERT_DENOMINATOR.EditValue = "1";
                txtCONVERT_NUMERATOR.Enabled = false;
                txtCONVERT_DENOMINATOR.Enabled = false;

            }
            else
            {
                txtCONVERT_NUMERATOR.EditValue = txtCONVERT_NUMERATOR.EditValue?.ToString();//分子
                txtCONVERT_DENOMINATOR.EditValue = txtCONVERT_DENOMINATOR.EditValue?.ToString();//分母
            }


            txtCMT.EditValue = unit.CMT?.ToString();//备注

            BindGridLookUpEdit();//分组

            //存在基准单位 捞取基准单位赋值到基础单位的栏位
            txtBaseUnitName.EditValue = BHelper.CheckBaseUnit(txtUnitGrpTkey.EditValue?.ToString()) == true ? string.Empty : BHelper.GetBaseUnit(txtUnitGrpTkey.EditValue?.ToString());


        }

        /// <summary>
        /// 更新方法
        /// </summary>
        /// <returns>返回一个计量单位实体</returns>
        public BCDF_UNIT UpdateUI()
        {
            unit.UNIT_CODE = txtUnitCode.EditValue?.ToString();
            unit.UNIT_NAME = txtUnitName.EditValue?.ToString();
            unit.UNIT_GRP_TKEY = txtUnitGrpTkey.EditValue?.ToString();
            unit.BASE_UNIT_FLAG = int.Parse(chBaseUnitFlag.EditValue?.ToString());
            unit.CMT = txtCMT.EditValue?.ToString();

            unit.CONVERT_NUMERATOR = int.Parse(txtCONVERT_NUMERATOR.EditValue?.ToString());
            unit.CONVERT_DENOMINATOR = int.Parse(txtCONVERT_DENOMINATOR.EditValue?.ToString());
            unit.BASE_UNIT_TKEY = txtBaseUnitName.EditValue?.ToString();

            return unit;
        }

        /// <summary>
        /// 分子分母数据验证
        /// </summary>
        public string DataValidation()
        {
            StringBuilder sbErrMsg = new StringBuilder();
            int NUMERATOR = int.Parse(txtCONVERT_NUMERATOR.EditValue.ToString() == "" ? "1" : txtCONVERT_NUMERATOR.EditValue.ToString());//分子
            int DENOMINATOR = int.Parse(txtCONVERT_DENOMINATOR.EditValue.ToString() == "" ? "1" : txtCONVERT_DENOMINATOR.EditValue.ToString());//分母
            if (NUMERATOR > DENOMINATOR) sbErrMsg.Append("分子必须小于分母 \n");
            return sbErrMsg.ToString();
        }

        #region 触发事件

        //计量单位名称触发事件  该栏位 = 编码 + 名称
        private void txtUnitName_EditValueChanged(object sender, EventArgs e)
        {
            if (BHelper.CheckBaseUnit(txtUnitGrpTkey.EditValue?.ToString()) || BHelper.CheckBASE_UNIT_FLAG(txtUnitGrpTkey.EditValue?.ToString()))
            {
                txtBaseUnitName.Text = txtUnitName.Text;
                txtBaseUnitName.Enabled = false;
            }
            else
            {

            }
        }

        //分母栏位输入后 验证数据正确性 分子必须小于等于分母
        private void txtCONVERT_DENOMINATOR_EditValueChanged(object sender, EventArgs e)
        {

            if (!BHelper.CheckBaseUnit(txtUnitGrpTkey.EditValue?.ToString()))
            {
                string Errmsg = DataValidation();
                if (Errmsg.Length > 0)
                {
                    XtraMessageBox.Show(Errmsg, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        //选择不同的组别 加载不同的数据 
        private void txtUnitGrpTkey_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dt = BHelper.GetBaseUnitFlag(txtUnitGrpTkey.EditValue?.ToString());
            if (dt.Rows.Count > 0)//当前组别是否存在基准单位
            {
                DataRow dr = dt.Rows[0];
                //已存在基准单位
                txtBaseUnitName.Text = dr["UNIT_NAME"].ToString();
                chBaseUnitFlag.EditValue = 0;
                txtCONVERT_NUMERATOR.EditValue = "";
                txtCONVERT_DENOMINATOR.EditValue = "";
                txtCONVERT_NUMERATOR.Enabled = true;
                txtCONVERT_DENOMINATOR.Enabled = true;
            }
            else
            {
                //不存在基准单位 则创建一个基准单位
                txtBaseUnitName.Text = string.Empty;
                chBaseUnitFlag.EditValue = 1;
                txtCONVERT_NUMERATOR.EditValue = 1;
                txtCONVERT_DENOMINATOR.EditValue = 1;
                txtCONVERT_NUMERATOR.Enabled = false;
                txtCONVERT_DENOMINATOR.Enabled = false;

                //chBaseUnitFlag.EditValue = Flag == true ? 1 : 0;//是否基准单位
                //txtCONVERT_NUMERATOR.EditValue = Flag == true ? "1" : "";//换算分子
                //txtCONVERT_DENOMINATOR.EditValue = Flag == true ? "1" : "";//换算分母

                //txtCONVERT_NUMERATOR.Enabled = Flag == true ? false : true;
                //txtCONVERT_DENOMINATOR.Enabled = Flag == true ? false : true;
                //txtBaseUnitName.EditValue = Flag == true ? string.Empty : BHelper.GetBaseUnit(txtUnitGrpTkey.EditValue?.ToString());//存在基准单位 捞取基准单位赋值到基础单位的栏位
            }
        }



        #endregion

        public void BindGridLookUpEdit()
        {
            BHelper.BindGridLookUpEdit_Unit(txtUnitGrpTkey);
        }

        private void txtUnitGrpTkey_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));

        }

        private void txtUnitGrpTkey_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);

        }
    }
}
