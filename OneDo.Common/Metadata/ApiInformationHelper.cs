using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;

namespace OneDo.Common.Metadata
{
    public static class ApiInformationHelper
    {
        public static bool Check(ApiContract contract, ushort majorVersion = 1, ushort minorVersion = 0)
        {
            switch (contract)
            {
            case ApiContract.Phone: return ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", majorVersion, minorVersion);
            default: return false;
            }
        }
    }
}
