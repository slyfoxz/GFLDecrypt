using System;
using System.IO;

namespace GFLDecrypt
{
    class Decoder
    {
        public static byte[] Decrypt(byte[] encrypt)
        {
            var length = encrypt.Length - 17;
            var v24 = new byte[16];
            var decrypt = new byte[length];
            for (var i = 0; i < 16; i++)
            {
                v24[i] = encrypt[i * 5 + 1];
            }
            var v20 = 0;
            var v10 = -1;
            var v11 = 0;
            do
            {
                var v12 = v10 + 1;
                if (v10 + 1 > 80)
                {
                    var v13 = v24[v10 - ((v10 + ((v10 - 16) >> 31 >> 28) - 16) & 0xFFFFFFF0) - 16];
                    decrypt[v11] = (byte)(encrypt[v10 + 1] ^ v13);
                    v12 = v10 + 1;
                    ++v11;
                }
                else
                {
                    if (v10 == 5 * (v10 / 5))
                    {
                        ++v20;
                    }
                    else if (v10 != -1)
                    {
                        decrypt[v11++] = (byte)(encrypt[v10 + 1] ^ v24[(v10 - v20) % 16]);
                    }
                }
                v10 = v12;
            }
            while (v11 != length);
            return decrypt;
        }

        static void Main(string[] args)
        {
            string rootPath = Directory.GetCurrentDirectory();
            DirectoryInfo root = new DirectoryInfo(rootPath);
            byte[] data;
            foreach(FileInfo f in root.GetFiles())
            {
                if (f.Extension != ".exe")
                {
                    data = File.ReadAllBytes(rootPath + "\\" + f.Name);
                    data = Decrypt(data);
                    File.WriteAllBytes(rootPath + "\\" + f.Name, data);
                    Console.WriteLine("Decrypt");
                }
            }
            rootPath += "\\motions";
            if (Directory.Exists(rootPath))
            {
                root =new DirectoryInfo(rootPath);
                foreach (FileInfo f in root.GetFiles())
                {
                        data = File.ReadAllBytes(rootPath + "\\" + f.Name);
                        data = Decrypt(data);
                        File.WriteAllBytes(rootPath + "\\" + f.Name, data);
                        Console.WriteLine("Decrypt");
                }
            }
            Console.WriteLine("Complete!");
            Console.ReadKey();
        }
    }
}
