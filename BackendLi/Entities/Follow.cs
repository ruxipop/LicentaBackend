using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendLi.Entities;

[Table("follow")]
public class Follow
{
    public Follow(int followerId, int followingId)
    {
        FollowerId = followerId;
        FollowingId = followingId;
    }

    [Key] public int Id { get; set; }

    [ForeignKey("Follower")] public int FollowerId { get; set; }

    public virtual User? Follower { get; set; }

    [ForeignKey("Following")] public int FollowingId { get; set; }

    public virtual User? Following { get; set; }
}