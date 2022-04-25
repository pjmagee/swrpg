# Database Setup


From the database folder with the Orleans scripts

```
docker run -p 5432:5432 -v C:\Projects\SWRPG\Database:/docker-entrypoint-initdb.d/ --name swrpg -e POSTGRES_PASSWORD=password -d postgres
```