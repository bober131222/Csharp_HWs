using System.Text.Json;

namespace Task4_5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Задача 4: создаём персонажа и сериализуем его в JSON с отступами
            GameCharacter hero = new GameCharacter
            {
                Name = "Арагорн",
                Level = 12,
                Health = 87.5,
                Inventory = new string[] { "Меч", "Щит", "Зелье здоровья" }
            };

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                // Чтобы кириллица в файле и на экране не превращалась в А...
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string json = JsonSerializer.Serialize(hero, options);
            Console.WriteLine("=== JSON персонажа ===");
            Console.WriteLine(json);

            // Десериализуем строку обратно и проверяем, что данные восстановились
            GameCharacter copy = JsonSerializer.Deserialize<GameCharacter>(json);
            Console.WriteLine();
            Console.WriteLine($"Восстановлен из строки: {copy.Name}, предметов в инвентаре: {copy.Inventory.Length}");

            // Задача 5: сохраняем JSON в файл save.json рядом с программой
            string path = Path.Combine(AppContext.BaseDirectory, "save.json");
            File.WriteAllText(path, json);
            Console.WriteLine($"Сохранено в файл: {path}");

            // Читаем файл обратно и десериализуем в новый объект
            string jsonFromFile = File.ReadAllText(path);
            GameCharacter loaded = JsonSerializer.Deserialize<GameCharacter>(jsonFromFile);

            Console.WriteLine();
            Console.WriteLine("=== Загруженный персонаж ===");
            Console.WriteLine($"Имя: {loaded.Name}");
            Console.WriteLine($"Уровень: {loaded.Level}");
            Console.WriteLine($"Здоровье: {loaded.Health}");
            Console.WriteLine($"Инвентарь: {string.Join(", ", loaded.Inventory)}");
        }
    }
}
