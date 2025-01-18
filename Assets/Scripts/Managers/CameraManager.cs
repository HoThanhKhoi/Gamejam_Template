using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
	public CinemachineCamera topDownCamera;
	public CinemachineCamera platformerCamera;

	private CinemachineCamera currentCamera;

	void Start()
	{
		SetupCameraForLevel();
	}

	void SetupCameraForLevel()
	{
		// Example logic: Adjust this to fit your level structure
		string levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

		if (levelName.Contains("TopDown"))
		{
			ActivateCamera(topDownCamera);
		}
		else if (levelName.Contains("Platformer"))
		{
			ActivateCamera(platformerCamera);
		}
	}

	void ActivateCamera(CinemachineCamera cameraToActivate)
	{
		if (currentCamera != null)
		{
			currentCamera.Priority = 0; // Lower the priority of the current camera
		}

		currentCamera = cameraToActivate;
		currentCamera.Priority = 10; // Increase the priority of the new camera
	}
}
