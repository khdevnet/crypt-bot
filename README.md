# Telegram bot

```
appsettings.json
{
  "bot": {
    "token": ""
  },
  "database": {
    "connectionString": "mongodb://admin:passs@localhost:27017"
  }
}
```

```
local testing 
docker run -d -e MONGO_INITDB_ROOT_USERNAME=admin -e MONGO_INITDB_ROOT_PASSWORD=ant123 -p 27017:27017 -v c:/mongodb:/data/db mongo:3.6

hosting
docker-compose build
docker-compose up

```