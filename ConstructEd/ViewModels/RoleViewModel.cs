using System.ComponentModel.DataAnnotations;

namespace ConstructEd.ViewModels
{
    public enum Role
    {
        Admin, Instructor, User
    }
    public class RoleViewModel
    {
        public Role UserRole { get; set; }
    }
}
