namespace ExtraDry.Core;

public class DataConverter {
    /// <summary>
    /// Given a date, formats it for display using a relative time.
    /// For example, 5 minutes ago, or Yesterday.
    /// </summary>
    public static string DateToRelativeTime(DateTime dateTime)
    {
        var delta = DateTime.UtcNow - dateTime;
        var today = DateTime.UtcNow.Date == dateTime.Date;
        var yesterday = DateTime.UtcNow.Date == dateTime.Date.AddDays(1);
        if(delta.TotalSeconds < 30) {
            return "Just Now";
        }
        else if(delta.TotalMinutes < 2) {
            return "1 minute ago";
        }
        else if(delta.TotalMinutes < 90) {
            var minutes = (int)delta.TotalMinutes;
            return $"{minutes} minutes ago";
        }
        else if(delta.TotalHours < 2) {
            return "1 hour ago";
        }
        else if(delta.TotalHours < 24 && today) {
            var hours = (int)delta.TotalHours;
            return $"{hours} hours ago";
        }
        else if(yesterday) {
            return $"Yesterday {dateTime:hh:mm tt}";
        }
        else if(delta.TotalDays < 6) {
            return $"{dateTime:ddd hh:mm tt}";
        }
        else {
            return $"{dateTime:MMM dd hh:mm tt}";
        }
    }

}
