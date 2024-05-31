using System;
using System.Linq;
using _3._Scripts.Pets.Scriptables;
using UnityEngine;

namespace _3._Scripts.Pets
{
    public class Pet : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float followDistance;
        [SerializeField] public float separationDistance = 1f;
        private Vector3 _targetPosition;

        private Transform _player;
        public PetData Data { get; private set; }

        public void SetData(PetData data) => Data = data;

        public void Activate() => PetData.Activate();
        public void Deactivate() => PetData.Deactivate();

        private void Start()
        {
            _player = Player.Player.Instance.transform;
        }

        private void Update()
        {
            var playerPosition = _player.position;

            var directionToPlayer = playerPosition - transform.position;

            _targetPosition = playerPosition - directionToPlayer.normalized * followDistance;

            foreach (var separationDirection in from pet in Player.Player.Instance.PetsHandler.Pets
                where pet != this
                select transform.position - pet.transform.position)
            {
                var direction = separationDirection;
                direction.y = 0f;
                if (direction.magnitude < separationDistance)
                {
                    _targetPosition += direction.normalized *
                                       (separationDistance - direction.magnitude);
                }
            }

            var distanceToPlayer = directionToPlayer.magnitude;
            var moveSpeed = Mathf.Lerp(speed * 0.1f, speed * 1.25f, distanceToPlayer / followDistance);

            transform.position = Vector3.Lerp(transform.position, _targetPosition, moveSpeed * Time.deltaTime);

            if (directionToPlayer.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionToPlayer),
                    moveSpeed * Time.deltaTime);
            }
        }
    }
}