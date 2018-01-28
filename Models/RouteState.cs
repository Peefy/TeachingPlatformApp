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
                    str = "航线正常";
                    break;
                case RouteState.OutOfLeft:
                    str = "航线偏左";
                    break;
                case RouteState.OutOfRight:
                    str = "航线偏右";
                    break;
            }
            return str;
        }
    }

}
