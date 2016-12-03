using System.Collections.Generic;
using Windows.Foundation.Metadata;

namespace OneDo.Common.Metadata
{
    public static class ApiInformationHelper
    {
        private static Dictionary<ApiContract, string> contracts = new Dictionary<ApiContract, string>
        {
            [ApiContract.Phone] = "Windows.Phone.PhoneContract",
        };

        public static bool Check(ApiContract contract, ushort majorVersion = 1, ushort minorVersion = 0)
        {
            if (contracts.ContainsKey(contract))
            {
                return ApiInformation.IsApiContractPresent(contracts[contract], majorVersion, minorVersion);
            }
            return false;
        }
    }
}
