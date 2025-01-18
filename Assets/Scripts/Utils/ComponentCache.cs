using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class ComponentCache
    {
        #region ___FIELDS___

        private static ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, Component>> cache;

        private static ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, Component>> cacheInChildren;
        private static ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, Component[]>> cacheInChildrenArray;

        private static ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, Component>> cacheInParent;
        private static ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, Component[]>> cacheInParentArray;

        private static ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, object>> interfaceCache;
        private static ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, object>> interfaceCacheInParent;

        private static ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, object>> interfaceCacheInChildren;
        private static ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, object[]>> interfaceCacheInChildrenArray;

        #endregion

        #region ___METHODS FOR COMPONENTS___

        public static bool TryComponentGetInParent<T>(GameObject gameObject, out T target) where T : Component
        {
            target = GetComponentInParent<T>(gameObject);
            return target != null;
        }

        public static bool TryGetComponent<T>(GameObject gameObject, out T target) where T : Component
        {
            target = GetComponent<T>(gameObject);
            return target != null;
        }

        public static T GetComponent<T>(GameObject gameObject, bool clearEmpty = false) where T : Component
        {
            if (gameObject == null) return null;

            cache ??= new ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, Component>>();
            if (cache.TryGetValue(gameObject, out var typeDictionary) &&
                typeDictionary.TryGetValue(typeof(T), out var cachedComponent) &&
                cachedComponent != null)
            {
                return (T)cachedComponent;
            }

            // Remove empty entries if needed
            if (clearEmpty && typeDictionary?.Count == 0)
            {
                cache.TryRemove(gameObject, out _);
            }

            T component = gameObject.GetComponent<T>();
            if (component != null)
            {
                AddToCache(gameObject, component);
            }

            return component;
        }

        public static T TryGeComponentInChildren<T>(GameObject gameObject, out T component, bool includeInActive = false) where T : Component
        {
            component = GetComponentInChildren<T>(gameObject, includeInActive);
            return component != null ? component : null;
        }
        public static T TryGetComponentInChildren<T>(GameObject gameObject, out T component, int ndepth) where T : Component
        {
            component = GetComponentInChildren<T>(gameObject, ndepth);
            return component != null ? component : null;
        }
        public static T GetComponentInChildren<T>(GameObject gameObject, bool includeInActive = false) where T : Component
        {
            if (gameObject == null) return null;

            cacheInChildren ??= new ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, Component>>();
            if (cacheInChildren.TryGetValue(gameObject, out var typeDictionary) &&
                typeDictionary.TryGetValue(typeof(T), out var cachedComponent) &&
                cachedComponent != null)
            {
                return (T)cachedComponent;
            }

            T component = gameObject.GetComponentInChildren<T>(includeInActive);
            if (component != null)
            {
                AddToCacheInChildren(gameObject, component);
            }

            return component;
        }

        public static T[] GetComponentsInChildren<T>(GameObject gameObject, bool includeInactive = false) where T : Component
        {
            if (gameObject == null) return null;

            cacheInChildrenArray ??= new ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, Component[]>>();
            if (cacheInChildrenArray.TryGetValue(gameObject, out var typeDictionary) &&
                typeDictionary.TryGetValue(typeof(T), out var cachedComponentArray) &&
                cachedComponentArray != null)
            {
                return cachedComponentArray as T[];
            }

            T[] components = gameObject.GetComponentsInChildren<T>(includeInactive);
            if (components.Length > 0)
            {
                AddToCacheInChildrenArray(gameObject, components);
            }

            return components;
        }
        public static T GetComponentInParent<T>(GameObject gameObject) where T : Component
        {
            if (gameObject == null) return null;

            cacheInParent ??= new ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, Component>>();
            if (cacheInParent.TryGetValue(gameObject, out var typeDictionary) &&
                typeDictionary.TryGetValue(typeof(T), out var cachedComponent) &&
                cachedComponent != null)
            {
                return (T)cachedComponent;
            }

            T component = gameObject.GetComponentInParent<T>();
            if (component != null)
            {
                AddToCacheInParent(gameObject, component);
            }

            return component;
        }

        public static T[] GetComponentsInParent<T>(GameObject gameObject) where T : Component
        {
            if (gameObject == null) return null;

            cacheInParentArray ??= new ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, Component[]>>();
            if (cacheInParentArray.TryGetValue(gameObject, out var typeDictionary) &&
                typeDictionary.TryGetValue(typeof(T), out var cachedComponentArray) &&
                cachedComponentArray != null)
            {
                return cachedComponentArray as T[];
            }

            T[] components = gameObject.GetComponentsInParent<T>();
            if (components.Length > 0)
            {
                AddToCacheInParentArray(gameObject, components);
            }

            return components;
        }
        /// <summary>
        /// Gets the component of type T in the nth-level child of the given GameObject.
        /// </summary>
        public static T GetComponentInChildren<T>(GameObject gameObject, int depth) where T : Component
        {
            if (gameObject == null) return null;

            cacheInChildren ??= new ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, Component>>();

            if (cacheInChildren.TryGetValue(gameObject, out var typeDictionary) &&
                typeDictionary.TryGetValue(typeof(T), out var cachedComponent) &&
                cachedComponent != null)
            {
                return (T)cachedComponent;
            }

            // Search in nth-depth child
            T component = FindComponentInNthDepthChild<T>(gameObject.transform, depth);

            if (component != null)
            {
                AddToCacheInChildren(gameObject, component);
            }

            return component;
        }

        /// <summary>
        /// Gets all components of type T in the nth-level children of the given GameObject.
        /// </summary>
        public static T[] GetComponentsInChildren<T>(GameObject gameObject, int depth, bool includeInactive = false) where T : Component
        {
            if (gameObject == null) return null;

            cacheInChildrenArray ??= new ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, Component[]>>();

            if (cacheInChildrenArray.TryGetValue(gameObject, out var typeDictionary) &&
                typeDictionary.TryGetValue(typeof(T), out var cachedComponentArray) &&
                cachedComponentArray != null)
            {
                return cachedComponentArray as T[];
            }

            // Search in nth-depth children
            List<T> components = FindComponentsInNthDepthChildren<T>(gameObject.transform, depth, includeInactive);

            if (components.Count > 0)
            {
                AddToCacheInChildrenArray(gameObject, components.ToArray());
            }

            return components.ToArray();
        }
        #endregion

        #region ___INTERFACE METHODS___
        /// <summary>
        /// Gets the interface of type T from the nth-level child of the given GameObject.
        /// </summary>
        public static T GetInterfaceInChildren<T>(GameObject gameObject, int depth) where T : class
        {
            if (gameObject == null) return null;

            interfaceCacheInChildren ??= new ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, object>>();

            if (interfaceCacheInChildren.TryGetValue(gameObject, out var typeDictionary) &&
                typeDictionary.TryGetValue(typeof(T), out var cachedInterface) &&
                cachedInterface != null)
            {
                return cachedInterface as T;
            }

            // Search in nth-depth child
            T foundInterface = FindInterfaceInNthDepthChild<T>(gameObject.transform, depth);

            if (foundInterface != null)
            {
                AddToInterfaceCacheInChildren(gameObject, foundInterface);
            }

            return foundInterface;
        }

        /// <summary>
        /// Gets all interfaces of type T from the nth-level children of the given GameObject.
        /// </summary>
        public static T[] GetInterfacesInChildren<T>(GameObject gameObject, int depth, bool includeInactive = false) where T : class
        {
            if (gameObject == null) return null;

            interfaceCacheInChildrenArray ??= new ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, object[]>>();

            if (interfaceCacheInChildrenArray.TryGetValue(gameObject, out var typeDictionary) &&
                typeDictionary.TryGetValue(typeof(T), out var cachedInterfaceArray) &&
                cachedInterfaceArray != null)
            {
                return cachedInterfaceArray as T[];
            }

            // Search in nth-depth children
            List<T> foundInterfaces = FindInterfacesInNthDepthChildren<T>(gameObject.transform, depth, includeInactive);

            if (foundInterfaces.Count > 0)
            {
                AddToInterfaceCacheInChildrenArray(gameObject, foundInterfaces.ToArray());
            }

            return foundInterfaces.ToArray();
        }

        public static bool TryGetInterface<T>(GameObject gameObject, out T target) where T : class
        {
            target = GetInterface<T>(gameObject);
            return target != null;
        }
        public static T GetInterface<T>(GameObject gameObject) where T : class
        {
            if (gameObject == null) return null;

            interfaceCache ??= new ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, object>>();
            if (interfaceCache.TryGetValue(gameObject, out var typeDictionary) &&
                typeDictionary.TryGetValue(typeof(T), out var cachedInterface) &&
                cachedInterface != null)
            {
                return cachedInterface as T;
            }

            T component = gameObject.GetComponent(typeof(T)) as T;
            if (component != null)
            {
                AddToInterfaceCache(gameObject, component);
            }

            return component;
        }
        public static bool TryGetInterfaceInParent<T>(GameObject gameObject, out T target) where T : class
        {
            target = GetInterfaceInParent<T>(gameObject);
            return target != null;
        }
        public static T GetInterfaceInParent<T>(GameObject gameObject) where T : class
        {
            if (gameObject == null) return null;

            interfaceCacheInParent ??= new ConcurrentDictionary<GameObject, ConcurrentDictionary<Type, object>>();
            if (interfaceCacheInParent.TryGetValue(gameObject, out var typeDictionary) &&
                typeDictionary.TryGetValue(typeof(T), out var cachedComponent) &&
                cachedComponent != null)
            {
                return (T)cachedComponent;
            }

            T component = gameObject.GetComponentInParent<T>();
            if (component != null)
            {
                AddToInterfaceCacheInParent(gameObject, component);
            }

            return component;
        }

        #endregion

        #region ___CACHE HELPER METHODS___
        private static T FindComponentInNthDepthChild<T>(Transform parent, int depth) where T : Component
        {
            if (depth < 0 || parent == null) return null;

            if (depth == 0)
            {
                return parent.GetComponent<T>();
            }

            foreach (Transform child in parent)
            {
                T component = FindComponentInNthDepthChild<T>(child, depth - 1);
                if (component != null)
                {
                    return component;
                }
            }

            return null;
        }

        private static List<T> FindComponentsInNthDepthChildren<T>(Transform parent, int depth, bool includeInactive) where T : Component
        {
            List<T> components = new List<T>();

            if (depth < 0 || parent == null) return components;

            if (depth == 0)
            {
                T[] foundComponents = parent.GetComponents<T>();
                if (includeInactive)
                {
                    components.AddRange(foundComponents);
                }
                else
                {
                    foreach (T component in foundComponents)
                    {
                        if (component.gameObject.activeSelf)
                        {
                            components.Add(component);
                        }
                    }
                }
            }
            else
            {
                foreach (Transform child in parent)
                {
                    components.AddRange(FindComponentsInNthDepthChildren<T>(child, depth - 1, includeInactive));
                }
            }

            return components;
        }

        private static T FindInterfaceInNthDepthChild<T>(Transform parent, int depth) where T : class
        {
            if (depth < 0 || parent == null) return null;

            if (depth == 0)
            {
                foreach (var component in parent.GetComponents<Component>())
                {
                    if (component is T targetInterface)
                    {
                        return targetInterface;
                    }
                }
            }
            else
            {
                foreach (Transform child in parent)
                {
                    T foundInterface = FindInterfaceInNthDepthChild<T>(child, depth - 1);
                    if (foundInterface != null)
                    {
                        return foundInterface;
                    }
                }
            }

            return null;
        }

        private static List<T> FindInterfacesInNthDepthChildren<T>(Transform parent, int depth, bool includeInactive) where T : class
        {
            List<T> foundInterfaces = new List<T>();

            if (depth < 0 || parent == null) return foundInterfaces;

            if (depth == 0)
            {
                foreach (var component in parent.GetComponents<Component>())
                {
                    if (component is T targetInterface && (includeInactive || component.gameObject.activeSelf))
                    {
                        foundInterfaces.Add(targetInterface);
                    }
                }
            }
            else
            {
                foreach (Transform child in parent)
                {
                    foundInterfaces.AddRange(FindInterfacesInNthDepthChildren<T>(child, depth - 1, includeInactive));
                }
            }

            return foundInterfaces;
        }

        private static void AddToInterfaceCacheInChildren<T>(GameObject gameObject, T foundInterface) where T : class
        {
            var typeDictionary = interfaceCacheInChildren.GetOrAdd(gameObject, _ => new ConcurrentDictionary<Type, object>());
            typeDictionary[typeof(T)] = foundInterface;
        }

        private static void AddToInterfaceCacheInChildrenArray<T>(GameObject gameObject, T[] foundInterfaces) where T : class
        {
            var typeDictionary = interfaceCacheInChildrenArray.GetOrAdd(gameObject, _ => new ConcurrentDictionary<Type, object[]>());
            typeDictionary[typeof(T)] = foundInterfaces;
        }
        private static void AddToCache<T>(GameObject gameObject, T component) where T : Component
        {
            var typeDictionary = cache.GetOrAdd(gameObject, _ => new ConcurrentDictionary<Type, Component>());
            typeDictionary[typeof(T)] = component;
        }

        private static void AddToCacheInChildren<T>(GameObject gameObject, T component) where T : Component
        {
            var typeDictionary = cacheInChildren.GetOrAdd(gameObject, _ => new ConcurrentDictionary<Type, Component>());
            typeDictionary[typeof(T)] = component;
        }

        private static void AddToCacheInChildrenArray<T>(GameObject gameObject, T[] components) where T : Component
        {
            var typeDictionary = cacheInChildrenArray.GetOrAdd(gameObject, _ => new ConcurrentDictionary<Type, Component[]>());
            typeDictionary[typeof(T)] = components;
        }

        private static void AddToCacheInParent<T>(GameObject gameObject, T component) where T : Component
        {
            var typeDictionary = cacheInParent.GetOrAdd(gameObject, _ => new ConcurrentDictionary<Type, Component>());
            typeDictionary[typeof(T)] = component;
        }

        private static void AddToCacheInParentArray<T>(GameObject gameObject, T[] components) where T : Component
        {
            var typeDictionary = cacheInParentArray.GetOrAdd(gameObject, _ => new ConcurrentDictionary<Type, Component[]>());
            typeDictionary[typeof(T)] = components;
        }

        private static void AddToInterfaceCache<T>(GameObject gameObject, T component) where T : class
        {
            var typeDictionary = interfaceCache.GetOrAdd(gameObject, _ => new ConcurrentDictionary<Type, object>());
            typeDictionary[typeof(T)] = component;
        }
        private static void AddToInterfaceCacheInParent<T>(GameObject gameObject, T component) where T : class
        {
            var typeDictionary = interfaceCacheInParent.GetOrAdd(gameObject, _ => new ConcurrentDictionary<Type, object>());
            typeDictionary[typeof(T)] = component;
        }
        #endregion

        #region ___DISPOSE METHODS___

        public static void ClearCache()
        {
            cache?.Clear();
            cache = null;

            cacheInChildren?.Clear();
            cacheInChildren = null;
            cacheInChildrenArray?.Clear();
            cacheInChildrenArray = null;

            cacheInParent?.Clear();
            cacheInParent = null;
            cacheInParentArray?.Clear();
            cacheInParentArray = null;

            interfaceCache?.Clear();
            interfaceCache = null;
            interfaceCacheInParent?.Clear();
            interfaceCacheInParent = null;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSceneLoad()
        {
            ClearCache(); // Clear cache when a new scene is loaded
        }

        #endregion
    }
}
