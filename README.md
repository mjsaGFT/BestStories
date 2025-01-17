# BestStories

To run the application simply open a console and type "dotnet run".

Use Postman or any Browser to make a request.

Available endpoint: http://localhost:5261/BestStories/10

Where 10 is the number of stories you wish to get.

To avoid overloading of Hacker News API response cached is configured for 30 seconds.

To speed up the execution I used parallel foreach loop which sends n requests. This way we benefit from multithreading.

to separate API logic from business logic I used Mediator pattern implemented in MediatR nuget.
Hacker News API endpoints are configurable via appsettings.json and retrieved in the application using Options pattern.

If there is an error during request of details, executing is not stopped. Instead warning is logged and application tries to fetch all remaining ids.

For simple mappings I used automapper.

Ids can grow infinitely so paging of some sort is also something worth to consider.
