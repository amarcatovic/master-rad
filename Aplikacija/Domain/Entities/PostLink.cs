namespace Domain.Entities;

public class PostLink
{
    public int Id { get; set; }

    public DateTime CreationDate { get; set; }

    public int PostId { get; set; }

    public int RelatedPostId { get; set; }

    public int LinkTypeId { get; set; }
}
