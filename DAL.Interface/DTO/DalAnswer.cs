using System;

namespace DAL.Interface.DTO
{
    public class DalAnswer : IEntity
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int QuestionId { get; set; }
        public int? AuthorId { get; set; }
        public bool IsAnswer { get; set; }
        public DalUser Author { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
