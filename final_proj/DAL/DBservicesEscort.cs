﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using final_proj;
using final_proj.BL;
using final_proj.Controllers;

public class DBservicesEscort
{
    public DBservicesEscort()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public SqlConnection connect(String conString)
    {
        // Read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    public int InsertEscort(Escort escort)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateEscortInsertCommandWithStoredProcedure("SPproj_InsertEscort", con, escort);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public List<Escort> GetEscort()
    {
        SqlConnection con;
        SqlCommand cmd;
        List<Escort> escortList = new List<Escort>();

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateCommandWithSPWithoutParametersEs("SPproj_GetEscort",con);

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Escort escort = new Escort();
                escort.Esc_id = dataReader["escort_id"].ToString();
                escort.Esc_fullName = dataReader["full_name"].ToString();
                escort.Esc_dateOfBirth = Convert.ToDateTime(dataReader["birth_date"]);
                escort.Esc_cell = dataReader["cellphone"].ToString(); // Corrected to "cellphone"
                escort.Esc_city = dataReader["city"].ToString(); // Corrected to "city"
                escort.Esc_street = dataReader["street"].ToString(); // Corrected to "street"
                escort.Esc_homeNum = Convert.ToInt32(dataReader["house_number"]); // Corrected to "house_number"

                escortList.Add(escort);
            }
            return escortList;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public int DeleteEscort(string esc_id)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = DeleteEscortCommandWithStoredProcedure("SPproj_DeleteEscort", con, esc_id);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public int UpdateEscort(Escort escort)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = UpdateEscortCommandWithStoredProcedure("SPproj_UpdateEscort", con, escort);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    private SqlCommand DeleteEscortCommandWithStoredProcedure(string spName, SqlConnection con, string esc_id)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = spName;
        cmd.CommandTimeout = 10;
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@escort_id", esc_id);
        return cmd;
    }

    private SqlCommand CreateEscortInsertCommandWithStoredProcedure(string spName, SqlConnection con, Escort escort)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = spName;
        cmd.CommandTimeout = 10;
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@escort_id", escort.Esc_id);
        cmd.Parameters.AddWithValue("@full_name", escort.Esc_fullName);
        cmd.Parameters.AddWithValue("@birth_date", escort.Esc_dateOfBirth);
        cmd.Parameters.AddWithValue("@cellphone", escort.Esc_cell);
        cmd.Parameters.AddWithValue("@city", escort.Esc_city);
        cmd.Parameters.AddWithValue("@street", escort.Esc_street);
        cmd.Parameters.AddWithValue("@house_number", escort.Esc_homeNum);
        return cmd;
    }
    private SqlCommand CreateCommandWithSPWithoutParametersEs(String spName, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        return cmd;
    }

    private SqlCommand UpdateEscortCommandWithStoredProcedure(string spName, SqlConnection con, Escort escort)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = spName;
        cmd.CommandTimeout = 10;
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@escort_id", escort.Esc_id);
        cmd.Parameters.AddWithValue("@full_name", escort.Esc_fullName);
        cmd.Parameters.AddWithValue("@birth_date", escort.Esc_dateOfBirth);
        cmd.Parameters.AddWithValue("@cellphone", escort.Esc_cell);
        cmd.Parameters.AddWithValue("@city", escort.Esc_city);
        cmd.Parameters.AddWithValue("@street", escort.Esc_street);
        cmd.Parameters.AddWithValue("@house_number", escort.Esc_homeNum);
        return cmd;
    }
}
