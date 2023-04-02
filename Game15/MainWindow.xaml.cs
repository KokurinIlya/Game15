using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Media.Animation;


namespace Game15
{
    public partial class MainWindow : Window
    {
        // Модель поля
        int[,] pole = new int[4, 4];
        Button animButton;
        double width, height;
        bool animCompleted;

        public MainWindow()
        {
            InitializeComponent();
            CreateButtons();
            animButton = null;
            width = 0;
        }

        private void CreateButtons()
        {
            Random random = new Random();

            for(int i = 0; i < 15; i++)
            {
                // Grid.Column="1" Grid.Row="1"
                Button button = new Button();
                button.Content = (i + 1);
                button.FontSize = 40;

                SetRandomButton(button, random, i);
                //SetCorrectButton(button, random, i);

                button.Click += new RoutedEventHandler(onClick);
                grid.Children.Add(button);
            }
            // if SetCorrectButton(...)
            //ShuffleButtons(random, 10);
        }

        private void ShuffleButtons(Random random, int n)
        {
            int i = 3, j = 3;
            while (n-- > 0)
            {
                int dir = random.Next(4);
                switch (dir)
                {
                    case 0: // Up
                        if(i > 0)
                        {
                            pole[i, j] = pole[i - 1, j];
                            pole[i - 1, j] = 0;
                            Button button = FindButton(i - 1, j);
                            if (button != null)
                                Grid.SetRow(button, i + 1);
                            i--;
                        }
                        break;

                    case 1: // Down
                        if (i < 3)
                        {
                            pole[i, j] = pole[i + 1, j];
                            pole[i + 1, j] = 0;
                            Button button = FindButton(i + 1, j);
                            if (button != null)
                                Grid.SetRow(button, i - 1);
                            i++;
                        }
                        break;

                    case 2: // Left
                        if (j > 0)
                        {
                            pole[i, j] = pole[i, j - 1];
                            pole[i, j - 1] = 0;
                            Button button = FindButton(i, j - 1);
                            if (button != null)
                                Grid.SetColumn(button, j + 1);
                            j--;
                        }
                        break;

                    case 3: // Right
                        if (j < 3)
                        {
                            pole[i, j] = pole[i, j + 1];
                            pole[i, j + 1] = 0;
                            Button button = FindButton(i, j + 1);
                            if (button != null)
                                Grid.SetColumn(button, j - 1);
                            j++;
                        }
                        break;
                }
            }
        }

        private Button FindButton(int row, int col)
        {
            for(int i = 0; i < grid.Children.Count; i++)
            {
                Button button = (Button)grid.Children[i];
                if(Grid.GetRow(button) == row)
                    if(Grid.GetColumn(button) == col)
                        return button;
            }
            return null;
        }

        private void SetCorrectButton(Button button, Random random, int i)
        {
            int y = i / 4;
            int x = i % 4;
            pole[y, x] = i + 1;

            Grid.SetRow(button, y);
            Grid.SetColumn(button, x);
        }

        private void SetRandomButton(Button button, Random random, int i)
        {
            int x = random.Next(0, 4);
            int y = random.Next(0, 4);

            while (pole[y, x] > 0)
            {
                x = random.Next(0, 4);
                y = random.Next(0, 4);
            }

            pole[y, x] = i + 1;

            Grid.SetRow(button, y);
            Grid.SetColumn(button, x);
        }

        private void onClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            animButton = button;
            width = button.ActualWidth;
            height = button.ActualHeight;
            animCompleted = false;
            AnimateButton1(button);
        }

        private void Anim_Completed(object sender, EventArgs e)
        {
            Button button = animButton;
            int row = Grid.GetRow(button);
            int col = Grid.GetColumn(button);

            if (col != 0 && pole[row, col - 1] == 0)
            {
                Grid.SetColumn(button, col - 1);
                pole[row, col - 1] = pole[row, col];
                pole[row, col] = 0;
            }
            if (col != 3 && pole[row, col + 1] == 0)
            {
                Grid.SetColumn(button, col + 1);
                pole[row, col + 1] = pole[row, col];
                pole[row, col] = 0;
            }

            if (row != 0 && pole[row - 1, col] == 0)
            {
                Grid.SetRow(button, row - 1);
                pole[row - 1, col] = pole[row, col];
                pole[row, col] = 0;
            }
            if (row != 3 && pole[row + 1, col] == 0)
            {
                Grid.SetRow(button, row + 1);
                pole[row + 1, col] = pole[row, col];
                pole[row, col] = 0;
            }

            if (GameOver())
            {
                MessageBox.Show("You Win!!", "Game Over", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }

            AnimateButton1(button);
        }

        private void AnimateButton1(Button button)
        {
            DoubleAnimation anim = new DoubleAnimation();
            anim.From = width;
            anim.To = width * 0.75;
            anim.Duration = TimeSpan.FromMilliseconds(500);
            //anim.RepeatBehavior = new RepeatBehavior(3);
            anim.Completed += Anim_Completed2;
            button.BeginAnimation(Button.WidthProperty, anim);

            DoubleAnimation anim2 = new DoubleAnimation();
            anim2.From = height;
            anim2.To = height * 0.75;
            anim2.Duration = TimeSpan.FromMilliseconds(500);
            //anim.RepeatBehavior = new RepeatBehavior(3);
            //anim2.Completed += Anim_Completed2;
            button.BeginAnimation(Button.HeightProperty, anim2);

            SolidColorBrush brush = new SolidColorBrush();
            button.Background = brush;
            ColorAnimation colorAnimation = new ColorAnimation();
            colorAnimation.From = Colors.Gray;
            colorAnimation.To = Colors.Red;
            colorAnimation.Duration = TimeSpan.FromMilliseconds(500);
            button.Background.BeginAnimation(SolidColorBrush.ColorProperty, 
                colorAnimation);

            DoubleAnimation opacityAnimation = new DoubleAnimation();
            opacityAnimation.From = 1.0;
            opacityAnimation.To = 0.5;
            opacityAnimation.Duration = TimeSpan.FromSeconds(0.5);
            button.BeginAnimation(Button.OpacityProperty, opacityAnimation);
        }

        private void Anim_Completed2(object sender, EventArgs e)
        {
            Button button = animButton;
            DoubleAnimation anim = new DoubleAnimation();
            anim.From = width * 0.75;
            anim.To = width;
            anim.Duration = TimeSpan.FromMilliseconds(500);
            if (!animCompleted)
            {
                animCompleted = true;
                anim.Completed += Anim_Completed;
            }
            //anim.RepeatBehavior = new RepeatBehavior(3);
            button.BeginAnimation(Button.WidthProperty, anim);

            DoubleAnimation anim2 = new DoubleAnimation();
            anim2.From = height * 0.75;
            anim2.To = height;
            anim2.Duration = TimeSpan.FromMilliseconds(500);
            button.BeginAnimation(Button.HeightProperty, anim2);

            SolidColorBrush brush = new SolidColorBrush();
            button.Background = brush;
            ColorAnimation colorAnimation = new ColorAnimation();
            colorAnimation.From = Colors.Red;
            colorAnimation.To = Colors.Gray;
            colorAnimation.Duration = TimeSpan.FromMilliseconds(500);
            button.Background.BeginAnimation(SolidColorBrush.ColorProperty,
                colorAnimation);

            DoubleAnimation opacityAnimation = new DoubleAnimation();
            opacityAnimation.From = 0.5;
            opacityAnimation.To = 1.0;
            opacityAnimation.Duration = TimeSpan.FromSeconds(0.5);
            button.BeginAnimation(Button.OpacityProperty, opacityAnimation);
        }

        private bool GameOver()
        {
            for(int i=0; i<4; i++)
            {
                for(int j=0; j<4; j++)
                {
                    if (pole[i, j] == 0) continue;
                    if (pole[i, j] != i * 4 + j + 1) 
                        return false;
                }
            }
            return true;
        }

        private void onMouseDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void Button_MouseLeftButtonDown(object sender, 
            MouseButtonEventArgs e)
        {
            
        }

        private void Button_MouseDown(object sender, 
            MouseButtonEventArgs e)
        {
            
        }
    }
}
