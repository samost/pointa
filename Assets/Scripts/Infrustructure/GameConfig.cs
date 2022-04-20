using UnityEngine;

namespace Infrustructure
{
    [CreateAssetMenu(fileName = "Config", menuName = "ScriptableObjects/GameConfig", order = 1)]
    public class GameConfig : ScriptableObject
    {
        public int maxNeutralCount;
        public int maxDependentCount;

        public float globalAnimationDelay;
        public int multiplierValue;
    }
}