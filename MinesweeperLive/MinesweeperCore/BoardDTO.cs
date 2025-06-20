namespace MinesweeperLive.MinesweeperCore;

public class BoardDTO(int width, int height, int mines, Cell[][] cells)
{
    public int Width { get; set; } = width;
    public int Height { get; set; } = height;
    public int Mines { get; set; } = mines;
    public Cell[][] Cells { get; set; } = cells;
}