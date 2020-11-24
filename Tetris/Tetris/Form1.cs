using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        private float FPS = 60;
        private Graphics backgraph;
        private Graphics windgraph;
        private Bitmap backBmp;
        private const int W = 1024;
        private const int H = 768;
        private const int Cell_W = 10;
        private const int Cell_H = 20;
        private const int Cell_size = 25;
        private int[][] checkcell;
        private const int moveCell_W = 4;
        private const int moveCell_H = 4;
        private int[][] movecell;
        private int moveCellx;
        private int moveCelly;
        private int originx;
        private int originy;
        private int state;
        private int Score;
        class KeyState
        {
            private Keys theKey;
            bool bPress;
            bool bDown;
            bool pPreDown;
            public KeyState(Keys k)
            {
                theKey = k;
                bPress = false;
                bDown = false;
                pPreDown = false;
            }
            public bool isPress()
            {
                return bPress;
            }
            public void onTimer()
            {

                if (bDown == true)
                {
                    if (pPreDown == false)
                        bPress = true;
                    else
                        bPress = false;
                }
                else
                {
                    bPress = false;
                }
                pPreDown = bDown;
            }
            public void onKeyDown(Keys k)
            {
                if (theKey == k)
                {
                    bDown = true;
                }
            }
            public void onKeyUp(Keys k)//偵測
            {
                if (theKey == k)
                {
                    //偵測到同一個按鍵,放開
                    bDown = false;
                    bPress = false;
                }
            }
            public bool isDown()//回報是否壓著
            {
                return bDown;
            }
        } ;
        private KeyState keyUp;
        private KeyState keyDown;
        private KeyState keyLeft;
        private KeyState keyRight;
        private SoundPlayer gameBackSound;
        private int gameState;
        public Form1()
        {

            InitializeComponent();
            gameBackSound = new SoundPlayer(".\\BGM.wav");
            gameBackSound.PlayLooping();
            Score = 0;
            gameState = 1;
            keyUp = new KeyState(Keys.Up);
            keyDown = new KeyState(Keys.Down);
            keyLeft = new KeyState(Keys.Left);
            keyRight = new KeyState(Keys.Right);

            moveCellx = 0;
            moveCelly = 0;
            timer1.Interval = 1000 / (int)FPS;
            timer1.Start();
            timer2.Interval = 1000 / (int)FPS * 25;
            timer2.Start();
            windgraph = CreateGraphics();

            backBmp = new Bitmap(W, H);
            backgraph = Graphics.FromImage(backBmp);
            checkcell = new int[Cell_H][];

            for (int i = 0; i < Cell_H; i++)
            {
                checkcell[i] = new int[Cell_W];
                for (int m = 0; m < Cell_W; m++)
                    checkcell[i][m] = 0;
            }
            movecell = new int[Cell_H][];
            for (int i = 0; i < moveCell_H; i++)
            {
                movecell[i] = new int[moveCell_W];
                for (int m = 0; m < moveCell_W; m++)
                    movecell[i][m] = 0;
            }

            movecell[1][1] = 1;
            movecell[1][2] = 1;
            movecell[2][1] = 1;
            movecell[2][2] = 1;
        }
        private void drawgame()
        {
            backgraph.FillRectangle(Brushes.White, 0, 0, W, H);
            backgraph.DrawRectangle(Pens.Black, 0, 0, Cell_size * Cell_W, Cell_size * Cell_H);
            for (int i = 0; i < Cell_H; i++)
            {
                for (int m = 0; m < Cell_W; m++)
                {
                    int tx = m * Cell_size;
                    int ty = i * Cell_size;
                    if (checkcell[i][m] == 1)
                    {
                        backgraph.FillRectangle(Brushes.Yellow, tx, ty, Cell_size, Cell_size);
                    }
                    if (checkcell[i][m] == 2)
                    {
                        backgraph.FillRectangle(Brushes.LightBlue, tx, ty, Cell_size, Cell_size);
                    }
                    if (checkcell[i][m] == 3)
                    {
                        backgraph.FillRectangle(Brushes.Purple, tx, ty, Cell_size, Cell_size);
                    }
                    if (checkcell[i][m] == 4)
                    {
                        backgraph.FillRectangle(Brushes.Green, tx, ty, Cell_size, Cell_size);
                    }
                    if (checkcell[i][m] == 5)
                    {
                        backgraph.FillRectangle(Brushes.Red, tx, ty, Cell_size, Cell_size);
                    }
                    if (checkcell[i][m] == 6)
                    {
                        backgraph.FillRectangle(Brushes.Blue, tx, ty, Cell_size, Cell_size);
                    }
                    if (checkcell[i][m] == 7)
                    {
                        backgraph.FillRectangle(Brushes.Orange, tx, ty, Cell_size, Cell_size);
                    }
                }
            }
            for (int i = 0; i < moveCell_H; i++)
            {
                for (int m = 0; m < moveCell_W; m++)
                {
                    int tx = (m + moveCellx) * Cell_size;
                    int ty = (i + moveCelly) * Cell_size;
                    if (movecell[i][m] == 1)
                    {
                        backgraph.FillRectangle(Brushes.Yellow, tx, ty, Cell_size, Cell_size);
                    }
                    if (movecell[i][m] == 2)
                    {
                        backgraph.FillRectangle(Brushes.LightBlue, tx, ty, Cell_size, Cell_size);
                    }
                    if (movecell[i][m] == 3)
                    {
                        backgraph.FillRectangle(Brushes.Purple, tx, ty, Cell_size, Cell_size);
                    }
                    if (movecell[i][m] == 4)
                    {
                        backgraph.FillRectangle(Brushes.Green, tx, ty, Cell_size, Cell_size);
                    }
                    if (movecell[i][m] == 5)
                    {
                        backgraph.FillRectangle(Brushes.Red, tx, ty, Cell_size, Cell_size);
                    }
                    if (movecell[i][m] == 6)
                    {
                        backgraph.FillRectangle(Brushes.Blue, tx, ty, Cell_size, Cell_size);
                    }
                    if (movecell[i][m] == 7)
                    {
                        backgraph.FillRectangle(Brushes.Orange, tx, ty, Cell_size, Cell_size);
                    }
                }
            }
            windgraph.DrawImageUnscaled(backBmp, 0, 0);
        }
        private bool checkoutside()
        {
            for (int i = 0; i < moveCell_H; i++)
            {
                for (int m = 0; m < moveCell_W; m++)
                {
                    if (movecell[i][m] != 0)
                    {
                        int tx = (m + moveCellx);
                        int ty = (i + moveCelly);
                        if (tx >= Cell_W)
                            return true;
                        if (tx < 0)
                            return true;
                        if (ty >= Cell_H)
                            return true;
                    }
                }
            }
            return false;
        }
        private void cleanFullRow()
        {
            for (int i = 0; i < Cell_H; i++)
            {
                bool bFull = true;
                for (int m = 0; m < Cell_W; m++)
                {
                    if (checkcell[i][m] == 0)
                    {
                        bFull = false;//有空格
                        break;
                    }
                }

                if (bFull)
                {
                    Score++;
                    label2.Text = (Score * 100).ToString();
                    for (int k = i; k >= 1; k--)
                        for (int m = 0; m < Cell_W; m++)
                            checkcell[k][m] = checkcell[k - 1][m];
                }
            }
        }
        private bool checkOverlap()
        {
            for (int i = 0; i < moveCell_H; i++)
            {
                for (int m = 0; m < moveCell_W; m++)
                {
                    if (movecell[i][m] != 0)
                    {
                        //4x4的座標,轉20x10的座標
                        int tx = (m + moveCellx);
                        int ty = (i + moveCelly);

                        if (checkcell[ty][tx] != 0)
                            return true;
                    }
                }
            }

            return false;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            keyDown.onTimer();
            keyUp.onTimer();
            keyRight.onTimer();
            keyLeft.onTimer();
            if (gameState == -1)
            {
                string C = "Game over !!!";
                label3.Text = C;
                return;//
            }
            originx = moveCellx;
            originy = moveCelly;
            if (keyRight.isPress())
                moveCellx++;
            if (keyLeft.isPress())
                moveCellx--;
            if (checkoutside() || checkOverlap())
            {
                moveCellx = originx;
                moveCelly = originy;
            }
            originx = moveCellx;
            originy = moveCelly;
            if (keyUp.isPress())
            {
                if (state == 2)
                {
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[0][0] = 2;
                    movecell[0][1] = 2;
                    movecell[0][2] = 2;
                    movecell[0][3] = 2;
                    state = 8;
                }
                else if (state == 3)
                {
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[0][2] = 4;
                    movecell[0][1] = 4;
                    movecell[1][1] = 4;
                    movecell[1][0] = 4;
                    state = 9;
                }
                else if (state == 4)
                {
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[0][0] = 5;
                    movecell[0][1] = 5;
                    movecell[1][1] = 5;
                    movecell[1][2] = 5;
                    state = 10;
                }
                else if (state == 5)
                {
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[1][1] = 3;
                    movecell[1][2] = 3;
                    movecell[1][3] = 3;
                    movecell[2][2] = 3;
                    state = 11;
                }
                else if (state == 6)
                {
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[1][1] = 6;
                    movecell[1][2] = 6;
                    movecell[1][3] = 6;
                    movecell[2][1] = 6;
                    state = 14;
                }
                else if (state == 7)
                {
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[0][0] = 7;
                    movecell[1][0] = 7;
                    movecell[1][1] = 7;
                    movecell[1][2] = 7;
                    state = 17;
                }
                else if (state == 8)
                {
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[1][1] = 2;
                    movecell[2][1] = 2;
                    movecell[3][1] = 2;
                    movecell[0][1] = 2;
                    state = 2;
                }
                else if (state == 9)
                {
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[1][1] = 4;
                    movecell[0][1] = 4;
                    movecell[1][2] = 4;
                    movecell[2][2] = 4;
                    state = 3;
                }
                else if (state == 10)
                {
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[0][2] = 5;
                    movecell[1][2] = 5;
                    movecell[1][1] =5;
                    movecell[2][1] = 5;
                    state = 4;
                }
                else if (state == 11)
                {
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[1][2] = 3;
                    movecell[2][2] = 3;
                    movecell[1][1] = 3;
                    movecell[0][2] = 3;
                    state = 12;
                }
                else if (state == 12)
                {
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[0][2] = 3;
                    movecell[1][2] = 3;
                    movecell[1][3] = 3;
                    movecell[1][1] = 3;
                    state = 13;
                }
                else if (state == 13)
                {
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[0][2] = 3;
                    movecell[1][2] = 3;
                    movecell[2][2] = 3;
                    movecell[1][3] = 3;
                    state = 5;
                }
                else if (state == 14)
                {
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[0][2] = 6;
                    movecell[1][2] = 6;
                    movecell[2][2] = 6;
                    movecell[0][1] = 6;
                    state = 15;
                }
                else if (state == 15)
                {
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[1][1] = 6;
                    movecell[1][2] = 6;
                    movecell[1][3] = 6;
                    movecell[0][3] = 6;
                    state = 16;
                }
                else if (state == 16)
                {
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[0][2] = 6;
                    movecell[1][2] = 6;
                    movecell[2][2] = 6;
                    movecell[2][3] = 6;
                    state = 6;
                }
                else if (state == 17)
                {
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[0][2] = 7;
                    movecell[1][2] = 7;
                    movecell[2][2] = 7;
                    movecell[0][3] = 7;
                    state = 18;
                }
                else if (state == 18)
                {
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[2][3] = 7;
                    movecell[1][1] = 7;
                    movecell[1][2] = 7;
                    movecell[1][3] = 7;
                    state = 19;
                }
                else if (state == 19)
                {
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[0][3] = 7;
                    movecell[1][3] = 7;
                    movecell[2][3] = 7;
                    movecell[2][2] = 7;
                    state = 7;
                }
            }


            if (keyDown.isPress())
            {
                moveCelly++;
                if (checkoutside() || checkOverlap())
                {
                    moveCellx = originx;
                    moveCelly = originy;
                    copy();
                    cleanFullRow();

                    Random A = new Random();
                    int op = A.Next(0, 100000);
                    op %= 7;
                    op += 1;
                    if (op == 1)
                    {
                        state = 1;
                        for (int i = 0; i < moveCell_H; i++)
                        {
                            movecell[i] = new int[moveCell_W];
                            for (int m = 0; m < moveCell_W; m++)
                                movecell[i][m] = 0;
                        }
                        movecell[1][1] = 1;
                        movecell[1][2] = 1;
                        movecell[2][1] = 1;
                        movecell[2][2] = 1;
                    }
                    else if (op == 2)
                    {
                        state = 2;
                        for (int i = 0; i < moveCell_H; i++)
                        {
                            movecell[i] = new int[moveCell_W];
                            for (int m = 0; m < moveCell_W; m++)
                                movecell[i][m] = 0;
                        }
                        movecell[1][1] = 2;
                        movecell[2][1] = 2;
                        movecell[3][1] = 2;
                        movecell[0][1] = 2;
                    }
                    else if (op == 3)
                    {
                        state = 3;
                        for (int i = 0; i < moveCell_H; i++)
                        {
                            movecell[i] = new int[moveCell_W];
                            for (int m = 0; m < moveCell_W; m++)
                                movecell[i][m] = 0;
                        }
                        movecell[1][1] = 4;
                        movecell[0][1] = 4;
                        movecell[1][2] = 4;
                        movecell[2][2] = 4;
                    }
                    else if (op == 4)
                    {
                        state = 4;
                        for (int i = 0; i < moveCell_H; i++)
                        {
                            movecell[i] = new int[moveCell_W];
                            for (int m = 0; m < moveCell_W; m++)
                                movecell[i][m] = 0;
                        }
                        movecell[0][2] = 5;
                        movecell[1][2] = 5;
                        movecell[1][1] = 5;
                        movecell[2][1] = 5;
                    }
                    else if (op == 5)
                    {
                        state = 5;
                        for (int i = 0; i < moveCell_H; i++)
                        {
                            movecell[i] = new int[moveCell_W];
                            for (int m = 0; m < moveCell_W; m++)
                                movecell[i][m] = 0;
                        }
                        movecell[1][2] = 3;
                        movecell[2][2] = 3;
                        movecell[3][2] = 3;
                        movecell[2][1] = 3;
                    }
                    else if (op == 6)
                    {
                        state = 6;
                        for (int i = 0; i < moveCell_H; i++)
                        {
                            movecell[i] = new int[moveCell_W];
                            for (int m = 0; m < moveCell_W; m++)
                                movecell[i][m] = 0;
                        }
                        movecell[1][2] = 6;
                        movecell[2][2] = 6;
                        movecell[3][2] = 6;
                        movecell[3][3] = 6;
                    }
                    else if (op == 7)
                    {
                        state = 7;
                        for (int i = 0; i < moveCell_H; i++)
                        {
                            movecell[i] = new int[moveCell_W];
                            for (int m = 0; m < moveCell_W; m++)
                                movecell[i][m] = 0;
                        }
                        movecell[1][3] = 7;
                        movecell[2][3] = 7;
                        movecell[3][3] = 7;
                        movecell[3][2] = 7;
                    }
                    moveCellx = 0;
                    moveCelly = 0;
                }

                if (checkOverlap())
                    gameState = -1;

            }
            drawgame();
        }
        private void copy()
        {
            for (int i = 0; i < moveCell_H; i++)
            {
                for (int m = 0; m < moveCell_W; m++)
                {
                    if (movecell[i][m] != 0)
                    {
                        int tx = (m + moveCellx);
                        int ty = (i + moveCelly);
                        checkcell[ty][tx] = movecell[i][m];
                    }
                }
            }
        }
        private void onkeydown(object sender, KeyEventArgs e)
        {
            keyUp.onKeyDown(e.KeyCode);
            keyDown.onKeyDown(e.KeyCode);
            keyLeft.onKeyDown(e.KeyCode);
            keyRight.onKeyDown(e.KeyCode);
        }

        private void onkeyup(object sender, KeyEventArgs e)
        {
            keyUp.onKeyUp(e.KeyCode);
            keyDown.onKeyUp(e.KeyCode);
            keyLeft.onKeyUp(e.KeyCode);
            keyRight.onKeyUp(e.KeyCode);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            moveCelly++;
            if (checkoutside() || checkOverlap())
            {
                moveCellx = originx;
                moveCelly = originy;
                copy();
                cleanFullRow();
                Random A = new Random();
                int op = A.Next(0, 100000);
                op %= 7;
                op += 1;

                if (op == 1)
                {
                    state = 1;
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[1][1] = 1;
                    movecell[1][2] = 1;
                    movecell[2][1] = 1;
                    movecell[2][2] = 1;
                }
                else if (op == 2)
                {
                    state = 2;
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[1][1] = 2;
                    movecell[2][1] = 2;
                    movecell[3][1] = 2;
                    movecell[0][1] = 2;
                }
                else if (op == 3)
                {
                    state = 3;
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[1][1] = 4;
                    movecell[0][1] = 4;
                    movecell[1][2] = 4;
                    movecell[2][2] = 4;
                }
                else if (op == 4)
                {
                    state = 4;
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[0][2] = 5;
                    movecell[1][2] = 5;
                    movecell[1][1] = 5;
                    movecell[2][1] = 5;
                }
                else if (op == 5)
                {
                    state = 5;
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[0][1] = 3;
                    movecell[1][1] = 3;
                    movecell[2][1] = 3;
                    movecell[1][2] = 3;
                }
                else if (op == 6)
                {
                    state = 6;
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[1][2] = 6;
                    movecell[2][2] = 6;
                    movecell[3][2] = 6;
                    movecell[3][3] = 6;
                }
                else if (op == 7)
                {
                    state = 7;
                    for (int i = 0; i < moveCell_H; i++)
                    {
                        movecell[i] = new int[moveCell_W];
                        for (int m = 0; m < moveCell_W; m++)
                            movecell[i][m] = 0;
                    }
                    movecell[1][3] = 7;
                    movecell[2][3] = 7;
                    movecell[3][3] = 7;
                    movecell[3][2] = 7;
                }
                moveCellx = 0;
                moveCelly = 0;
            }
            if (checkOverlap())
            {
                gameState = -1;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}
