#pragma checksum "C:\Users\Soufiane\Desktop\Comptes\Comptes\Views\Compte\Detail.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6c1912aef0332c21b5e5551b7bc2218caab113d7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Compte_Detail), @"mvc.1.0.view", @"/Views/Compte/Detail.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Compte/Detail.cshtml", typeof(AspNetCore.Views_Compte_Detail))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\Soufiane\Desktop\Comptes\Comptes\Views\_ViewImports.cshtml"
using Comptes;

#line default
#line hidden
#line 2 "C:\Users\Soufiane\Desktop\Comptes\Comptes\Views\_ViewImports.cshtml"
using Comptes.Models;

#line default
#line hidden
#line 1 "C:\Users\Soufiane\Desktop\Comptes\Comptes\Views\Compte\Detail.cshtml"
using Comptes_WebAPI.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6c1912aef0332c21b5e5551b7bc2218caab113d7", @"/Views/Compte/Detail.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5e9067751f4006ea3218665bc9ad2f914eae63fa", @"/Views/_ViewImports.cshtml")]
    public class Views_Compte_Detail : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Account>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "C:\Users\Soufiane\Desktop\Comptes\Comptes\Views\Compte\Detail.cshtml"
  
    ViewBag.Title = "Forum Detail";
    ViewData["Mode"] = "ShowingDetail";

#line default
#line hidden
            BeginContext(131, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(134, 44, false);
#line 8 "C:\Users\Soufiane\Desktop\Comptes\Comptes\Views\Compte\Detail.cshtml"
Write(Html.Partial("_AccountDetailPartial", Model));

#line default
#line hidden
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Account> Html { get; private set; }
    }
}
#pragma warning restore 1591
