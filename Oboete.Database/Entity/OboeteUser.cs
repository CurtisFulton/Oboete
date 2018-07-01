using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oboete.Database.Entity
{
    [Table("OboeteUser")]
    public class OboeteUser : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OboeteUserID { get; private set; }
        [Required]
        public string UserName { get; private set; }

        public string Email { get; private set; }
        public bool IsEmailConfirmed { get; private set; }

        [Required]
        public string PasswordHash { get; private set; }
        
        public int LoginFailedCount { get; private set; }
        public DateTimeOffset? LockoutEndDateTime { get; private set; }
        
        public Guid SecurityStamp { get; private set; } = Guid.NewGuid();

        #region Constructor

        private OboeteUser() { }
        public OboeteUser(string username, string passwordHash, string email = null, int oboeteUserID = 0)
        {
            OboeteUserID = oboeteUserID;
            UserName = username;
            PasswordHash = passwordHash;
            Email = email;
        }

        #endregion


        #region Public Accessors

        public void UpdatePasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash), $"Password hash cannot be set to null for user: {UserName}");
            SecurityStamp = Guid.NewGuid();
        }

        public void UpdateEmail(string email)
        {
            Email = email;
            // Should changing the email force logged in users to be reset?
            SecurityStamp = Guid.NewGuid();
        }

        public void EmailConfirmed(bool value) => IsEmailConfirmed = value;
        public void AddFailedLogin() => LoginFailedCount++; // Increases the login failed count by 1
        public void UpdateLoginFailedCount(int loginFailedCount) => LoginFailedCount = loginFailedCount;
        public void UpdateLockoutEndDateTime(DateTimeOffset lockoutEndDatetime) => LockoutEndDateTime = lockoutEndDatetime;

        #endregion
    }
}