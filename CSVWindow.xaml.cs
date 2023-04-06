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
using System.Windows.Shapes;

namespace AnagramWPF
{
    /// <summary>
    /// Interaction logic for CSVWindow.xaml
    /// </summary>
    public partial class CSVWindow : Window
    {
        private Dictionary<string, List<string>> AnagramsDict = new Dictionary<string, List<string>>();
        private string selectedWord;
        private string selectedAna;
        public CSVWindow()
        {
            InitializeComponent();

            string path = MainWindow.pathy;
            string[] csvData = File.ReadAllLines(path);
            AnagramsDict.Add("All", new List<string>());
            cbxOriginal.Items.Add("All");
            foreach (string line in csvData.Skip(1))
            {
                string[] split = line.Split(",");
                string original = split[0];
                string anagram = split[1];
                List<string> list = new List<string>();                
                if (!(cbxOriginal.Items.Contains(original)))
                {
                    cbxOriginal.Items.Add(original);
                }
                if (!(AnagramsDict.ContainsKey(original)))
                {
                    AnagramsDict.Add(original, list);
                }
                AnagramsDict[original].Add(anagram);
                AnagramsDict["All"].Add(anagram);
            }
            
        }

        private void cbxOriginal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstAnagramz.Items.Clear();
            foreach (string key in AnagramsDict.Keys)
            {
                if (AnagramsDict[key].Count < 1 && key != "All")
                {
                    AnagramsDict.Remove(key);
                    cbxOriginal.Items.Remove(key);
                }

            }

            selectedWord = cbxOriginal.SelectedItem.ToString();
            foreach (var anagram in AnagramsDict[selectedWord])
            {
                lstAnagramz.Items.Add(anagram);
            }
            
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string removeAna = string.Empty;
            string removeKey = string.Empty;
            foreach (var key in AnagramsDict.Keys)
            {
                foreach (string x in AnagramsDict[key])
                {
                    if (selectedAna == x)
                    {
                        removeAna = x;
                        removeKey = key;
                    }
                }
            }
            AnagramsDict[removeKey].Remove(removeAna);
            AnagramsDict["All"].Remove(removeAna);
            lstAnagramz.Items.Remove(removeAna);
            SaveData();

        }
        private void SaveData()
        {
            List<string> list = new List<string>();
            list.Add("Original,Anagram");
            foreach (var key in AnagramsDict.Keys.Skip(1))
            {
                foreach (string x in AnagramsDict[key])
                {
                    
                    list.Add($"{key},{x}");
                }
            }
            File.WriteAllLines(MainWindow.pathy,list);
        }

        private void lstAnagramz_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedAna = (string)lstAnagramz.SelectedItem;
        }
    }
}
