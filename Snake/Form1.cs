using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Krug> Snake = new List<Krug>();
        private Krug food = new Krug();
        public Form1()
        {
            InitializeComponent();
            new Params();

            MainTimer.Interval = 1000 / Params.Speed;
            MainTimer.Tick += UpdateScreen;
            MainTimer.Start();

            StartGame();
        }
        private void StartGame()
        {
            lblGameOver.Visible = false;

            new Params();

            //Создаем нового игрока
            Snake.Clear();
            Krug head = new Krug();
            head.X = 10;
            head.Y = 5;
            Snake.Add(head);


            lblScore.Text = Params.Score.ToString();
            SpawnFood();
        }
        //метод который спавнит еду
        private void SpawnFood()
        {
            int maxXPos = pbCanvas.Size.Width / Params.Width;
            int maxYPos = pbCanvas.Size.Height / Params.Height;

            Random random = new Random();
            food = new Krug();
            food.X = random.Next(0, maxXPos);
            food.Y = random.Next(0, maxYPos);
        }
        private void UpdateScreen(object sender, EventArgs e)
        {
            if(Params.GameOver == true)
            {
                if(InputManager.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (InputManager.KeyPressed(Keys.Right) && Params.direction != Direction.Left)
                    Params.direction = Direction.Right;
                else if (InputManager.KeyPressed(Keys.Left) && Params.direction != Direction.Right)
                    Params.direction = Direction.Left;
                else if (InputManager.KeyPressed(Keys.Up) && Params.direction != Direction.Down)
                    Params.direction = Direction.Up;
                else if (InputManager.KeyPressed(Keys.Down) && Params.direction != Direction.Up)
                    Params.direction = Direction.Down;

                MovePlayer();
            }

            pbCanvas.Invalidate();
        }

        private void pbCanvas_Paint(object Sender, PaintEventArgs e)
        {
            
        }
        private void MovePlayer()
        {
            for(int i=Snake.Count-1; i>=0;i--)
            {
                if(i==0)
                {
                    switch(Params.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }

                    int maxXPos = pbCanvas.Size.Width / Params.Width;
                    int maxYPos = pbCanvas.Size.Height / Params.Height;


                    //Проверка столкновений с границами
                    if(Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X>=maxXPos||Snake[i].Y>=maxYPos)
                    {
                        Die();
                    }

                    //Проверка столкновений с телом
                    for(int k=1;k<Snake.Count;k++)
                    {
                        if(Snake[i].X==Snake[k].X && Snake[i].Y == Snake[k].Y)
                        {
                           Die();
                        }
                    }

                    //Проверка на столкновение с едой
                    if(Snake[0].X==food.X && Snake[0].Y == food.Y)
                    {
                       Eat();
                    }
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void pbCanvas_Click(object sender, EventArgs e)
        {

        }

        private void pbCanvas_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!Params.GameOver)
            {
                //Set colour of snake

                //Draw snake
                for (int i = 0; i < Snake.Count; i++)
                {
                    Brush snakeColour;
                    if (i == 0)
                        snakeColour = Brushes.Black;     //Draw head
                    else
                        snakeColour = Brushes.Green;    //Rest of body

                    //Draw snake
                    canvas.FillEllipse(snakeColour,
                        new Rectangle(Snake[i].X * Params.Width,
                                      Snake[i].Y * Params.Height,
                                      Params.Width, Params.Height));


                    //Draw Food
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * Params.Width,
                             food.Y * Params.Height, Params.Width, Params.Height));

                }
            }
            else
            {
                string gameOver = "Game over \nYour final score is: " + Params.Score + "\nPress Enter to try again";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            InputManager.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            InputManager.ChangeState(e.KeyCode, false);
        }

        private void Die()
        {
            Params.GameOver = true;
        }
        private void Eat()
        {
            //Добавляем к телу круг
            Krug food = new Krug();
            food.X = Snake[Snake.Count - 1].X;
            food.Y = Snake[Snake.Count - 1].Y;

            Snake.Add(food);

            //Добавляем очки
            Params.Score += Params.Points;
            lblScore.Text = Params.Score.ToString();

            SpawnFood();
        }
    }
}
