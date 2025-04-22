using System.Text.Json;

namespace ProjectSpace.Services;

internal sealed class ContactUsService(IWebHostEnvironment environment) : IContactUsService
{
    private readonly IWebHostEnvironment _environment = environment;
    private bool _init = false;

    public async Task<ContactUsRecord?> GetRecordAsync(Guid id)
    {
        EnsureDiectoryExists();
        string path = Path.Combine(_environment.WebRootPath, "ContactUs", $"{id}.json");

        try
        {
            using FileStream stream = File.OpenRead(path);
            return await JsonSerializer.DeserializeAsync<ContactUsRecord>(stream);
        }
        catch (Exception ex) when (ex is FileNotFoundException or DirectoryNotFoundException)
        {
            return null;
        }
    }

    public async Task<ICollection<ContactUsRecord>> GetAllRecords()
    {
        EnsureDiectoryExists();
        List<ContactUsRecord> records = [];
        string path = Path.Combine(_environment.WebRootPath, "ContactUs");

        foreach (var filePath in Directory.GetFiles(path))
        {
            try
            {
                using FileStream stream = File.OpenRead(path);
                var record = await JsonSerializer.DeserializeAsync<ContactUsRecord>(stream);
                if (record is not null)
                {
                    records.Add(record);
                }
            }
            catch (FileNotFoundException) { }
        }

        return records;
    }

    public async Task SaveRecord(ContactUsRecord record)
    {
        EnsureDiectoryExists();
        string path = Path.Combine(_environment.WebRootPath, "ContactUs", $"{record.Id}.json");
        using var stream = File.Open(path, FileMode.Create);
        await JsonSerializer.SerializeAsync(stream, record);
    }

    private void EnsureDiectoryExists()
    {
        if (_init)
        {
            return;
        }

        _init = true;
        string path = Path.Combine(_environment.WebRootPath, "ContactUs");
        Directory.CreateDirectory(path);
    }
}

public sealed record ContactUsRecord(Guid Id, string Name, string EmailAddress, string Description, DateTime Timestamp);
