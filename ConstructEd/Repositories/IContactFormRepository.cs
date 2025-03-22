using ConstructEd.Models;

namespace ConstructEd.Repositories
{
    public interface IContactFormRepository
    {
        Task AddContactFormAsync(ContactForm contactForm);
        Task<IEnumerable<ContactForm>> GetAllContactFormsAsync();
    }
}
