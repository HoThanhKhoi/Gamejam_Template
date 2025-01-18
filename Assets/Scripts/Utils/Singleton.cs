using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();
        private static bool _applicationIsQuitting = false;

        [SerializeField] private List<string> deactivationScenes = new List<string>(); // Scenes to deactivate in

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        /// <summary>
        /// The Singleton instance.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning($"[Singleton] Instance of {typeof(T)} already destroyed. Returning null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();

                        if (_instance == null)
                        {
                            GameObject singletonObject = new GameObject($"{typeof(T)} (Singleton)");
                            _instance = singletonObject.AddComponent<T>();
                            DontDestroyOnLoad(singletonObject);
                        }
                    }

                    return _instance;
                }
            }
        }

        /// <summary>
        /// Unity callback when the application quits.
        /// </summary>
        private void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }

        /// <summary>
        /// Unity callback when the singleton is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            _applicationIsQuitting = true;
        }

        /// <summary>
        /// Unity callback when the script is enabled.
        /// </summary>
        protected virtual void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        /// <summary>
        /// Unity callback when the script is disabled.
        /// </summary>
        protected virtual void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        /// <summary>
        /// Handles scene changes to deactivate the singleton GameObject if needed.
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (deactivationScenes.Contains(scene.name))
            {
                gameObject.SetActive(false);
                Debug.Log($"[Singleton] {typeof(T)} deactivated in scene {scene.name}");
            }
            else
            {
                gameObject.SetActive(true);
                Debug.Log($"[Singleton] {typeof(T)} activated in scene {scene.name}");
            }
        }
    }
}