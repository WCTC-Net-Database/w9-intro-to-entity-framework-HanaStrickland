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

        int roomId = 0;
        string roomNameFound = string.Empty;
                
        while (roomId == 0)
        {
            Console.Write("Enter room ID for the character: ");
            roomId = int.Parse(Console.ReadLine());

            bool roomFound = false;

            var rooms = _context.Rooms.ToList();
            foreach (var room in rooms)
            {
                if (roomId == room.Id)
                {
                    roomFound = true;
                    roomNameFound = room.Name;
                    break;
                }
            }

            if (roomFound)
            {
                Console.WriteLine($"Room {roomNameFound} is Found");
                var character = new Character
                {
                    Name = name,
                    Level = 1,
                    RoomId = roomId
                };

                _context.Characters.Add(character);
                _context.SaveChanges();

                Console.WriteLine($"{name} added to the game in the {roomNameFound}");
            }
            else
            {
                roomId = 0;
                Console.WriteLine("Room was not Found");
            }
        }
    }
    public void FindCharacter()
    {
        Console.Write("Enter character name to search: ");
        var name = Console.ReadLine();

        var characters = _context.Characters.Include(c => c.Room).ToList();

        bool nameFound = false;
        foreach (var character in characters)
        {
            if (name == character.Name)
            {
                nameFound = true;

                Console.WriteLine($"Name: {character.Name}\nLevel: {character.Level}\nRoom: {character.Room.Name}\n");
            }
        }

        if (nameFound == false)
        {
            Console.WriteLine("No Character Found\n");
        }
    }
}