using UnityEngine;

public class KeepZAtZero : MonoBehaviour
{
	void LateUpdate()
	{
		Vector3 pos = transform.position;
		pos.z = 0f;
		transform.position = pos;
	}
}
