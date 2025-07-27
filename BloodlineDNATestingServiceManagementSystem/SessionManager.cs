using DAL.Entities;

namespace BloodlineDNATestingServiceManagementSystem
{
    public static class SessionManager
    {
        public static User CurrentUser { get; set; }
    }
}