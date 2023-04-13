# ViewR: Large Scale Multi-User Mixed Reality
<img src="Images/cover.jpg">

ViewR is an open source framework for rapidly constructing and deploying large-scale multi-user MR experiences.
ViewR includes tools for rapid alignment and re-calibration of
real and virtual worlds, tracking loss detection and recovery, and world state synchronisation between
users with persistence across sessions. ViewR also provides
control over the blending of the real and the virtual, specification
of site-specific blending zones, and video-passthrough avatars,
allowing users to see and interact with one another naturally.

## How to install:

### The full ViewR package:

1. Clone repository
2. Add the following assets:
    -   Normcore 2.4.1
    -   Oculus SDK 46.0:    https://developer.oculus.com/downloads/package/unity-integration/46.0
    -   Surge
3. Add your Normcore AppID
4. Use the "ViewR_Start" scene as a starting template

### Just the calibration package:

1. Install package through git URL: https://github.com/ixlabTUD/ViewR.git?path=/Assets/ViewR/Core/Calibration
2. Add "CalibrationManager" and "CalibrationStations" prefabs to your SceneContent
3. Set references in CalibrationManager

## How to use:

### Calibration

For the calibration, place calibration stations at same positions as in the real world. The corresponding sheets can be printed on A3 paper with the picutres under: "ViewR/Core/Calibration/Materials"

Calibration using the controllers is performed by holding both primary buttons. This can be changed in the ControllerBasedCalibrator under the CalibrationManager

Calibration using the hands is performed by looking on a printed hand calibration station. Try to stay close to the station, dont move and hold your hands behind your back for optimal calibration.

<img src="Images/calibration.png" width="600" height="200">

### Blending

To enable passthorugh blending with your model, drag it under the "SpaceContainer -> World -> Space"
To adjust the blending on runtime, open the menu with the left controllers menu button
Content which should stay fully virtual can be plaed inside "Scene Content"

<img src="Images/PassthroughSlider.png">
