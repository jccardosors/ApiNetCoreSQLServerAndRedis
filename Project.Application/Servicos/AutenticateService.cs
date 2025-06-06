using AutoMapper;
using Project.Application.Interfaces;
using Project.Application.Utils;
using Project.Application.ViewModels;
using Project.Domain;
using Project.Domain.Entities;

namespace Project.Application.Servicos
{
    public class AutenticateService : IAutenticateService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogsService _logsService;
        private readonly IControlUserAccessRepository _controlUserAccessRepository;

        public AutenticateService(IMapper mapper, IUserRepository userRepository,
            ITokenService tokenService, ILogsService logs,
            IControlUserAccessRepository controlUserAccessRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logsService = logs;
            _controlUserAccessRepository = controlUserAccessRepository;
        }

        public async Task<CustomResult<AutenticateVM>> Autenticate(string email, string password)
        {
            try
            {
                await _logsService.Add(email, "UserService", "Authenticate", string.Empty);

                var usuario = await _userRepository.GetItemByEmail(email.Trim(), password.Trim());
                if (usuario == null)
                {
                    await this.RegisterTryAccess(email);

                    return CustomResult<AutenticateVM>.Failure(CustomError.RecordNotFound("Usuário não autenticado."));
                }

                var usuarioDesbloqueado = await _controlUserAccessRepository.GetUserBlocked(email.Trim());
                if (!usuarioDesbloqueado)
                {
                    usuarioDesbloqueado = await this.CheckTemporaryBlock(usuario);
                    if (usuarioDesbloqueado)
                    {
                        return CustomResult<AutenticateVM>.Failure(CustomError.RecordNotFound("Usuário bloqueado, tente novamente após 5 minutos."));
                    }
                }

                var resultToken = new AutenticateVM
                {
                    Email = usuario.Email,
                    UserId = usuario.Id,
                    Token = _tokenService.GenerateToken(usuario.Email)
                };

                return CustomResult<AutenticateVM>.Success(resultToken);
            }
            catch (Exception ex)
            {
                await _logsService.Add(email, "UserService", "Authenticate", ex.Message);
                return CustomResult<AutenticateVM>.Failure(CustomError.ExceptionError(ex.Message));
            }
        }

        #region Private Methods

        private async Task RegisterTryAccess(string email)
        {
            var controlUser = await _controlUserAccessRepository.GetItem(email);
            if (controlUser is null)
            {
                await _controlUserAccessRepository.Add(new ControlUserAccess
                {
                    Blocked = false,
                    TryNumber = 1,
                    LastAccess = DateTime.Now,
                    UserEmail = email
                });
            }
            else if (!controlUser.Blocked)
            {
                controlUser.TryNumber++;
                controlUser.Blocked = controlUser.TryNumber == 3 ? true : false;

                await _controlUserAccessRepository.Update(controlUser);
            }
        }

        private async Task<bool> CheckTemporaryBlock(User usuario)
        {
            var userControl = await _controlUserAccessRepository.GetItem(usuario.Email);
            var tempoDecorrido = DateTime.Now.Minute - userControl.LastAccess.Minute;
            if (tempoDecorrido > 5)
            {
                userControl.TryNumber = 0;
                userControl.LastAccess = DateTime.Now;
                userControl.Blocked = false;

                await _controlUserAccessRepository.Update(userControl);

                return false;
            }

            return true;
        }

        #endregion
    }
}
