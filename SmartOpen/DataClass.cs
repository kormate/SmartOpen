using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;


namespace SmartOpen
{
    public class DataClass
    {
        private string connectionString;

        public DataClass()
        {
            this.connectionString = "Server=localhost;Port=3306;Database=smartopen;User Id=root";
        }

        public List<Dolgozo> GetDataFromDataBase()
        {
            List<Dolgozo> dolgozok = new List<Dolgozo>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT fname AS 'Név', fmail AS 'Email', fmunkakor AS 'Munkakör', fstatusz AS 'Státusz', fpw AS 'Jelszó', fsalt FROM felhasznalok";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Dolgozo felhasznalok = new Dolgozo();
                    felhasznalok.fname = reader.GetString(0);
                    felhasznalok.fmail = reader.GetString(1);
                    felhasznalok.fmunkakor = reader.GetString(2);
                    felhasznalok.fstatusz = reader.GetString(3);
                    felhasznalok.fpw = reader.GetString(4);
                    felhasznalok.fsalt = reader.GetString(5);
                    


                    dolgozok.Add(felhasznalok);
                }
                reader.Close();
            }
            return dolgozok;
        }

        public void AddEmps(Dolgozo ujDolgozo)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO felhasznalok (fname, fmail, fmunkakor, fpw, fsalt) VALUES (@fname, @fmail, @fmunkakor, @fpw, @fsalt)";

                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@fname", ujDolgozo.fname);
                cmd.Parameters.AddWithValue("@fmail", ujDolgozo.fmail);
                cmd.Parameters.AddWithValue("@fmunkakor", ujDolgozo.fmunkakor);
                cmd.Parameters.AddWithValue("@fpw", ujDolgozo.fpw);
                cmd.Parameters.AddWithValue("@fsalt", ujDolgozo.fsalt);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public class PwHasher
        {
            private static readonly string Chars = "abcdefghijklmnopqrstuvWxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789./";
            private static readonly Random random = new Random();

            public static string GenSalt(int lenght)
            {
                var salt = new StringBuilder();
                for (int i = 0; i < lenght; i++)
                {
                    salt.Append(Chars[random.Next(Chars.Length)]);
                }
                return salt.ToString();
            }

            public static string HashPw(string password, string salt)
            {
                using (var sha512 = SHA512.Create())
                {
                    var saltedPw = password + salt;
                    var bytes = Encoding.UTF8.GetBytes(saltedPw);
                    var hashedBytes = sha512.ComputeHash(bytes);
                    return Convert.ToBase64String(hashedBytes);
                }
            }

            public static bool VerifyPw(string password, string salt, string HashedPw)
            {
                var newHashedPw = HashPw(password, salt);
                return newHashedPw == HashedPw;
            }
        }


        public class Dolgozo
        {

            public string fname { get; set; }
            public string fmail { get; set; }
            public string fmunkakor { get; set; }
            public string fpw { get; set; }
            public string fstatusz { get; set; }
            public string fsalt { get; set; }
        }
    }
}
