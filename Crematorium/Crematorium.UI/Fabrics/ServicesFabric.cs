using Crematorium.Domain.Entities;
using Crematorium.UI.Pages;
using System;

namespace Crematorium.UI.Fabrics
{
    public static class ServicesFabric
    {
        public static IServiceProvider Services { get; set; }

        public static User? CurrentUser { get; set; }

        public static object GetPage(Type typeOfPage)
        {
            if (Services is null)
                throw new ArgumentNullException(nameof(Services));

            object? page = Services.GetService(typeOfPage);
            if (page is null)
                throw new Exception("Not found Page");

            return page;
        }

        public static ErrorPage GetErrorPage(string errorInfo)
        {
            var page = (ErrorPage)GetPage(typeof(ErrorPage));
            page.ErorText.Text = errorInfo;
            return page;
        }
    }
}
