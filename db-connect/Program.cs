using System;
using System.Data;
using System.Data.SqlClient;

namespace BasicConnection;

class Program
{
    static string ConnectionString = "Data Source=DESKTOP-B9H18JH;Database=db_hr_dts;Integrated Security=True;Connect Timeout=30;";

    static SqlConnection connection; 
    static void Main(String[] args)
    {
        connection = new SqlConnection(ConnectionString);
        try {
            connection.Open();
            Console.WriteLine("Koneksi Berhasil Dibuka!");
            connection.Close();
        } catch (Exception e) {
            Console.WriteLine(e.Message);
        }

        //GetAllData();
        //GetDataById(1);
        //InsertData("Jogja");
        //InsertData("Bandung");
        //InsertData("Ngawi");
        //InsertData("Jakarta");
        //InsertData("Bogor");
        //UpdateData(2, "Kediri");
        GetAllData();
        DeleteData(5);
        GetAllData();
    }

    static void GetAllData()
    {
        connection = new SqlConnection(ConnectionString);

        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM tbl_regions";

        connection.Open();

        using(SqlDataReader reader = command.ExecuteReader())
        {
            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    Console.WriteLine("Id : " + reader[0]);
                    Console.WriteLine("Nama : " + reader[1]);
                    Console.WriteLine("==========================");
                }
            } else
            {
                Console.WriteLine("data tidak ditemukan");
            }
            reader.Close();
        }

        connection.Close();
    }

    static void GetDataById(int id)
    {
        connection = new SqlConnection(ConnectionString);

        connection.Open();

        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM tbl_regions WHERE id = @id";

        SqlParameter param = new SqlParameter();
        param.ParameterName = "@id"; 
        param.Value = id;
        param.SqlDbType = SqlDbType.Int;

        command.Parameters.Add(param);
        using(SqlDataReader reader = command.ExecuteReader())
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine("Id : " + reader[0]);
                    Console.WriteLine("Nama : " + reader[1]);
                    Console.WriteLine("==========================");
                }
            }
            else
            {
                Console.WriteLine("data tidak ditemukan");
            }
            reader.Close();
        }
        connection.Close();

    }

    static void InsertData(string name)
    {
        connection = new SqlConnection(ConnectionString);

        connection.Open();
        SqlTransaction transaction = connection.BeginTransaction();

        try
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO tbl_regions(name) VALUES (@name)";
            command.Transaction = transaction;

            SqlParameter param = new SqlParameter();
            param.ParameterName = "@name";
            param.Value = name;
            param.SqlDbType = SqlDbType.VarChar;

            command.Parameters.Add(param);

            int result = command.ExecuteNonQuery();
            transaction.Commit();
            if(result > 0)
            {
                Console.WriteLine("data berhasil ditambahkan");
            } else
            {
                Console.WriteLine("data gagal ditambahkan");
            }

            connection.Close();

        } 
        catch(Exception ex) 
        {
            Console.WriteLine(ex.Message);
            try
            {
                transaction.Rollback();
            } 
            catch(Exception rb)
            {
                Console.WriteLine(rb.Message);
            }
        }
    }

    static void UpdateData(int id, string name)
    {
        connection = new SqlConnection(ConnectionString);

        connection.Open();
        SqlTransaction transaction = connection.BeginTransaction();

        try
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "UPDATE tbl_regions SET name=@name WHERE id=@id";
            command.Transaction = transaction;

            SqlParameter pName = new SqlParameter();
            pName.ParameterName = "@name";
            pName.Value = name;
            pName.SqlDbType = SqlDbType.VarChar;

            SqlParameter pId = new SqlParameter();
            pId.ParameterName = "@id";
            pId.Value = id;
            pId.SqlDbType = SqlDbType.Int;

            command.Parameters.Add(pName);
            command.Parameters.Add(pId);

            int result = command.ExecuteNonQuery();
            transaction.Commit();
            if(result>0)
            {
                Console.WriteLine("Data berhasil diubah");
            } else
            {
                Console.WriteLine("Data gagal diubah");
            }

            connection.Close();
        } 
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            try
            {
                transaction.Rollback();
            }
            catch (Exception rb)
            {
                Console.WriteLine(rb.Message);
            }
        }
    }

    static void DeleteData(int id)
    {
        connection = new SqlConnection(ConnectionString);

        connection.Open();
        SqlTransaction transaction = connection.BeginTransaction();

        try
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "DELETE FROM tbl_regions WHERE id = @id ";
            command.Transaction = transaction;

            SqlParameter pId = new SqlParameter();
            pId.ParameterName = "@id";
            pId.Value = id;
            pId.SqlDbType = SqlDbType.Int;

            command.Parameters.Add(pId);

            int result = command.ExecuteNonQuery();
            transaction.Commit();
            if (result>0)
            {
                Console.WriteLine("Data telah dihapus");
            } else
            {
                Console.WriteLine("Data gagal dihapus");
            }
            connection.Close();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            try
            {
                transaction.Rollback();
            }
            catch (Exception rb)
            {
                Console.WriteLine(rb.Message);
            }
        }
    }
}