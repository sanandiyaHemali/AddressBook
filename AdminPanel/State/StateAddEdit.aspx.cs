using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class AddressBook_AdminPanel_State_StateAddEdit : System.Web.UI.Page
{
    #region Page Lode
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!Page.IsPostBack)
        {
            FillCountryDropDownList();
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["StateID"] != null)
                {
                    lblHeader.Text = "Edit State | StateID = " + Request.QueryString["StateID"].ToString();
                    FillControls(Convert.ToInt32(Request.QueryString["StateID"].ToString()));
                }
                else
                {
                    lblHeader.Text = "Add State";
                }
            }
        }
    }
    #endregion Page Lode

    #region Button : Save
    protected void btnSave_Click(object sender, EventArgs e)
    {
        addStateData();
    }
    #endregion Button : Save

    #region Fill Country DropDown List
    private void FillCountryDropDownList()
    {
        #region Local Variables
        SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        #endregion Local Variables

        try
        {
            #region Set Connection & Commend Object
            if (objConn.State != ConnectionState.Open)
                objConn.Open();

            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Country_SelectForDropDownList";
            SqlDataReader objSDR = objCmd.ExecuteReader();
            #endregion Set Connection & Commend Object

            #region Fill Data Value In DropDownList
            if (objSDR.HasRows == true)
            {
                ddlCountry.DataSource = objSDR;
                ddlCountry.DataValueField = "CountryID";
                ddlCountry.DataTextField = "CountryName";
                ddlCountry.DataBind();
            }

            ddlCountry.Items.Insert(0, new ListItem("Select Country", "-1"));
            #endregion Fill Data Value In DropDownList

            if (objConn.State == ConnectionState.Open)
                objConn.Close();
        }

        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }

        finally
        {
            if (objConn.State == ConnectionState.Open)
                objConn.Close();
        }
        
    }
    #endregion Fill Country DropDown List

    #region Add State Data
    private void addStateData()
    {
        #region Local Variables
        SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        SqlInt32 StrCountryId = SqlInt32.Null;
        SqlString StrStateName = SqlString.Null;
        SqlString StrStateCode = SqlString.Null;
        #endregion Local Variables

        try
        {
            #region Server Side Validation
            string StrErrorMessage = "";

            if (ddlCountry.SelectedIndex == 0)
            {
                StrErrorMessage += "- Select Country <br /><br />";
            }
            if (txtStateName.Text.Trim() == "")
            {
                StrErrorMessage += "- Enter State Name <br /><br />";
            }
            if (StrErrorMessage != "")
            {
                lblMessage.Text = StrErrorMessage;
                return;
            }
            #endregion Server Side Validation

            #region Gather Information
            if (ddlCountry.SelectedIndex > 0)
                StrCountryId = Convert.ToInt32(ddlCountry.SelectedValue);
            if (txtStateName.Text.Trim() != "")
                StrStateName = txtStateName.Text.Trim();
            if (txtStateCode.Text.Trim() != "")
                StrStateCode = txtStateCode.Text.Trim();
            #endregion Gather Information

            #region Set Connection & Commend Object
            if (objConn.State != ConnectionState.Open)
                objConn.Open();

            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.AddWithValue("@CountryID", StrCountryId);
            objCmd.Parameters.AddWithValue("@StateName", StrStateName);
            objCmd.Parameters.AddWithValue("@StateCode", StrStateCode);
            #endregion Set Connection & Commend Object

            if (Request.QueryString["StateID"] != null)
            {
                #region Update Record
                objCmd.Parameters.AddWithValue("StateId", Request.QueryString["StateID"].ToString().Trim());
                objCmd.CommandText = "PR_State_UpdateByPK";
                objCmd.ExecuteNonQuery();
                Response.Redirect("~/AdminPanel/State/StateList.aspx");
                #endregion Update Record
            }
            else
            {
                #region Insert Record
                objCmd.CommandText = "PR_State_Insert";
                objCmd.ExecuteNonQuery();
                lblMessage.Text = "Data Inserted Successfully <br /><br />";
                ddlCountry.SelectedIndex = 0;
                txtStateName.Text = "";
                txtStateCode.Text = "";
                ddlCountry.Focus();
                #endregion Insert Record
            }
            if (objConn.State == ConnectionState.Open)
                objConn.Close();

        }

        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }

        finally
        {
            if (objConn.State == ConnectionState.Open)
                objConn.Close();
        }
    }
    #endregion Add State Data

    #region Fill Controls
    private void FillControls(SqlInt32 StateID)
    {
        #region Local Variables
        SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        #endregion Local Variables

        try
        {
            #region Set Connection & Commend Object
            if (objConn.State != ConnectionState.Open)
                objConn.Open();

            SqlCommand objCmd = objConn.CreateCommand();

            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_State_SelectByPK";
            objCmd.Parameters.AddWithValue("@StateID", StateID.ToString().Trim());
            #endregion Set Connection & Commend Object

            #region Read the value and set the controls
            SqlDataReader objSDR = objCmd.ExecuteReader();
            if (objSDR.HasRows)
            {
                while (objSDR.Read())
                {
                    if (!objSDR["CountryID"].Equals(DBNull.Value))
                    {
                        ddlCountry.SelectedValue = objSDR["CountryID"].ToString().Trim();
                    }
                    if (!objSDR["StateName"].Equals(DBNull.Value))
                    {
                        txtStateName.Text = objSDR["StateName"].ToString().Trim();
                    }
                    if (!objSDR["StateCode"].Equals(DBNull.Value))
                    {
                        txtStateCode.Text = objSDR["StateCode"].ToString().Trim();
                    }
                    break;
                }
            }
            else
            {
                lblMessage.Text = "No data available for the StateID = " + StateID.ToString();
            }
            #endregion Read the value and set the controls

            if (objConn.State == ConnectionState.Open)
                objConn.Close();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }

        finally
        {
            if (objConn.State == ConnectionState.Open)
                objConn.Close();
        }
    }
    #endregion Fill Controls
}