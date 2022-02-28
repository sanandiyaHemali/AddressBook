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
public partial class AddressBook_AdminPanel_Contact_ContactList : System.Web.UI.Page
{
    #region Page Lode
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            FillContactData();
        }
    }
    #endregion Page Lode

    #region gvContact : RowCommand
    protected void gvContact_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteRecord")
        {
            if (e.CommandArgument.ToString() != "")
            {
                DeleteCountry(Convert.ToInt32(e.CommandArgument.ToString().Trim()));
            }
        }
    }
    #endregion gvContact : RowCommand

    #region Fill Contact Data
    private void FillContactData()
    {
        #region Local Variables
        SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        #endregion Local Variables

        try
        {
            #region Set Connection & Commend Object
            if (objConn.State != ConnectionState.Open)
                objConn.Open();

            SqlCommand objCmd = new SqlCommand();
            objCmd.Connection = objConn;
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Contact_SelectAll";

            SqlDataReader objSDR = objCmd.ExecuteReader();
            gvContact.DataSource = objSDR;
            gvContact.DataBind();

            if (objConn.State == ConnectionState.Open)
                objConn.Close();
            #endregion Set Connection & Commend Object
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
    #endregion Fill Contact Data

    #region Delete State
    private void DeleteCountry(SqlInt32 ContactID)
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
            objCmd.CommandText = "[dbo].[PR_Contact_DeleteByPK]";
            objCmd.Parameters.AddWithValue("@ContactID", ContactID);
            objCmd.ExecuteNonQuery();

            if (objConn.State == ConnectionState.Open)
                objConn.Close();

            FillContactData();
            #endregion Set Connection & Commend Object
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
        finally
        {
            objConn.Close();
        }
    }
    #endregion Delete State
}