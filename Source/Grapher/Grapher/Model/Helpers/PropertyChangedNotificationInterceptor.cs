using System;

namespace Grapher
{
    public static class PropertyChangedNotificationInterceptor
    {
        public static void Intercept(object target, Action onPropertyChangedAction, string propertyName)
        {
            if(target.ToString().Equals("Grapher.Point2D"))
                onPropertyChangedAction();
        }
    }
}
