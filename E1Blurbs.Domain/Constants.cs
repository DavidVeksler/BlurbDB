using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E1Blurbs.Domain
{
    public class Constants
    {
        public static class CultureCodes
        {
            // SELECT count(culturecode), culturecode from translation GROUP BY culturecode ORDER BY count(culturecode) desc
            public const string Indonesian = "id-ID     ";
            public const string Russian = "ru-RU     ";
            public const string Chinese = "zh-CN     ";
            public const string Spanish = "es-ES     ";     
        }
    }
}
