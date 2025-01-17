using UnityEngine;

namespace GameJam.Modules.Core
{
    public interface IMoveable
    {
        void Move(Vector2 direction);
        void Stop();
    }
}