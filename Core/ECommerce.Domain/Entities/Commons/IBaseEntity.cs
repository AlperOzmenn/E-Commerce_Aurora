namespace ECommerce.Domain.Entities.Commons
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime? UpdatedDate { get; set; }
        DateTime? DeletedDate { get; set; }
        bool IsDeleted { get; set; }
        void SoftDelete();
        void Restore();
        void Update();
    }
}