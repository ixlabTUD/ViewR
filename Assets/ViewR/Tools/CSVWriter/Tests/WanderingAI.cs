// Based on https://forum.unity.com/threads/solved-random-wander-ai-using-navmesh.327950/#post-2130674
// Modified by F. Schier

using UnityEngine;
using UnityEngine.AI;

namespace ViewR.Tools.CSVWriter.Tests
{
    public class WanderingAI : MonoBehaviour
    {
        public float wanderRadius;
        public float wanderTimer;

        private Transform _target;
        private NavMeshAgent _agent;
        private float _timer;

        private void OnEnable()
        {
            _agent = GetComponent<NavMeshAgent>();
            _timer = wanderTimer;
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= wanderTimer)
            {
                var newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                _agent.SetDestination(newPos);
                _timer = 0;
            }
        }

        private static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
        {
            var randDirection = Random.insideUnitSphere * dist;

            randDirection += origin;

            NavMeshHit navHit;

            NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

            return navHit.position;
        }
    }
}