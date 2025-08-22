using AutoMapper;
using ECommerce.Application.DTOs.CommentDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistence.Concrates
{
    public class CommentService : GenericService<Comment, CommentCreateDTO, CommentUpdateDTO, CommentListDTO, CommentDTO>, ICommentService
    {
        private readonly IRepository<Comment> _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppUserService _appUserService;
        private readonly ICommentRepository _commentRepository;

        public CommentService(IRepository<Comment> repository, IUnitOfWork unitOfWork, IMapper mapper,IAppUserService userService,ICommentRepository commentRepository)
            : base(repository, unitOfWork, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _appUserService = userService;
            _commentRepository = commentRepository;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(Guid userId)
        {
            return await _repository.FindConditionAsync(x => x.AppUserId == userId, isTrack: false);
        }

        public async Task<IEnumerable<Comment>> GetRepliesAsync(Guid parentId)
        {
            return await _repository.FindConditionAsync(c => c.ParentCommentId == parentId, isTrack: false);
        }

        public async Task CreateAsync(CommentCreateDTO dto)
        {
            var entity = _mapper.Map<Comment>(dto);
            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<List<Comment>> GetCommentsByProductIdAsync(Guid productId)
        {
            return await _repository
                .GetQuery()
                .Where(c => c.ProductId == productId && !c.IsDeleted)
                .Include(c => c.AppUser)
                .ToListAsync();
        }
        public async Task<CommentCreateDTO> AddCommentAsync(CommentCreateDTO model, Guid userId)
        {
            var user = await _appUserService.GetByIdAsync(userId); // user bilgisi almak için servis üzerinden

            var comment = new Comment
            {
                ProductId = model.ProductId,
                Content = model.Content,
                ParentCommentId = model.ParentCommentId,
                AppUserId =userId
            };

            _commentRepository.Add(comment);
            await _commentRepository.SaveAsync();

            return new CommentCreateDTO
            {
                ProductId = model.ProductId,
                Content = model.Content,
                ParentCommentId = model.ParentCommentId,
                AppUserId =userId
            };
        }

    }
}