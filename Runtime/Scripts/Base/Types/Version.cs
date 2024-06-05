namespace Devenant
{
    public class Version
    {
        public readonly int major;
        public readonly int minor;
        public readonly int patch;

        public Version (int major, int minor, int patch)
        {
            this.major = major;
            this.minor = minor;
            this.patch = patch;
        }

        public Version(string version)
        {
            string[] splitedVersion = version.Split(".");

            major = int.Parse(splitedVersion[0]);
            minor = int.Parse(splitedVersion[1]);
            patch = int.Parse(splitedVersion[2]);
        }

        public enum Comparison
        {
            Less,
            Equal,
            Greater
        }

        public Comparison Compare(Version target)
        {
            if (major == target.major)
            {
                if (minor == target.minor)
                {
                    if (patch == target.patch)
                    {
                        return Comparison.Equal;
                    }
                    else
                    {
                        return target.patch < patch ? Comparison.Less : Comparison.Greater;
                    }
                }
                else
                {
                    return target.minor < minor ? Comparison.Less : Comparison.Greater;
                }
            }
            else
            {
                return target.major < major ? Comparison.Less : Comparison.Greater;
            }
        }
    }
}
