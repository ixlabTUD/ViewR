using System.Collections.Generic;
using UnityEngine;

namespace ViewR.HelpersLib.Extensions.General
{
    public static class GameObjectExtensionMethods
    {
        public static bool HasComponent<T>(this GameObject obj)
        {
            //return (obj.GetComponent<T>() as Component) != null;
            return obj.GetComponent(typeof(T)) != null;

        }

        /// <summary>
        /// use like this: parentObject.FindComponentsInChildrenWithTag SpriteRenderer ("Your Tag");
        /// Alternative: use LINQ: var comps = GetComponentsInChildren().Where(r => r.tag == "Your Tag").ToArray();
        /// </summary>
        /// modified from: http://answers.unity.com/answers/893984/view.html
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="tag"></param>
        /// <param name="forceActive"></param>
        /// <returns>all components in all children with given Tag.</returns>
        public static T[] FindComponentsInChildrenWithTag<T>(this GameObject parent, string tag, bool forceActive = false) where T : Component
        {
            if (parent == null) { throw new System.ArgumentNullException(); }
            if (string.IsNullOrEmpty(tag) == true) { throw new System.ArgumentNullException(); }
            var list = new List<T>(parent.GetComponentsInChildren<T>(forceActive));
            if (list.Count == 0) { return null; }

            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].CompareTag(tag) == false)
                {
                    list.RemoveAt(i);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// use like this: parentObject.FindComponentInChildWithTag SpriteRenderer ("Your Tag");
        /// Alternative: use LINQ: var comps = GetComponentsInChildren().Where(r => r.tag == "Your Tag").ToArray()[0];
        /// </summary>
        /// modified from: http://answers.unity.com/answers/893984/view.html
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="tag"></param>
        /// <param name="includeInactive"> include not active children</param>
        /// <returns>First component in all children with given Tag.</returns>
        public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag, bool includeInactive = true) where T : Component
        {
            if (parent == null) { throw new System.ArgumentNullException(); }
            if (string.IsNullOrEmpty(tag) == true) { throw new System.ArgumentNullException(); }

            var list = parent.GetComponentsInChildren<T>(includeInactive);
            if (list.Length == 0) { return null; }

            foreach (var t in list)
            {
                if (t.CompareTag(tag) == true)
                {
                    return t;
                }
            }
            return null;
        }
    }


}