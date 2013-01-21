﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using FubuCore;
using NuGet;

namespace ripple.New
{
    public class NugetFile : INugetFile
    {
        private readonly string _path;

        public NugetFile(string path)
        {
            _path = path;

            var file = Path.GetFileNameWithoutExtension(path);
            var parts = file.Split('.');
            Name = parts.First();
            Version = SemanticVersion.Parse(parts.Skip(1).Join("."));

            IsPreRelease = Version.SpecialVersion.IsNotEmpty();
        }

        public string Name { get; private set; }
        public SemanticVersion Version { get; private set; }
        public bool IsPreRelease { get; private set; }

        public IPackage ExplodeTo(string directory)
        {
            var explodedDirectory = directory.AppendPath(Name);
            var fileSystem = new FileSystem();
            fileSystem.CreateDirectory(explodedDirectory);
            fileSystem.CleanDirectory(explodedDirectory);

            var package = new ZipPackage(_path);

            package.GetFiles().Each(file => {
                var target = explodedDirectory.AppendPath(file.Path);
                fileSystem.CreateDirectory(target.ParentDirectory());
                
                using (var stream = new FileStream(target, FileMode.Create, FileAccess.Write))
                {
                    file.GetStream().CopyTo(stream);
                }
            });

            fileSystem.CopyToDirectory(_path, explodedDirectory);

            var repository = new LocalPackageRepository(directory);
            return repository.FindPackagesById(Name).Single();
        }
    }
}