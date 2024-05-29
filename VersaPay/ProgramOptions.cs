namespace VersaPay;

using System.ComponentModel.DataAnnotations;

public sealed class ProgramOptions
{
    public const string ConfigurationSectionName = "Program";

    [Required]
    required public string IncommingDirectory { get; set; }

    [Required]
    required public string DestinationDirectory { get; set; }
}
