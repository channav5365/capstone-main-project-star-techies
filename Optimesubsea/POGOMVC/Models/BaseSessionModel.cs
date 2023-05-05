using System.Diagnostics.CodeAnalysis;

namespace POGOMVC.Models
{
    public static class BaseSessionModel
    {
        public static int UserPK { get; set; }
        public static bool IsValidUser { get; set; }
        public static string UserRole { get; set; }
        public static string UserName { get; set; }
        public static string SecrateKey { get; set; }
    }
}
