using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class BaseEntity
    {
        #region Contructor
        public BaseEntity()
        {
            CreatedOn = DateTime.UtcNow;
        }
        #endregion

        #region Properties

        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } 
        #endregion
    }
}
