using ECommerce.Application.DTOs.ReportDTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IReportService : IGenericService<Report, ReportCreateDTO, ReportUpdateDTO, ReportListDTO, ReportDTO>
    {
        
        Task<IEnumerable<ReportListDTO>> GetReportsByUserMailAsync(string mail);
    }
}

