using MHL_DB_Model;
using MHLCommon.MHLBook;
using Microsoft.IdentityModel.Tokens;


namespace MHL_DB_BizLogic.BLClasses
{
    public class SequenceVolume
    {
        private readonly Sequence4Book _sequence;
        private readonly ushort _volume;

        public SequenceVolume(Sequence4Book sequence, ushort volume)
        {
            _sequence = sequence;
            _volume = volume;
        }

        public Sequence4Book Sequence { get { return _sequence; } }
        public ushort Volume { get { return _volume; } }
    }

    public class BLVolumes : BL4Entity<SequenceVolume, Volume, string>
    {
        #region [Constructors]
        public BLVolumes(DBModel dB) : this(dB, null) { }
        public BLVolumes(DBModel dB, object? locker) : base(dB, locker)
        {
            FilterDuplicatedAttrubute = (List<SequenceVolume> attributes) =>
            {
                return attributes
                     .Where(a => a.Sequence != null || a.Volume > 0)
                     .GroupBy(a => ConvertAttribute(a))
                     .Select(a => a.First()).ToList();
            };
        }
        #endregion

        #region [Methods]
        protected override bool AddInDB(List<Volume>? newEntitys)
        {
            bool ret = !newEntitys.IsNullOrEmpty();

            if (ret)
                DB.Volumes.AddRange(newEntitys);

            return ret;
        }

        protected override List<Volume> CheckInDB(IEnumerable<string> filter)
        {
            return DB.Volumes.Where(v => filter.Contains(v.SequenceId.ToString() + "|" + v.Number.ToString())).ToList();
        }

        protected override string ConvertAttribute(SequenceVolume a)
        {
            return string.Format("{0}|{1}", a.Sequence.Id, a.Volume);
        }

        protected override Volume ConvertAttribute2DBEntity(SequenceVolume a)
        {
            return new Volume() { Number = a.Volume, Sequence = a.Sequence };
        }

        protected override IEnumerable<string>? ConvertAttributes2ComparedList(List<SequenceVolume> attributes)
        {
            return attributes
               .Where(ab => ab.Sequence != null)
               .GroupBy(a => ConvertAttribute(a))
               .Select(a => a.Key);
        }

        protected override string ConvertDBEntity(Volume a)
        {
            return string.Format("{0}|{1}", a.Sequence.Id, a.Number);
        }

        protected override void GetAttributesFromBook(List<SequenceVolume> list, IMHLBook book)
        {
            if (list != null && !(book?.SequenceAndNumber?.Name.IsNullOrEmpty() ?? true))
            {
                Sequence4Book? sequence = DB.Sequence4Books
                   .FirstOrDefault(a => a.Name.Trim().ToLower() == book.SequenceAndNumber.Name.Trim().ToLower());

                if (sequence == null)
                {
                    sequence = new Sequence4Book() { Name = book.SequenceAndNumber.Name };
                    DB.Sequence4Books.Add(sequence);
                }
                list.Add(new SequenceVolume(sequence, book.SequenceAndNumber.Number));
            }
        }
        #endregion
    }
}