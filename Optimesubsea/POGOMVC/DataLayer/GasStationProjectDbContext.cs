using Microsoft.EntityFrameworkCore;
using POGOMVC.Models;

namespace POGOMVC.DataLayer
{
    public class GasStationProjectDbContext : DbContext
    {
        public GasStationProjectDbContext()
        {

        }
        public GasStationProjectDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ProjectModel> t_ProjectTable { get; set; }
        public DbSet<RolesModel> m_Roles { get; set; }
        public DbSet<PasscodeRecoveryQuestionnaireModel> m_PasscodeRecoveryQuestionnaire { get; set; }
        public DbSet<UserRegistrationModel> t_UserRegistration { get; set; }
        public DbSet<UserHasProjectsModel> t_UserHasProjects { get; set; }
        public DbSet<FileUploadModel> t_FileUploadModels { get; set; }

    }
}
