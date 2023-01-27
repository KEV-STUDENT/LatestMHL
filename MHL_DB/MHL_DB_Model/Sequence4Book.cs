using MHLCommon.DataModels;
using System.ComponentModel.DataAnnotations;

namespace MHL_DB_Model
{
    public interface ISequenceDB : ICommonSequence
    {
        int Id { get; set; }
        List<Volume> Volumes { get; set; }
    }

    public class Sequence4Book: ISequenceDB
    {
        [Required]
        public string Name { get; set; }
        public int Id { get; set; }
        public List<Volume> Volumes { get; set; }
        public Sequence4Book()
        {
            Volumes = new List<Volume>();
        }
        string ICommonSequence.Name { get => Name; set => Name = value; }
        int ISequenceDB.Id { get => Id; set => Id = value; }
        List<Volume> ISequenceDB.Volumes { get => Volumes; set => Volumes=value; }
    }
}