using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cache.Fight
{
    public class RoundModel
    {
        /// <summary>
        /// current round who's cards is biggest
        /// </summary>
        public int BiggestUserId { get; set; }

        /// <summary>
        /// current deal player id
        /// </summary>
        public int CurrentUserId { get; set; }

        public void Start(int userId)
        {
            this.CurrentUserId = userId;
            this.BiggestUserId = userId;
        }
    }
}
