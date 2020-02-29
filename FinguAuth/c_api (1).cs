using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace c_auth
{
    class c_api
    {
        public static string program_key { get; set; }

        public static string enc_key { get; set; }

        private static string api_link = "https://firefra.me/auth/api/"; //maybe you'll make your own auth based on mine

        private static string user_agent = "Mozilla FireFrame"; //my ddos protection needs Mozilla in front

        public static void c_init(string c_version)
        {
            try
            {
                WebClient web = new WebClient();
                web.Headers["User-Agent"] = user_agent;
                web.Proxy = null;

                string result = web.DownloadString(api_link + "init.php?version=" + c_version + "&program_key=" + program_key);

                if (result == "program_doesnt_exist")
                {
                    MessageBox.Show("the program doesnt exist");
                    Environment.Exit(0);
                }
                else if (result == c_encryption.encrypt("wrong_version"))
                {
                    MessageBox.Show("wrong program version");
                    Environment.Exit(0);
                }
                else if (result == c_encryption.encrypt("started_program"))
                {
                    //guess 
                }
                else
                {
                    MessageBox.Show("invalid encryption key");
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Environment.Exit(0);
            }
        }
        public static bool c_login(string c_username, string c_password, string c_hwid = "default")
        {
            if (c_hwid == "default") c_hwid = WindowsIdentity.GetCurrent().User.Value;

            try
            {
                WebClient web = new WebClient();
                web.Headers["User-Agent"] = user_agent;
                web.Proxy = null;

                string result = c_encryption.decrypt(web.DownloadString(api_link + "login.php?program_key=" + program_key + "&username=" + c_username + "&password=" + c_password + "&hwid=" + c_hwid));

                if(result == "invalid_username")
                {
                    MessageBox.Show("invalid username");
                    return false;
                }
                else if (result == "invalid_password")
                {
                    MessageBox.Show("invalid password");
                    return false;
                }
                else if (result == "no_sub")
                {
                    MessageBox.Show("no sub");
                    return false;
                }
                else if (result == "invalid_hwid")
                {
                    MessageBox.Show("invalid hwid");
                    return false;
                }
                else if (result.Contains("logged_in"))
                {
                    string[] s = result.Split('|');

                    c_userdata.username = s[1];
                    c_userdata.email = s[2];
                    c_userdata.expires = c_encryption.unix_to_date(Convert.ToDouble(s[3]));
                    c_userdata.rank = Convert.ToInt32(s[4]);

                    shit_pass = c_encryption.encrypt(c_password);

                    MessageBox.Show("logged in!");
                    return true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Environment.Exit(0);
                return false;
            }
            return true;
        }
        public static bool c_register(string c_username, string c_email, string c_password, string c_token, string c_hwid = "default")
        {
            if (c_hwid == "default") c_hwid = WindowsIdentity.GetCurrent().User.Value;

            try
            {
                WebClient web = new WebClient();
                web.Headers["User-Agent"] = user_agent;
                web.Proxy = null;

                string result = c_encryption.decrypt(web.DownloadString(api_link + "register.php?program_key=" + program_key + "&username=" + c_username + "&email=" + c_email + "&password=" + c_password + "&token=" + c_token + "&hwid=" + c_hwid));

                if (result == "user_already_exists")
                {
                    MessageBox.Show("user already exists");
                    return false;
                }
                else if (result == "email_already_exists")
                {
                    MessageBox.Show("email already exists");
                    return false;
                }
                else if (result == "invalid_email_format")
                {
                    MessageBox.Show("invalid email format");
                    return false;
                }
                else if (result == "invalid_token")
                {
                    MessageBox.Show("invalid token");
                    return false;
                }
                else if (result == "used_token")
                {
                    MessageBox.Show("used token");
                    return false;
                }
                else if (result == "success")
                {
                    MessageBox.Show("success");
                    return true;
                }
                else
                {
                    MessageBox.Show("invalid encryption key");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Environment.Exit(0);
                return false;
            }
        }
        public static bool c_activate(string c_username, string c_password, string c_token)
        {
            try
            {
                WebClient web = new WebClient();
                web.Headers["User-Agent"] = user_agent;
                web.Proxy = null;

                string result = c_encryption.decrypt(web.DownloadString(api_link + "activate.php?program_key=" + program_key + "&username=" + c_username + "&password=" + c_password + "&token=" + c_token));

                if (result == "invalid_username")
                {
                    MessageBox.Show("invalid username");
                    return false;
                }
                else if (result == "invalid_password")
                {
                    MessageBox.Show("invalid password");
                    return false;
                }
                else if (result == "invalid_token")
                {
                    MessageBox.Show("invalid token");
                    return false;
                }
                else if (result == "used_token")
                {
                    MessageBox.Show("used token");
                    return false;
                }
                else if (result == "success")
                {
                    MessageBox.Show("success");
                    return true;
                }
                else
                {
                    MessageBox.Show("invalid encryption key");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Environment.Exit(0);
                return false;
            }
        }
        private static string shit_pass { get; set; }
        public static string c_var(string c_var_name, string c_hwid = "default")
        {
            if (c_hwid == "default") c_hwid = WindowsIdentity.GetCurrent().User.Value;

            try
            {
                WebClient web = new WebClient();
                web.Headers["User-Agent"] = user_agent;
                web.Proxy = null;

                string result = c_encryption.decrypt(web.DownloadString(api_link + "var.php?program_key=" + program_key + "&var_name=" + c_var_name + "&username=" + c_userdata.username + "&password=" + c_encryption.decrypt(shit_pass) + "&hwid=" + c_hwid));

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Environment.Exit(0);
                return "";
            } 
        }
    }

    class c_userdata
    {
        public static string username { get; set; }
        public static string email { get; set; }
        public static DateTime expires { get; set; }
        public static int rank { get; set; }
    }
    class c_encryption
    {
        public static string EncryptString(string plainText, byte[] key, byte[] iv)
        {
            // Instantiate a new Aes object to perform string symmetric encryption
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;

            // Set key and IV
            byte[] aesKey = new byte[32];
            Array.Copy(key, 0, aesKey, 0, 32);
            encryptor.Key = aesKey;
            encryptor.IV = iv;

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

            // Convert the plainText string into a byte array
            byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);

            // Encrypt the input plaintext string
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);

            // Complete the encryption process
            cryptoStream.FlushFinalBlock();

            // Convert the encrypted data from a MemoryStream to a byte array
            byte[] cipherBytes = memoryStream.ToArray();

            // Close both the MemoryStream and the CryptoStream
            memoryStream.Close();
            cryptoStream.Close();

            // Convert the encrypted byte array to a base64 encoded string
            string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

            // Return the encrypted data as a string
            return cipherText;
        }

        public static string DecryptString(string cipherText, byte[] key, byte[] iv)
        {
            // Instantiate a new Aes object to perform string symmetric encryption
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;

            // Set key and IV
            byte[] aesKey = new byte[32];
            Array.Copy(key, 0, aesKey, 0, 32);
            encryptor.Key = aesKey;
            encryptor.IV = iv;

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

            // Will contain decrypted plaintext
            string plainText = String.Empty;

            try
            {
                // Convert the ciphertext string into a byte array
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                // Decrypt the input ciphertext string
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

                // Complete the decryption process
                cryptoStream.FlushFinalBlock();

                // Convert the decrypted data from a MemoryStream to a byte array
                byte[] plainBytes = memoryStream.ToArray();

                // Convert the decrypted byte array to string
                plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
            }
            finally
            {
                // Close both the MemoryStream and the CryptoStream
                memoryStream.Close();
                cryptoStream.Close();
            }

            // Return the decrypted data as a string
            return plainText;
        }

        public static string encrypt(string message)
        {
            // Create sha256 hash
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(c_api.enc_key));

            // Create secret IV
            byte[] iv = new byte[16] { 0x1, 0x5, 0x1, 0x4, 0x8, 0x3, 0x4, 0x6, 0x2, 0x6, 0x5, 0x7, 0x8, 0x3, 0x9, 0x4 };

            return EncryptString(message, key, iv);
        }

        public static string decrypt(string message)
        {
            // Create sha256 hash
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(c_api.enc_key));

            // Create secret IV
            byte[] iv = new byte[16] { 0x1, 0x5, 0x1, 0x4, 0x8, 0x3, 0x4, 0x6, 0x2, 0x6, 0x5, 0x7, 0x8, 0x3, 0x9, 0x4 };

            return DecryptString(message, key, iv);
        }

        public static DateTime unix_to_date(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

    }
}
