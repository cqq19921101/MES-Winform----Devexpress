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
using DevExpress.Utils;

namespace ASJ.BCOR
{
    /// <summary>
    /// 工作班次
    /// </summary>
    public partial class UcWorkShift : BaseUserControl
    {
        ASJBCOR_ORG BHelper = new ASJBCOR_ORG();
        //声明实体
        private BCOR_WORKSHIFT workshift;


        /// <summary>
        /// 加载
        /// </summary>
        public UcWorkShift()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_workorg"></param>
        public UcWorkShift(BCOR_WORKSHIFT _workshift) : this()
        {
            workshift = _workshift;
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcWorkShift_Load(object sender, EventArgs e)
        {
            txtWORKSHIFT_CODE.EditValue = workshift.WORKSHIFT_CODE?.ToString();//编码
            txtWORKSHIFT_NAME.EditValue = workshift.WORKSHIFT_NAME?.ToString();//名称
            txtSTART_TIME.EditValue = workshift.START_TIME?.ToString();//开始时间
            txtFINISH_TIME.EditValue = workshift.FINISH_TIME?.ToString();//结束时间
            txtCMT.EditValue = workshift.CMT?.ToString();//备注
        }

        /// <summary>
        /// 更新方法
        /// </summary>
        /// <returns></returns>
        public BCOR_WORKSHIFT UpdateUI()
        {
            workshift.WORKSHIFT_CODE = txtWORKSHIFT_CODE.EditValue?.ToString();
            workshift.WORKSHIFT_NAME = txtWORKSHIFT_NAME.EditValue?.ToString();
            workshift.START_TIME = txtSTART_TIME.EditValue?.ToString();
            workshift.FINISH_TIME = txtFINISH_TIME.EditValue?.ToString();
            workshift.CMT = txtCMT.EditValue?.ToString();

            return workshift;
        }


    }
}
