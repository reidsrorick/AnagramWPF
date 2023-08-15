using System;
using System.Collections.Generic;
using System.IO;
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
using static System.Net.WebRequestMethods;

namespace AnagramWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string word;
        private string selectedWord;
        private string oldWord;
        private List<string> anagrams;
        
        public static string pathy = "Anagrams.csv";
        public MainWindow()
        {
            InitializeComponent();
            lblPercentage.Visibility = Visibility.Hidden;
            lblShown.Visibility = Visibility.Hidden;
            lblTotal.Visibility = Visibility.Hidden;
            lblTime.Visibility = Visibility.Hidden;
            gif.Visibility = Visibility.Hidden;
            //var gifPath = "C:\\Users\\reids\\OneDrive\\Desktop\\Code\\AnagramWPF\\Loading.gif";
            //var gifUri = new Uri(gifPath);
            //gif.Source = gifUri;
            //gif.LoadedBehavior = MediaState.Manual;
            //gif.Stop();

        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            lstbxAnagrams.Items.Clear();

            word = txtWord.Text;
            //if (word.Length > 5)
            //{
            //      gif.Visibility = Visibility.Visible;
            //    gif.Play();
            //}
            if (oldWord != word)
            {
                oldWord = null;
            }
            else
            {
                string specify = txtSpecification.Text;
                double count = 0;
                double anagramcount = anagrams.Count;
                DateTime startTime = DateTime.Now;
                foreach (string anagram in anagrams)
                {
                    if (anagram.Substring(0, specify.Length) == specify)
                    {
                        lstbxAnagrams.Items.Add(anagram);
                        count++;
                    }

                }
                lblShown.Content = $"Anagrams Shown: {count.ToString("#,##0")}";
                lblTotal.Content = $"Total iterations for your word: {anagramcount.ToString("#,##0")}";
                double percentage = (count / anagramcount);
                lblPercentage.Content = $"Percentage of anagrams shown: {percentage.ToString("P2")}";
                TimeSpan difference = DateTime.Now - startTime;
                lblTime.Content = $"Runtime: {difference.TotalSeconds.ToString("N2")} seconds";
                lblShown.Visibility = Visibility.Visible;
                lblTotal.Visibility = Visibility.Visible;
                lblTime.Visibility = Visibility.Visible;
                lblPercentage.Visibility = Visibility.Visible;
            }
            if (oldWord == null)
            {
                oldWord = txtWord.Text;
                string specify = txtSpecification.Text;
                DateTime startTime = DateTime.Now;
                anagrams = GenerateAnagrams(word);
                double count = 0;
                double anagramcount = anagrams.Count;
                foreach (string anagram in anagrams)
                {
                    if (anagram.Substring(0, specify.Length) == specify)
                    {
                        lstbxAnagrams.Items.Add(anagram);
                        count++;
                    }

                }
                //gif.Visibility = Visibility.Hidden;
                lblShown.Content = $"Anagrams Shown: {count.ToString("#,##0")}";
                lblTotal.Content = $"Total iterations for your word: {anagramcount.ToString("#,##0")}";
                double percentage = (count / anagramcount);
                lblPercentage.Content = $"Percentage of anagrams shown: {percentage.ToString("P2")}";
                TimeSpan difference = DateTime.Now - startTime;
                lblTime.Content = $"Runtime: {difference.TotalSeconds.ToString("N2")} seconds";
                lblShown.Visibility = Visibility.Visible;
                lblTotal.Visibility = Visibility.Visible;
                lblTime.Visibility = Visibility.Visible;
                lblPercentage.Visibility = Visibility.Visible;
            }
            
        }
        static List<string> GenerateAnagrams(string word)
        {
            List<string> anagrams = new List<string>();
            if (word.Length == 1)
            {
                anagrams.Add(word);
            }
            else
            {
                for (int i = 0; i < word.Length; i++)
                {
                    string word1 = word.Substring(0, i);
                    string word2 = word.Substring(i + 1);
                    string remaining = word1 + word2;
                    List<string> subAnagrams = GenerateAnagrams(remaining);
                    foreach (string subAnagram in subAnagrams)
                    {
                        anagrams.Add(word[i] + subAnagram);
                    }
                }
            }
            return anagrams;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //pathy = "Anagrams.csv";
            string newInput = $"{word},{selectedWord}";
            List<string> csvOutput = new List<string>();
            //csvOutput.Add("Original, Anagram");
            string[] file = System.IO.File.ReadAllLines(pathy);
            foreach (string line in file)
            {
                csvOutput.Add(line);
            }
            csvOutput.Add(newInput);
            System.IO.File.WriteAllLines(pathy, csvOutput.ToArray());

        }
        private void lstbxAnagrams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(lstbxAnagrams.SelectedItem == null))
            {
                selectedWord = lstbxAnagrams.SelectedItem.ToString();
            }

        }
        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            CSVWindow flarg = new CSVWindow();
            flarg.Title = "Anagrams";
            flarg.Show();
        }
        private void btnResetCSV_Click(object sender, RoutedEventArgs e)
        {
            if (txtSpecification.Text == "707425")
            {
                List<string> csvOutput = new List<string>();
                csvOutput.Add("Original, Anagram");
                System.IO.File.WriteAllLines(pathy, csvOutput.ToArray());
            }

        }
    }
}