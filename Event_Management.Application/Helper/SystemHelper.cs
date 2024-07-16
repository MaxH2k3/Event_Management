using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
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

        public static string GenerateSenderBatchId()
        {
            Random random = new Random();
            /**
             * Generates a unique sender_batch_id based on the current date and a sequential number.
             * 
             * Returns:
             *     A unique sender_batch_id string.
             */
            DateTime today = DateTime.Today;
            string dateStr = today.ToString("yyyyMMdd");

            // Retrieve the last used number from a database or a file
            int randomNumber = random.Next(100000, 999999);

            string senderBatchId = $"{dateStr}_{randomNumber.ToString("D5")}";

            return senderBatchId;
        }
    }
}
