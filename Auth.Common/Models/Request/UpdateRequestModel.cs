namespace Auth.Common.Models.Request
{
    public class UpdateRequestModel
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
