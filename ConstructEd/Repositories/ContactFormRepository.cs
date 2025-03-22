using ConstructEd.Data;
using ConstructEd.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructEd.Repositories
{
    public class ContactFormRepository : IContactFormRepository
    {
        private readonly DataContext _context;

        public ContactFormRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddContactFormAsync(ContactForm contactForm)
        {
            await _context.ContactForms.AddAsync(contactForm);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ContactForm>> GetAllContactFormsAsync()
        {
            return await _context.ContactForms.ToListAsync();
        }
    }
}
