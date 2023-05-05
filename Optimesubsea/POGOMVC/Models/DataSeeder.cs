using POGOMVC.DataLayer;
using System;

namespace POGOMVC.Models
{

    public class DataSeeder
    {
        private readonly GasStationProjectDbContext _dbContext;

        public DataSeeder(GasStationProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (!_dbContext.m_Roles.Any())
            {
                var roles = new List<RolesModel>()
                            {
                                new RolesModel {
                                    //Id = 1,
                                    IsActive = true,
                                    CreatedBy = 1,
                                    CreatedOn= DateTime.Now,
                                    Narration = "Admin Role Seed",
                                    RoleName = "Admin",
                                    UpdatedBy = 1,
                                    UpdatedOn = DateTime.Now,
                                },
                                new RolesModel {
                                    //Id = 2,
                                    IsActive = true,
                                    CreatedBy = 1,
                                    CreatedOn= DateTime.Now,
                                    Narration = "Super User Role Seed",
                                    RoleName = "Super User",
                                    UpdatedBy = 1,
                                    UpdatedOn = DateTime.Now,
                                },
                                new RolesModel {
                                    //Id = 3,
                                    IsActive = true,
                                    CreatedBy = 1,
                                    CreatedOn= DateTime.Now,
                                    Narration = "End User Role Seed",
                                    RoleName = "End User",
                                    UpdatedBy = 1,
                                    UpdatedOn = DateTime.Now,
                                },
                            };
                _dbContext.m_Roles.AddRange(roles);
                _dbContext.SaveChanges();
            }
            if (!_dbContext.m_PasscodeRecoveryQuestionnaire.Any())
            {
                var passcodeRecovery = new List<PasscodeRecoveryQuestionnaireModel>()
                    {
                        new PasscodeRecoveryQuestionnaireModel()
                        {
                            //Id = 1,
                            IsActive = true,
                            CreatedBy = 1,
                            CreatedOn = DateTime.Now,
                            QuestionName = "What is your nickname?",
                            QuestionNarration = "Enter your Nick Name",
                            UpdatedBy = 1,
                            UpdatedOn = DateTime.Now,
                        },
                        new PasscodeRecoveryQuestionnaireModel()
                        {
                            //Id = 2,
                            IsActive = true,
                            CreatedBy = 1,
                            CreatedOn = DateTime.Now,
                            QuestionName = "What is your first school name?",
                            QuestionNarration = "Enter your first school name",
                            UpdatedBy = 1,
                            UpdatedOn = DateTime.Now,
                        },
                        new PasscodeRecoveryQuestionnaireModel()
                        {
                            //Id = 3,
                            IsActive = true,
                            CreatedBy = 1,
                            CreatedOn = DateTime.Now,
                            QuestionName = "What is your favourite place?",
                            QuestionNarration = "Enter your favourite place",
                            UpdatedBy = 1,
                            UpdatedOn = DateTime.Now,
                        },
                        new PasscodeRecoveryQuestionnaireModel()
                        {
                            //Id = 4,
                            IsActive = true,
                            CreatedBy = 1,
                            CreatedOn = DateTime.Now,
                            QuestionName = "What is your favourite food?",
                            QuestionNarration = "Enter your favourite food",
                            UpdatedBy = 1,
                            UpdatedOn = DateTime.Now,
                        }
                };
                _dbContext.m_PasscodeRecoveryQuestionnaire.AddRange(passcodeRecovery);
                _dbContext.SaveChanges();
            }
            if (!_dbContext.t_UserRegistration.Any())
            {
                var _passcode = EncryptDecrypt.Encrypt("Admin", BaseSessionModel.SecrateKey);
                var userRegistrations = new List<UserRegistrationModel>()
                            {
                                new UserRegistrationModel {
                                    IsActive = true,
                                    CreatedBy = 1,
                                    CreatedOn= DateTime.Now,
                                    UserName = "Admin",
                                     FirstName ="Admin",
                                    Narration = "Admin Role Seed",
                                    EmailId = "admin@admin.com",
                                    Passcode = _passcode,
                                    UserRoleId = _dbContext.m_Roles.FirstOrDefault(a => a.RoleName.ToLower() == "admin"),
                                    LastName = "Admin",
                                    PasscodeRecoveryQuestionnaireId1 = _dbContext.m_PasscodeRecoveryQuestionnaire.FirstOrDefault(a => a.QuestionName == "What is your nickname?"),
                                    PasscodeRecoveryAnswer1 = "admin",
                                    UpdatedBy = 1,
                                    UpdatedOn = DateTime.Now,
                                }
                            };
                _dbContext.t_UserRegistration.AddRange(userRegistrations);
                _dbContext.SaveChanges();
            }
        }
    }
}
