
using W03.Models;

namespace W03.Services;

/// <summary>
/// CharacterWriter - Responsible for ONE thing: WRITING character data.
///
/// This class demonstrates the Single Responsibility Principle (SRP):
/// - It ONLY writes character data to files
/// - It does NOT read data (that's CharacterReader's job)
/// - It does NOT handle the menu or user interaction
///
/// Why separate reading and writing?
/// - Different operations might need different file formats
/// - Writing might need validation that reading doesn't
/// - We can add features (like backup) without changing reading code
/// </summary>
public class CharacterWriter
{
    private readonly string _filePath;

    /// <summary>
    /// Creates a new CharacterWriter for the specified file.
    /// </summary>
    /// <param name="filePath">Path to the CSV file to write to</param>
    public CharacterWriter(string filePath)
    {
        _filePath = filePath;
    }

    /// <summary>
    /// Writes all characters to the CSV file.
    ///
    /// This REPLACES the entire file with the new data.
    /// Use this when you've modified characters (like leveling up).
    ///
    /// TODO: Implement this method to:
    /// 1. Convert each character to a CSV line
    /// 2. Write all lines to the file
    ///
    /// HINT: Use File.WriteAllLines() to write all at once
    /// </summary>
    /// <param name="characters">The list of characters to save</param>
    public void WriteAll(List<Character> characters)
    {
        // TODO: Convert characters to CSV lines and write to file
         var lines = new List<string>();
         foreach (var character in characters)
        {
           lines.Add(FormatCharacter(character));
         }
         File.WriteAllLines(_filePath, lines);
    }

    /// <summary>
    /// Appends a single character to the end of the CSV file.
    ///
    /// Use this when adding a new character without rewriting everything.
    ///
    /// Done: Implement this method to:
    /// 1. Format the character as a CSV line
    /// 2. Append to the file (not overwrite!)
    ///
    /// HINT: Use File.AppendAllText() with a newline
    /// </summary>
    /// <param name="character">The character to add</param>
    public void AppendCharacter(Character character)
    {
        // Done: Format and append the character
         string line = FormatCharacter(character);
         File.AppendAllText(_filePath, line + Environment.NewLine);
    }

    /// <summary>
    /// Helper method to format a Character object as a CSV line.
    ///
    /// HINT: Remember to:
    /// - Quote names that contain commas: "John, Brave"
    /// - Join equipment with | (pipe): sword|shield|potion
    /// </summary>
    private string FormatCharacter(Character character)
    {
        // Done: Format the character as a CSV line
        // Consider:
        // - Should the name be quoted? (if it contains a comma)
        // - Join equipment array with | separator

        // Example (without quote handling):
        string equipment = string.Join("|", character.Equipment);
        string name = character.Name.Contains(",") ? $"\"{character.Name}\"" : character.Name;
        return $"{name},{character.Profession},{character.Level},{character.HP},{equipment}";

        //return string.Empty;
    }
}
