using Microsoft.Win32;
using PromptGenerator.Services;
using System.Windows;

namespace PromptGenerator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddLanguageButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ViewModels.MainViewModel;
            var name = Microsoft.VisualBasic.Interaction.InputBox("Nom du language :", "Ajouter Language", "Nouvel Language");
            vm?.AddLanguageToBaseCommand.Execute(name);
        }

        private void AddAppTypeButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ViewModels.MainViewModel;
            var name = Microsoft.VisualBasic.Interaction.InputBox("Nom du type d'application :", "Ajouter Type", "Nouveau Type");
            vm?.AddAppTypeToBaseCommand.Execute(name);
        }

        private void AddDomainButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ViewModels.MainViewModel;
            var name = Microsoft.VisualBasic.Interaction.InputBox("Nom du domaine :", "Ajouter Domaine", "Nouveau Domaine");
            vm?.AddDomainToBaseCommand.Execute(name);
        }

        private void ExportTxt_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ViewModels.MainViewModel;
            vm?.ExportCommand.Execute("txt");
        }

        private void ExportJson_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ViewModels.MainViewModel;
            vm?.ExportCommand.Execute("json");
        }
    }
}
