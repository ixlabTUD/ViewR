using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ViewR.HelpersLib.Extensions.General
{
    public static class TransformExtensionMethods
    {
        /// <summary>
        /// Recursively iterates over all children of all children.
        /// Use like: transform.FindObjectsWithTag("myTag"); Optionally: .FirstOrDefault(); to return only first.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="tag"></param>
        /// <returns>All children with a certain tag.</returns>
        public static List<GameObject> FindChildrenGameObjectsWithTag(this Transform parent, string tag)
        {
            var taggedGameObjects = new List<GameObject>();

            for (var i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.tag == tag)
                {
                    taggedGameObjects.Add(child.gameObject);
                }
                if (child.childCount > 0)
                {
                    taggedGameObjects.AddRange(FindChildrenGameObjectsWithTag(child, tag));
                }
            }
            return taggedGameObjects;
        }

        /// <summary>
        /// Recursively iterates over all children of all children.
        /// Use like: transform.FindObjectsWithTag("myTag"); Optionally: .FirstOrDefault(); to return only first.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="tag"></param>
        /// <returns>All children transforms with a certain tag.</returns>
        public static List<Transform> FindChildrenTransformsWithTag(this Transform parent, string tag)
        {
            var taggedTransforms = new List<Transform>();

            for (var i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.tag == tag)
                {
                    taggedTransforms.Add(child);
                }
                if (child.childCount > 0)
                {
                    taggedTransforms.AddRange(FindChildrenTransformsWithTag(child, tag));
                }
            }
            return taggedTransforms;
        }
        
        
        public static Transform GetClosestTransform (this Transform fromTransform, Transform[] transforms)
        {
            return GetClosestTransform(transforms, fromTransform.position);
        }
        
        
        public static Transform GetClosestTransform (Transform[] transforms, Vector3 fromPosition)
        {
            Transform nearest = null;
            var closestSqrDist = Mathf.Infinity;
            for (var i = 0; i < transforms.Length; i++)
            {
                var candidateTransform = transforms[i];
                var sqrDist = (candidateTransform.position - fromPosition).sqrMagnitude;
                if (sqrDist < closestSqrDist)
                {
                    closestSqrDist = sqrDist;
                    nearest = candidateTransform;
                }
            }

            return nearest;
        }
        
        public static int? GetClosestObjectIndex (Vector3[] positions, Vector3 fromPosition)
        {
            int? nearest = null;
            var closestSqrDist = Mathf.Infinity;
            for (var i = 0; i < positions.Length; i++)
            {
                var sqrDist = (positions[i] - fromPosition).sqrMagnitude;
                if (sqrDist < closestSqrDist)
                {
                    closestSqrDist = sqrDist;
                    nearest = i;
                }
            }

            return nearest;
        }

        public static Transform GetClosestItemLinq(Transform[] transforms, Vector3 fromPosition)
        {
            return transforms.OrderBy(t => (t.position - fromPosition).sqrMagnitude).FirstOrDefault();
        }

        public static Transform[] GetClosestItemsLinq(Transform[] transforms, Vector3 fromPosition, int numberOfItems = 1)
        {
            Transform[] nNearest;

            if (numberOfItems == 1)
            {
                var nearest = GetClosestItemLinq(transforms, fromPosition);
                nNearest = new Transform[] {nearest};
            }
            else
            {
                nNearest = transforms.OrderBy(t => (t.position - fromPosition).sqrMagnitude)
                    .Take(numberOfItems)
                    .ToArray();
            }

            return nNearest;
        }

    }

}