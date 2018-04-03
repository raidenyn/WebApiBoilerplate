using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebApiBoilerplate.Framework.Database.Configuration
{
    public class NHibernateConfigurationCache<TDbContext>
        where TDbContext : DbContext<TDbContext>
    {
        [NotNull] private readonly NHibernateConfigurationCacheOptions<TDbContext> _options;
        [CanBeNull] private readonly ILogger<NHibernateConfigurationCache<TDbContext>> _logger;

        [NotNull]
        private readonly object _locker = new object();

        [CanBeNull]
        public string FullPath => _options.FilePath != null ? Path.GetFullPath(_options.FilePath) : null;

        public bool Exists => _options.FilePath != null && File.Exists(FullPath);

        public NHibernateConfigurationCache(
            [NotNull] IOptions<NHibernateConfigurationCacheOptions<TDbContext>> options,
            [CanBeNull] ILogger<NHibernateConfigurationCache<TDbContext>> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger;
        }

        [CanBeNull]
        public NHibernate.Cfg.Configuration Load()
        {
            var fullPath = FullPath;

            if (fullPath != null && File.Exists(fullPath))
            {
                lock(_locker)
                {
                    if (File.Exists(fullPath))
                    {
                        try
                        {
                            return Deserialize(fullPath);
                        }
                        catch (Exception exp)
                        {
                            _logger?.LogError(exp, $"Deserialize nhibernate cache from path '{fullPath}'.");
                            try
                            {
                                File.Delete(fullPath);
                            }
                            catch (IOException) { }
                        }
                    }
                }
            }
            return null;
        }

        [CanBeNull]
        private NHibernate.Cfg.Configuration Deserialize([NotNull] string path)
        {
            using (var cacheFile = new FileStream
                (
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read
                ))
            {
                var serializer = new BinaryFormatter();
                return serializer.Deserialize(cacheFile) as NHibernate.Cfg.Configuration;
            }
        }

        public void Save([NotNull] NHibernate.Cfg.Configuration configuration)
        {
            var fullPath = FullPath;

            if (fullPath != null)
            {
                lock (_locker)
                {
                    var directoryName = Path.GetDirectoryName(fullPath) ?? "";
                    Directory.CreateDirectory(directoryName);

                    using (var cacheFile = new FileStream
                    (
                        fullPath,
                        FileMode.Create,
                        FileAccess.Write
                    ))
                    {
                        var serializer = new BinaryFormatter();
                        serializer.Serialize(cacheFile, configuration);
                        cacheFile.SetLength(cacheFile.Position);
                    }
                }
            }
        }

        public bool IsValid()
        {
            var isValid = Exists;

            var fullPath = FullPath;
            if (isValid && 
                !String.IsNullOrWhiteSpace(_options.DependceOnFile) && 
                !String.IsNullOrWhiteSpace(fullPath))
            {
                var depenceFullPath = Path.GetFullPath(_options.DependceOnFile);

                isValid &= File.GetLastWriteTimeUtc(depenceFullPath) > File.GetLastWriteTimeUtc(fullPath);
            }

            return isValid;
        }
    }
}
