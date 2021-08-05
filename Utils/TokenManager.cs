using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrendyolOrderService.Utils
{
    public class TokenManager
    {
        public static string GetToken(string textToEncode)
        {
            return EncodeDecodeManager.Base64Encode(textToEncode);
        }
    }
}
