using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Sapper
{
    enum CellState
    {
        Empty,
        UnderMined,
        Demined,
        Reserved,
        Numbered,
        Opened,
        OpenNumbered,
        EmptyMarked,
        NumberedMarked,
        EmptyUnknown,
        DeminedUnknown,
        NumberedUnknown
    }

    enum MinesNearCell
    {
        One = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eigth
    }

    public partial class MainForm : Form
    {
        const int CELL_SIZE = 20;
        const int CELLS_COUNT = 20;
        const int MINES_COUNT = 75;

        int numFlags;

        CellState[,] state;
        int[,] numMinesNearCell;
        int[] xCoordMine;
        int[] yCoordMine;

        bool firstMove = true;
        bool canClick = true;
        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnCreateField_Click(object sender, EventArgs e)
        {
            CreateField();
        }

        /// <summary>
        /// Создать поле
        /// </summary>
        private void CreateField()
        {
            Graphics createField = PlMineField.CreateGraphics();

            Pen blackPen = new Pen(Color.Black, 1);
            Pen gray = new Pen(Color.Gray);
            Brush grayBrush = gray.Brush;
            createField.Clear(BackColor);

            createField.FillRectangle(grayBrush, 0, 0, CELL_SIZE * CELLS_COUNT, CELL_SIZE * CELLS_COUNT);

            for (int i = 0; i < CELLS_COUNT + 1; i++)
            {
                createField.DrawLine(blackPen, i * CELL_SIZE, 0, i * CELL_SIZE, CELLS_COUNT * CELL_SIZE);
                createField.DrawLine(blackPen, 0, i * CELL_SIZE, CELLS_COUNT * CELL_SIZE, i * CELL_SIZE);
            }

            state = new CellState[CELLS_COUNT, CELLS_COUNT];
            numMinesNearCell = new int[CELLS_COUNT, CELLS_COUNT];
            firstMove = true;
            canClick = true;

            PBFlag.Visible = true;
            LblHeaderX.Visible = true;
            numFlags = MINES_COUNT;
            LblNumFlags.Text = numFlags.ToString();
            LblNumFlags.Visible = true;
        }

        /// <summary>
        /// Установить мины
        /// </summary>
        private void SetMines(int xCoordReserved, int yCoordReserved)
        {
            state[xCoordReserved, yCoordReserved] = CellState.Reserved;

            if (xCoordReserved - 1 >= 0 && yCoordReserved - 1 >= 0)
                state[xCoordReserved - 1, yCoordReserved - 1] = CellState.Reserved;

            if (xCoordReserved + 1 < CELLS_COUNT && yCoordReserved + 1 < CELLS_COUNT)
                state[xCoordReserved + 1, yCoordReserved + 1] = CellState.Reserved;

            if (xCoordReserved - 1 >= 0 && yCoordReserved + 1 < CELLS_COUNT)
                state[xCoordReserved - 1, yCoordReserved + 1] = CellState.Reserved;

            if (xCoordReserved + 1 < CELLS_COUNT && yCoordReserved - 1 >= 0)
                state[xCoordReserved + 1, yCoordReserved - 1] = CellState.Reserved;

            if (yCoordReserved - 1 >= 0)
                state[xCoordReserved, yCoordReserved - 1] = CellState.Reserved;

            if (yCoordReserved + 1 < CELLS_COUNT)
                state[xCoordReserved, yCoordReserved + 1] = CellState.Reserved;

            if (xCoordReserved - 1 >= 0)
                state[xCoordReserved - 1, yCoordReserved] = CellState.Reserved;

            if (xCoordReserved + 1 < CELLS_COUNT)
                state[xCoordReserved + 1, yCoordReserved] = CellState.Reserved;

            xCoordMine = new int[MINES_COUNT];
            yCoordMine = new int[MINES_COUNT];

            Random rand = new Random();

            for (int i = 0; i < MINES_COUNT; i++)
            {
                xCoordMine[i] = rand.Next(0, CELLS_COUNT);
                yCoordMine[i] = rand.Next(0, CELLS_COUNT);
            }

            for (int i = 0; i < MINES_COUNT; i++)
            {
                bool correct = true;

                do
                {
                    correct = true;

                    for (int j = 0; j < MINES_COUNT; j++)
                    {
                        if (i != j)
                        {
                            if (xCoordMine[i] == xCoordMine[j] && yCoordMine[i] == yCoordMine[j] ||
                                state[xCoordMine[i],yCoordMine[i]] == CellState.Reserved)
                            {
                                xCoordMine[i] = rand.Next(0, CELLS_COUNT);
                                yCoordMine[i] = rand.Next(0, CELLS_COUNT);

                                correct = false;
                            }
                        }
                    }
                }
                while (!correct);
            }           

            for (int i = 0; i < MINES_COUNT; i++)
            {
                if (state[xCoordMine[i], yCoordMine[i]] != CellState.Reserved)
                    if (state[xCoordMine[i], yCoordMine[i]] == CellState.EmptyMarked)
                    {
                        state[xCoordMine[i], yCoordMine[i]] = CellState.UnderMined;
                    }
                    else
                    {
                        state[xCoordMine[i], yCoordMine[i]] = CellState.UnderMined;
                    }
                else
                {

                }
            }

            firstMove = false;
        }

        /// <summary>
        /// Отобразить мины
        /// </summary>
        private void DrawMine()
        {
            Image mine = Properties.Resources.Mine;
            Graphics createField = PlMineField.CreateGraphics();

            for (int i = 0; i < CELLS_COUNT; i++)
            {
                for (int j = 0; j < CELLS_COUNT; j++)
                {
                    if (state[i, j] == CellState.UnderMined || state[i, j] == CellState.Demined || state[i, j] == CellState.DeminedUnknown)
                        createField.DrawImage(mine, i * CELL_SIZE + 1, j * CELL_SIZE + 1, CELL_SIZE - 1, CELL_SIZE - 1);
                }
            }
        }

        private void PlMineField_MouseClick(object sender, MouseEventArgs e)
        {
            MakeMove(e);
        }

        /// <summary>
        /// Сделать ход
        /// </summary>
        private void MakeMove(MouseEventArgs e)
        {
            for (int i = 0; i < CELLS_COUNT; i++)
            {
                if (e.X > i * CELL_SIZE && e.X < (i + 1) * CELL_SIZE)
                {
                    for (int j = 0; j < CELLS_COUNT; j++)
                    {
                        if (e.Y > j * CELL_SIZE && e.Y < (j + 1) * CELL_SIZE)
                        {
                            if (canClick)
                            {
                                if (e.Button == MouseButtons.Left)
                                {
                                    switch (state[i, j])
                                    {
                                        case CellState.Empty:
                                            if (firstMove)
                                            {
                                                SetMines(i, j);
                                                FindMines();                                                
                                                OpenCells(i, j);
                                                DrawOpenedCells();
                                            }
                                            else
                                            {
                                                OpenCells(i, j);
                                                DrawOpenedCells();
                                            }
                                            break;
                                        case CellState.UnderMined:
                                            DrawMine();
                                            GameOver();
                                            break;
                                        case CellState.Numbered:
                                            OpenNumberedCell(i, j);
                                            break;
                                    }
                                    CheckWin();
                                }
                                else if (e.Button == MouseButtons.Right)
                                {
                                    PlaceFlag(i, j);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void GameOver()
        {
            MessageBox.Show("Проиграл!");
            canClick = false;
        }

        /// <summary>
        /// Найти сколько вокруг клетки установлено мин
        /// </summary>
        private void FindMines()
        {
            for (int i = 0; i < CELLS_COUNT; i++)
            {
                for (int j = 0; j < CELLS_COUNT; j++)
                {
                    int countMines = 0;
                    if (state[i, j] != CellState.UnderMined)
                    {
                        if (i - 1 >= 0 && j - 1 >= 0)
                            if (state[i - 1, j - 1] == CellState.UnderMined)
                                countMines++;

                        if (i + 1 < CELLS_COUNT && j + 1 < CELLS_COUNT)
                            if (state[i + 1, j + 1] == CellState.UnderMined)
                                countMines++;

                        if (i - 1 >= 0 && j + 1 < CELLS_COUNT)
                            if (state[i - 1, j + 1] == CellState.UnderMined)
                                countMines++;

                        if (i + 1 < CELLS_COUNT && j - 1 >= 0)
                            if (state[i + 1, j - 1] == CellState.UnderMined)
                                countMines++;

                        if (j - 1 >= 0)
                            if (state[i, j - 1] == CellState.UnderMined)
                                countMines++;

                        if (j + 1 < CELLS_COUNT)
                            if (state[i, j + 1] == CellState.UnderMined)
                                countMines++;

                        if (i - 1 >= 0)
                            if (state[i - 1, j] == CellState.UnderMined)
                                countMines++;

                        if (i + 1 < CELLS_COUNT)
                            if (state[i + 1, j] == CellState.UnderMined)
                                countMines++;

                        if (countMines > 0)
                        {
                            if (state[i, j] == CellState.EmptyMarked)
                                state[i, j] = CellState.NumberedMarked;

                            if (state[i, j] == CellState.EmptyUnknown)
                                state[i, j] = CellState.NumberedUnknown;

                            if (state[i, j] == CellState.Empty || state[i, j] == CellState.Reserved)
                                state[i, j] = CellState.Numbered;

                            numMinesNearCell[i, j] = countMines;
                        }
                    }
                }
            }
        }
       
        /// <summary>
        /// Открыть пустые клетки
        /// </summary>
        private void OpenCells(int x, int y)
        {
            int newX;
            int newY;

            if (x - 1 >= 0 && y - 1 >= 0)
                if (state[x - 1, y - 1] == CellState.Empty || state[x - 1, y - 1] == CellState.Reserved)
                {
                    state[x - 1, y - 1] = CellState.Opened;
                    newX = x - 1;
                    newY = y - 1;
                    OpenCells(newX, newY);
                }
                else if (state[x - 1, y - 1] == CellState.Numbered)
                {
                    OpenNumberedCell(x - 1, y - 1);
                }

            if (y - 1 >= 0)
                if (state[x, y - 1] == CellState.Empty || state[x, y - 1] == CellState.Reserved)
                {
                    state[x, y - 1] = CellState.Opened;
                    newX = x;
                    newY = y - 1;
                    OpenCells(newX, newY);
                }
                else if (state[x, y - 1] == CellState.Numbered)
                {
                    OpenNumberedCell(x, y - 1);
                }

            if (x + 1 < CELLS_COUNT && y - 1 >= 0)
                if (state[x + 1, y - 1] == CellState.Empty || state[x + 1, y - 1] == CellState.Reserved)
                {
                    state[x + 1, y - 1] = CellState.Opened;
                    newX = x + 1;
                    newY = y - 1;
                    OpenCells(newX, newY);
                }
                else if (state[x + 1, y - 1] == CellState.Numbered)
                {
                    OpenNumberedCell(x + 1, y - 1);
                }

            if (x + 1 < CELLS_COUNT)
                if (state[x + 1, y] == CellState.Empty || state[x + 1, y] == CellState.Reserved)
                {
                    state[x + 1, y] = CellState.Opened;
                    newX = x + 1;
                    newY = y;
                    OpenCells(newX, newY);
                }
                else if (state[x + 1, y] == CellState.Numbered)
                {
                    OpenNumberedCell(x + 1, y);
                }

            if (x + 1 < CELLS_COUNT && y + 1 < CELLS_COUNT)
                if (state[x + 1, y + 1] == CellState.Empty || state[x + 1, y + 1] == CellState.Reserved)
                {
                    state[x + 1, y + 1] = CellState.Opened;
                    newX = x + 1;
                    newY = y + 1;
                    OpenCells(newX, newY);
                }
                else if (state[x + 1, y + 1] == CellState.Numbered)
                {
                    OpenNumberedCell(x + 1, y + 1);
                }

            if (y + 1 < CELLS_COUNT)
                if (state[x, y + 1] == CellState.Empty || state[x, y + 1] == CellState.Reserved)
                {
                    state[x, y + 1] = CellState.Opened;
                    newX = x;
                    newY = y + 1;
                    OpenCells(newX, newY);
                }
                else if (state[x, y + 1] == CellState.Numbered)
                {
                    OpenNumberedCell(x, y + 1);
                }

            if (x - 1 >= 0 && y + 1 < CELLS_COUNT)
                if (state[x - 1, y + 1] == CellState.Empty || state[x - 1, y + 1] == CellState.Reserved)
                {
                    state[x - 1, y + 1] = CellState.Opened;
                    newX = x - 1;
                    newY = y + 1;
                    OpenCells(newX, newY);
                }
                else if (state[x - 1, y + 1] == CellState.Numbered)
                {
                    OpenNumberedCell(x - 1, y + 1);
                }

            if (x - 1 >= 0)
                if (state[x - 1, y] == CellState.Empty || state[x - 1, y] == CellState.Reserved)
                {
                    state[x - 1, y] = CellState.Opened;
                    newX = x - 1;
                    newY = y;
                    OpenCells(newX, newY);
                }
                else if (state[x - 1, y] == CellState.Numbered)
                {
                    OpenNumberedCell(x - 1, y);
                }

            state[x, y] = CellState.Opened;
        }

        /// <summary>
        /// Отобразить открытые клетки
        /// </summary>
        private void DrawOpenedCells()
        {
            Graphics graphics = PlMineField.CreateGraphics();
            Pen whitePen = new Pen(Color.White);
            Brush brush = whitePen.Brush;
            for (int i = 0; i < CELLS_COUNT; i++)
            {
                for (int j = 0; j < CELLS_COUNT; j++)
                {
                    if (state[i, j] == CellState.Opened)
                    {
                        graphics.FillRectangle(brush, i * CELL_SIZE + 1, j * CELL_SIZE + 1, CELL_SIZE - 1, CELL_SIZE - 1);
                    }
                }
            }
        }

        /// <summary>
        /// Отобразить клетку с номером
        /// </summary>
        private void OpenNumberedCell(int x, int y)
        {
            state[x, y] = CellState.OpenNumbered;

            Graphics writeNumber = PlMineField.CreateGraphics();

            switch ((MinesNearCell)numMinesNearCell[x, y])
            {
                case MinesNearCell.One:
                    Image one = Properties.Resources.One;
                    writeNumber.DrawImage(one, x * CELL_SIZE + 1, y * CELL_SIZE + 1, CELL_SIZE - 1, CELL_SIZE - 1);
                    break;
                case MinesNearCell.Two:
                    Image two = Properties.Resources.Two;
                    writeNumber.DrawImage(two, x * CELL_SIZE + 1, y * CELL_SIZE + 1, CELL_SIZE - 1, CELL_SIZE - 1);
                    break;
                case MinesNearCell.Three:
                    Image three = Properties.Resources.Three;
                    writeNumber.DrawImage(three, x * CELL_SIZE + 1, y * CELL_SIZE + 1, CELL_SIZE - 1, CELL_SIZE - 1);
                    break;
                case MinesNearCell.Four:
                    Image four = Properties.Resources.Four;
                    writeNumber.DrawImage(four, x * CELL_SIZE + 1, y * CELL_SIZE + 1, CELL_SIZE - 1, CELL_SIZE - 1);
                    break;
                case MinesNearCell.Five:
                    Image five = Properties.Resources.Five;
                    writeNumber.DrawImage(five, x * CELL_SIZE + 1, y * CELL_SIZE + 1, CELL_SIZE - 1, CELL_SIZE - 1);
                    break;
                case MinesNearCell.Six:
                    Image six = Properties.Resources.Six;
                    writeNumber.DrawImage(six, x * CELL_SIZE + 1, y * CELL_SIZE + 1, CELL_SIZE - 1, CELL_SIZE - 1);
                    break;
                case MinesNearCell.Seven:
                    Image seven = Properties.Resources.Seven;
                    writeNumber.DrawImage(seven, x * CELL_SIZE + 1, y * CELL_SIZE + 1, CELL_SIZE - 1, CELL_SIZE - 1);
                    break;
                case MinesNearCell.Eigth:
                    Image eight = Properties.Resources.Eight;
                    writeNumber.DrawImage(eight, x * CELL_SIZE + 1, y * CELL_SIZE + 1, CELL_SIZE - 1, CELL_SIZE - 1);
                    break;
            }
        }

        /// <summary>
        /// Поставить флажок
        /// </summary>
        private void PlaceFlag(int x, int y)
        {
            Image flag = Properties.Resources.Flag;
            Image unknown = Properties.Resources.Unknown;

            Graphics placeFlag = PlMineField.CreateGraphics();
            Pen pen = new Pen(Color.Gray);
            Brush brush = pen.Brush;

            switch (state[x, y])
            {
                case CellState.Empty:
                    state[x, y] = CellState.EmptyMarked;
                    placeFlag.DrawImage(flag, x * CELL_SIZE + 1, y * CELL_SIZE + 1, CELL_SIZE - 1, CELL_SIZE - 1);
                    numFlags--;
                    LblNumFlags.Text = numFlags.ToString();
                    break;
                case CellState.UnderMined:
                    state[x, y] = CellState.Demined;
                    placeFlag.DrawImage(flag, x * CELL_SIZE + 1, y * CELL_SIZE + 1, CELL_SIZE - 1, CELL_SIZE - 1);
                    numFlags--;
                    LblNumFlags.Text = numFlags.ToString();
                    break;
                case CellState.Numbered:
                    state[x, y] = CellState.NumberedMarked;
                    placeFlag.DrawImage(flag, x * CELL_SIZE + 1, y * CELL_SIZE + 1, CELL_SIZE - 1, CELL_SIZE - 1);
                    numFlags--;
                    LblNumFlags.Text = numFlags.ToString();
                    break;
                case CellState.Demined:
                    state[x, y] = CellState.DeminedUnknown;
                    placeFlag.DrawImage(unknown, x * CELL_SIZE + 1, y * CELL_SIZE + 1, CELL_SIZE - 1, CELL_SIZE - 1);
                    numFlags++;
                    LblNumFlags.Text = numFlags.ToString();
                    break;
                case CellState.EmptyMarked:
                    state[x, y] = CellState.EmptyUnknown;
                    placeFlag.DrawImage(unknown, x * CELL_SIZE + 1, y * CELL_SIZE + 1, CELL_SIZE - 1, CELL_SIZE - 1);
                    numFlags++;
                    LblNumFlags.Text = numFlags.ToString();
                    break;
                case CellState.NumberedMarked:
                    state[x, y] = CellState.NumberedUnknown;
                    placeFlag.DrawImage(unknown, x * CELL_SIZE + 1, y * CELL_SIZE + 1, CELL_SIZE - 1, CELL_SIZE - 1);
                    numFlags++;
                    LblNumFlags.Text = numFlags.ToString();
                    break;
                case CellState.DeminedUnknown:
                    state[x, y] = CellState.UnderMined;
                    placeFlag.FillRectangle(brush, x * CELL_SIZE + 1, y * CELL_SIZE + 1, CELL_SIZE - 1, CELL_SIZE - 1);
                    break;
                case CellState.EmptyUnknown:
                    state[x, y] = CellState.Empty;
                    placeFlag.FillRectangle(brush, x * CELL_SIZE + 1, y * CELL_SIZE + 1, CELL_SIZE - 1, CELL_SIZE - 1);
                    break;
                case CellState.NumberedUnknown:
                    state[x, y] = CellState.Numbered;
                    placeFlag.FillRectangle(brush, x * CELL_SIZE + 1, y * CELL_SIZE + 1, CELL_SIZE - 1, CELL_SIZE - 1);
                    break;
            }
        }

        private void CheckWin()
        {
            bool win = true;

            for (int i = 0; i < CELLS_COUNT; i++)
            {
                for (int j = 0; j < CELLS_COUNT; j++)
                {
                    if (state[i, j] == CellState.Empty || state[i, j] == CellState.Numbered)
                        win = false;
                }
            }

            if (win)
            {
                MessageBox.Show("Победил!");
                canClick = false;
            }
        }
    }
}
