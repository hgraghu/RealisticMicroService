namespace UserService.ViewModels
{
    public class UserManagerResponseViewModel
    {
        public string Message { get; set; } = String.Empty;
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
