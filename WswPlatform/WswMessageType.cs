﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachingPlatformApp.WswPlatform
{
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
        MaxMessageType = ResetFlightStatusMornitor

    }
}