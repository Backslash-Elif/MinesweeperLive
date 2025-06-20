namespace MinesweeperLive.MinesweeperCore;

public class MinesweeperCore
{
    private readonly Board _board = new Board();
    private GameState _gameState = GameState.Waiting;

    public void Initialize(int  width, int height)
    {
        _board.Init(width, height);
        _gameState = GameState.Playing;
    }

    public void Load(string template)
    {
        _board.DeserializeBoard(template);
        _gameState = GameState.Playing;
    }

    public string Save()
    {
        if (_gameState != GameState.Playing)
        {
            // the game isn't running
            return "";
        }
        
        return _board.SerializeBoard();
    }

    public void Reveal(int x, int y, bool cascade = false)
    {
        if (_gameState != GameState.Playing)
        {
            // the game isn't running
            return;
        }
        
        Cell revealCell = _board.Cells[y][x];

        if (revealCell.IsFlagged)
        {
            // nothing happens when a flagged cell is triggered
            return;
        }

        if (revealCell.IsMine)
        {
            // boom
            revealCell.IsRevealed = true;
            _gameState = GameState.GameOver;
            
            // show all mines
            foreach (Cell[] cellRow in _board.Cells)
            {
                foreach (Cell cell in cellRow)
                {
                    if (cell.IsMine)
                    {
                        cell.IsRevealed = true;
                    }
                }
            }
            return;
        }

        if (revealCell.IsRevealed)
        {
            // reveal all surrounding cells if the mine count matches, but only if it isn't triggered due to a cascade
            int mines = 0;
            foreach (int[] neighborId in revealCell.Neighbors)
            {
                if (_board.GetCell(neighborId).IsMine)
                {
                    mines++;
                }
            }
            

            if (revealCell.MineProximity == mines && !cascade && revealCell.MineProximity != 0)
            {
                foreach (int[] neighborId in revealCell.Neighbors)
                {
                    Reveal(neighborId[1], neighborId[0], true);
                }

                if (DetectWin())
                {
                    _gameState = GameState.Victory;
                }
            }
        }
        else
        {
            // reveal itself and if cell has no surrounding mines, reveal all surround cells
            revealCell.IsRevealed = true;
            if (revealCell.MineProximity == 0)
            {
                foreach (int[] neighborId in revealCell.Neighbors)
                {
                    Reveal(neighborId[1], neighborId[0], true);
                }
            }
            
            if (DetectWin())
            {
                _gameState = GameState.Victory;
            }
        }
    }

    private bool DetectWin()
    {
        int mines = 0;
        foreach (Cell[] cellRow in _board.Cells)
        {
            foreach (Cell cell in cellRow)
            {
                if (cell.IsMine && cell.IsFlagged)
                {
                    mines++;
                }
            }
        }

        if (mines == _board.Mines)
        {
            return true;
        }
        return false;
    }

    public void Flag(int x, int y)
    {
        if (_gameState != GameState.Playing)
        {
            // the game isn't running
            return;
        }
        
        _board.Cells[y][x].IsFlagged = !_board.Cells[y][x].IsFlagged;
    }
}