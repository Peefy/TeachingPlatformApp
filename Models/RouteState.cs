namespace TeachingPlatformApp.Models
{
    public enum RouteState
    {
        Normal,
        OutOfLeft,
        OutOfRight
    }

    public static class RouteStateExtension
    {
        public static string ToLeftRightString(this RouteState routeState)
        {
            var str = "";
            switch(routeState)
            {
                case RouteState.Normal:
                    str = "正常";
                    break;
                case RouteState.OutOfLeft:
                    str = "偏左";
                    break;
                case RouteState.OutOfRight:
                    str = "偏右";
                    break;
            }
            return str;
        }
    }

}
