# PromptGenerator (WPF .NET 8)

Application WPF (.NET 8) pour générer des prompts à partir de Language, ApplicationType, ApplicationDomain et d'une liste d'étapes.

Fonctionnalités :
- SQLite via EF Core
- Logging avec Serilog dans `logs/log.txt`
- Onglet Génération : choix Language / Type / Domaine + liste editable de Steps + génération + export (.txt/.json)
- Onglet Administration : CRUD basique (listes affichées, commandes pour reload/seed)
- Bouton pour charger des données de démonstration

Prérequis :
- .NET 8 SDK
- (Optionnel) Visual Studio 2022/2023

Installation / exécution :
1. dotnet restore
2. dotnet build
3. dotnet run --project PromptGenerator

Notes :
- La base SQLite est créée automatiquement dans `PromptGenerator/data/promptgenerator.db`.
- Les logs sont écrits dans `PromptGenerator/logs/log.txt`.
- Pour améliorer l'UI (thème Material), ajouter la dépendance `MaterialDesignThemes` et ajuster XAML.
