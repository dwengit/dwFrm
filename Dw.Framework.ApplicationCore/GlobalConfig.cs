using Dw.Framework.Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.ApplicationCore
{
    public class GlobalConfig
    {
        public static string BucketName = Appsettings.App("MinioConfig:BucketName");
        public static string PreviewEndpoint = Appsettings.App("MinioConfig:PreviewEndpoint");
    }
}
