# AMI.Persistence.EntityFramework.SQLite

Existing databases in the AMI.API project may need to be deleted beforehand e.g. AmiSqliteDatabase.db
Run the following command in Package Manager Console with the SQLite project selected and AMI.API as startup project:

```
PM> Add-Migration Initial
```

This command scaffold a migration to create the initial set of tables for your model. 
When it is executed successfully, then run the following command.

```
PM> Update-Database
```

Source: https://entityframeworkcore.com/providers-sqlite

If the model changes it is not planned to run an additional migration but instead recreate it from scratch.