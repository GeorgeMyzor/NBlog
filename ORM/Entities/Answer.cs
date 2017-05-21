using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Entities
{
    public class Answer
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int QuestionId { get; set; }
        public bool IsAnswer { get; set; }
        public DateTime PublicationDate { get; set; }

        public virtual User Author { get; set; }
        public virtual Question Question { get; set; }
    }
}
