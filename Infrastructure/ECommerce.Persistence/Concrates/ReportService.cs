using AutoMapper;
using ECommerce.Application.DTOs.ReportDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.UnitOfWorks;
using ECommerce.Persistence.Concrates;

namespace ECommerce.Persistence.Services
{
    public class ReportService : GenericService<Report, ReportCreateDTO, ReportUpdateDTO, ReportListDTO, ReportDTO>, IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Report> _repository;

        public ReportService(IRepository<Report> repository, IUnitOfWork unitOfWork, IMapper mapper)
            : base(repository ,unitOfWork, mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReportListDTO>> GetReportsByUserMailAsync(string mail)
        {
            var reports = await _repository.FindConditionAsync(x => x.Mail == mail && !x.IsDeleted);
            return _mapper.Map<IEnumerable<ReportListDTO>>(reports);
        }
    }
}
