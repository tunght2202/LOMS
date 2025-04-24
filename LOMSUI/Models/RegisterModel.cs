namespace LOMSUI.Models
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string FullName { get; set; }

        public byte[] AvatarData { get; set; }

        public string AvatarFileName { get; set; }

        public string AvatarContentType { get; set; }
    }
<<<<<<< HEAD
}
=======
    public class RegisterResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }

    }


}

>>>>>>> 7dcccd97e68a72de4489f90f8e8b12ae1625b9d2
