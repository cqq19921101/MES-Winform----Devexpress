using ASJ.TOOLS.Basic;
using ASJ.TOOLS.Data;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ASJ.BCMA
{
    public  class BCMAHelper : BC_Standard
    {
        Result rs = new Result();
        #region 物料管理
        public void InsertNewRow(DataSet ds, string DBNAME)
        {
            ds.Tables[DBNAME].NewRow();
            ds.Tables[DBNAME].Rows.Add();
        }

        public List<string> Getlsttkey(string MasterTkey,List<string> strsql)
        {
            List<string> lsttkey = new List<string>();
            if (MasterTkey != null)
            {
                for (int i = 0; i < strsql.Count; i++)
                {


                }
                return lsttkey;
            }

            return null;
        }

        public string GetMappingTKEY(string DBNAME)
        {
            string strsql = $@"Select TKEY,MATERIAL_TKEY FROM {DBNAME} WHERE FLAG = 1";
            DataSet ds  = OracleHelper.Query(strsql);
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0] != null) return ds.Tables[0].Rows[0]["TKEY"].ToString();
            return null;
        }
        #endregion
    }
}
