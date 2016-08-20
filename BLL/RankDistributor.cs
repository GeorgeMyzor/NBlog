using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class RankDistributor
    {
        private const int ActivityPerRank = 100;
        public static string GetRank(int userActivity)
        {
            int range = Math.Abs((userActivity - 1) / ActivityPerRank);
            switch (range)
            {
                case 0:
                    return "Student";
                case 1:
                    return "Junior";
                case 2:
                    return "Middle";
                case 3:
                    return "Senior";
                case 4:
                    return "Lead";
                case 5:
                default:
                    return "Solution Architecture";

            }
        }
    }
}
