namespace Final4.DTO.Account
{
    public class ResetPassword
    {
        public string NewPassword { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
