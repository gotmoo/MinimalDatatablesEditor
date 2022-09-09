# MinimalDatatablesEditor
Datatables.net Editor sample project implemented with DotNet 7 minimal API.

This project takes the official sample code from https://editor.datatables.net/download/index and changes it from an MVC setup to Minimal API instead.

To use this example: 
1. Clone/download the project.
2. Create a database for the project and load the example SQL scripts from https://editor.datatables.net/manual/net/core#Examples-SQL.
3. Modify the launchSettings.json file in the Properties directory to your database configuration. Supported databases are listed in the [Editor Manual](https://editor.datatables.net/manual/net/core#Database-connection). The current settings are for a LocalDB on Windows.

Note: The interface that handles registering all the endpoints is using a version 11 language feature. Make sure this is configured for your project.

Big shout out to Nick Chapsas ([@ElfoCrash](https://github.com/Elfocrash/elfocrash)) for his ``IEndpoints`` implementation. Seriously, check out his [YouTube channel](https://www.youtube.com/c/Elfocrash)! 