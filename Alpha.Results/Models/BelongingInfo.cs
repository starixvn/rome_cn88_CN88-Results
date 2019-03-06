namespace Alpha.Results.Site.Models
{
    public class BelongingInfo
    {
        public BelongingInfo(byte roleId, bool isActive)
        {
            RoleId = roleId;
            IsActive = isActive;
        }

        public byte RoleId { get; }

        public bool IsActive { get; }
    }
}