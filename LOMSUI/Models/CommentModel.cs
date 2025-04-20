    public class CommentModel
    {
    public string CommentID { get; set; }
    public string Content { get; set; }
    public DateTime CommentTime { get; set; }
    public string CustomerID { get; set; }
    public string CustomerName { get; set; }
    public string LiveStreamID { get; set; }
    public string customerAvatar { get; set; }
        
    public string GetFormattedTime()
    {
        TimeSpan timeDiff = DateTime.UtcNow - CommentTime.ToUniversalTime();
        if (timeDiff.TotalMinutes < 1) return "Vừa xong";
        if (timeDiff.TotalMinutes < 60) return $"{(int)timeDiff.TotalMinutes} phút trước";
        if (timeDiff.TotalHours < 24) return $"{(int)timeDiff.TotalHours} giờ trước";
        return CommentTime.ToString("dd/MM/yyyy HH:mm");
    }

}
