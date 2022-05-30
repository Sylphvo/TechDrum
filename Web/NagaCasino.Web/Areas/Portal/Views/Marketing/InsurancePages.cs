using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TechDrum.Web.Areas.Portal.Views.Marketing
{
    public static class InsurancePages
    {
        public static string ActivePageKey => "ActivePage";

        public static string BuyInsurance => "BuyInsurance";

        public static string InsuranceHistory => "InsuranceHistory";

        public static string ClaimInsurance => "ClaimInsurance";

        public static string ClaimHistory => "ClaimHistory";

        public static string BuyInsuranceNavClass(ViewContext viewContext) => PageNavClass(viewContext, BuyInsurance);

        public static string InsuranceHistoryNavClass(ViewContext viewContext) => PageNavClass(viewContext, InsuranceHistory);

        public static string ClaimInsuranceNavClass(ViewContext viewContext) => PageNavClass(viewContext, ClaimInsurance);

        public static string ClaimHistoryNavClass(ViewContext viewContext) => PageNavClass(viewContext, ClaimHistory);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
    }
}
