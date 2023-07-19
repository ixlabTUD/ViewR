namespace ViewR.Core.Calibration
{
    /// <summary>
    /// The orientation of the sockets within the calibration stations
    /// </summary>
    public enum CalibrationSocketOrientation
    {
        NorthXNorth,
        NorthXEast,
        NorthXSouth,
        NorthXWest,
        EastXNorth,
        EastXEast,
        EastXSouth,
        EastXWest,
        SouthXNorth,
        SouthXEast,
        SouthXSouth,
        SouthXWest,
        WestXNorth,
        WestXEast,
        WestXSouth,
        WestXWest,
        Undefined
    }
    
    public enum ControllerOrientation
    {
        North,
        East,
        South,
        West,
        Undefined
    }
    
    public enum CalibrationStationType
    {
        Controller,
        Hand
    }
    
    public enum HandCalibrationConfiguration
    {
        MiddleXMiddle,
        MiddleXUp,
        MiddleXDown
    }
    
}