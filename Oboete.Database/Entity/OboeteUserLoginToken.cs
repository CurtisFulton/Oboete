using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oboete.Database.Entity
{
    [Table("OboeteUserLoginToken")]
    public class OboeteUserLoginToken : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OboeteUserLoginTokenID { get; private set; }

        public int OboeteUserID { get; private set; }

        // Extra layer of protection. Also allows multiple tokens per user 
        public Guid LoginSelector { get; private set; } = Guid.NewGuid(); 
        public string TokenHash { get; private set; }  

        public DateTimeOffset Expires { get; private set; }

        public OboeteUser OboeteUser { get; private set; }

        #region Constructor

        private OboeteUserLoginToken() { }
        public OboeteUserLoginToken(int oboeteUserID, string tokenHash, DateTimeOffset expries, int oboeteUserLoginTokenID = 0)
        {
            OboeteUserID = oboeteUserID;
            OboeteUserLoginTokenID = oboeteUserLoginTokenID;
        }

        #endregion

        #region Public Accessors

        public void UpdateTokenHash(string tokenHash, DateTimeOffset newExpiry)
        {
            // Whenever the token has is changed, the expiry date should be changed.
            TokenHash = tokenHash;
            Expires = newExpiry;
        }

        #endregion
    }
}