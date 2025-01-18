using UnityEngine;

public interface IMoveable
{
    public bool IsMoving { get; set; }
    public void Move();
}
