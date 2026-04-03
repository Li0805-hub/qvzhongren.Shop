using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using qvzhongren.Application;
using qvzhongren.Application.Dtos;

namespace qvzhongren.Platform.Application.Services;

/// <summary>
/// 文件上传服务
/// </summary>
public class FileService : BaseService
{
    private readonly IWebHostEnvironment _env;

    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HttpPost("Upload")]
    [RequestSizeLimit(50 * 1024 * 1024)] // 50MB
    public async Task<ResultDto<object>> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return ResultDto<object>.BadRequest("文件为空");

        var uploadsDir = Path.Combine(_env.WebRootPath, "uploads");
        if (!Directory.Exists(uploadsDir))
            Directory.CreateDirectory(uploadsDir);

        var ext = Path.GetExtension(file.FileName);
        var savedName = $"{Guid.NewGuid():N}{ext}";
        var filePath = Path.Combine(uploadsDir, savedName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var url = $"/uploads/{savedName}";

        return ResultDto<object>.Success(new { fileName = file.FileName, fileUrl = url });
    }
}
