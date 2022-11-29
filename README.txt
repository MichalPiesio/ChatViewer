I ran this application in a Windows environment, but this is an ASP.NET Core application, so it should be able to run on Mac or Linux with slight changes to the launch settings, I haven't been able to test it though. 
I recommend running this project from an IDE, I tested it on both VS2022 and Rider.
Just click "Run" and the launch settings should take care of everything.
The IDE might ask you about the SSL certificate for the hot reload, you can safely click no, it's not going to be needed.
SPA proxy takes a minute to launch so just wait for the redirect.

Initial build may take a while due to the npm install for the frontend.
I used Sqlite for persistence, the local file for the db will get created and migrations will run automatically on startup.

Once it opens up on "Home" you get four other tabs up top, please first seed the database in "Seed data", each request adds 5 new people and 20 chat events.
The other three tabs are for showing the chat logs - the first for minute by minute, then aggregates by hour and day.

There are some unit tests in the ChatViewer.UnitTests project that you can run.
