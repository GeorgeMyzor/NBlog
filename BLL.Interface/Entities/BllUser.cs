using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Entities
{
    public class BllUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public string Rank { get; set; }
        public int SubscriptionCost { get; set; }
        public byte[] UserPic { get; set; }
        public IEnumerable<BllRole> Roles { get; set; }
        public IEnumerable<BllArticle> Articles { get; set; }
    }
}
