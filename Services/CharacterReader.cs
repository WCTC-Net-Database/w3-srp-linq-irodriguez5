using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using W03.Models;

namespace W03.Services;

/// <summary>
/// CharacterReader - Responsible for ONE thing: READING character data.
///
/// This class demonstrates the Single Responsibility Principle (SRP):
/// - It ONLY reads character data from files
/// - It does NOT write data (that's CharacterWriter's job)
/// - It does NOT handle the menu or user interaction
///
/// By separating reading from writing, we can:
/// 1. Test reading independently
/// 2. Change how we read without affecting how we write
/// 3. Reuse this class anywhere we need to read characters
/// </summary>
public class CharacterReader
{

    private readonly string _filePath;

    /// <summary>
    /// Creates a new CharacterReader for the specified file.
    /// </summary>
    /// <param name="filePath">Path to the CSV file containing character data</param>
    /// 

    public CharacterReader(string filePath)
    {
        _filePath = filePath;
    }

    /// <summary>
    /// Reads all characters from the CSV file.
    ///
    /// Done: Implement this method to:
    /// 1. Read all lines from the file
    /// 2. Parse each line into a Character object
    /// 3. Return the list of characters
    ///
    /// HINT: Remember that names may contain commas (e.g., "John, Brave")
    /// You'll need to handle quoted fields from Week 2!
    /// </summary>
    /// <returns>A list of all characters in the file</returns>
    public List<Character> ReadAll()
    {
        var characters = new List<Character>();

        // Done: Read all lines from file
         string[] lines = File.ReadAllLines(_filePath);

        // Done: Skip header if present, then parse each line
         foreach (string line in lines)
         {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue; // Skip empty lines
            }
            var trimmedLine = line.Trim();
            //skips header row if present
            if (trimmedLine.StartsWith("Name,", StringComparison.OrdinalIgnoreCase))
                continue;
           
            var character = ParseLine(line);
            if (character != null)
            {
                characters.Add(character);
            }
        }

        return characters;
    }

    /// <summary>
    /// Finds a character by name using LINQ.
    ///
    /// This is where you'll use LINQ's FirstOrDefault method!
    ///
    /// LINQ Example:
    /// var result = characters.FirstOrDefault(c => c.Name == name);
    ///
    /// FirstOrDefault returns:
    /// - The first character that matches the condition, OR
    /// - null if no character matches
    /// </summary>
    /// <param name="characters">The list of characters to search</param>
    /// <param name="name">The name to search for</param>
    /// <returns>The matching character, or null if not found</returns>
    public Character FindByName(List<Character> characters, string name)
    {
        // Done: Use LINQ to find the character
        //return characters.FirstOrDefault(c => c.Name == name);
  

        // For case-insensitive search, you could use:
        //return characters.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        return characters.FirstOrDefault(c => c.Name.Equals(name,StringComparison.OrdinalIgnoreCase))
            ?? characters.FirstOrDefault (c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));


        //return result; // Replace with LINQ query
    }

    /// <summary>
    /// Finds all characters matching a condition using LINQ.
    ///
    /// BONUS: This shows LINQ's Where method for filtering.
    ///
    /// LINQ Example:
    /// var warriors = characters.Where(c => c.Profession == "Fighter").ToList();
    /// </summary>
    /// <param name="characters">The list of characters to search</param>
    /// <param name="profession">The profession to filter by</param>
    /// <returns>All characters of the specified profession</returns>
    public List<Character> FindByProfession(List<Character> characters, string profession)
    {
        // Done: Use LINQ Where to filter characters
         return characters.Where(c => c.Profession == profession).ToList();

        //return new List<Character>(); // Replace with LINQ query
    }

    /// <summary>
    /// Helper method to parse a single CSV line into a Character object.
    ///
    /// HINT: This is similar to your Week 2 parsing, but now you're
    /// creating a Character object instead of just printing values.
    /// </summary>
    private Character ParseLine(string line)
    {
        // Done: Parse the line and create a Character object
        // Remember to handle:
        // - Quoted names with commas: "John, Brave",Fighter,1,10,sword|shield
        // - Equipment split by | (pipe): sword|shield|potion
        string name = "";
        string profession = "";
        string level = "";
        string health = "";
        string equipment = "";

        //parse name
        if (line.StartsWith("\"")){
            int closingQuote = line.IndexOf("\"", 1);
            name = line.Substring(1, closingQuote - 1);

            var restOfLine = line.Substring(closingQuote + 2); // Skip comma after closing quote

            var lines = restOfLine.Split(',');
            profession = lines[0];
            level = lines[1];
            health = lines[2];
            equipment = lines[3];
        }
        else
        {
            // No quotes, simple split
            var lines = line.Split(',');
            name = lines[0]; 
            profession = lines[1];
            level = lines[2];
            health = lines[3];
            equipment = lines[4];
        }

        //Build and return character object, splitting equipment on | into stirng array
        return new Character
        (
            name,
            profession,
            int.Parse(level),
            int.Parse(health),
            equipment.Split('|')
        );
    }
}
