using System.IO.Compression;
using KoGenerator.Domain;
using KoGenerator.Models;

namespace KoGenerator.Services;

public class GeneratorService : IGeneratorService
{
    public string GenerateRangePhpService(List<GeneratePhpServiceModel> models)
    {
        string folderName = GenerateRandomString(5);
        foreach (var model in models)
        {
            try
            {
                string templatePath = GetTemplateByMethod(model.Method);
            
                if(model.Method.ToUpper() == Constants.GetById && string.IsNullOrEmpty(model.Parameter))
                    throw new Exception("Parametre alanı gereklidir!");

                if (templatePath == "error") 
                    throw new Exception("Girmiş olduğunuz metot sistemin istediği metotları karşılamıyor!");
            
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();
                string inputFilePath = System.IO.Path.Combine(currentDirectory, "Templates", templatePath);
                string phpCode = System.IO.File.ReadAllText(inputFilePath);
                phpCode = phpCode.Replace(Constants.ProjectName, model.ProjectName);
                phpCode = phpCode.Replace(Constants.EndpointUrl, model.EndpointUrl);
                if(model.Method.ToUpper() == Constants.GetById)
                    phpCode = phpCode.Replace(Constants.ParameterName, model.Parameter);

                string? baseoutputFilePath = System.IO.Path.Combine(currentDirectory, "Outputs");
                string? phpFilePath = Path.GetDirectoryName(baseoutputFilePath + $"/{folderName}" + model.FilePath);
            
                if (!Directory.Exists(phpFilePath))
                {
                    if (phpFilePath != null) Directory.CreateDirectory(phpFilePath);
                }

                System.IO.File.WriteAllText(phpFilePath + "/index.php", phpCode);
            }
            catch (Exception ex)
            {
                throw new Exception( "Hata: " + ex.Message);
            }
        }

        return folderName;
    }
    
    private string GetTemplateByMethod(string method)
    {
        switch (method.ToUpper())
        {
            case "GET":
                return Constants.GetServiceTemplate;
            case "POST":
                return Constants.PostServiceTemplate;
            case "PUT":
                return Constants.PutServiceTemplate;
            case "GETBYID":
                return Constants.GetByIdServiceTemplate;
            default:
                return "error";
        }
    }
    
    private static string GenerateRandomString(int length)
    {
        const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        Random random = new Random();
        char[] randomArray = new char[length];

        for (int i = 0; i < length; i++)
        {
            randomArray[i] = chars[random.Next(chars.Length)];
        }

        return new string(randomArray);
    }
}