using Application.Features.Auth.Rules;
using Application.Services.Core.AuthService;
using Application.Services.Core.Repositories;
using AutoMapper;
using Crea.Core.Security.Entities;
using MediatR;

namespace Application.Features.Auth.Commands.Revoke;

public class RevokeCommand : IRequest<RevokedResponse>
{
    public string Token { get; set; }
    public string IpAddress { get; set; }

    public class RevokeCommandHandler : IRequestHandler<RevokeCommand, RevokedResponse>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly AuthBusinessRules _authBusinessRules;

        public RevokeCommandHandler(IRefreshTokenRepository refreshTokenRepository, IAuthService authService, IMapper mapper, AuthBusinessRules authBusinessRules)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _authService = authService;
            _mapper = mapper;
            _authBusinessRules = authBusinessRules;
        }

        public async Task<RevokedResponse> Handle(RevokeCommand request, CancellationToken cancellationToken)
        {
            RefreshToken? refreshToken = await _refreshTokenRepository.GetAsync(x => x.Token == request.Token);
            await _authBusinessRules.RefreshTokenShouldBeExists(refreshToken);
            await _authBusinessRules.RefreshTokenShouldBeActive(refreshToken!);

            await _authService.RevokeRefreshToken(refreshToken, request.IpAddress, "RefreshToken manuel olarak geçersiz kılınmıştır.");

            RevokedResponse response = _mapper.Map<RevokedResponse>(refreshToken);

            return response;
        }
    }
}
