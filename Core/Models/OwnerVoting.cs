namespace Models;
public class OwnerVoting
{
    public int OwnerId { get; set; }
    public string OwnerName { get; set; }
    public VoteType? Vote { get; set; }
    public int FlatId { get; set; }
    public int FlatNumber { get; set; }
}