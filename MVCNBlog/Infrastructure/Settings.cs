using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace MVCNBlog.Infrastructure
{
    public static class Settings
    {
        public static byte[] GetDefaultProfilePicture()
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("~/Content/no-profile-img.gif");
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            Image image = Image.FromStream(stream);
            using (var ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }
    }
}