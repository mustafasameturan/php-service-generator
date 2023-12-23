using KoGenerator.Models;
using Microsoft.AspNetCore.Mvc;

namespace KoGenerator.Services;

public interface IGeneratorService
{
    string GenerateRangePhpService(List<GeneratePhpServiceModel> models);
}