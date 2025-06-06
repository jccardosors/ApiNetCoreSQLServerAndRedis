using AutoMapper;
using Project.Application.Interfaces;
using Project.Application.Utils;
using Project.Application.ViewModels;
using Project.Domain;
using Project.Domain.Entities;

namespace Project.Application.Servicos
{
    public class LogsService : ILogsService
    {
        private readonly IMapper _mapper;
        private readonly ILogsRepository _logsRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogsService _logsService;

        public LogsService(IMapper mapper, ILogsRepository logsRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _logsRepository = logsRepository;
            _userRepository = userRepository;
        }

        public async Task<CustomResult<LogsVM>> Add(string email, string classe, string method, string messageError)
        {
            try
            {
                var user = await _userRepository.GetItemByEmail(email.Trim());
                if (user is not null)
                {
                    var log = new Logs
                    {
                        Data = DateTime.Now,
                        Description = $"Usuário {email}, metodo {method}, classe {classe}, Erro {messageError}",
                        IdUser = user.Id
                    };

                    var result = await _logsRepository.Add(log);
                    var resultMap = _mapper.Map<LogsVM>(result);

                    return CustomResult<LogsVM>.Success(resultMap);
                }

                return CustomResult<LogsVM>.Failure(CustomError.RecordNotFound("Erro ao gravar log."));
            }
            catch (Exception ex)
            {
                await _logsRepository.Add(new Logs
                {
                    Data = DateTime.Now,
                    Description = $"Usuário {email}, metodo {method}, classe {classe}, Erro {ex.Message}",
                    IdUser = 1
                });

                return CustomResult<LogsVM>.Failure(CustomError.ExceptionError(ex.Message));
            }
        }

    }
}
