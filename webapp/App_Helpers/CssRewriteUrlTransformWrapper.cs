using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;

namespace SmartAdminMvc.App_Helpers
{
    public class CssRewriteUrlTransformWrapper : IItemTransform
    {
        public string Process(string includedVirtualPath, string input)
        {
            return new CssRewriteUrlTransform().Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input);
        }
    }
}