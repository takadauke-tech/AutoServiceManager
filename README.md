# AutoService Manager

Учебное WPF-приложение для методической работы № 5 «Разработка и интеграция модулей проекта (командная работа)».

## Технологии

* C#
* WPF / XAML
* .NET 10
* Git / GitHub
* JSON для локального хранения данных

## Участники

* Рыбьянов Даниил — интерфейс, навигация, модуль заказов, интеграция.
* Шигаев Ильяс — модуль клиентов и автомобилей, модели и хранение данных.

## Запуск

Требуется Windows с установленным .NET 10 SDK и компонентами Windows Desktop.

Из терминала VS Code:

```powershell
cd путь\\к\\AutoServiceManager
dotnet --info
dotnet restore
dotnet build
dotnet run
```

## Хранение данных

Приложение автоматически создаёт JSON-файлы в:
`%LOCALAPPDATA%\\AutoServiceManager`

Файлы: `clients.json`, `vehicles.json`, `orders.json`.

## Предлагаемая схема Git

```text
main
├── feature/ui
├── feature/clients
├── feature/orders
└── feature/database
```

## Важно для командной работы на одном ноутбуке

Перед переключением участника убедитесь, что изменения предыдущей задачи сохранены коммитом:

```powershell
git status
git add .
git commit -m "Краткое описание изменений"
```

Пример создания ветки:

```powershell
git switch main
git pull origin main
git switch -c feature/clients
```

После завершения модуля:

```powershell
git add .
git commit -m "Add clients and vehicles module"
git push -u origin feature/clients
```

## Проверочный сценарий

1. Добавить клиента.
2. Добавить автомобиль этому клиенту.
3. Создать заказ для автомобиля.
4. Выделить заказ и нажать «Завершить заказ».
5. Закрыть и повторно открыть программу — данные должны сохраниться.



## Демонстрационные данные

При первом запуске, если локальное хранилище ещё пустое, приложение автоматически создаёт 8 тестовых клиентов, 10 автомобилей и 10 заказов. Данные сохраняются в `%LOCALAPPDATA%\\AutoServiceManager` и не добавляются повторно при следующих запусках. Их можно изменять и удалять через интерфейс.

Contributor: espref

