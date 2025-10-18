// Entities/Admin.cs
namespace AnimArt.Entities
{
    public class Admin : User
    {
        public string Permissions { get; set; } // JSON або розділений список прав
        public bool CanManageUsers { get; set; }
        public bool CanManageContent { get; set; }
        public bool CanManageSystem { get; set; }
    }
}
