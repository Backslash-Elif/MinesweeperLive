namespace MinesweeperLive.MinesweeperCore;

public class Cell
{
    public bool IsMine {get; set;}
    public bool IsFlagged {get; set;}
    public bool IsRevealed {get; set;}
    public int MineProximity {get; set;}
    
    public List<int[]> Neighbors {get; set;}
    
    public Cell()
    {
        this.IsMine = false;
        this.IsFlagged = false;
        this.IsRevealed = false;
        this.MineProximity = 0;
        this.Neighbors = new List<int[]>();
    }

    public Cell(bool isMine, bool isFlagged, bool isRevealed, int mineProximity,  List<int[]> neighbors)
    {
        this.IsMine = isMine;
        this.IsFlagged = isFlagged;
        this.IsRevealed = isRevealed;
        this.MineProximity = mineProximity;
        this.Neighbors = neighbors;
    }
}