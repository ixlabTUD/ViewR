using System.Linq;
using UnityEngine;

namespace ViewR.HelpersLib.Universals.Meshes
{
    /// <summary>
    /// A class that adds a "FlipVertices" functionality to an component in the editor
    /// Largely based on http://answers.unity.com/comments/1874288/view.html
    /// </summary>
    public class FlipVertices : MonoBehaviour
    {
        [ContextMenu("FlipVertices")]
        public void FlipMeshContext()
        {
            var mesh = GetComponent<MeshFilter>().mesh;
            mesh.triangles = mesh.triangles.Reverse().ToArray();
        }
    }
}