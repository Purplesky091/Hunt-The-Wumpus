using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunt_the_Wumpus_Text_based
{
    /// <summary>
    /// An "Empty" hazard. <para/>
    /// Used to make rooms have "no" hazards <para/>
    /// Essentially does nothing, other than say it's blank.
    /// </summary>
    class BlankHazard: IHazard
    {
        /// <summary>
        /// Blank hazards cannot attack players
        /// </summary>
        /// <param name="player"></param>
        public void Attack(Player player)
        {
            
        }

        public bool IsBlank()
        {
            return true;
        }

        /// <summary>
        /// Blank hazards do not print warnings
        /// </summary>
        public void PrintWarning()
        {
            
        }
    }
}
