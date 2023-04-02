using System;
//Library Koneksi Database : Sysytem.Data dan System.Data.SqlClient
using System.Data;
using System.Data.SqlClient;

namespace BasicConnection;

class Program
{
    //Deklarasi ConnectionString berisi variabel-variabel yang digunakan untuk menyambungkan project dengan database
    static string ConnectionString = "Data Source=DESKTOP-B9H18JH;Database=db_hr_dts;Integrated Security=True;Connect Timeout=30;";

    //Membuat variable koneksi menggunakan SqlConnection
    static SqlConnection connection; 
    static void Main(String[] args)
    {
        //Membuat variabel objek connection
        connection = new SqlConnection(ConnectionString);
        //Melakukan test koneksi program dengan database
        try {
            //Membuka koneksi
            connection.Open();
            Console.WriteLine("Koneksi Berhasil Dibuka!");
            //Menutup koneksi
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

    //Fungsi menampilkan semua data dalam database
    static void GetAllData()
    {
        //Deklarasi koneksi
        connection = new SqlConnection(ConnectionString);

        //Membuat SqlCommand object : memuat perintah SQL
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM tbl_regions";

        //Membuka koneksi
        connection.Open();

        //Menampilkan hasil pembacaan data database
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

        //Menutup koneksi
        connection.Close();
    }

    //Fungsi menampilkan data berdasarjan id
    static void GetDataById(int id)
    {
        connection = new SqlConnection(ConnectionString);

        //Membuka koneksi
        connection.Open();

        //Deklarasi SqlCommand object
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM tbl_regions WHERE id = @id";

        //Deklarasi parameter yang dibawa fungsi untuk dapat diolah oleh SQL syntax
        SqlParameter param = new SqlParameter();
        param.ParameterName = "@id"; 
        param.Value = id;
        param.SqlDbType = SqlDbType.Int;

        //Menambahkan parameter
        command.Parameters.Add(param);

        //Membaca database kemudian menampilkan hasil
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

        //Menutup konksi
        connection.Close();

    }

    //Fungsi memasukkan data ke dalam database
    static void InsertData(string name)
    {
        connection = new SqlConnection(ConnectionString);

        //Membuka koneksi
        connection.Open();
        //Deklarasi SqlTransaction object
        SqlTransaction transaction = connection.BeginTransaction();

        try
        {
            //Membuat variable object dari class Sqlcommand
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO tbl_regions(name) VALUES (@name)";
            command.Transaction = transaction;

            //Mambuat variable object dari class SqlParameter 
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@name";
            param.Value = name;
            param.SqlDbType = SqlDbType.VarChar;

            //Menambahkan nilai variable ke dalam collection
            command.Parameters.Add(param);

            //Mengecek berhasil atau tidak perintah dilakukan
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
            //Handling saat terjadi error dalam proses SQL
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

    //Fungsi update data dalam database
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

    //Fungsi menghapus data dalam database
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