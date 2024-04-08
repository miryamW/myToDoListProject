# MyToDoListProject

Welcome to MyToDoListProject! This is a .NET Core Web API system designed to help users manage their tasks efficiently. Users can register, log in, and receive JWT tokens for authentication. Once logged in, users can view and manage their tasks. Additionally, users with manager privileges can oversee other users, adding and managing them as needed.

## Getting Started

To get started with this project, follow these steps:

### Prerequisites

Make sure you have the following installed on your system:

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/) (optional, but recommended)

### Installation

1. Clone this repository to your local machine:

```bash
git clone https://github.com/miryamW/myToDoListProject.git
```

The API should now be running locally and accessible at https://localhost:5001.

## Usage
### Authentication
To use the API endpoints, you need to authenticate with JWT tokens. Use the /api/auth/login endpoint to obtain a token by providing your credentials.

```bash
POST /Login
Content-Type: application/json

{
  "username": "your_username",
  "password": "your_password"
}
```
The response will contain an access token that you can use to access protected endpoints.

## Endpoints
GET /TasksList: Retrieve all tasks for the authenticated user.

POST /TasksList: Create a new task for the authenticated user.

PUT /TasksList/{id}: Update an existing task for the authenticated user.

DELETE /TasksList/{id}: Delete a task for the authenticated user.

### For manage users:

GET /Users: Retrieve all users.

GET /Users/{id}: Retrieve a specific user.

POST /Users: Create a new user.

PUT /Users/{id}: Update an existing user.

DELETE /Users/{id}: Delete a user.



### Contributing

Contributions to this project are welcome! To contribute:


1.Fork the repository.

2.Create a new branch (git checkout -b feature)

3.Make your changes.

4.Commit your changes (git commit -am 'Add new feature')

5.Push to the branch (git push origin feature)

6.Create a new Pull Request.

### License
This project is licensed under the MIT License. See the LICENSE file for details.

### Acknowledgements

ASP.NET Core

---

Feel free to customize this README to include any additional information specific to your project. If you have any questions or need further assistance, don't hesitate to ask!

