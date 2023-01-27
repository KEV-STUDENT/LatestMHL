using MHLCommon.DataModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MHL_DB_Model
{
    public interface IVolumeDB : IVolume
    {
        int Id { get; set; }
        List<Book> Books { get; set; }

        public int SequenceId { get; set; }
        Sequence4Book Sequence { get; set; }
    }
    public class Volume : IVolumeDB
    {
        public int Id { get; set; }
        public List<Book> Books { get; set; }
        [Required]
        public ushort Number { get; set; }

        [ForeignKey("SequenceId")]
        public Sequence4Book Sequence { get; set; }
        [Required]
        public int SequenceId { get; set; }


        public Volume()
        {
            Books= new List<Book>();
        }

        ushort IVolume.Number { get => Number; set => Number = value; }
        int IVolumeDB.Id { get => Id; set => Id = value; }
        List<Book> IVolumeDB.Books { get => Books; set => Books = value; }
        Sequence4Book IVolumeDB.Sequence { get => Sequence; set => Sequence = value; }
        int IVolumeDB.SequenceId { get => SequenceId; set => SequenceId = value; }
    }
}