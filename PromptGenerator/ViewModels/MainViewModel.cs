using Microsoft.EntityFrameworkCore;
using PromptGenerator.Data;
using PromptGenerator.Models;
using PromptGenerator.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;

namespace PromptGenerator.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Language> Languages { get; } = new();
        public ObservableCollection<ApplicationType> ApplicationTypes { get; } = new();
        public ObservableCollection<ApplicationDomain> ApplicationDomains { get; } = new();
        public ObservableCollection<Step> Steps { get; } = new();

        private Language? _selectedLanguage;
        public Language? SelectedLanguage { get => _selectedLanguage; set { _selectedLanguage = value; OnPropertyChanged(nameof(SelectedLanguage)); } }

        private ApplicationType? _selectedAppType;
        public ApplicationType? SelectedAppType { get => _selectedAppType; set { _selectedAppType = value; OnPropertyChanged(nameof(SelectedAppType)); } }

        private ApplicationDomain? _selectedDomain;
        public ApplicationDomain? SelectedDomain { get => _selectedDomain; set { _selectedDomain = value; OnPropertyChanged(nameof(SelectedDomain)); } }

        private string _generatedPrompt = string.Empty;
        public string GeneratedPrompt { get => _generatedPrompt; set { _generatedPrompt = value; OnPropertyChanged(nameof(GeneratedPrompt)); } }

        public RelayCommand LoadFromDbCommand { get; }
        public RelayCommand AddLanguageToBaseCommand { get; }
        public RelayCommand AddAppTypeToBaseCommand { get; }
        public RelayCommand AddDomainToBaseCommand { get; }
        public RelayCommand GenerateCommand { get; }
        public RelayCommand ExportCommand { get; }
        public RelayCommand SeedDemoDataCommand { get; }
        public RelayCommand SaveStepCommand { get; }
        public RelayCommand DeleteStepCommand { get; }

        private Step? _selectedStep;
        public Step? SelectedStep { get => _selectedStep; set { _selectedStep = value; OnPropertyChanged(nameof(SelectedStep)); } }

        public MainViewModel()
        {
            LoadFromDbCommand = new RelayCommand(_ => LoadFromDatabase());
            AddLanguageToBaseCommand = new RelayCommand(param => AddLanguage(param?.ToString() ?? string.Empty));
            AddAppTypeToBaseCommand = new RelayCommand(param => AddAppType(param?.ToString() ?? string.Empty));
            AddDomainToBaseCommand = new RelayCommand(param => AddDomain(param?.ToString() ?? string.Empty));
            GenerateCommand = new RelayCommand(_ => GeneratePrompt());
            ExportCommand = new RelayCommand(param => ExportPrompt(param?.ToString() ?? "txt"));
            SeedDemoDataCommand = new RelayCommand(_ => { DataSeeder.SeedDemoData(); LoadFromDatabase(); });
            SaveStepCommand = new RelayCommand(_ => SaveStep());
            DeleteStepCommand = new RelayCommand(_ => DeleteStep());

            // initial load
            LoadFromDatabase();
        }

        public void LoadFromDatabase()
        {
            try
            {
                using var db = new PromptDbContext();
                db.Database.EnsureCreated();

                Languages.Clear();
                foreach (var l in db.Languages.AsNoTracking().OrderBy(x => x.Name)) Languages.Add(l);

                ApplicationTypes.Clear();
                foreach (var t in db.ApplicationTypes.AsNoTracking().OrderBy(x => x.Name)) ApplicationTypes.Add(t);

                ApplicationDomains.Clear();
                foreach (var d in db.ApplicationDomains.AsNoTracking().OrderBy(x => x.Name)) ApplicationDomains.Add(d);

                Steps.Clear();
                foreach (var s in db.Steps.AsNoTracking().OrderBy(x => x.OrderIndex)) Steps.Add(s);

                AppLogger.Log.Information("Loaded data from database");
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error(ex, "Error loading from database");
                MessageBox.Show("Erreur lors du chargement de la base : " + ex.Message);
            }
        }

        private void AddLanguage(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return;
            using var db = new PromptDbContext();
            var entity = new Language { Name = name.Trim() };
            db.Languages.Add(entity);
            db.SaveChanges();
            Languages.Add(entity);
            AppLogger.Log.Information("Added Language {Name}", name);
        }

        private void AddAppType(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return;
            using var db = new PromptDbContext();
            var entity = new ApplicationType { Name = name.Trim() };
            db.ApplicationTypes.Add(entity);
            db.SaveChanges();
            ApplicationTypes.Add(entity);
            AppLogger.Log.Information("Added ApplicationType {Name}", name);
        }

        private void AddDomain(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return;
            using var db = new PromptDbContext();
            var entity = new ApplicationDomain { Name = name.Trim() };
            db.ApplicationDomains.Add(entity);
            db.SaveChanges();
            ApplicationDomains.Add(entity);
            AppLogger.Log.Information("Added ApplicationDomain {Name}", name);
        }

        private void GeneratePrompt()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Language: {SelectedLanguage?.Name ?? "<non sélectionné>"}");
            sb.AppendLine($"Application Type: {SelectedAppType?.Name ?? "<non sélectionné>"}");
            sb.AppendLine($"Domain: {SelectedDomain?.Name ?? "<non sélectionné>"}");
            sb.AppendLine();
            sb.AppendLine("Steps:");
            foreach (var s in Steps.OrderBy(x => x.OrderIndex))
            {
                sb.AppendLine($"{s.OrderIndex}. {s.Description}");
            }

            GeneratedPrompt = sb.ToString();
            AppLogger.Log.Information("Generated prompt (Language={Language}, Type={Type}, Domain={Domain})",
                SelectedLanguage?.Name, SelectedAppType?.Name, SelectedDomain?.Name);
        }

        private void ExportPrompt(string format)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(GeneratedPrompt))
                {
                    MessageBox.Show("Aucun prompt généré à exporter.");
                    return;
                }

                var exportsDir = Path.Combine(Directory.GetCurrentDirectory(), "exports");
                if (!Directory.Exists(exportsDir)) Directory.CreateDirectory(exportsDir);

                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                if (format.ToLower() == "json")
                {
                    var obj = new
                    {
                        Language = SelectedLanguage?.Name,
                        ApplicationType = SelectedAppType?.Name,
                        Domain = SelectedDomain?.Name,
                        Steps = Steps.OrderBy(x => x.OrderIndex).Select(s => new { s.OrderIndex, s.Description })
                    };
                    var text = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
                    var path = Path.Combine(exportsDir, $"prompt_{timestamp}.json");
                    File.WriteAllText(path, text);
                    AppLogger.Log.Information("Exported prompt to {Path}", path);
                    MessageBox.Show("Exporté en JSON : " + path);
                }
                else
                {
                    var path = Path.Combine(exportsDir, $"prompt_{timestamp}.txt");
                    File.WriteAllText(path, GeneratedPrompt);
                    AppLogger.Log.Information("Exported prompt to {Path}", path);
                    MessageBox.Show("Exporté en TXT : " + path);
                }
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error(ex, "Error exporting prompt");
                MessageBox.Show("Erreur lors de l'export : " + ex.Message);
            }
        }

        private void SaveStep()
        {
            try
            {
                using var db = new PromptDbContext();
                if (SelectedStep == null)
                {
                    var newStep = new Step { Description = "Nouvelle étape", OrderIndex = Steps.Count + 1 };
                    db.Steps.Add(newStep);
                    db.SaveChanges();
                    Steps.Add(newStep);
                    AppLogger.Log.Information("Added Step {Id}", newStep.Id);
                }
                else
                {
                    var s = db.Steps.Find(SelectedStep.Id);
                    if (s != null)
                    {
                        s.Description = SelectedStep.Description;
                        s.OrderIndex = SelectedStep.OrderIndex;
                        db.SaveChanges();
                        AppLogger.Log.Information("Updated Step {Id}", s.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error(ex, "Error saving step");
                MessageBox.Show("Erreur lors de l'enregistrement de l'étape : " + ex.Message);
            }
        }

        private void DeleteStep()
        {
            if (SelectedStep == null) return;
            try
            {
                using var db = new PromptDbContext();
                var s = db.Steps.Find(SelectedStep.Id);
                if (s != null)
                {
                    db.Steps.Remove(s);
                    db.SaveChanges();
                    Steps.Remove(SelectedStep);
                    AppLogger.Log.Information("Deleted Step {Id}", s.Id);
                }
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error(ex, "Error deleting step");
                MessageBox.Show("Erreur lors de la suppression de l'étape : " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
