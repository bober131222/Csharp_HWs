# ДЗ 8 — Часть 1: задачи на понимание

Все ответы проверены на практике (System.Text.Json, .NET 8+).

## Задача 1. Анализ JSON-сериализации

Результат: `{"Name":"Alex","Score":100}`

**а)**
- `Name` — **попадёт**: публичное свойство с `get`.
- `Score` — **попадёт**: публичное свойство с `get`.
- `secretCode` — **не попадёт**: это приватное поле, а `JsonSerializer` по умолчанию сериализует только **публичные свойства**.
- `IsAlive` — **не попадёт**: оно публичное, но это **поле**, а не свойство (нет `{ get; set; }`). Поля по умолчанию игнорируются.

**б)** Пометить поле атрибутом `[JsonInclude]` (`using System.Text.Json.Serialization;`):

```csharp
[JsonInclude]
private int secretCode = 42;
```

Начиная с .NET 8 `[JsonInclude]` работает и для непубличных членов. Поле остаётся `private`, но попадает в JSON: `"secretCode":42`.

**в)** `IsAlive` не попадает, потому что это поле, а сериализатор по умолчанию смотрит только на свойства. Два способа:
1. Превратить поле в свойство: `public bool IsAlive { get; set; }`.
2. Оставить полем, но включить поля:
   - атрибутом `[JsonInclude] public bool IsAlive;`, или
   - настройкой `new JsonSerializerOptions { IncludeFields = true }`, переданной в `Serialize` (включает все **публичные** поля; приватный `secretCode` всё равно не попадёт).

## Задача 2. Восстановление объекта из JSON

**а)**

```csharp
public class Game
{
    public string Title { get; set; }
    public int Year { get; set; }
    public double Rating { get; set; }
    public List<string> Genres { get; set; }   // подойдёт и string[]
    public bool IsMultiplayer { get; set; }
}
```

Нужны публичные свойства с `get; set;`, имена совпадают с ключами JSON, типы соответствуют значениям: `2011` → `int`, `9.5` → `double`, массив строк → `List<string>`/`string[]`, `true` → `bool`.

**б)** По умолчанию `System.Text.Json` сравнивает имена ключей и свойств **с учётом регистра**. Ключ `title` не совпадает со свойством `Title`, поэтому он пропускается, и `Title` остаётся `null`. Решение — включить регистронезависимое сопоставление:

```csharp
var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
Game game = JsonSerializer.Deserialize<Game>(json, options);
```

(Можно также `PropertyNamingPolicy = JsonNamingPolicy.CamelCase`, если все ключи в JSON записаны в camelCase.)

**в)** Да, `[1, "hello", true, null]` — **валидный JSON**: массив может содержать значения разных типов, `null` — допустимое значение. Так как типы элементов разные, десериализовать его можно в `object[]` / `List<object>` (каждый непустой элемент станет `JsonElement`, а `null` — `null`) или целиком в `JsonElement` / `JsonNode`. В `int[]` или `string[]` — нельзя.

## Задача 3. File vs FileStream

1. **File** — `File.ReadAllText` / `File.ReadAllLines` читает маленький файл целиком одной строкой кода; потоковое чтение тут не нужно.
2. **File** — прогресс сериализуется в JSON-строку и перезаписывается целиком через `File.WriteAllText`; файл маленький, а целиком перезаписать проще всего.
3. **FileStream** (с `FileMode.Append`, обычно через `StreamWriter`) — поток держится открытым и дописывает строки в конец, не загружая гигабайтный файл в память и не открывая его заново на каждую строку.
4. **File** — `File.Exists` проверяет наличие файла, `File.ReadAllText` читает его целиком в строку.
5. **FileStream** — только поток позволяет перейти на нужное место (`Seek` / `Position`) и записать 10 байт (`Write`), не переписывая остальной файл. `File` умеет работать только с файлом целиком.
