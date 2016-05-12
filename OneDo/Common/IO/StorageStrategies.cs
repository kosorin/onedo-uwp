using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using Windows.Storage;

namespace OneDo.Common.IO
{
    public enum StorageStrategies
    {
        Local,
        Roaming,
        Temporary
    }
}