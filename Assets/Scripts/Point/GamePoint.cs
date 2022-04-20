using System.Collections;
using DG.Tweening;
using Infrustructure;
using MyPooler;
using TMPro;
using Units;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace Point
{
    public class GamePoint : MonoBehaviour
    {
        private const int UnitInWaveCount = 5; 
        
        public PointState State;

        [SerializeField]
        private SpriteRenderer _sprite;
        [SerializeField] 
        private int _startCount = 0;
        [SerializeField]
        private TextMeshProUGUI _countUILabel = null;
        [SerializeField] 
        private Transform[] _spawnPoints = null;

        private int _multiplierValue;
        private int _maxNeturalCount;
        private int _maxDependentCount;
        private float _animationDelay;
        private int _unitsCount;
        private Coroutine _mainRoutine;
        

        private int MaxCount
        {
            get
            {
                switch (State)
                {
                    case PointState.Union:
                        return _maxDependentCount;
                    case PointState.Enemy:
                        return _maxDependentCount;
                    case PointState.Neutral:
                        return _maxNeturalCount;
                }

                return -1;
            }
        }
        public int UnitsCount
        {
            get
            {
                return _unitsCount;
            } 
            private set
            {
                if (value >= 0)
                {
                    if (value >= MaxCount)
                    {
                        value = MaxCount;
                    }
                    
                    _unitsCount = value;
                    _countUILabel.text = _unitsCount.ToString();
                }
            } 
        }

        [Inject]
        private void LoadBalance(GameConfig gameConfig)
        {
            _maxNeturalCount = gameConfig.maxNeutralCount;
            _maxDependentCount = gameConfig.maxDependentCount;
            _multiplierValue = gameConfig.multiplierValue;
            _animationDelay = gameConfig.globalAnimationDelay;
        }
        
        private void Start()
        {
            UnitsCount = _startCount;
            _mainRoutine = StartCoroutine(MainRoutine());
        }

        private IEnumerator MainRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_animationDelay);
                UnitsCount += _multiplierValue;
                yield return null;
            }
        }

        public void StartUnitsMove(int wavesCount, Vector3 target)
        {
            StartCoroutine(StartUnits(wavesCount, target));
        }
        
        private IEnumerator StartUnits(int wavesCount, Vector3 target)
        {
            for (int i = 0; i < wavesCount; i++)
            {
                UnitsCount -= UnitInWaveCount;

                for (int j = 0; j < UnitInWaveCount; j++)
                {
                    var u = ObjectPooler.Instance.GetFromPool("Unit", _spawnPoints[j].position, quaternion.identity);
                    u.transform.DOMove(target, _animationDelay).OnComplete((() => ObjectPooler.Instance.ReturnToPool("Unit", u)));
                }
                
                yield return new WaitForSeconds(_animationDelay);
            }
        }
        

        public void ApplyTransaction(Transaction transaction)
        {
            StartCoroutine(TransactionRoutine(transaction));
        }

        private IEnumerator TransactionRoutine(Transaction transaction)
        {
            for (int i = 0; i < transaction.Value; i++)
            {
                yield return new WaitForSeconds(_animationDelay);

                ValidateTransaction(transaction);
            }
            
        }

        private void ValidateTransaction(Transaction transaction)
        {
            int result = 0;

            if (transaction.CreatorType != State)
            {
                result = State == PointState.Neutral ? UnitsCount + UnitInWaveCount : UnitsCount - UnitInWaveCount;
                
                if (result < 0 || State == PointState.Neutral)
                {
                    SwitchState(transaction.CreatorType);
                }
            }
            else
            {
                result = UnitsCount + UnitInWaveCount;
            }

            UnitsCount = Mathf.Abs(result);
        }

        private void SwitchState(PointState to)
        {
            State = to;

            if (to == PointState.Enemy)
            {
                _sprite.color = Color.red;
            }
            else if (to == PointState.Union)
            {
                _sprite.color = Color.green;
            }
            else if (to == PointState.Neutral)
            {
                _sprite.color = Color.white;
            }
        }
    }
}