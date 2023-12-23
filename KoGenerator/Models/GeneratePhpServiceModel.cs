namespace KoGenerator.Models;

public class GeneratePhpServiceModel
{
    public string FilePath { get; set; }
    public string Method { get; set; }
    public string ProjectName { get; set; }
    public string EndpointUrl { get; set; }
    public string? Parameter { get; set; }
}