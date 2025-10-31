using UnityEngine;

namespace BS.GameObjects
{
    public class SnailMover : AbstractCharacterMover
    {
        public override void Move(Vector2 direction, float speed)
        {
            base.Move(direction, speed);
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}