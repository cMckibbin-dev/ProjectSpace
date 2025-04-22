using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectSpace.Services;

namespace ProjectSpace.Pages;

public sealed class ContactUsModel(IContactUsService contactUsService) : PageModel
{
    [BindProperty]
    public ContactUsViewModel? ContactUs { get; set; }

    public async Task<IActionResult> OnPost()
    {
        if (ContactUs is null)
        {
            return BadRequest();
        }

        ContactUsRecord record = new(Guid.NewGuid(), ContactUs.Name, ContactUs.Email, ContactUs.Message, DateTime.Now);
        await contactUsService.SaveRecord(record);
        ContactUs = null;
        return RedirectToPage("Index");
    }
}

public sealed class ContactUsViewModel
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Message { get; init; }
}
