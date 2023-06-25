using System.ComponentModel.DataAnnotations;

namespace DataMigrator.Plugins.Csv
{
    public enum FileDelimiter : byte
    {
        [Display(Name = "Comma (,)")]
        Comma = 0,

        [Display(Name = "Tab")]
        Tab = 1,

        [Display(Name = "Vertical Bar (|)")]
        VerticalBar = 2,

        [Display(Name = "Semicolon (;)")]
        Semicolon = 3
    }
}