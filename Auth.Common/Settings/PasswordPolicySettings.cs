namespace Auth.Common.Settings
{
    public class PasswordPolicySettings
    {
        public int MinLengthCount { get; set; }
        public int MaxLengthCount { get; set; }
        public int MinLowercaseCount { get; set; }
        public int MinUppercaseCount { get; set; }
        public int MinNumberCount { get; set; }
        public bool HasSpecialCharacter { get; set; }
    }
}
