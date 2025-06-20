namespace MinesweeperLive.MinesweeperCore;

public class Demo
{
    public static void Start()
    {
        MinesweeperCore core = new MinesweeperCore();
        Board board = new Board();
        
        Console.WriteLine("Initializing random Board (10x10)");
        core.Initialize(10, 10);
        
        board.DeserializeBoard(core.Save());
        PrintBoard(board, true);
        
        Console.WriteLine("Flagging cells (0, 0), (1, 0) and (2, 0)");
        core.Flag(0, 0);
        core.Flag(1, 0);
        core.Flag(2, 0);
        
        board.DeserializeBoard(core.Save());
        PrintBoard(board);
        
        Console.WriteLine("Revealing cell (9, 0)");
        core.Reveal(9, 0);
        board.DeserializeBoard(core.Save());
        PrintBoard(board);
        
        Console.WriteLine("Revealing cell (9, 9)");
        core.Reveal(9, 9);
        board.DeserializeBoard(core.Save());
        PrintBoard(board);
        
        Console.WriteLine("Revealing cell (0, 9)");
        core.Reveal(0, 9);
        board.DeserializeBoard(core.Save());
        PrintBoard(board);
    }

    private static void PrintBoard(Board board, bool xray = false)
    {
        Console.WriteLine("Board:");
        Console.WriteLine(".----------.");
        for (int y = 0; y < board.Height; y++)
        {
            Console.Write("|");
            for (int x = 0; x < board.Width; x++)
            {
                string c = " ";
                if (board.Cells[y][x].MineProximity > 0)
                {
                    c = board.Cells[y][x].MineProximity.ToString();
                }

                if (board.Cells[y][x].IsMine)
                {
                    c = "M";
                }
                
                if (!(xray || board.Cells[y][x].IsRevealed))
                {
                    c = "#";
                }
                
                if (board.Cells[y][x].IsFlagged)
                {
                    c = "F";
                }
                Console.Write(c);
            }
            Console.WriteLine("|");
        }
        Console.WriteLine("'----------'\n");
    }
}