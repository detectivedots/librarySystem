# Library System API
This repository contains the backend API for a comprehensive Library Management System, built with ASP.NET Core. It provides functionalities for user management, book and category organization, borrowing and returning books, and generating insightful reports.

## Table of Contents
*   [Features](#features)
*   [Architecture](#architecture)
*   [Technology Stack](#technology-stack)
*   [API Endpoints](#api-endpoints)
*   [Setup and Installation](#setup-and-installation)
*   [User Roles](#user-roles)

## Features

*   **Authentication & Authorization:** Secure JWT-based authentication with role-based access control. New users require librarian approval.
*   **User Management:** Register, log in, assign roles, and approve new users.
*   **Book Management:** Full CRUD (Create, Read, Update, Delete) operations for books.
*   **Category Management:** Organize books into categories with full CRUD support.
*   **Borrowing System:** Users can request to borrow books. Librarians can manage these requests (approve, decline, mark as returned).
*   **Search Functionality:** Search for books by title, author, or ISBN.
*   **Reporting:**
    *   Generate a system-wide report including total books, borrowed counts, most popular book, and most popular category.
    *   Generate detailed reports for individual books, showing borrowing history and availability.

## Architecture

The project follows a clean, layered architecture to ensure separation of concerns and maintainability.

*   **Presentation Layer (Controllers):** Handles incoming HTTP requests and responses using ASP.NET Core MVC.
*   **Service Layer (`Services`):** Contains the core business logic of the application.
*   **Infrastructure Layer (`Infrastructure`):** Implements the Repository and Unit of Work patterns for data access, abstracting the data source from the rest of the application.
*   **Data Layer (`Data`):** Defines the database context and models using Entity Framework Core.
*   **Models:** Defines the core domain entities (`Book`, `Category`, `BorrowRequest`) and Data Transfer Objects (DTOs).

## Technology Stack

*   **.NET 8**
*   **ASP.NET Core Web API**
*   **Entity Framework Core 8**
*   **SQL Server**
*   **ASP.NET Core Identity:** For user management and authentication.
*   **JWT (JSON Web Tokens):** For stateless, secure API authorization.
*   **Swagger (OpenAPI):** For API documentation and testing.

## API Endpoints

The API is configured with Swagger, providing an interactive way to explore and test the endpoints.

### Auth Controller (`/api/auth`)
| Method | Endpoint                        | Description                                                               | Access      |
| :----- | :------------------------------ | :------------------------------------------------------------------------ | :---------- |
| POST   | `/register-v2`                  | Registers a new user. The first user is automatically approved and made a Librarian. | Public      |
| POST   | `/login-v2`                     | Logs in a user and returns a JWT. User must be approved.                  | Public      |
| POST   | `/approve-user?username={name}` | Approves a registered user, allowing them to log in.                      | Librarian   |
| POST   | `/assign-role`                  | Assigns a role (`NORMALUSER` or `LIBRARIAN`) to a user.                 | Librarian   |

### Books Controller (`/api/books`)
| Method | Endpoint                    | Description                                         | Access      |
| :----- | :-------------------------- | :-------------------------------------------------- | :---------- |
| GET    | `/`                         | Gets a list of all books.                           | Authorized  |
| GET    | `/{id}`                     | Gets a single book by its ID.                       | Public      |
| POST   | `/`                         | Creates a new book.                                 | Librarian   |
| PUT    | `/{id}`                     | Updates an existing book.                           | Librarian   |
| DELETE | `/{id}`                     | Deletes a book.                                     | Librarian   |
| GET    | `/search?query={query}`     | Searches for books by title or author.              | Public      |
| GET    | `/borrowable-books`         | Gets books with available copies to borrow.         | Public      |
| GET    | `/find-by-isbn?isbn={isbn}` | Finds a book by its unique ISBN.                    | Public      |

### Categories Controller (`/api/categories`)
| Method | Endpoint       | Description                          | Access    |
| :----- | :------------- | :----------------------------------- | :-------- |
| GET    | `/`            | Gets a list of all categories.       | Public    |
| GET    | `/{id}`        | Gets a single category by its ID.    | Public    |
| POST   | `/`            | Creates a new category.              | Librarian |
| PUT    | `/{id}`        | Updates an existing category.        | Librarian |
| DELETE | `/{id}`        | Deletes a category.                  | Librarian |
| GET    | `/{id}/books`  | Gets all books within a category.    | Public    |

### Borrow Requests Controller (`/api/borrowrequests`)
| Method | Endpoint                                 | Description                                                     | Access      |
| :----- | :--------------------------------------- | :-------------------------------------------------------------- | :---------- |
| POST   | `/`                                      | Creates a new borrow request for a book.                        | Authorized  |
| GET    | `/`                                      | Gets all borrow requests in the system.                         | Librarian   |
| PUT    | `/{requestId}?status={status}`           | Updates a request's status (PENDING, BORROWED, RETURNED, DECLINED). | Librarian |
| GET    | `/borrowed-books/{userName}`             | Gets all books currently borrowed by a specific user.           | Public      |
| GET    | `/pending-requests/{userName}`           | Gets all pending borrow requests for a specific user.           | Public      |
| GET    | `/requests-log/{userName}`               | Gets the full borrowing history for a specific user.            | Public      |
| PUT    | `/return-book/{requestID}`               | Marks a borrowed book as returned.                              | Librarian   |

### Report Controller (`/api/report`)
| Method | Endpoint | Description                                                               | Access    |
| :----- | :------- | :------------------------------------------------------------------------ | :-------- |
| GET    | `/`      | Generates a general report for the entire library.                        | Librarian |
| GET    | `/{id}`  | Generates a detailed borrower and availability report for a specific book.| Librarian |

## Setup and Installation

1.  **Clone the repository:**
    ```sh
    git clone https://github.com/detectivedots/librarySystem.git
    cd librarySystem
    ```

2.  **Configure Database Connection:**
    Open `LibrarySystem/appsettings.json` and update the `DefaultConnection` string to point to your SQL Server instance.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Data Source=YOUR_SERVER_NAME;Initial Catalog=LibrarySystemDB;Integrated Security=True;Trust Server Certificate=True"
    },
    ```

3.  **Apply Migrations:**
    Open the solution in Visual Studio. In the Package Manager Console, ensure the default project is `LibrarySystem` and run the following command to create the database schema:
    ```sh
    Update-Database
    ```

4.  **Run the application:**
    Build and run the project from Visual Studio (or using `dotnet run`). The application will launch, and you can access the Swagger UI at `/swagger` to interact with the API.

## User Roles

The system defines two primary user roles with distinct permissions:

*   **NormalUser**:
    *   Can register and log in (after being approved by a librarian).
    *   Can view all books and categories.
    *   Can search for books.
    *   Can create borrow requests for books.
    *   Can view their own borrowing history.

*   **Librarian**:
    *   Inherits all permissions of a `NormalUser`.
    *   Can approve new user registrations.
    *   Can assign roles to users.
    *   Has full CRUD access to books and categories.
    *   Can manage all borrow requests (approve, decline, mark as returned).
    *   Can generate system-wide and book-specific reports.

**Note:** The first user to register in the system is automatically approved and assigned the **Librarian** role.
