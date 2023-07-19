# ViewR: Large Scale Multi-User Mixed Reality
<img src="Images/cover.png">


ViewR Description
## How to install:

### The full ViewR package:

1. Clone repository
2. Install dependencies:
    -   Normcore 2.4.1              
            https://assetstore.unity.com/packages/tools/network/normcore-free-multiplayer-voice-chat-for-all-platforms-195224
    -   Oculus 46.0:                
            https://developer.oculus.com/downloads/package/unity-integration/46.0
    -   Surge (Into plugin folder)  
            https://assetstore.unity.com/packages/tools/utilities/surge-107312
3. Add Normcore AppID               
    https://normcore.io/dashboard/app/applications/create

### Just the calibration package:

1. Install package through git URL:     
2. Add "CalibrationManager" and "CalibrationStations" prefabs to your SceneContent
3. Set references in CalibrationManager
    -   CenterEye- Camera of the player
    -   TransformToMove - the gameobjects which is at the end calibrated (usally the tracking space of the camera)
    -   Left/RightHandAnchor - Gameobjects which resemble either the hand and/or controller positions. This is used for the feedback UI
    -   Left/Right wrist - The wrist joints of the tracked hands which should match the position in the calibration station image
    -   Left/Right controller calibration point - The point on the controller which should match the point on the calibration stations. 

## How to use

### Calibration

For the calibration system: place calibration stations at same position as in the real world. The corresponding sheets can be printed on A3 paper with the picutres under: "ViewR/Core/Calibration/Materials"
Controllers are used with primary button changable in "ControllerBasedCalibrator" under the "CalibrationManager"
Pointer on controller has to match station


<img src="Images/calibration.png" width="600" height="200">

### Blending

To enable passthorugh blending with your model, drag it under the "SpaceContainer -> World -> Space"
To adjust the blending on runtime, open the menu with the left controllers menu button
Content which should stay fully virtual can be placed inside "Scene Content"

<img src="Images/PassthroughSlider.png">
