using Point;
using Units;
using UnityEngine;

namespace Transition
{
    public class TransisionManager
    {
        public bool TryMove(GamePoint from, GamePoint to)
        {
            int count = from.UnitsCount;

            if (count >= 5)
            {
                int countWaves = to.State == PointState.Neutral ? 1 : count / 5;

                
                from.StartUnitsMove(countWaves, to.gameObject.transform.position);
                Transaction transaction = new Transaction(from.State, countWaves);
                
                to.ApplyTransaction(transaction);
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}