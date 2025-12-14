### 1. Підготовка
Тобі потрібен **Docker Desktop** та **.NET SDK 9**.

### 2. Запуск Бази Даних
У корені проєкту є файл `docker-compose.yml`. Просто відкрий термінал у папці проєкту і напиши:

```bash
docker-compose up -d
Це підніме PostgreSQL (порт 5432, юзер admin, пароль password123).
3. Запуск Сервера
Тобі НЕ треба вручну створювати базу чи таблиці. Програма все зробить сама при старті.
Запусти API через термінал (або кнопку Run в IDE):
code
Bash
dotnet run --project Reservreit.API
Після запуску відкривай документацію:
👉 http://localhost:5000/swagger
🔑 Тестові акаунти (Вже створені)
Можеш використовувати їх для логіну, щоб отримати токен:
Роль	Email	Пароль
Admin	admin@reserveit.com	Admin123$
Owner (Barber)	barber@demo.com	Owner123$
Owner (Spa)	spa@demo.com	Owner123$
Staff (Dmytro)	dmytro@staff.com	Staff123$
Client	client@demo.com	Client123$
⚙️ Конфігурація (Якщо треба змінити БД)
Налаштування підключення лежать у Reservreit.API/appsettings.json.
code
JSON
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=reserveit_db;Username=admin;Password=password123"
}
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=reserveit_db;Username=admin;Password=password123"
}
