using UnityEngine;

namespace BS.GameObjects
{
    public interface ICharacterMover 
    {
        public Vector2 ViewDirection { get; }

        public float Velocity { get; }

        public void Move(Vector2 direction, float speed);
        public void Jump(float force);
        public void Stop();
    }
}