using Point;

namespace Units
{
    public class Transaction
    {
        public PointState CreatorType
        {
            get;
            private set;
        }
        public int Value
        {
            get;
            private set;
        }
        
        public Transaction(PointState creatorType, int value)
        {
            CreatorType = creatorType;
            Value = value;
        }

        public void ConvertType(PointState to)
        {
            CreatorType = to;
        }
        
    }
}