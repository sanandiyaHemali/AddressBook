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
public partial class AddressBook_AdminPanel_Contact_ContactAddEdit : System.Web.UI.Page
{
    #region Page Lode
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            FillCountryDropDownList();
            FillStateDropDownList();
            FillCityDropDownList();
            FillContactCategoryDropDownList();
            if (Request.QueryString["ContactID"] != null)
            {
                lblHeader.Text = "Edit Contact | ContactID = " + Request.QueryString["ContactID"].ToString();
                FillControls(Convert.ToInt32(Request.QueryString["ContactID"]));
            }
            else
            {
                lblHeader.Text = "Add Contact";
            }
        }
    }
    #endregion Page Lode

    #region Button : Save
    protected void btnSave_Click(object sender, EventArgs e)
    {
        addContactData();
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
            if (objConn.State != ConnectionState.Open)
                objConn.Close();
        }
    }
    #endregion Fill Country DropDown List

    #region Fill State DropDown List
    private void FillStateDropDownList()
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
            objCmd.CommandText = "PR_State_SelectForDropDownList";
            SqlDataReader objSDR = objCmd.ExecuteReader();
            #endregion Set Connection & Commend Object

            #region Fill Data Value In DropDownList
            if (objSDR.HasRows == true)
            {
                ddlState.DataSource = objSDR;
                ddlState.DataValueField = "StateID";
                ddlState.DataTextField = "StateName";
                ddlState.DataBind();
            }

            ddlState.Items.Insert(0, new ListItem("Select State", "-1"));
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
    #endregion Fill State DropDown List

    #region Fill City DropDown List
    private void FillCityDropDownList()
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
            objCmd.CommandText = "PR_City_SelectForDropDownList";
            SqlDataReader objSDR = objCmd.ExecuteReader();
            #endregion Set Connection & Commend Object

            #region Fill Data Value In DropDownList
            if (objSDR.HasRows == true)
            {
                ddlCity.DataSource = objSDR;
                ddlCity.DataValueField = "CityID";
                ddlCity.DataTextField = "CityName";
                ddlCity.DataBind();
            }

            ddlCity.Items.Insert(0, new ListItem("Select City", "-1"));
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
    #endregion Fill City DropDown List

    #region Fill ContactCategory DropDown List
    private void FillContactCategoryDropDownList()
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
            objCmd.CommandText = "PR_ContactCategory_SelectForDropDownList";
            SqlDataReader objSDR = objCmd.ExecuteReader();
            #endregion Set Connection & Commend Object

            #region Fill Data Value In DropDownList
            if (objSDR.HasRows == true)
            {
                ddlContactCategory.DataSource = objSDR;
                ddlContactCategory.DataValueField = "ContactCategoryID";
                ddlContactCategory.DataTextField = "ContactCategoryName";
                ddlContactCategory.DataBind();
            }

            ddlContactCategory.Items.Insert(0, new ListItem("Select ContactCategory", "-1"));
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
    #endregion Fill ContactCategory DropDown List

    #region Add Contact Data
    private void  addContactData()
    {
        #region Local Variables
        SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        SqlInt32 StrCountryId = SqlInt32.Null;
        SqlInt32 StrStateId = SqlInt32.Null;
        SqlInt32 StrCityId = SqlInt32.Null;
        SqlInt32 StrContactCategoryId = SqlInt32.Null;
        SqlString StrContactName = SqlString.Null;
        SqlString StrContactNo = SqlString.Null;
        SqlString StrWhatsAppNo = SqlString.Null;
        SqlDateTime StrBirthDate = SqlDateTime.Null;
        SqlString StrEmail = SqlString.Null;
        SqlInt32 StrAge = SqlInt32.Null;
        SqlString StrAddress = SqlString.Null;
        SqlString StrBloodGroup = SqlString.Null;
        SqlString StrFacebookID = SqlString.Null;
        SqlString StrLinkedINID = SqlString.Null;
        #endregion Local Variables

        try
        {
            #region Server Side Validation
            string StrErrorMessage = "";

            if (ddlCountry.SelectedIndex == 0)
            {
                StrErrorMessage += "- Select Country <br /><br />";
            }
            if (ddlState.SelectedIndex == 0)
            {
                StrErrorMessage += "- Select State <br /><br />";
            }
            if (ddlCity.SelectedIndex == 0)
            {
                StrErrorMessage += "- Select City <br /><br />";
            }
            if (ddlContactCategory.SelectedIndex == 0)
            {
                StrErrorMessage += "- Select Contact Category <br /><br />";
            }
            if (txtContactName.Text.Trim() == "")
            {
                StrErrorMessage += "- Enter Contact Name <br /><br />";
            }
            if (txtContactNo.Text.Trim() == "")
            {
                StrErrorMessage += "- Enter Contact No. <br /><br />";
            }
            if (txtEmail.Text.Trim() == "")
            {
                StrErrorMessage += "- Enter Email ID <br /><br />";
            }
            if (txtAddress.Text.Trim() == "")
            {
                StrErrorMessage += "- Enter Address <br /><br />";
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
            if (ddlState.SelectedIndex > 0)
                StrStateId = Convert.ToInt32(ddlState.SelectedValue);
            if (ddlCity.SelectedIndex > 0)
                StrCityId = Convert.ToInt32(ddlCity.SelectedValue);
            if (ddlContactCategory.SelectedIndex > 0)
                StrContactCategoryId = Convert.ToInt32(ddlContactCategory.SelectedValue);
            if (txtContactName.Text.Trim() != "")
                StrContactName = txtContactName.Text.Trim();
            if (txtContactNo.Text.Trim() != "")
                StrContactNo = txtContactNo.Text.Trim();
            if (txtWhatsAppNo.Text.Trim() != "")
                StrWhatsAppNo = txtWhatsAppNo.Text.Trim();
            if (txtBirthDate.Text.Trim() != "")
                StrBirthDate = Convert.ToDateTime(txtBirthDate.Text.Trim());
            if (txtEmail.Text.Trim() != "")
                StrEmail = txtEmail.Text.Trim();
            if (txtAge.Text.Trim() != "")
                StrAge = Convert.ToInt32(txtAge.Text.Trim());
            if (txtAddress.Text.Trim() != "")
                StrAddress = txtAddress.Text.Trim();
            if (txtBloodGroup.Text.Trim() != "")
                StrBloodGroup = txtBloodGroup.Text.Trim();
            if (txtFacebookID.Text.Trim() != "")
                StrFacebookID = txtFacebookID.Text.Trim();
            if (txtLinkedINID.Text.Trim() != "")
                StrLinkedINID = txtLinkedINID.Text.Trim();
            #endregion Gather Information

            #region Set Connection & Commend Object
            if (objConn.State != ConnectionState.Open)
                objConn.Open();

            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.AddWithValue("@CountryID", StrCountryId);
            objCmd.Parameters.AddWithValue("@StateID", StrStateId);
            objCmd.Parameters.AddWithValue("@CityID", StrCityId);
            objCmd.Parameters.AddWithValue("@ContactCategoryID", StrContactCategoryId);
            objCmd.Parameters.AddWithValue("@ContactName", StrContactName);
            objCmd.Parameters.AddWithValue("@ContactNo", StrContactNo);
            objCmd.Parameters.AddWithValue("@WhatsAppNo", StrWhatsAppNo);
            objCmd.Parameters.AddWithValue("@BirthDate", StrBirthDate);
            objCmd.Parameters.AddWithValue("@Email", StrEmail);
            objCmd.Parameters.AddWithValue("@Age", StrAge);
            objCmd.Parameters.AddWithValue("@Address", StrAddress);
            objCmd.Parameters.AddWithValue("@BloodGroup", StrBloodGroup);
            objCmd.Parameters.AddWithValue("@FacebookID", StrFacebookID);
            objCmd.Parameters.AddWithValue("@LinkedINID", StrLinkedINID);
            #endregion Set Connection & Commend Object

            if (Request.QueryString["ContactID"] != null)
            {
                #region Update Record
                objCmd.CommandText = "PR_Contact_UpdateByPK";
                objCmd.Parameters.AddWithValue("ContactID", Request.QueryString["ContactID"].ToString().Trim());
                objCmd.ExecuteNonQuery();
                Response.Redirect("~/AdminPanel/Contact/ContactList.aspx");
                #endregion Update Record
            }
            else
            {
                #region Insert Record
                objCmd.CommandText = "PR_Contact_Insert";
                objCmd.ExecuteNonQuery();
                lblMessage.Text = "Data Inserted Successfully <br /><br />";
                ddlCountry.SelectedIndex = 0;
                ddlState.SelectedIndex = 0;
                ddlCity.SelectedIndex = 0;
                ddlContactCategory.SelectedIndex = 0;
                txtContactName.Text = "";
                txtContactNo.Text = "";
                txtWhatsAppNo.Text = "";
                txtBirthDate.Text = "";
                txtEmail.Text = "";
                txtAge.Text = "";
                txtAddress.Text = "";
                txtBloodGroup.Text = "";
                txtFacebookID.Text = "";
                txtLinkedINID.Text = "";
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
    #endregion Add Contact Data

    #region Fill Controls
    private void FillControls(SqlInt32 ContactID)
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
            objCmd.CommandText = "PR_Contact_SelectByPK";
            objCmd.Parameters.AddWithValue("@ContactID", ContactID.ToString().Trim());
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
                    if (!objSDR["StateID"].Equals(DBNull.Value))
                    {
                        ddlState.SelectedValue = objSDR["StateID"].ToString().Trim();
                    }
                    if (!objSDR["CityID"].Equals(DBNull.Value))
                    {
                        ddlCity.SelectedValue = objSDR["CityID"].ToString().Trim();
                    }
                    if (!objSDR["ContactCategoryID"].Equals(DBNull.Value))
                    {
                        ddlContactCategory.SelectedValue = objSDR["ContactCategoryID"].ToString().Trim();
                    }
                    if (!objSDR["ContactName"].Equals(DBNull.Value))
                    {
                        txtContactName.Text = objSDR["ContactName"].ToString().Trim();
                    }
                    if (!objSDR["ContactNo"].Equals(DBNull.Value))
                    {
                        txtContactNo.Text = objSDR["ContactNo"].ToString().Trim();
                    }
                    if (!objSDR["ContactNo"].Equals(DBNull.Value))
                    {
                        txtContactNo.Text = objSDR["ContactNo"].ToString().Trim();
                    } 
                    if (!objSDR["WhatsAppNo"].Equals(DBNull.Value))
                    {
                        txtWhatsAppNo.Text = objSDR["WhatsAppNo"].ToString().Trim();
                    }
                    if (!objSDR["BirthDate"].Equals(DBNull.Value))
                    {
                        txtBirthDate.Text = objSDR["BirthDate"].ToString().Trim();
                    }
                    if (!objSDR["Email"].Equals(DBNull.Value))
                    {
                        txtEmail.Text = objSDR["Email"].ToString().Trim();
                    }
                    if (!objSDR["Age"].Equals(DBNull.Value))
                    {
                        txtAge.Text = objSDR["Age"].ToString().Trim();
                    }
                    if (!objSDR["Address"].Equals(DBNull.Value))
                    {
                        txtAddress.Text = objSDR["Address"].ToString().Trim();
                    } 
                    if (!objSDR["BloodGroup"].Equals(DBNull.Value))
                    {
                        txtBloodGroup.Text = objSDR["BloodGroup"].ToString().Trim();
                    }
                    if (!objSDR["FacebookID"].Equals(DBNull.Value))
                    {
                        txtFacebookID.Text = objSDR["FacebookID"].ToString().Trim();
                    }
                    if (!objSDR["LinkedINID"].Equals(DBNull.Value))
                    {
                        txtLinkedINID.Text = objSDR["LinkedINID"].ToString().Trim();
                    }
                    break;
                }
            }
            else
            {
                lblMessage.Text = "No data available for the ContactID = " + ContactID.ToString();
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