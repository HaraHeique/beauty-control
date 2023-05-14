using BeautyControl.API.Features.Account._Common;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Account.ListUsers
{
    [DisplayName("ListUsersRequest")]
    public record Query() : IRequest<IEnumerable<UserResponse>>;
}
