namespace ProjectSpace.Services;

public interface IContactUsService
{
    Task<ICollection<ContactUsRecord>> GetAllRecords();
    Task<ContactUsRecord?> GetRecordAsync(Guid id);
    Task SaveRecord(ContactUsRecord record);
}