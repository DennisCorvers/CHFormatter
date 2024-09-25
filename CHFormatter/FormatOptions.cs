namespace CHFormatter
{
    internal struct FormatOptions
    {
        public static FormatOptions Default = new FormatOptions(true);

        public bool RemoveArtist { get; set; }

        public FormatOptions(bool removeArtist)
        {
            RemoveArtist = removeArtist;
        }
    }
}
