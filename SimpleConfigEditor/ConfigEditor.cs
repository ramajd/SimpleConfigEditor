using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace SimpleConfigEditor
{
    public class ConfigEditor
    {
        public string ConfigPath { get; set; }

        /// <summary>
        /// Constructor for ConfigEditor Class
        /// </summary>
        /// <param name="cfgPath">Path to Config File</param>
        public ConfigEditor(string cfgPath)
        {
            this.ConfigPath = cfgPath;

            if (!File.Exists(ConfigPath))
            {
                Set("", "");
            }
        }

        /// <summary>
        /// Get Value Of a key from config file
        /// </summary>
        /// <param name="KEY">the key that we want its value</param>
        /// <returns>value of the key if exists, else returns empty string.</returns>
        public string Get(string KEY)
        {
            return Get(KEY, "");
        }

        /// <summary>
        ///  Get Value Of a key from config file
        /// </summary>
        /// <param name="KEY">>the key that we want its value</param>
        /// <param name="DefaultVal">default value that returns if the key not exists.</param>
        /// <returns>value of the key if exists, else returns default value.</returns>
        public string Get(string KEY, string DefaultVal)
        {
            if (!File.Exists(ConfigPath))
            {
                Set(KEY, DefaultVal);
                return DefaultVal;
            }
            else
            {
                string result = "";

                DataTable dt = new DataTable("Configs");
                dt.ReadXml(ConfigPath);

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Key"].ToString() == KEY)
                    {
                        result = dr["Value"].ToString();
                        return result;
                    }
                }
                Set(KEY, DefaultVal);
            }
            return DefaultVal;
        }

        /// <summary>
        /// Set Value Of a Key in Config File.
        /// </summary>
        /// <param name="KEY">Key Name in config file</param>
        /// <param name="Val">Key Value in config file</param>
        /// <returns>true if the operation successful, otherwise false</returns>
        public bool Set(string KEY, string Val)
        {
            bool result = false;
            try
            {
                #region [Create  Empty Config File]

                if (!File.Exists(ConfigPath))
                {
                    DataTable dtEmptyConfig = new DataTable("Configs");
                    dtEmptyConfig.Columns.Add("Key");
                    dtEmptyConfig.Columns.Add("Value");

                    DataRow dr = dtEmptyConfig.NewRow();
                    dr[0] = dr[1] = "";
                    dtEmptyConfig.Rows.Add(dr);
                    dtEmptyConfig.WriteXml(ConfigPath, XmlWriteMode.WriteSchema, false);
                }

                #endregion
                //==========================================


                if (KEY == "")
                {
                    #region [Just Create Empty Config]

                    return true;

                    #endregion
                }
                else
                {
                    #region [Add New Value To KEY]

                    string found = "";
                    DataTable dt = new DataTable("Configs");

                    dt.ReadXml(ConfigPath);

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["Key"].ToString() == KEY)
                        {
                            dr["Value"] = Val;
                            found = "1";
                            result = true;

                            break;
                        }
                    }//end for

                    if (found != "1")
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = KEY;
                        dr[1] = Val;
                        dt.Rows.Add(dr);
                        result = true;
                    }//end if

                    File.Delete(ConfigPath);
                    dt.AcceptChanges();
                    dt.WriteXml(ConfigPath, XmlWriteMode.WriteSchema, false);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return result;
        }

        /// <summary>
        /// Delete a specific Key & its Value From Config File
        /// </summary>
        /// <param name="KEY">Key that we want to delete</param>
        /// <returns>true is key delete succesfully, otherwise false.</returns>
        public bool Delete(string KEY)
        {
            bool result = false;

            try
            {
                if (!File.Exists(ConfigPath))
                    return true;
                else
                {
                    DataTable dt1 = new DataTable("Configs");
                    DataTable dt2 = new DataTable("Configs");
                    dt2.Columns.Add("Key");
                    dt2.Columns.Add("Value");

                    dt1.ReadXml(ConfigPath);

                    foreach (DataRow dr in dt1.Rows)
                    {
                        if (dr["Key"].ToString() != KEY)
                        {
                            DataRow newRow = dt2.NewRow();
                            newRow["Key"] = dr["Key"];
                            newRow["Value"] = dr["Value"];
                            dt2.Rows.Add(newRow);
                        }
                    }//end foreach

                    File.Delete(ConfigPath);
                    dt2.AcceptChanges();
                    dt2.WriteXml(ConfigPath, XmlWriteMode.WriteSchema, false);
                    result = true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }

            return result;
        }


        public string[] AllKeys
        {
            get
            {
                try
                {
                    if (!File.Exists(ConfigPath))
                    {
                        return new string[0];
                    }
                    else
                    {
                        DataTable dt = new DataTable("Configs");
                        dt.ReadXml(ConfigPath);

                        List<string> keyList = new List<string>();

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["key"].ToString().Trim() != "")
                                keyList.Add(dr["key"].ToString());
                        }

                        return keyList.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    return new string[0];
                }
            }
        }

    }
}
