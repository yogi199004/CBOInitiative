namespace AAPS.L10nPortal.Bal.Extensions
{
    public static class FileExtension
    {
        private static string[] blacklistedExtension = new[]
        {
            ".ade",".adp",".app",".asa",".ashx",".asmx",".asp",".bas",".bat",
            ".cdx",".cer",".chm",".class",".cmd",".com",".config",".cpl",".crt",
            ".csh",".dll",".exe",".fxp",".hlp",".hta",".htr",".htw",".ida",".idc",
            ".idq",".ins",".isp",".its",".jse",".ksh",".lnk",".mad",".maf",".mag",
            ".mam",".maq",".mar",".mas",".mat",".mau",".mav",".maw",".mda",".mdb",
            ".mde",".mdt",".mdw",".mdz",".msc",".msh",".msh1",".msh1xml",".msh2",
            ".msh2xml",".mshxml",".msi",".msp",".mst",".ops",".pcd",".pif",".prf",
            ".prg",".printer",".pst",".reg",".rem",".scf",".scr",".sct",".shb",
            ".shs",".shtm",".shtml",".soap",".stm",".url",".vb",".vbe",".vbs",
            ".ws",".wsc",".wsf",".wsh"
        };
        public static bool IsFileExtensionBlacklisted(this string filename)
        {
            return blacklistedExtension.Any(x => filename.ToLower().EndsWith(x.ToLower()));
        }

    }
}
