# ДЗ 8 — Сериализация. Потоки. Чтение-запись

Шмачилин Данила Алексеевич, БРИЦП 263.

| Задачи | Где |
|---|---|
| 1–3 (теория) | [answers.md](answers.md) |
| 4–5 (персонаж, save.json) | [Task4_5](Task4_5) |
| 6 (настройки, settings.json) | [Task6](Task6) |

## Запуск

Из папки `ДЗ8`:

```bash
dotnet run --project Task4_5
dotnet run --project Task6
```

Проекты рассчитаны на .NET 8 (`RollForward=Major` позволяет запустить их и на более новом .NET).
Файлы `save.json` и `settings.json` создаются рядом с программой (`AppContext.BaseDirectory`, т.е. в `bin/Debug/net8.0/`).
Чтобы снова увидеть первый запуск задачи 6, удалите `Task6/bin/Debug/net8.0/settings.json`.
