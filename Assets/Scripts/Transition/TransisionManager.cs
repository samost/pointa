using Point;
using UnityEngine;

namespace Transition
{
    public class TransisionManager : MonoBehaviour
    {
        public static TransisionManager Instance = null;

        void Start()
        {
            Init();
        }

        public void TryMove(GamePoint from, GamePoint to)
        {
            if (to.State == PointState.Enemy)
            {
                int count = from.UnitsCount;

                if (count >=5)
                {
                    int countWaves = count / 5;
                    bool canConquer = countWaves * 5 > to.UnitsCount;
                    
                    from.StartUnitsMove(countWaves, to.gameObject.transform.position);
                    to.RemoveDelay(countWaves, canConquer);
                }
            }

            if (to.State == PointState.Neutral)
            {
                from.StartUnitsMove(1, to.gameObject.transform.position);
                to.RemoveDelay(1, false);
                to.ChangeState(PointState.Union);
            }

            if (to.State == PointState.Union)
            {
                from.StartUnitsMove(1, to.gameObject.transform.position);
                to.AIREmove();
            }
        }

        private void Init()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance == this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}