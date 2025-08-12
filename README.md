# Bank Account


## Описание проекта
**Bank Account**
Сервис реализует REST API для управления банковскими счетами и транзакциями.

## 🏗 **Архитектура проекта**

1. **Domain**  
2. **BusinessLogic**  
3. **DataAccess**  
4. **API**  

---

## 🛠 **Используемые технологии**

- **.NET 9**
- **MediatR**
- **FluentValidation**
- **Swagger (Swashbuckle)**

## Полный запуск

Для запуска всей системы перейдите в папку с `docker-compose.yml` и выполните команду:

```bash
docker-compose up --build
```

- Backend (API + Swagger) будет доступен по адресу: http://localhost:8080/swagger/index.html
- Hangfire будет доступен по адресу: http://localhost:8080/hangfire
- 