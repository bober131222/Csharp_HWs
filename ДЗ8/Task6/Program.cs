using System.Text.Json;

namespace Task6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "settings.json");

            if (File.Exists(path))
            {
                // Файл есть — читаем и показываем сохранённые настройки
                string json = File.ReadAllText(path);
                GameSettings settings = JsonSerializer.Deserialize<GameSettings>(json);
                Console.WriteLine("Загружены сохранённые настройки:");
                PrintSettings(settings);
            }
            else
            {
                // Файла нет — спрашиваем пользователя и сохраняем
                Console.WriteLine("Файл настроек не найден. Создаём новые настройки.");

                GameSettings settings = new GameSettings();
                settings.PlayerName = ReadName();
                settings.Difficulty = ReadIntInRange("Сложность (1 — легко, 2 — нормально, 3 — сложно): ", 1, 3);
                settings.MusicVolume = ReadIntInRange("Громкость музыки (0–100): ", 0, 100);

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                string json = JsonSerializer.Serialize(settings, options);
                File.WriteAllText(path, json);

                Console.WriteLine($"Настройки сохранены в файл: {path}");
                PrintSettings(settings);
            }
        }

        static void PrintSettings(GameSettings settings)
        {
            Console.WriteLine($"Имя игрока: {settings.PlayerName}");
            Console.WriteLine($"Сложность: {settings.Difficulty} ({DifficultyName(settings.Difficulty)})");
            Console.WriteLine($"Громкость музыки: {settings.MusicVolume}");
        }

        static string DifficultyName(int difficulty)
        {
            switch (difficulty)
            {
                case 1: return "легко";
                case 2: return "нормально";
                case 3: return "сложно";
                default: return "неизвестно";
            }
        }

        // Просим имя, пока не введут непустую строку
        static string ReadName()
        {
            while (true)
            {
                Console.Write("Имя игрока: ");
                string input = Console.ReadLine();
                if (input == null)
                {
                    // Ввод закончился (например, Ctrl+D) — дальше спрашивать бессмысленно
                    Environment.Exit(1);
                }
                input = input.Trim();
                if (input.Length > 0)
                {
                    return input;
                }
                Console.WriteLine("Имя не может быть пустым.");
            }
        }

        // Просим целое число в диапазоне [min, max], пока ввод не станет корректным
        static int ReadIntInRange(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (input == null)
                {
                    Environment.Exit(1);
                }
                if (int.TryParse(input, out int value) && value >= min && value <= max)
                {
                    return value;
                }
                Console.WriteLine($"Введите целое число от {min} до {max}.");
            }
        }
    }
}
