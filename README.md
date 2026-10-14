# Flash Card Manager (.NET MAUI & SQLite)

A cross-platform flashcard study application built with .NET MAUI and C#, featuring local SQLite database persistence, interactive study sessions, and full CRUD card management.

## Features

- **Interactive Study Interface:** Presents randomized cards with masked answers, right/wrong tracking, and immediate session score updating.
- **Full Database CRUD:** Supports adding new cards, editing existing questions and answers, and deleting records directly from an embedded SQLite database.
- **UI State & Data Binding:** Uses a tabbed layout, `CollectionView` selection handling, input validation, and dynamic panel visibility transitions in XAML.

## Tech Stack

- **Framework:** .NET MAUI (Targeting MacCatalyst / macOS)
- **Language:** C#
- **Database:** SQLite via `Microsoft.Data.Sqlite`
- **Architecture:** Code-behind with data binding (`BindingList<T>`) and separated database manager service (`DBManager`)

## Project Structure

- `MainPage.xaml` / `MainPage.xaml.cs`: Core interface and event-driven view logic handling study sessions and management views.
- `DBManager.cs`: Service layer managing SQLite connection lifecycles, parameterized queries, and data mapping.
- `Card.cs`: Data model defining card entities, study statistics (`NumRight`, `NumWrong`), and record keys.

## Setup & Running

1. Ensure the .NET SDK with MAUI workloads is installed:

dotnet workload install maui

2. Navigate to the project directory:

cd WellsP9

3. Restore dependencies and run on macOS:

dotnet build -t:Run -f net10.0-maccatalyst