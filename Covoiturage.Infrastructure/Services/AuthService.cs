using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Covoiturage.Application.DTOs.Auth;
using Covoiturage.Application.Interfaces.Services;
using Covoiturage.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Covoiturage.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthService> _logger;
    private readonly IEmailService _emailService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IConfiguration config,
        ILogger<AuthService> logger,
        IEmailService emailService)
    {
        _userManager = userManager;
        _config = config;
        _logger = logger;
        _emailService = emailService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            return new AuthResponseDto { Success = false, Message = "Cet email est déjà associé à un compte." };
        }

        // Génération d'un code OTP à 6 chiffres
        var verificationCode = new Random().Next(100000, 999999).ToString();

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            Nom = dto.Nom,
            Prenom = dto.Prenom,
            PhoneNumber = dto.PhoneNumber,
            EmailConfirmed = false, // En attente de validation
            VerificationCode = verificationCode,
            VerificationCodeExpiresAt = DateTime.UtcNow.AddMinutes(15),
            DateInscription = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponseDto { Success = false, Message = errors };
        }

        await _userManager.AddToRolesAsync(user, new[] { "Passager", "Conducteur" });

        // Envoi réel de l'e-mail dans la boîte de réception de l'utilisateur
        await _emailService.SendVerificationCodeAsync(user.Email!, user.Prenom, verificationCode);

        // Log de secours dans la console serveur
        _logger.LogInformation("Email OTP envoyé à {Email} (Code: {Code})", user.Email, verificationCode);

        return new AuthResponseDto
        {
            Success = true,
            Message = "Inscription réussie ! Un code de vérification à 6 chiffres a été envoyé à votre adresse e-mail.",
            Email = user.Email,
            NomComplet = $"{user.Prenom} {user.Nom}"
        };
    }

    public async Task<AuthResponseDto> VerifyEmailAsync(VerifyEmailDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return new AuthResponseDto { Success = false, Message = "Utilisateur introuvable." };

        if (user.EmailConfirmed)
            return new AuthResponseDto { Success = false, Message = "Ce compte est déjà vérifié." };

        if (user.VerificationCode != dto.Code)
            return new AuthResponseDto { Success = false, Message = "Code de vérification incorrect." };

        if (user.VerificationCodeExpiresAt < DateTime.UtcNow)
            return new AuthResponseDto { Success = false, Message = "Ce code a expiré. Veuillez demander un nouveau code." };

        // Validation du compte
        user.EmailConfirmed = true;
        user.VerificationCode = null;
        user.VerificationCodeExpiresAt = null;
        await _userManager.UpdateAsync(user);

        var roles = await _userManager.GetRolesAsync(user);
        var token = GenerateJwtToken(user, roles);

        return new AuthResponseDto
        {
            Success = true,
            Message = "Compte vérifié avec succès !",
            Token = token,
            Email = user.Email,
            NomComplet = $"{user.Prenom} {user.Nom}",
            Roles = roles
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            return new AuthResponseDto { Success = false, Message = "Email ou mot de passe incorrect." };
        }

        // Blocage si l'email n'est pas encore confirmé
        if (!user.EmailConfirmed)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Votre compte n'est pas encore vérifié. Veuillez saisir le code reçu par email.",
                Email = user.Email
            };
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = GenerateJwtToken(user, roles);

        return new AuthResponseDto
        {
            Success = true,
            Message = "Connexion réussie.",
            Token = token,
            Email = user.Email,
            NomComplet = $"{user.Prenom} {user.Nom}",
            Roles = roles
        };
    }

    private string GenerateJwtToken(ApplicationUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Name, $"{user.Prenom} {user.Nom}"),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var secret = _config["JwtSettings:Secret"]
            ?? throw new InvalidOperationException("La clé secrète JWT est introuvable.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_config["JwtSettings:DurationInDays"] ?? "7"));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            Issuer = _config["JwtSettings:Issuer"],
            Audience = _config["JwtSettings:Audience"],
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}