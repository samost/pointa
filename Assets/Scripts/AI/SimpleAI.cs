using System;
using System.Collections;
using System.Linq;
using Point;
using Transition;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace AI
{
    public class SimpleAI: MonoBehaviour
    {
        [SerializeField] private GamePoint[] points;

        [Inject] private TransisionManager _transisionManager;
        private void Start()
        {
            StartCoroutine(MainRoutine());
        }

        private IEnumerator MainRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(10f);
                Attack();
                yield return null;
            }
        }

        private void Attack()
        {
            GamePoint enemy = null;
            GamePoint union = null;

            while (enemy == null && points.Any(p =>p.State == PointState.Enemy))
            {
                int r = Random.Range(0, points.Length);
                if (points[r].State == PointState.Enemy)
                {
                    enemy = points[r];
                }
            }
            
            while (union == null && points.Any(p =>p.State == PointState.Union))
            {
                int r = Random.Range(0, points.Length);
                if (points[r].State == PointState.Union)
                {
                    union = points[r];
                }
            }
            
            
            _transisionManager.TryMove(enemy, union);
        }
    }
}