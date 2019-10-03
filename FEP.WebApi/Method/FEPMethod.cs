using FEP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace FEP.WebApi.Method
{
    public static class FEPMethod
    {
        public static SystemMode CurrentSystemMode()
        {
            var mode = WebConfigurationManager.AppSettings["SystemMode"] ?? "Development";

            if (Enum.IsDefined(typeof(SystemMode), mode))
            {
                return (SystemMode)Enum.Parse(typeof(SystemMode), mode);
            }
            else
            {
                return SystemMode.Development;
            }
        }
    }
}