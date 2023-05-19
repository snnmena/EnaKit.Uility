using System.Collections;
using UnityEngine;

namespace Yoziya.manjuu
{
    public class MoveCommand : ICommand
    {
        private Vector3 _direction;
        private Transform _transform;
        private float _speed;

        public MoveCommand(Transform transform, Vector3 direction, float speed)
        {
            _transform = transform;
            _direction = direction;
            _speed = speed;
        }

        public void Execute()
        {
            _transform.Translate(_direction * _speed * Time.deltaTime);
        }

        public void Undo()
        {
            _transform.Translate(-_direction * _speed * Time.deltaTime);
        }
    }
}