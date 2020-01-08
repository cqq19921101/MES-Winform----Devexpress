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
using ASJ.TOOLS.Basic;

namespace ASJ.BCOR
{
    public partial class UcWorkORG : BaseUserControl
    {
        BCORHelper BHelper = new BCORHelper();
        Result rs = new Result();

        //声明实体
        private BCOR_WORKORGANIZATION workorg;


        /// <summary>
        /// 加载
        /// </summary>
        public UcWorkORG()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_workorg"></param>
        public UcWorkORG(BCOR_WORKORGANIZATION _workorg) : this()
        {
            workorg = _workorg;
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcWorkORG_Load(object sender, EventArgs e)
        {
            txtWORKORGAN_CODE.EditValue = workorg.WORKORGAN_CODE?.ToString();//生产组织编码
            txtWORKORGAN_NAME.EditValue = workorg.WORKORGAN_NAME?.ToString();//生产组织名称
            txtSHORTNAME.EditValue = workorg.SHORTNAME?.ToString();//组织简称
            txtF_WORKORGAN_TKEY.EditValue = workorg.F_WORKORGAN_TKEY?.ToString();//上级分组KEY
            txtORGANADMIN_EMPL_TKEY.EditValue = workorg.ORGANADMIN_EMPL_TKEY?.ToString();//组织负责人
            txtCMT.EditValue = workorg.CMT?.ToString();//备注

            BindWorkORGNode();//初始化时加载分组节点
            BindGridLookUpEdit();//绑定组织负责人

            //根节点 下拉框无法编辑
            if (txtF_WORKORGAN_TKEY.Text.ToUpper() == "ROOT")
            {
                txtF_WORKORGAN_TKEY.Enabled = false;
            }
        }

        /// <summary>
        /// 更新方法
        /// </summary>
        /// <returns></returns>
        public BCOR_WORKORGANIZATION UpdateUI()
        {
            workorg.WORKORGAN_CODE = txtWORKORGAN_CODE.EditValue?.ToString();
            workorg.WORKORGAN_NAME = txtWORKORGAN_NAME.EditValue?.ToString();
            workorg.SHORTNAME = txtSHORTNAME.EditValue?.ToString();
            workorg.F_WORKORGAN_TKEY = txtF_WORKORGAN_TKEY.EditValue?.ToString();
            workorg.ORGANADMIN_EMPL_TKEY = txtORGANADMIN_EMPL_TKEY.EditValue?.ToString();
            workorg.CMT = txtCMT.EditValue?.ToString();

            return workorg;
        }

        /// <summary>
        /// 绑定分组下拉框
        /// </summary>
        public void BindWorkORGNode()
        {
            rs = BHelper.QueryGroupTable("BCOR_WORKORGANIZATION");
            BHelper.BindGridLookUpEdit(rs.Ds.Tables[0], txtF_WORKORGAN_TKEY);
        }

        /// <summary>
        /// 绑定下拉框的值
        /// </summary>
        public void BindGridLookUpEdit()
        {
            string sql = @"SELECT TKEY,EMPLOYEE_NAME,EMPLOYEE_CODE FROM BCOR_EMPLOYEE WHERE FLAG = 1";
            BHelper.BindGridLookUpEdit(sql,txtORGANADMIN_EMPL_TKEY);
        }

        #region 多列模糊查询
        private void txtF_WORKORGAN_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtF_WORKORGAN_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        private void txtORGANADMIN_EMPL_TKEY_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                BHelper.SetGridLookUpEditMoreColumnFilter(sender);
            }));
        }

        private void txtORGANADMIN_EMPL_TKEY_Popup(object sender, EventArgs e)
        {
            BHelper.SetGridLookUpEditMoreColumnFilter(sender);
        }

        #endregion

    }
}
