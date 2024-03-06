using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DO_AN_CUA_HAN.Model
{
    internal class Bcrypt
    {
        public static string CreateMD5(string input)
        {
            // Step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input); //Chuỗi đầu vào (input) được chuyển đổi thành mảng byte sử dụng bảng mã ASCII.
            byte[] hashBytes = md5.ComputeHash(inputBytes); // Đối tượng MD5 tính toán mã băm MD5 từ mảng byte của chuỗi đầu vào và kết quả được lưu vào một mảng byte hashBytes.

            // Step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();

            //Mỗi byte trong mảng hashBytes được chuyển đổi thành một chuỗi hex (hệ 16) và được thêm vào StringBuilder. Với ToString("X2"), mỗi byte được biểu diễn bằng 2 ký tự hex.
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            return sb.ToString(); //Kết quả cuối cùng là chuỗi biểu diễn của mã băm MD5 của chuỗi đầu vào.


        }
    }
}
