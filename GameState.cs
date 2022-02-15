using System;
using System.Collections.Generic;
using System.Text;

namespace Match3Test
{
    enum GameState
    {
        Input,
        Swap,
        CheckLines,
        DropCells,
        SpawnCells,
        CheckAfterSwap
    }
}
