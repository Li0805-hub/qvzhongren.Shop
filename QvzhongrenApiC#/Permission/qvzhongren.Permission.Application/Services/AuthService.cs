using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using qvzhongren.Application;
using qvzhongren.Application.Dtos;
using qvzhongren.Permission.Application.Dtos;
using qvzhongren.Permission.Model;
using qvzhongren.Repository.SqlSugar;
using qvzhongren.Shared.Helper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace qvzhongren.Permission.Application.Services;

/// <summary>
/// 认证服务
/// </summary>
public class AuthService : BaseService
{
    private readonly IBaseRepository<SysUser> _userRepo;

    public AuthService(IBaseRepository<SysUser> userRepo)
    {
        _userRepo = userRepo;
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<ResultDto<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var user = (await _userRepo.GetListAsync(x => x.UserName == request.UserName)).FirstOrDefault();
            if (user == null)
                return ResultDto<LoginResponse>.Error("用户名或密码错误");

            var inputPwd = Tool.MD5Encrypt(request.Password);
            if (user.Password != inputPwd)
                return ResultDto<LoginResponse>.Error("用户名或密码错误");

            if (user.Status != "1")
                return ResultDto<LoginResponse>.Error("账号已被禁用");

            var token = GenerateJwtToken(user);

            return ResultDto<LoginResponse>.Success(new LoginResponse
            {
                Token = token,
                UserId = user.UserId,
                UserName = user.UserName,
                RealName = user.RealName
            });
        }
        catch (Exception ex)
        {
            return ResultDto<LoginResponse>.Error($"登录失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 初始化管理员账号（仅当无用户时可用）
    /// </summary>
    [AllowAnonymous]
    [HttpPost("InitAdmin")]
    public async Task<ResultDto<bool>> InitAdmin()
    {
        try
        {
            var users = await _userRepo.GetAllAsync();
            if (users != null && users.Count > 0)
                return ResultDto<bool>.Error("已存在用户，无法初始化");

            var admin = new SysUser
            {
                UserId = Guid.NewGuid().ToString("N"),
                UserName = "admin",
                Password = Tool.MD5Encrypt("admin123"),
                RealName = "管理员",
                Status = "1",
                CreateCode = "system",
                CreateDate = DateTime.Now,
                UpdateCode = "system",
                UpdateDate = DateTime.Now
            };
            await _userRepo.InsertAsync(admin);
            return ResultDto<bool>.Success(true, "管理员账号创建成功，用户名: admin，密码: admin123");
        }
        catch (Exception ex)
        {
            return ResultDto<bool>.Error($"初始化失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 重置密码为 123456
    /// </summary>
    [HttpPost("ResetPassword")]
    public async Task<ResultDto<bool>> ResetPassword([FromBody] string userId)
    {
        try
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                return ResultDto<bool>.Error("用户不存在");

            user.Password = Tool.MD5Encrypt("123456");
            await _userRepo.UpdateAsync(user);
            return ResultDto<bool>.Success(true, "密码已重置为 123456");
        }
        catch (Exception ex)
        {
            return ResultDto<bool>.Error($"重置失败: {ex.Message}");
        }
    }

    private string GenerateJwtToken(SysUser user)
    {
        var secretKey = AppSettings.GetValue("Jwt:SecretKey");
        var issuer = AppSettings.GetValue("Jwt:Issuer");
        var audience = AppSettings.GetValue("Jwt:Audience");
        var hours = int.Parse(AppSettings.GetValue("Jwt:ExpirationHours") ?? "24");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("RealName", user.RealName ?? ""),
            new Claim("UserCode", user.UserId),
            new Claim("DeptCode", user.DeptCode ?? ""),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddHours(hours),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
