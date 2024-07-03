using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Drawing.Imaging;

namespace Event_Management.Application.Helper
{
    public class SystemHelper
    {

        public static byte[] ImageToBytes(Image imageIn)
        {
            using (var stream = new MemoryStream())
            {
                imageIn.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public static string GetIpAddress()
        {
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            return httpContextAccessor.HttpContext.Connection?.LocalIpAddress?.ToString() ?? string.Empty;
        }
    }
}
