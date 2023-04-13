using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Pixelplacement;
using UnityEditor;
using UnityEngine;
using ViewR.Core.OVR.Passthrough.Highlight;
using ViewR.Core.OVR.Passthrough.Overlay;
using ViewR.Core.OVR.Passthrough.Visuals;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;

namespace ViewR.Managers
{
    public class PassthroughManager : SingletonExtended<PassthroughManager>
    {
        [Help("For performance reasons, these should be OVRPassthroughLayer::hidden, whenever possible.\nFor startup, you can do so by using the \"Settings\" area.", MessageType.Warning)]
        
        [Header("References")]
        public OVRPassthroughLayer mainPassthroughLayer;
        public PassthroughLayerStyler mainPassthroughLayerStyler;
        public PassthroughLayerInOutManager mainPassthroughInOutManager;

        [Space]
        public OVRPassthroughLayer userDefinedPassthroughLayer;
        public PassthroughLayerStyler userDefinedPassthroughLayerStyler;
        public PassthroughLayerInOutManager userDefinedPassthroughInOutManager;

        [Space]
        public OVRPassthroughLayer userDefinedPassthroughLayerHighlighter;
        public PassthroughLayerStyler userDefinedPassthroughLayerHighlighterStyler;
        public PassthroughLayerInOutManager userDefinedPassthroughHighlightInOutManager;

        [Space]
        public OVRPassthroughLayer overlayPassthroughLayer;
        public PassthroughOverlayStyler passthroughOverlayStyler;
        public PassthroughOverlayEdgeOpacityStyler passthroughOverlayEdgeOpacityStyler;
        public PassthroughLayerInOutManager overlayPassthroughLayerInOutManager;
        public PassthroughOverlayManager passthroughOverlayManager;


        [Header("Settings")]
        [SerializeField]
        private bool hideMainLayerOnStartup;
        [SerializeField]
        private bool hideOtherLayersOnStartup;
        [SerializeField]
        private bool hideOverlayLayerOnStartup;
        [SerializeField]
        private bool fadeInMainPassthroughOnStartup = true;

        private int _numberOfReprojectedObjects = 0;
        private int _numberOfHighlightedObjects = 0;

        private void Start()
        {
            if (hideMainLayerOnStartup)
                mainPassthroughLayer.hidden = false;
            else if (fadeInMainPassthroughOnStartup)
                FadeMainPassthrough(true);

            if (hideOtherLayersOnStartup)
                userDefinedPassthroughLayerHighlighter.hidden = userDefinedPassthroughLayer.hidden = false;

            if (hideOverlayLayerOnStartup)
                overlayPassthroughLayer.hidden = false;
        }

        #region Faders: In/Out

        /// <summary>
        /// Fades in this passthrough layer from black to visual or 
        /// fades out and in (visual to black)
        /// </summary>
        public void FadeMainPassthrough(bool fadeIn, bool hideAfterFadeOut = true, bool bailIfAlreadyVisible = false)
        {
            if (bailIfAlreadyVisible && !mainPassthroughLayer.hidden)
                return;

            mainPassthroughInOutManager.FadeInPassthrough(fadeIn, hideAfterFadeOut);
        }

        /// <summary>
        /// Fades in this passthrough layer from black to visual or 
        /// fades out and in (visual to black)
        /// </summary>
        public void FadeReprojectedPassthrough(bool fadeIn, bool hideAfterFadeOut = true, bool bailIfAlreadyVisible = false)
        {
            if (bailIfAlreadyVisible && !userDefinedPassthroughLayer.hidden)
                return;

            userDefinedPassthroughInOutManager.FadeInPassthrough(fadeIn, hideAfterFadeOut);
        }

        /// <summary>
        /// Fades in this passthrough layer from black to visual or 
        /// fades out and in (visual to black)
        /// </summary>
        public void FadeReprojectedHighlightedPassthrough(bool fadeIn, bool hideAfterFadeOut = true, bool bailIfAlreadyVisible = false)
        {
            if (bailIfAlreadyVisible && !userDefinedPassthroughLayerHighlighter.hidden)
                return;

            userDefinedPassthroughHighlightInOutManager.FadeInPassthrough(fadeIn, hideAfterFadeOut);
        }

        /// <summary>
        /// Fades in this passthrough layer from black to visual or 
        /// fades out and in (visual to black)
        /// </summary>
        public void FadeOverlayLayer(bool fadeIn, bool hideAfterFadeOut = true, bool bailIfAlreadyVisible = false)
        {
            if (bailIfAlreadyVisible && !overlayPassthroughLayer.hidden)
                return;

            overlayPassthroughLayerInOutManager.FadeInPassthrough(fadeIn, hideAfterFadeOut);
        }

        #endregion

        #region Faders: In AND Out / Blinking

        /// <summary>
        /// Blink-like effect on this Passthrough layer (from visual to black to visual).
        /// </summary>
        public void BlinkMainPassthrough()
        {
            mainPassthroughInOutManager.FadePassthroughOutAndIn();
        }

        /// <summary>
        /// Blink-like effect on this Passthrough layer (from visual to black to visual).
        /// </summary>
        public void BlinkReprojectedPassthrough()
        {
            userDefinedPassthroughInOutManager.FadePassthroughOutAndIn();
        }

        /// <summary>
        /// Blink-like effect on this Passthrough layer (from visual to black to visual).
        /// </summary>
        public void BlinkReprojectedHighlightedPassthrough()
        {
            userDefinedPassthroughHighlightInOutManager.FadePassthroughOutAndIn();
        }

        /// <summary>
        /// Blink-like effect on this Passthrough layer (from visual to black to visual).
        /// </summary>
        public void BlinkOverlayLayer()
        {
            overlayPassthroughLayerInOutManager.FadePassthroughOutAndIn();
        }

        #endregion

        #region Hide/Show

        /// <summary>
        /// Fades in the main PT layer from black to visual.
        /// </summary>
        public void HideMainPassthrough(bool hideLayer)
        {
            mainPassthroughLayer.hidden = hideLayer;
        }

        /// <summary>
        /// Fades in the main PT layer from black to visual.
        /// </summary>
        public void HideReprojectedPassthrough(bool hideLayer)
        {
            userDefinedPassthroughLayer.hidden = hideLayer;
        }

        /// <summary>
        /// Fades in the main PT layer from black to visual.
        /// </summary>
        public void HideReprojectedHighlightedPassthrough(bool hideLayer)
        {
            userDefinedPassthroughLayerHighlighter.hidden = hideLayer;
        }

        /// <summary>
        /// Fades in the main PT layer from black to visual.
        /// </summary>
        public void HideOverlayPassthrough(bool hideLayer)
        {
            overlayPassthroughLayer.hidden = hideLayer;
        }

        #endregion

        #region Surface geometry methods

        /// <summary>
        /// Tunnels <see cref="OVRPassthroughLayer.RemoveSurfaceGeometry"/> and keeps track of whether or not there are any objects on this layer.
        /// If not, the layer will be hidden to free up overhead.
        /// </summary>
        /// <param name="obj">Object to add/remove to the Insight Passthrough projection surface.</param>
        /// <param name="updateTransform">Indicate if the transform should be updated every frame.</param>
        public void AddSurfaceGeometry(GameObject obj, bool updateTransform = false)
        {
            // Keep track of things
            _numberOfReprojectedObjects++;
            if (_numberOfReprojectedObjects > 0)
            {
                userDefinedPassthroughLayer.hidden = false;
            }
            
            // Tunnel
            userDefinedPassthroughLayer.AddSurfaceGeometry(obj, updateTransform);
        }

        /// <summary>
        /// Tunnels <see cref="OVRPassthroughLayer.RemoveSurfaceGeometry"/> and keeps track of whether or not there are any objects on this layer.
        /// If not, the layer will be hidden to free up overhead.
        /// </summary>
        /// <param name="obj">Object to add/remove to the Insight Passthrough projection surface.</param>
        /// <param name="updateTransform">Indicate if the transform should be updated every frame.</param>
        public void RemoveSurfaceGeometry(GameObject obj, bool updateTransform = false)
        {
            // Tunnel
            userDefinedPassthroughLayer.AddSurfaceGeometry(obj, updateTransform);

            // Keep track of things
            _numberOfReprojectedObjects--;
            if (_numberOfReprojectedObjects <= 0)
            {
                // Ensure it's at 0
                _numberOfReprojectedObjects = 0;
                userDefinedPassthroughLayer.hidden = true;
            }
        }

        
        /// <summary>
        /// Tunnels <see cref="OVRPassthroughLayer.RemoveSurfaceGeometry"/> and keeps track of whether or not there are any objects on this layer.
        /// If not, the layer will be hidden to free up overhead.
        /// </summary>
        /// <param name="obj">Object to add/remove to the Insight Passthrough projection surface.</param>
        /// <param name="updateTransform">Indicate if the transform should be updated every frame.</param>
        public void AddSurfaceGeometryToHighlightLayer(GameObject obj, bool updateTransform = false)
        {
            // Keep track of things
            _numberOfHighlightedObjects++;
            if (_numberOfHighlightedObjects > 0)
            {
                userDefinedPassthroughLayerHighlighter.hidden = false;
            }
            
            // Tunnel
            userDefinedPassthroughLayerHighlighter.AddSurfaceGeometry(obj, updateTransform);
        }

        /// <summary>
        /// Tunnels <see cref="OVRPassthroughLayer.RemoveSurfaceGeometry"/> and keeps track of whether or not there are any objects on this layer.
        /// If not, the layer will be hidden to free up overhead.
        /// </summary>
        /// <param name="obj">Object to add/remove to the Insight Passthrough projection surface.</param>
        /// <param name="updateTransform">Indicate if the transform should be updated every frame.</param>
        public void RemoveSurfaceGeometryFromHighlightLayer(GameObject obj, bool updateTransform = false)
        {
            // Tunnel
            userDefinedPassthroughLayerHighlighter.AddSurfaceGeometry(obj, updateTransform);

            // Keep track of things
            _numberOfHighlightedObjects--;
            if (_numberOfHighlightedObjects <= 0)
            {
                // Ensure it's at 0
                _numberOfHighlightedObjects = 0;
                userDefinedPassthroughLayerHighlighter.hidden = true;
            }
        }

        /// <summary>
        /// Tunnels <see cref="AddSurfaceGeometryToHighlightLayer(GameObject, bool )"/> and keeps track of whether or not there are any objects on this layer.
        /// </summary>
        /// <param name="objs">Objects to add/remove to the Insight Passthrough projection surface.</param>
        /// <param name="updateTransform">Indicate if the transform should be updated every frame.</param>
        public void AddSurfaceGeometryToHighlightLayer(IEnumerable<GameObject> objs, bool updateTransform = false)
        {
            foreach (var obj in objs)
            {
                AddSurfaceGeometryToHighlightLayer(obj, updateTransform);
            }
        }

        /// <summary>
        /// Tunnels <see cref="AddSurfaceGeometryToHighlightLayer(GameObject, bool )"/> and keeps track of whether or not there are any objects on this layer.
        /// </summary>
        /// <param name="objs">Objects to add/remove to the Insight Passthrough projection surface.</param>
        /// <param name="updateTransform">Indicate if the transform should be updated every frame.</param>
        public void AddSurfaceGeometryToHighlightLayer(IEnumerable<MeshFilter> objs, bool updateTransform = false)
        {
            foreach (var obj in objs)
            {
                AddSurfaceGeometryToHighlightLayer(obj.gameObject, updateTransform);
            }
        }

        /// <summary>
        /// Tunnels <see cref="RemoveSurfaceGeometryFromHighlightLayer(GameObject, bool )"/> and keeps track of whether or not there are any objects on this layer.
        /// </summary>
        /// <param name="objs">Objects to add/remove to the Insight Passthrough projection surface.</param>
        /// <param name="updateTransform">Indicate if the transform should be updated every frame.</param>
        public void RemoveSurfaceGeometryFromHighlightLayer(IEnumerable<GameObject> objs, bool updateTransform = false)
        {
            foreach (var obj in objs)
            {
                RemoveSurfaceGeometryFromHighlightLayer(obj, updateTransform);
            }
        }

        /// <summary>
        /// Tunnels <see cref="RemoveSurfaceGeometryFromHighlightLayer(GameObject, bool )"/> and keeps track of whether or not there are any objects on this layer.
        /// </summary>
        /// <param name="objs">Objects to add/remove to the Insight Passthrough projection surface.</param>
        /// <param name="updateTransform">Indicate if the transform should be updated every frame.</param>
        public void RemoveSurfaceGeometryFromHighlightLayer(IEnumerable<MeshFilter> objs, bool updateTransform = false)
        {
            foreach (var obj in objs)
            {
                RemoveSurfaceGeometryFromHighlightLayer(obj.gameObject, updateTransform);
            }
        }

        #endregion

        #region Static helpers

        /// <summary>
        /// Sets the <see cref="OVROverlay.OverlayType"/> on the main layer
        /// </summary>
        /// <param name="overlayType"></param>
        public static void UpdateMainPassthroughLayerOverlayType(OVROverlay.OverlayType overlayType)
        {
            Instance.mainPassthroughLayer.overlayType = overlayType;
        }

        #endregion
    }
}