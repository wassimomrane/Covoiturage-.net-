using Covoiturage.Application.Interfaces;
using Covoiturage.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Covoiturage.Application.Features.Admin;

public record AdminStatsDto(
    int TotalUsers,
    int TotalTrajets,
    int TotalReservations,
    int TotalVilles);

public record AdminUserDto(
    string Id,
    string Nom,
    string Prenom,
    string Email,
    string PhoneNumber,
    bool EmailConfirmed,
    DateTime DateInscription,
    IList<string> Roles);

public record GetAdminStatsQuery : IRequest<AdminStatsDto>;
public record GetAdminUsersQuery : IRequest<IEnumerable<AdminUserDto>>;

public class AdminHandlers :
    IRequestHandler<GetAdminStatsQuery, AdminStatsDto>,
    IRequestHandler<GetAdminUsersQuery, IEnumerable<AdminUserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminHandlers(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<AdminStatsDto> Handle(GetAdminStatsQuery request, CancellationToken cancellationToken)
    {
        var usersCount = _userManager.Users.Count();
        var trajets = await _unitOfWork.Trajets.GetAllAsync();
        var reservations = await _unitOfWork.Reservations.GetAllAsync();
        var villes = await _unitOfWork.Villes.GetAllAsync();

        return new AdminStatsDto(
            usersCount,
            trajets.Count(),
            reservations.Count(),
            villes.Count());
    }

    public async Task<IEnumerable<AdminUserDto>> Handle(GetAdminUsersQuery request, CancellationToken cancellationToken)
    {
        var users = _userManager.Users.ToList();
        var result = new List<AdminUserDto>();

        foreach (var u in users)
        {
            var roles = await _userManager.GetRolesAsync(u);
            result.Add(new AdminUserDto(
                u.Id,
                u.Nom,
                u.Prenom,
                u.Email ?? string.Empty,
                u.PhoneNumber ?? string.Empty,
                u.EmailConfirmed,
                u.DateInscription,
                roles
            ));
        }

        return result;
    }
}