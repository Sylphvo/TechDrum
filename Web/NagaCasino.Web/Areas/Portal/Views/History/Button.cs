using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;

namespace TechDrum.Web.Areas.Portal.Views.History
{
    public static class Button
    {
        public static string ActivePageKey => "ActivePage";

        public static string Index => "Index";

        public static string ETH => "Transaction ETH";

        public static string NAGA => "Transaction NAGA";
        public static string NAGAINS => "Transaction NAGAINS";

        public static string Commission => "Commission";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string ETHNavClass(ViewContext viewContext) => PageNavClass(viewContext, ETH);

        public static string NAGANavClass(ViewContext viewContext) => PageNavClass(viewContext, NAGA);

        public static string CommissionNavClass(ViewContext viewContext) => PageNavClass(viewContext, Commission);
        public static string NAGANavInsClass(ViewContext viewContext) => PageNavClass(viewContext, NAGAINS);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
    }
}
