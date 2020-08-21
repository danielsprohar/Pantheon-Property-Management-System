# Pantheon Property Management System
A simple property management system for a small RV Park.

**This is a work in progress**

# Application Architecture
I am utilizing the [Onion Architecture](https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/)
pattern to build this small and humble piece of software.
 

## Architecture Layers
### Core
The Core layer is where the business domain models reside, such as ParkingSpaces and Customers.


### Infrastructure
This is the data access layer. I use [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
as my choice of ORM and Microsoft's SQL Server for data persistence.

### Identity
I am using (and learning) [Identity Server 4](https://identityserver4.readthedocs.io/en/latest/index.html)
for my Security Token Service to protect my web api.

The Identity Server runs on port 6001.

https://localhost:6001

### Services
This is where my web api resides and exposes the endpoints to serve requests for 
resources.

The Hermes web api runs on port 5001.

See the [Swagger doc](https://localhost:5001/swagger) for more info. 

### Web
This is where my web app resides, which presides the UI for user interaction.
The web app calls the Hermes web api to perform CRUD operations.
 
The Vulcan web app runs on port 5001.

https://localhost:5002

