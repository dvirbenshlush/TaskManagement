# Task Management API

This is a backend project built with **.NET 8**, **MySQL (Aurora-compatible)**, and **AWS Cognito** for secure user authentication and authorization.

## 🚀 Features

- User registration and login (via AWS Cognito)
- JWT-based authentication and authorization
- Role-based access control (admin / user)
- CRUD for Projects and Tasks
- Pagination support
- Centralized error handling and logging (AWS CloudWatch)
- Swagger for API documentation and testing

---

## 🛠️ Getting Started

### 1. Clone the repository

```
git clone https://github.com/yourusername/TaskManagementApi.git
cd TaskManagementApi
```

### 2. Configuration

Update your `appsettings.json` with:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "<your-mysql-connection>"
  },
  "TaskMangementSettings": {
    "Jwt": {
      "Authority": "<your-cognito-authority-url>",
      "ValidIssuer": "<your-cognito-authority-url>",
      "ValidateAudience": false
    },
    "Cognito": {
      "ClientId": "<your-client-id>"
    }
  },
  "AWS": {
    "Region": "us-east-1",
    "Profile": "default"
  },
  "Logging": {
    "LogGroup": "TaskManagementLogs",
    "Region": "us-east-1",
    "IncludeLogLevel": true,
    "IncludeCategory": true,
    "IncludeNewline": true,
    "IncludeException": true
  }
}
```

### 3. Run the Application

```bash
dotnet ef database update
dotnet run
```

Visit Swagger UI at: `https://localhost:<port>/swagger`

---

## 🔐 Authorization & Role Testing

To test role-based access with Cognito and JWT:

1. **Register User** via `/api/Auth/signup`.
2. **Login** via `/api/Auth/signin` to receive the JWT.
3. **Authorize in Swagger**:
   - Click "Authorize"
   - Paste:
     ```
     Bearer <your-jwt-token>
     ```
4. The token includes role claims like `user` or `admin`, which are used to control access to endpoints.

---

## 🧪 Unit Tests

Run unit tests with:

```bash
dotnet test
```

Test coverage includes:
- Core service logic (ProjectService, TaskService)
- Role-based access enforcement
- Error handling scenarios

---

## 🚚 Deployment Suggestion

If the system must support around **10K users per day** with a client-side application:

- **Backend API**: Host on AWS Elastic Beanstalk or ECS Fargate for scalability and managed deployments.
- **Client-side app**: Host the frontend (e.g., Angular/React) on AWS S3 with CloudFront for fast delivery.
- **Database**: Use Amazon Aurora (MySQL-compatible) for high performance and reliability.
- **Authentication**: AWS Cognito is already integrated and scales well.
- **Monitoring & Logs**: Enable CloudWatch logging with log groups.
- **CI/CD**: Use GitHub Actions or AWS CodePipeline for deployment automation.

Avoid overcomplication – focus on scalability, observability, and fault tolerance with cost-effective services.

---

## 📬 Contact

Dvir Ben Shlush  
[GitHub Profile](https://github.com/dvirbenshlush)
