using System.Collections.Generic;
using System.Linq;
using NuGet;

namespace ripple.Nuget
{
    public interface IPackageRepositoryBuilder
    {
        IPackageRepository BuildRemote(IEnumerable<string> packageSources);
        IPackageRepository BuildLocal(string packageSource);
        IPackageRepository BuildSource(params IPackageRepository[] repositories);
    }

    public class PackageRepositoryBuilder : IPackageRepositoryBuilder
    {
        private readonly IPackageRepositoryFactory _factory;

        public PackageRepositoryBuilder() : this(new PackageRepositoryFactory()) {}
        public PackageRepositoryBuilder(IPackageRepositoryFactory factory)
        {
            _factory = factory;
        }

        public IPackageRepository BuildRemote(IEnumerable<string> packageSources)
        {
            var feeds = packageSources.Any() ? packageSources.Distinct() : RippleConstants.NugetOrgFeed;
            var repos = new List<IPackageRepository>();
            feeds.Each(f =>
            {
                var repo =_factory.CreateRepository(f);
                repos.Add(repo);
            });

            return new AggregateRepository(repos);
        }

        public IPackageRepository BuildLocal(string packageSource)
        {
            return _factory.CreateRepository(packageSource);
        }

        public IPackageRepository BuildSource(params IPackageRepository[] repositories)
        {
            return new AggregateRepository(repositories);
        }
    }
}