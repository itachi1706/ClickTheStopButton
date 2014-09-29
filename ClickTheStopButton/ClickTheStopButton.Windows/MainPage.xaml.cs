using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ClickTheStopButton
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        DispatcherTimer pregame, ingame;
        long preTime = 6;
        long preTimeToTick = 1;
        long timesTicked = 1;
        long timesToTick = 100;
        long preTimesChecked = 0;


        public void StartTimer()
        {
            ingame = new DispatcherTimer();
            ingame.Tick += ingame_Tick;
            ingame.Interval = new TimeSpan(0, 0, 0, 0, 1);
            ingame.Start();
        }

        void ingame_Tick(object sender, object e)
        {
            txtStopwatch.Text = parseToTime(timesTicked);
            if (timesTicked > timesToTick)
            {
                ingame.Stop();
                txtResults.Foreground = new SolidColorBrush(Colors.Red);
                txtResults.Text = "TIMES UP!";
                if (ingame.IsEnabled)
                {
                    ingame.Stop();
                }
                if (pregame.IsEnabled)
                {
                    pregame.Stop();
                }
                btnReset.Visibility = Windows.UI.Xaml.Visibility.Visible;
                btnStopClock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            timesTicked++;
        }

        public void StartPreTimer()
        {
            pregame = new DispatcherTimer();
            pregame.Tick += pregame_Tick;
            pregame.Interval = new TimeSpan(0, 0, 1);
            pregame.Start();
        }

        void pregame_Tick(object sender, object e)
        {
            txtStopwatch.Text = parseToTime(preTime);
            if (preTime < preTimeToTick)
            {
                pregame.Stop();
                btnStopClock.IsEnabled = true;
                StartTimer();
            }
            preTime--;
        }

        void generateRandom()
        {
            Random random = new Random();
            preTimesChecked = random.Next(1, 50);
            txtTime.Text = parseToTime(preTimesChecked);
        }

        private String parseToTime(long time)
        {
            long second = 0, millisecond = 0, hours = 0, minutes = 0;
            if (time >= 10)
            {
                //Parse to second and millisecond
                second = time / 10;
                millisecond = time % 10;
                if (second >= 60)
                {
                    //Parse to minutes and seconds
                    minutes = second / 60;
                    second = second % 60;
                    if (minutes >= 60)
                    {
                        //Parse as hours and minutes
                        hours = minutes / 60;
                        minutes = minutes % 60;
                    }
                }
            }
            else
            {
                //Parse as millisecond only
                millisecond = time;
            }

            //Parse as a string
            String returnValue = "";
            returnValue += hours + ":";
            if (minutes >= 10)
            {
                returnValue += minutes + ":";
            }
            else
            {
                returnValue += "0" + minutes + ":";
            }
            if (second >= 10)
            {
                returnValue += second + ":";
            }
            else
            {
                returnValue += "0" + second + ":";
            }
            if (millisecond >= 10)
            {
                returnValue += millisecond + ":";
            }
            else
            {
                returnValue += "0" + millisecond;
            }
            return returnValue;

        }

        public MainPage()
        {
            this.InitializeComponent();
        }

        private bool checkVictory()
        {
            txtDebug.Text = (timesTicked -1) + ":" + preTimesChecked;
            if ((timesTicked - 1) == preTimesChecked)
            {
                return true;
            }
            return false;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            btnReset.Content = "Reset!";
            txtPrompt.Visibility = Windows.UI.Xaml.Visibility.Visible;
            txtResults.Text = "Game will start soon! ";
            preTime = 6;
            timesTicked = 1;
            txtStopwatch.Text = parseToTime(preTime);
            generateRandom();
            StartPreTimer();
            btnStopClock.IsEnabled = false;
            txtResults.Foreground = new SolidColorBrush(Colors.White);
            txtTime.Foreground = new SolidColorBrush(Colors.Gold);
            btnReset.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            btnStopClock.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void btnStopClock_Click(object sender, RoutedEventArgs e)
        {
            if (ingame.IsEnabled)
            {
                ingame.Stop();
            }
            if (pregame.IsEnabled)
            {
                pregame.Stop();
            }
            btnReset.Visibility = Windows.UI.Xaml.Visibility.Visible;
            btnStopClock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            txtTime.Foreground = new SolidColorBrush(Colors.Gray);
            if (checkVictory())
            {
                txtResults.Foreground = new SolidColorBrush(Colors.Green);
                txtResults.Text = "YOU WIN!";
            }
            else
            {
                txtResults.Foreground = new SolidColorBrush(Colors.Red);
                txtResults.Text = "YOU LOSE";
            }
            
        }
    }
}
