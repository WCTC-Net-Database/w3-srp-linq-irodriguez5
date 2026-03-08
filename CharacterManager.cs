using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using Spectre.Console;
using W03.Models;
using W03.Services;
namespace W03;

/// <summary>
/// CharacterManager - THE PROBLEM (SRP Violation!)
///
/// LOOK AT THIS CLASS CAREFULLY. It does MANY different things:
/// 1. Reads from files (DisplayCharacters reads lines)
/// 2. Writes to files (AddCharacter would append)
/// 3. Searches data (FindCharacter)
/// 4. Handles user interface (menus, prompts)
/// 5. Business logic (LevelUpCharacter increments level)
///
/// This violates the Single Responsibility Principle!
/// Why is this bad?
/// - Hard to test (need file system for any test)
/// - Hard to reuse (can't use reading without the menu)
/// - Hard to change (modifying file format affects everything)
///
/// YOUR ASSIGNMENT: Refactor this into separate classes:
/// - Character (data class) - see Models/Character.cs
/// - CharacterReader (reading) - see Services/CharacterReader.cs
/// - CharacterWriter (writing) - see Services/CharacterWriter.cs
/// - Keep CharacterManager for just the menu/coordination
///
/// After refactoring, CharacterManager should:
/// - Create Reader and Writer objects
/// - Handle menu display and user input
/// - Call Reader/Writer methods to do the actual work
/// </summary>
public class CharacterManager
{

    private readonly string _filePath = "Files/input.csv";
    private readonly CharacterReader _reader;
    private readonly CharacterWriter _writer;

    public CharacterManager()
    {
        _reader = new CharacterReader(_filePath);
        _writer = new CharacterWriter(_filePath);
    }

    public void AddCharacter()
    {
        // Done: Replace this stub with code that prompts for character details,
        // adds the new character to the CSV file, and displays confirmation.
        // Use string interpolation for formatting output.
        // Hint: You will need to append the new character to the file.
        AnsiConsole.Write(new Rule("[bold pink1]+ Add Character +[/]").RuleStyle("pink1"));
        Console.WriteLine();
        var name = PromptRequired("Enter character name: ");
        var profession = PromptRequired("Enter character profession: ");
        var level = PromptInt("Enter character level (default 1): ", 1);
        var hp = PromptInt("Enter character HP (default 10): ", 10);

        var equipmentList = new List<string>();
        while (true)
        {
            Console.Write("Enter equipment item (leave blank to finish): ");
            var item = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(item))
                break;
            equipmentList.Add(item.Trim());
        }

        var character = new Character(name, profession, level, hp, equipmentList.ToArray());
        _writer.AppendCharacter(character);
        Console.WriteLine($"Character '{character.Name}' added successfully!");


    }

    public void DisplayCharacters()
    {
        // Done: Replace this stub with code that reads all character data from the CSV file
        // and displays each character.
        // Use string interpolation for formatting output.
        // Hint: You will need to parse each line and output the character details.
        var characters = _reader.ReadAll();

       if (characters == null || characters.Count == 0)
       {
           Console.WriteLine("No characters found.");
           return;
        }
        //Table header + stretch goal  ( i did have copilot help with how to do this portion)
        AnsiConsole.Write(new Rule("[bold pink1]+ Characters + [/]").RuleStyle("pink1"));
        Console.WriteLine();
        Console.WriteLine($"{"Name",-20} {"Profession",-12} {"Level",5} {"HP",5}  {"Equipment"}");
        Console.WriteLine(new string('-', 60));

        //Character rows 
        foreach (var character in characters)
        {
           string equipment = character.Equipment.Length > 0 ? string.Join(", ", character.Equipment) : "(none)";

            Console.WriteLine($"{character.Name,-20} {character.Profession,-12} {character.Level,5} {character.HP,5}  {equipment}");
        }
        Console.WriteLine(new string('-', 60));
        Console.WriteLine($"Total characters: {characters.Count}");
        Console.WriteLine();
    }

    public void FindCharacter()
    {
        // Done: Replace this stub with code that prompts for a character name,
        // searches for the character using LINQ, and displays their details.
        // If not found, notify the user.
        // Use string interpolation for formatting output.
        // Hint: You will need to parse the CSV and use LINQ's FirstOrDefault.
        AnsiConsole.Write(new Rule("[bold pink1]+ Find Character +[/]").RuleStyle("pink1"));
        Console.WriteLine();
        Console.Write("Enter character name to find: ");
        var name = Console.ReadLine();

        var characters = _reader.ReadAll();
        var found = _reader.FindByName(characters, name);

        if (found == null)
        {
            Console.WriteLine("Character not found");
            return;
        }

        string equipment = found.Equipment.Length >0 ? string.Join(", ", found.Equipment) : "(none)";

        //Displays resultes in aligned table format
        Console.WriteLine();
        Console.WriteLine(new string('-', 60));
        Console.WriteLine($"{"Name",-20} {"Profession",-12} {"Level",5} {"HP",5}  {"Equipment"}");
        Console.WriteLine(new string('-', 60));
        Console.WriteLine($"{found.Name,-20} {found.Profession,-12} {found.Level,5} {found.HP,5}  {equipment}");
        Console.WriteLine(new string('-', 60));
        Console.WriteLine();
    }

    public void LevelUpCharacter()
    {
        // Done: Replace this stub with code that prompts for a character name,
        // increases their level, updates the CSV file, and displays confirmation.
        // Use string interpolation for formatting output.
        // Hint: You will need to find the character, increment their level, and save changes.

        AnsiConsole.Write(new Rule("[bold pink1]+ Level Up +[/]").RuleStyle("pink1"));
        Console.WriteLine();
        //prompt for character name: 
        Console.Write("Enter character you'd like to level up: ");
        var name = Console.ReadLine() ?? string.Empty;

        var character = _reader.ReadAll();

        var target = _reader.FindByName(character, name);

        if (target == null)
        {
            Console.WriteLine("Character not found.");
            return;
        }

        target.Level += 1;
        _writer.WriteAll(character);

        Console.WriteLine($"{target.Name} is now level {target.Level}");


    }
    private string PromptRequired(string prompt)
    {
        string value;
        do
        {
            Console.Write(prompt);
            value = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(value))
                Console.WriteLine("  This field cannot be empty. Please try again.");
        }
        while (string.IsNullOrWhiteSpace(value));

        return value;
    }

    private int PromptInt(string prompt, int defaultValue)
    {
        Console.Write(prompt);
        var input = Console.ReadLine();
        if (int.TryParse(input, out int result) && result > 0)
            return result;
        Console.WriteLine($"  Invalid number, defaulting to {defaultValue}.");
        return defaultValue;
    }

    public void Run()
    {
        AnsiConsole.Write(new Rule("[bold pink1]+ Character Management +[/]").RuleStyle("pink1"));



        while (true)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Display Characters");
            Console.WriteLine("2. Find Character");
            Console.WriteLine("3. Add Character");
            Console.WriteLine("4. Level Up Character");
            Console.WriteLine("0. Exit");
            Console.Write("Enter your choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayCharacters();
                    break;
                case "2":
                    FindCharacter();
                    break;
                case "3":
                    AddCharacter();
                    break;
                case "4":
                    LevelUpCharacter();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}
