namespace TeachingPlatformApp.WswPlatform
{
    /// <summary>
    /// Wsw平台通信的消息类型
    /// </summary>
    public enum WswMessageType
    {
        Idle = 0,
        LauncherCalibration = 1,
        TrajectoryCalibration = 2,
        DataGloveCalibrationLeft = 3,
        DataGloveCalibrationRight = 4,
        HelicopterController = 5,
        HelicopterFeedback = 6,
        HelicopterLocked = 7,
        LauncherPosture = 8,
        MissileEmission = 9,
        DataGlovePosture = 10,
        ResetFlightStatusDefault = 11,
        ResetFlightStatusMornitor = 12,
        ShowText = 13,
        MaxMessageType = ShowText

    }
}
