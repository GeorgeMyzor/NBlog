using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using BLL.Interface.Services;
using LoggingModule;

namespace MVCNBlog.Infrastructure
{
    public static class Settings
    {
        static Settings()
        {
            var roleService = (IRoleService)System.Web.Mvc.DependencyResolver.Current.GetService(typeof (IRoleService));

            VipRoleId = roleService.GetRoleEntity("VipUser").Id;
        }

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

        public static int VipRoleId { get; set; }
    }
}