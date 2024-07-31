using System.IO.Compression;
using KoGenerator.Models;
using KoGenerator.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KoGenerator.Controllers;

[ApiController]
[Route("[controller]")]
public class GeneratorController : ControllerBase
{
    private readonly IGeneratorService _generatorService;

    public GeneratorController(IGeneratorService generatorService)
    {
        _generatorService = generatorService;
    }
    
    [HttpPost]
    public IActionResult Generate(List<GeneratePhpServiceModel> models)
    {
        var folderName = _generatorService.GenerateRangePhpService(models);
        return DownloadDirectory(folderName);
    }
    
    private IActionResult DownloadDirectory(string folderName)
    {
        try
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string baseDirectoryPath = Path.Combine(currentDirectory, "Outputs");
            string directoryPath = Path.Combine(baseDirectoryPath, folderName);

            
            if (Directory.Exists(directoryPath))
            {
                string zipFileName = folderName + ".zip";
                string zipFilePath = Path.Combine(baseDirectoryPath, zipFileName);

                ZipFile.CreateFromDirectory(directoryPath, zipFilePath);

                var fileStream = System.IO.File.OpenRead(zipFilePath);
                return File(fileStream, "application/zip", zipFileName);
            }

            return BadRequest("Directory not found");
        }
        catch (Exception ex)
        {
            // Log the exception for debugging purposes
            Console.WriteLine($"Exception in DownloadDirectory: {ex}");
            return BadRequest(ex.Message);
        }
    }
    

}