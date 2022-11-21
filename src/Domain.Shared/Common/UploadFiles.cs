using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot.Domain.Shared.Common
{
    public class UploadFiles
    {
        // Create file and store that to server
        public static string CreateImg(IFormFile file, string folderName)
        {
            try
            {
                string imgname = GenerateCode.GuidCode() + file.FileName;
                string ImgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/bot/"+folderName, imgname);

                using (var stream = new FileStream(ImgPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return imgname;
            }
            catch (Exception)
            {
                return "false";
            }

        }


        // Delete the file and it's root from server
        public static bool DeleteImg(string imgname)
        {
            try
            {
                string Fullpath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/"+ imgname);
                if (File.Exists(Fullpath))
                {
                    File.Delete(Fullpath);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }


        // make a random generated numbers

        public class GenerateCode
        {
            public static string ActiveCode()
            {
                Random random = new Random();
                return random.Next(100000, 999000).ToString();
            }

            public static string GuidCode()
            {
                return Guid.NewGuid().ToString().Replace("-", "");
            }
        }
    }
}
