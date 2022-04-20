using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MyPooler;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Point
{
    public class GamePoint : MonoBehaviour
    {
        [SerializeField] 
        private int _startCount = 0;
        [SerializeField]
        private int _maxNeturalCount;
        [SerializeField]
        private int _maxUnionCount;

        [SerializeField] 
        private int _multiplierValue;

        [SerializeField]
        private SpriteRenderer _sprite;

        public PointState State;

        [SerializeField] private TextMeshProUGUI _countUILabel = null;
        [SerializeField] private Transform[] _spawnPoints = null;

        private int unitsCount;
        private Coroutine _mainRoutine;

        private int MaxCount
        {
            get
            {
                switch (State)
                {
                    case PointState.Union:
                        return _maxUnionCount;
                    case PointState.Enemy:
                        return _maxUnionCount;
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
                return unitsCount;
            } 
            private set
            {
                if (value >= 0)
                {
                    if (value >= MaxCount)
                    {
                        value = MaxCount;
                    }
                    
                    unitsCount = value;
                    _countUILabel.text = unitsCount.ToString();
                }
            } 
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
                yield return new WaitForSeconds(2f);
                UnitsCount += _multiplierValue;
                yield return null;
            }
        }

        public void StartUnitsMove(int wavesCount, Vector3 target)
        {
            StartCoroutine(StartUnits(wavesCount, target));
        }

        public void RemoveDelay(int wavesCount, bool canDie)
        {
            StartCoroutine(Remove(wavesCount, canDie));
        }

        public void AIREmove()
        {
            UnitsCount -= 5;
        }

        private IEnumerator Remove(int wavesCount, bool canDie)
        {
            for (int i = 0; i < wavesCount; i++)
            {
                yield return new WaitForSeconds(2f);
                
                if (State == PointState.Enemy)
                {
                   UnitsCount -= 5; 
                }
                else if(State == PointState.Neutral || State == PointState.Union)
                {
                    UnitsCount += 5;
                }
            }

            if (canDie)
            {
                ChangeState(PointState.Union);
            }
        }

        private IEnumerator StartUnits(int wavesCount, Vector3 target)
        {
            for (int i = 0; i < wavesCount; i++)
            {
                UnitsCount -= 5;

                for (int j = 0; j < 5; j++)
                {
                    var u = ObjectPooler.Instance.GetFromPool("Unit", _spawnPoints[j].position, quaternion.identity);
                    u.transform.DOMove(target, 2f).OnComplete((() => ObjectPooler.Instance.ReturnToPool("Unit", u)));
                }
                
                yield return new WaitForSeconds(2f);
            }
        }

        public void ChangeState(PointState to)
        {
            if (unitsCount == 0)
            {
                State = PointState.Neutral;
                _sprite.color = Color.white;
                return;
            }
            else
            {
                if (to == PointState.Enemy)
                {
                    _sprite.color = Color.red;
                    State = PointState.Enemy;
                }
                else if(to == PointState.Union)
                {
                    _sprite.color = Color.green;
                    State = PointState.Union;
                }
            }
            
        }
    }
}