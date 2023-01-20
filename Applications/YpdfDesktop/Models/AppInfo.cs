using System;

namespace YpdfDesktop.Models
{
    public class AppInfo : IAppInfo, IEquatable<AppInfo?>
    {
        public string? Name { get; set; }
        public string? Version { get; set; }

        public AppInfo(string? name, string? version)
        {
            Name = name;
            Version = version;
        }

        public override string ToString()
        {
            return $"{Name} {Version}";
        }

        public bool Equals(AppInfo? other)
        {
            return other is not null &&
                   Name == other.Name &&
                   Version == other.Version;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AppInfo);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Version);
        }
    }
}
