namespace W03.Models;

/// <summary>
/// Represents an RPG character with their attributes and equipment.
///
/// This class demonstrates a proper DATA CLASS - it only holds data,
/// it doesn't read files, write files, or contain business logic.
/// That's the Single Responsibility Principle in action!
///
/// Compare this to CharacterManager which does EVERYTHING - that's what
/// you'll be refactoring in this assignment.
/// </summary>
public class Character
{
    /// <summary>
    /// The character's name (e.g., "John, Brave" - note: may contain commas!)
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The character's profession (Fighter, Wizard, Rogue, etc.)
    /// </summary>
    public string Profession { get; set; }

    /// <summary>
    /// The character's current level (starts at 1)
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// Hit Points - the character's health
    /// </summary>
    public int HP { get; set; }

    /// <summary>
    /// Array of equipment items the character carries
    /// In the CSV, these are separated by | (pipe character)
    /// Example: sword|shield|potion becomes ["sword", "shield", "potion"]
    /// </summary>
    public string[] Equipment { get; set; }

    /// <summary>
    /// Default constructor - creates an empty character
    /// </summary>
    public Character()
    {
        Equipment = Array.Empty<string>();
    }

    /// <summary>
    /// Convenience constructor to create a fully-initialized character
    /// </summary>
    public Character(string name, string profession, int level, int hp, string[] equipment)
    {
        Name = name;
        Profession = profession;
        Level = level;
        HP = hp;
        Equipment = equipment ?? Array.Empty<string>();
    }


    /// <summary>
    /// Returns a formatted string representation of the character.
    /// Useful for debugging and display.
    /// </summary>
    public override string ToString()
    {
        string equipmentList = Equipment.Length > 0
            ? string.Join(", ", Equipment)
            : "none";
        return $"{Name} the {Profession} (Level {Level}, {HP} HP) - Equipment: {equipmentList}";
    }
}
