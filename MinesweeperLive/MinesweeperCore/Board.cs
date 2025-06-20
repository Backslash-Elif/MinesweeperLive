using System.Text.Json;

namespace MinesweeperLive.MinesweeperCore;

public class Board
{
    private const int MineChance = 16; // in percent

    public int Width { get; set; }
    public int Height { get; set; }
    public int Mines { get; set; }
    public Cell[][] Cells { get; set; }

    public void DeserializeBoard(string template)
    {
        if (!string.IsNullOrEmpty(template))
        {
            BoardDTO dto = JsonSerializer.Deserialize<BoardDTO>(template) ?? throw new InvalidOperationException();
            Width = dto.Width;
            Height = dto.Height;
            Mines = dto.Mines;
            Cells = dto.Cells;
        }
    }

    public string SerializeBoard()
    {
        if (Cells == null || Cells.Length == 0)
        {
            throw new InvalidOperationException();
        }
        BoardDTO dto = new BoardDTO(Width, Height, Mines, Cells);
        return JsonSerializer.Serialize(dto);
    }

    public void Init(int width, int height)
    {
        int mines = 0;
        
        // init 2D array
        Cell[][] cells = new Cell[height][];
        for (int y = 0; y < height; y++)
        {
            cells[y] = new Cell[width];
            for (int x = 0; x < width; x++)
            {
                cells[y][x] = new Cell();

                // make it a mine with chance
                if (Random.Shared.Next(100) + 1 < MineChance)
                {
                    cells[y][x].IsMine = true;
                }
            }
        }
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // compute neighbours
                bool north = y != 0;
                bool east = x != width - 1;
                bool south = y != height - 1;
                bool west = x != 0;
                
                if (north)
                {
                    cells[y][x].Neighbors.Add([y - 1, x]);
                }

                if (north && east)
                {
                    cells[y][x].Neighbors.Add([y - 1, x + 1]);
                }

                if (east)
                {
                    cells[y][x].Neighbors.Add([y, x + 1]);
                }

                if (east && south)
                {
                    cells[y][x].Neighbors.Add([y + 1, x + 1]);
                }

                if (south)
                {
                    cells[y][x].Neighbors.Add([y + 1, x]);
                }

                if (south && west)
                {
                    cells[y][x].Neighbors.Add([y + 1, x - 1]);
                }

                if (west)
                {
                    cells[y][x].Neighbors.Add([y, x - 1]);
                }

                if (west && north)
                {
                    cells[y][x].Neighbors.Add([y - 1, x - 1]);
                }
                
                // calculate mine proximity
                if (!cells[y][x].IsMine)
                {
                    foreach (int[] neighbor in cells[y][x].Neighbors)
                    {
                        if (cells[neighbor[0]][neighbor[1]].IsMine)
                        {
                            cells[y][x].MineProximity++;
                        }
                    }
                }
                else
                {
                    // count total mines
                    mines++;
                }
            }
        }
        
        Width = width;
        Height = height;
        Mines = mines;
        Cells = cells;
    }

    public Cell GetCell(int[] id)
    {
        return Cells[id[0]][id[1]];
    }
}