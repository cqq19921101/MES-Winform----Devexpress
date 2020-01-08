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

namespace ASJ.BCTE
{
    public partial class UcProcessModel : BaseUserControl
    {
        BCTEHelper BHelper = new BCTEHelper();
        //声明实体
        private BCTE_PROCESS_MODEL processmodel;


        /// <summary>
        /// 加载
        /// </summary>
        public UcProcessModel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体集成
        /// </summary>
        /// <param name="_workorg"></param>
        public UcProcessModel(BCTE_PROCESS_MODEL _processmodel) : this()
        {
            processmodel = _processmodel;
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcProcessModel_Load(object sender, EventArgs e)
        {
            txtPROMODEL_CODE.EditValue = processmodel.PROMODEL_CODE?.ToString();//编码
            txtPROMODEL_NAME.EditValue = processmodel.PROMODEL_NAME?.ToString();//名称
            chPROMODEL_PMTFLAG.EditValue = processmodel.PROMODEL_PMTFLAG.ToString() == "" ? 0 : processmodel.PROMODEL_PMTFLAG;//工序参数标记
            chPROMODEL_MEASUREFLAG.EditValue = processmodel.PROMODEL_MEASUREFLAG.ToString() == "" ? 0 : processmodel.PROMODEL_MEASUREFLAG;//工序测量标记
            chPROMODEL_PQCFLAG.EditValue = processmodel.PROMODEL_PQCFLAG.ToString() == "" ? 0 : processmodel.PROMODEL_PQCFLAG;//工序测量标记
            chPROMODEL_EQMFLAG.EditValue = processmodel.PROMODEL_EQMFLAG.ToString() == "" ? 0 : processmodel.PROMODEL_EQMFLAG;//工序设备标记
            txtCMT.EditValue = processmodel.CMT?.ToString();//备注

        }

        /// <summary>
        /// 更新方法
        /// </summary>
        /// <returns></returns>
        public BCTE_PROCESS_MODEL UpdateUI()
        {
            processmodel.PROMODEL_CODE = txtPROMODEL_CODE.EditValue?.ToString();
            processmodel.PROMODEL_NAME = txtPROMODEL_NAME.EditValue?.ToString();
            processmodel.PROMODEL_PMTFLAG = int.Parse(chPROMODEL_PMTFLAG.EditValue?.ToString());
            processmodel.PROMODEL_MEASUREFLAG = int.Parse(chPROMODEL_MEASUREFLAG.EditValue?.ToString());
            processmodel.PROMODEL_PQCFLAG = int.Parse(chPROMODEL_PQCFLAG.EditValue?.ToString());
            processmodel.PROMODEL_EQMFLAG = int.Parse(chPROMODEL_EQMFLAG.EditValue?.ToString());
            processmodel.CMT = txtCMT.EditValue?.ToString();

            return processmodel;
        }



    }
}
