using System;

namespace WebApiBoilerplate.WebApi.Helpers
{
    public static class ApplicationInfo
    {
        private static readonly Lazy<Version> LazyVersion = new Lazy<Version>(() => typeof(ApplicationInfo).Assembly.GetName().Version);

        public static Version Version => LazyVersion.Value;
    }
}
