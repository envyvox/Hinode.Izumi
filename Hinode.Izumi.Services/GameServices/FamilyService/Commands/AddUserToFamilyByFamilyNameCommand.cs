using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Commands
{
    /// <summary>
    /// Используется при создании семьи. Пользователь будет назначен главой семьи.
    /// </summary>
    public record AddUserToFamilyByFamilyNameCommand(long UserId, string FamilyName) : IRequest;
}
