using HAKATON_2022_NASA.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

namespace HAKATON_2022_NASA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random random = new Random();
        List<Hero> heroList = new List<Hero>();
        List<Hero> bestHeroList = new List<Hero>();
        TextBlock[] infoTextBlocks = new TextBlock[28];
        Hero bestHero = null;
        public MainWindow()
        {
            InitializeComponent();
            var colors = typeof(Brushes).GetProperties().Select(x => x.Name).ToList();

            for (int i = 1; i < 29; i++)
            {
                int colorIndex = i * 2;
                heroList.Add(new Hero(i*10, i, 5, new BrushConverter().ConvertFromString(colors[colorIndex]) as Brush));
                infoTextBlocks[i - 1] = new TextBlock();
            }

            for (int i = 0; i < 28; i += 2)
            {
                int row = i / 2;
                Hero hero1 = heroList.ElementAt(i);
                infoTextBlocks[i].Text = $"{hero1.GenerationDescription}, Speed = {hero1.MakeStepCount}, Mutation Value = {hero1.MutationQualifer}.";
                Grid.SetColumn(infoTextBlocks[i], 0);
                Grid.SetRow(infoTextBlocks[i], row);
                infoGrid.Children.Add(infoTextBlocks[i]);
                hero1 = heroList.ElementAt(i+1);
                infoTextBlocks[i + 1].Text = $"{hero1.GenerationDescription}, Speed = {hero1.MakeStepCount}, Mutation Value = {hero1.MutationQualifer}.";
                Grid.SetColumn(infoTextBlocks[i + 1], 1);
                Grid.SetRow(infoTextBlocks[i + 1], row);
                infoGrid.Children.Add(infoTextBlocks[i + 1]);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void StartGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            SimulationCanvas.Children.Clear();
            Simulate(heroList);
            int generation = int.Parse(GenerationNumberTextblock.Text) + int.Parse(GenerationSpeedTextBox.Text);
            GenerationNumberTextblock.Text = generation.ToString();
        }

        private void Simulate(List<Hero> heroList)
        {
            for (int i = 0; i < int.Parse(GenerationSpeedTextBox.Text); i++)
            {
                List<Line> lines = new List<Line>();
                foreach (var hero in heroList.Where(x=>x.IsAlive))
                {
                    int turn = random.Next(1, hero.MutationQualifer);
                    if (turn == 1)
                        Thread.Sleep(10);
                    else if (turn >= hero.MutationQualifer - 2)
                    {
                        hero.makeMutation(turn);
                        SimulationCanvas.Children.Add(hero.makeStep());
                    }
                    else
                        SimulationCanvas.Children.Add(hero.makeStep());

                    if (!hero.IsAlive)
                        bestHeroList.Add(hero);
                }
                if (heroList.Any(x => x.IsAlive))
                {
                    bestHero = heroList.First(x => x.IsAlive && x.MakeStepCount == heroList.Where(y => y.IsAlive).Max(y => y.MakeStepCount));
                    BestHeroTextBlock.Text = $"{bestHero.GenerationDescription} is the best Hero, Speed = {bestHero.MakeStepCount}, Mutation Value = {bestHero.MutationQualifer}.";
                }
                else
                    BestHeroTextBlock.Text = $"Best Hero";
            }
            for (int i = 0; i < 28; i += 2)
            {
                Hero hero1 = heroList.ElementAt(i);
                infoTextBlocks[i].Text = $"{hero1.GenerationDescription}, Speed = {hero1.MakeStepCount}, Mutation Value = {hero1.MutationQualifer}.";
                hero1 = heroList.ElementAt(i + 1);
                infoTextBlocks[i + 1].Text = $"{hero1.GenerationDescription}, Speed = {hero1.MakeStepCount}, Mutation Value = {hero1.MutationQualifer}.";
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            heroList = new List<Hero>();
            var colors = typeof(Brushes).GetProperties().Select(x => x.Name).ToList();

            for (int i = 1; i < 29; i++)
            {
                int colorIndex = i * 2;
                heroList.Add(new Hero(i * 10, i, string.IsNullOrEmpty(MutationQualiferTextBox.Text) ? int.Parse(MutationQualiferTextBox.Text) : 5, new BrushConverter().ConvertFromString(colors[colorIndex]) as Brush));
            }

            for (int i = 0; i < 28; i += 2)
            {
                Hero hero1 = heroList.ElementAt(i);
                infoTextBlocks[i].Text = $"{hero1.GenerationDescription}, Speed = {hero1.MakeStepCount}, Mutation Value = {hero1.MutationQualifer}.";
                hero1 = heroList.ElementAt(i + 1);
                infoTextBlocks[i + 1].Text = $"{hero1.GenerationDescription}, Speed = {hero1.MakeStepCount}, Mutation Value = {hero1.MutationQualifer}.";
            }

            BestHeroTextBlock.Text = "Best Hero";
            SimulationCanvas.Children.Clear();
        }

        private void SetMutationButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var hero in heroList)
                hero.MutationQualifer = int.Parse(MutationQualiferTextBox.Text);

            for (int i = 0; i < 28; i += 2)
            {
                Hero hero1 = heroList.ElementAt(i);
                infoTextBlocks[i].Text = $"{hero1.GenerationDescription}, Speed = {hero1.MakeStepCount}, Mutation Value = {hero1.MutationQualifer}.";
                hero1 = heroList.ElementAt(i + 1);
                infoTextBlocks[i + 1].Text = $"{hero1.GenerationDescription}, Speed = {hero1.MakeStepCount}, Mutation Value = {hero1.MutationQualifer}.";
            }
        }

        private void GetListHeroButton_Click(object sender, RoutedEventArgs e)
        {
            if (bestHeroList.Count() == 0)
                return;
            else
            {
                File.WriteAllLines("Heros.txt", bestHeroList.Select(x => $"{x.GenerationNumber}, Speed = {x.MakeStepCount}, Mutation = {x.MutationQualifer}, Generation = {x.GenerationNumber}").ToArray());
                MessageBox.Show("File Created!");
            }
        }

        private void MutationQualiferTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void GenerationSpeedTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
