using Microsoft.EntityFrameworkCore;
using W9_assignment_template.Data;
using W9_assignment_template.Models;

namespace W9_assignment_template.Services;

public class GameEngine
{
    private readonly GameContext _context;

    public GameEngine(GameContext context)
    {
        _context = context;
    }

    public void DisplayRooms()
    {
        var rooms = _context.Rooms.Include(r => r.Characters).ToList();

        foreach (var room in rooms)
        {
            Console.WriteLine($"Room: {room.Name} - {room.Description}");
            foreach (var character in room.Characters)
            {
                Console.WriteLine($"    Character: {character.Name}, Level: {character.Level}");
            }
        }
    }

    public void DisplayCharacters()
    {
        var characters = _context.Characters.ToList();
        if (characters.Any())
        {
            Console.WriteLine("\nCharacters:");
            foreach (var character in characters)
            {
                Console.WriteLine($"Character ID: {character.Id}, Name: {character.Name}, Level: {character.Level}, Room ID: {character.RoomId}");
            }
        }
        else
        {
            Console.WriteLine("No characters available.");
        }
    }

    public void AddRoom()
    {
        Console.Write("Enter room name: ");
        var name = Console.ReadLine();

        Console.Write("Enter room description: ");
        var description = Console.ReadLine();

        var room = new Room
        {
            Name = name,
            Description = description
        };

        _context.Rooms.Add(room);
        _context.SaveChanges();

        Console.WriteLine($"Room '{name}' added to the game.");
    }

    public void AddCharacter()
    {
        Console.Write("Enter character Name: ");
        var name = Console.ReadLine();

        Room foundRoom = null;
                
        while (foundRoom == null)
        {
            Console.Write("Enter room ID for the character: ");
            int roomId = int.Parse(Console.ReadLine());
            foundRoom = _context.Rooms.Where(r => r.Id == roomId).FirstOrDefault();

            if (foundRoom == null)
            {
                Console.WriteLine("That room doesn't exist. Try Again.");
            }
            else
            {
                Console.WriteLine($"Room {foundRoom.Name} is Found");
                var character = new Character
                {
                    Name = name,
                    Level = 1,
                    RoomId = roomId
                };

                _context.Characters.Add(character);
                _context.SaveChanges();

                Console.WriteLine($"{name} added to the game in the {foundRoom.Name}");
            }
        }
    }
    
    public void FindCharacter()
    {
        Console.Write("Enter character name to search: ");
        var name = Console.ReadLine();

        Character foundCharacter = _context.Characters.Include(c => c.Room).Where(c => c.Name == name).FirstOrDefault();

        if (foundCharacter != null)
        {
            Console.WriteLine($"Name: {foundCharacter.Name}\nLevel: {foundCharacter.Level}\nRoom: {foundCharacter.Room.Name}\n");
        }
        else
        {
            Console.WriteLine("No Character Found");
        }
    }
    public void ChangeCharacterLevel()
    {
        Console.Write("Enter character name to search: ");
        var name = Console.ReadLine();

        Character foundCharacter = _context.Characters.Where(c => c.Name == name).FirstOrDefault();

        if (foundCharacter != null)
        {
            Console.Write($"{foundCharacter.Name} is at level {foundCharacter.Level}. Do you want to change this character's level? (Y/N) --- ");
            char levelChangeAnswer = Console.ReadLine().ToUpper().FirstOrDefault();

            if (levelChangeAnswer == 'Y')
            {
                Console.Write("Do you want to level up or level down? (U/D) --- ");
                char levelUpDownAnswer = Console.ReadLine().ToUpper().FirstOrDefault();

                if (levelUpDownAnswer == 'U')
                {
                    foundCharacter.Level += 1;
                }
                if (levelUpDownAnswer == 'D')
                {
                    if (foundCharacter.Level == 1)
                    {
                        Console.WriteLine("You can't level down.");
                    }
                    else
                    {
                        foundCharacter.Level -= 1;
                    }
                }
                _context.Update(foundCharacter);
                _context.SaveChanges();
                Console.WriteLine($"{foundCharacter.Name} is now at level {foundCharacter.Level}");
            }
        }
        else
        {
            Console.WriteLine("No Character Found");
        }
    }
}