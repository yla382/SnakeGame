using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private List<Circle> snake = new List<Circle>();
        private Circle food = new Circle();
        public Form1()
        {
            InitializeComponent();
            new Settings();
            gameTimer.Interval = 1000 / Settings.speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            startGame();
        }

        private void startGame()
        {

            labelGameOver.Visible = false;
            new Settings();
            snake.Clear();
            Circle head = new Circle();
            head.x = 10;
            head.y = 5;
            snake.Add(head);

            labelScore.Text = Settings.score.ToString();
            generateFood();
        }

        private void generateFood()
        {
            int maxXPos = canvas.Size.Width / Settings.width;
            int maxYPos = canvas.Size.Height / Settings.height;

            Random random = new Random();
            food = new Circle();
            food.x = random.Next(0, maxXPos);
            food.y = random.Next(0, maxYPos);
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            if(Settings.gameOver == true)
            {
                if(Input.keyPressed(Keys.Enter))
                {
                    startGame();
                }
            } 
            else
            {
                if (Input.keyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.keyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.keyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.keyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                movePlayer();
            }
            canvas.Invalidate();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {

        }
  

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canva = e.Graphics;
            if(Settings.gameOver == false)
            {
                //Set color of snake
                Brush snakeColour;
                //Draw Snake
                for(int i = 0; i < snake.Count; i++)
                {
                    if (i == 0)
                        snakeColour = Brushes.Black;
                    else
                        snakeColour = Brushes.Green;

                    canva.FillEllipse(snakeColour, new Rectangle(snake[i].x * Settings.width,
                                                                 snake[i].y * Settings.height,
                                                                 Settings.width, Settings.height));
                    canva.FillEllipse(Brushes.Red, new Rectangle(food.x * Settings.width,
                                                                 food.y * Settings.height,
                                                                 Settings.width, Settings.height));
                }
            }
            else
            {
                String gameOver = "Game Over\n Your final score is: " + Settings.score + "\nPress Enter to try again";
                labelGameOver.Text = gameOver;
                labelGameOver.Visible = true;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }
 
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }
 

        private void movePlayer()
        {
            for(int i = snake.Count - 1; i >= 0; i--)
            {
                //Move head
                if(i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            snake[i].x++;
                            break;
                        case Direction.Left:
                            snake[i].x--;
                            break;
                        case Direction.Up:
                            snake[i].y--;
                            break;
                        case Direction.Down:
                            snake[i].y++;
                            break;
                    }
                    int maxXPos = canvas.Size.Width / Settings.width;
                    int maxYPos = canvas.Size.Height / Settings.height;

                    if(snake[i].x < 0 || snake[i].y < 0 || snake[i].x >= maxXPos || snake[i].y >= maxYPos)
                    {
                        die();
                    }

                    for(int j = 1; j < snake.Count; j++)
                    {
                        if(snake[i].x == snake[j].x && snake[i].y == snake[j].y)
                        {
                            die();
                        }
                    }

                    if(snake[0].x == food.x && snake[0].y == food.y)
                    {
                        eat();
                    }
                }
                else
                {
                    //Move body
                    snake[i].x = snake[i - 1].x;
                    snake[i].y = snake[i - 1].y;
                }
            }
        }

        private void die()
        {
            Settings.gameOver = true;
        }

        private void eat()
        {
            Circle food = new Circle();
            food.x = snake[snake.Count - 1].x;
            food.y = snake[snake.Count - 1].y;

            snake.Add(food);

            Settings.score += Settings.points;
            labelScore.Text = Settings.score.ToString();

            generateFood();
        }
    }
}
