using CDNAPI.freelanceCDNModel;

namespace CDNAPI.Controllers
{
    public class Classes
    {
    }
    public class Err
    {
        public bool HasError { get; set; }

        public string ErrMsg { get; set; }
    }
    public class User
    {
        public User()
        {
            Skill = new List<Skills>();
            Hobby = new List<Hobbies>();
        }
        public Int32 UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }
        public List<Skills> Skill { get; set; }
        public List<Hobbies> Hobby { get; set; }
    }
    public class Skills
    {
        public Int32? SkillID { get; set; }
        public string? SkillName { get; set; }
    }
    public class Hobbies
    {
        public Int32? HobbyID { get; set; }
        public string? HobbyName { get; set; }
    }
    public class RegisterParams
    {
        public RegisterParams()
        {
            Skill = new List<Skills>();
            Hobby = new List<Hobbies>();
        }
        public string Username { get; set; }
        public string? Email { get; set; }
        public string? PhoneNum { get; set; }
        public List<Skills> Skill { get; set; }
        public List<Hobbies> Hobby { get; set; }
    }
    public class UpdateParams
    {
        public UpdateParams()
        {
            Skill = new List<Skills>();
            Hobby = new List<Hobbies>();
        }
        public string? Email { get; set; }
        public string? PhoneNum { get; set; }
        public List<Skills> Skill { get; set; }
        public List<Skills> SkillToRemove { get; set; }
        public List<Hobbies> Hobby { get; set; }
        public List<Hobbies> HobbyToRemove { get; set; }
    }
    public class Users
    {
        public Users()
        {
            User = new List<User>();
        }
        public List<User> User { get; set; }
        public Int32 TotalRecord { get; set; }
    }
}
