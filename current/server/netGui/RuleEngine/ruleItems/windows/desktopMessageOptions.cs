using System;

namespace netGui.RuleEngine.ruleItems
{
    public class desktopMessageOptions
    {
        // These are in hundreds of milliseconds
        public int fadeInSpeed = 5;
        public int holdSpeed = 40;
        public int fadeOutSpeed = 5;
        public string message = "Something happened!";

        public desktopMessageLocation location = desktopMessageLocation.BottomRight;

        public desktopMessageOptions(desktopMessageOptions newOptions)
        {
            fadeInSpeed = newOptions.fadeInSpeed;
            holdSpeed = newOptions.holdSpeed;
            fadeOutSpeed = newOptions.fadeOutSpeed;
            message = newOptions.message;
            location = newOptions.location;
        }

        public desktopMessageOptions() { }
    }
}

public enum desktopMessageLocation
{
    TopLeft,
    TopMiddle,
    TopRight,
    BottomLeft,
    BottomMiddle,
    BottomRight
}