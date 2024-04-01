# Car Rental Application - Ferchau

## Description
This application is used for managing Customers, Cars and Rentals.
You are able to add, delete and update customers.
You are able to add, delete and update cars.
You are able to create a new Rental which is consisting of a customer and a car, 
which can easily be selected by a drop down menu.
Its ensured that a customer is only able to rent a single car 
and a car is only able to be rented by a single customer.
You are able to inspect all current Rentals at any time, 
while also being able to search for a specific customer for optimised workflow.


## Installation
Clone the repository on Github:
https://github.com/BennoJaegertPr/CarRentalApp.git

Ensure that you have the .NET Core SDK installed on your system.

Navigate to the project Folder and run following command prompts:
* dotnet restore
* dotnet build

Make sure to edit the appsettings.json file to your liking.
**Suggestions** for the appsettings.json file config:
* Make sure to enable the inmemoryDB -> "UseInMemoryDatabase": true,
* Make sure to configure the logging file output folder -> "path": "YOUR FILE PATH"
    * Default logging Path: Logs/logfile_.log 

## Technology

### Frontend Razor Views
Razor Views are used to create the Frontend of the application.
It does not represent a state of the art single page application, 
but they are directly rendered at the server and sent to the client as complete pages
which is faster on initial page load.

### Backend ASP.Net Core
I used a ASP.Net Core MVC approach to create my backend.
It is a reliable and high performance framework to build Applications.
ASP.NET Core is designed for scalability, enabling you to build applications that can handle high traffic and scale easily as your user base grows.

### Entity Framework core
Entity Framework core is used to communicate with my MSSQL Database.
It offers a object relational mapping.
High performance framework due to query caching.
Works seamlessly with ASP.NET Core.
It is offering a perfect Code first approach which was also used for this application.

### MSSQL
MSSQL was used as the Database engine.
It offers a perfect solution to be integrated into my Microsoft based application.
It is scaleable and is offering a robust and consistent Database solution 
making it suitable for handling large volumes of data.