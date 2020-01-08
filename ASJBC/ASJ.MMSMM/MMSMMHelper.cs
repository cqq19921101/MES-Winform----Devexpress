using ASJ.TOOLS.Basic;
using ASJ.TOOLS.Data;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ASJ.MMSMM
{
    public class MMSMMHelper : BC_Standard
    {

        /// <summary>
        /// 绑定下拉框控件的值 节点 LookUpEdit  
        /// </summary>
        /// <param name="Control">控件名</param>
        /// <param name="enumtype">枚举</param>
        /// <param name="DisplayMember">DisplayMember</param>
        /// <param name="ValueMember">ValueMember</param>
        public void BindNode(DevExpress.XtraEditors.LookUpEdit Control, Type enumtype, string DisplayMember, string ValueMember)
        {
            DataTable dtNode = ASJ.TOOLS.Data.DataHelper.EnumToDataTable(enumtype, DisplayMember, ValueMember);
            Control.Properties.DataSource = dtNode;
            Control.Properties.DisplayMember = DisplayMember;
            Control.Properties.ValueMember = ValueMember;
        }



        /// <summary>
        /// 根据计量单位组KEY 查找是否存在该组的计量单位数据 （一个计量单位分组只有一个基准单位）
        /// </summary>
        /// <param name="GRPTKEY">计量单位组Key UNIT_GRP_TKEY </param>
        /// <returns> true :没有数据 基准单位框选中 反灰    false : 有数据 复选框不选中 反灰  </returns>
        public bool CheckBaseUnit(string  GRPTKEY)
        {
            string Sql = @"SELECT * FROM BCDF_UNIT WHERE FLAG = 1 AND UNIT_GRP_TKEY = " + "'" + GRPTKEY + "'";
            DataSet ds = OracleHelper.Query(Sql);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 捞取分组表中的所有的数据
        /// </summary>
        /// <param name="DBNAME"></param>
        /// <returns></returns>
        public Result QueryGroupTable(string DBNAME)
        {
            Result rs = new Result();
            string sql = @"SELECT * FROM  " + "'" + DBNAME + "'" + " WHERE FLAG = 1";
            DataSet ds = OracleHelper.Query(sql);
            rs.Ds = ds;
            rs.Msg = "Success";
            rs.Status = true;

            return rs;

        }

        /// <summary>
        /// GridView DataSource查询绑定 BCMA_MATERIAL MMSMM_PURCHASE_REQ_D 连接查询
        /// </summary>
        /// <returns></returns>
        public Result QueryBindGridView()
        {
            Result rs = new Result();
            string sql = @"select T1.*,T2.TKEY,T2.MATERIAL_CODE,T2.MATERIAL_NAME,T2.MAPID,T2.BASE_UNIT_TKEY from MMSMM_PURCHASE_REQ_D T1 
                            left join BCMA_MATERIAL T2 ON T1.MATERIAL_TKEY = T2.TKEY AND T1.FLAG = T2.FLAG
                            WHERE T1.FLAG = 1";
            DataSet ds = OracleHelper.Query(sql);
            rs.Ds = ds;
            rs.Msg = "Success";
            rs.Status = true;
            return rs;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public Result QueryMaterialMaster()
        {
            Result rs = new Result();
            string sql = @"select T1.*,T2.TKEY,T2.MATERIAL_CODE,T2.MATERIAL_NAME,T2.MAPID,T2.BASE_UNIT_TKEY from MMSMM_PURCHASE_REQ_D T1 
                            left join BCMA_MATERIAL T2 ON T1.MATERIAL_TKEY = T2.TKEY AND T1.FLAG = T2.FLAG
                            WHERE T1.FLAG = 1";
            DataSet ds = OracleHelper.Query(sql);
            rs.Ds = ds;
            rs.Msg = "Success";
            rs.Status = true;
            return rs;
        }



    }
}
