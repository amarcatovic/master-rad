﻿namespace Domain.Entities;

public class Comment
{
    public int Id { get; set; }

    public DateTime CreationDate { get; set; }

    public int PostId { get; set; }

    public int? Score { get; set; }

    public string Text { get; set; } = null!;

    public int? UserId { get; set; }
}
