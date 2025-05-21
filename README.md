
# ReqClientAPI - Simple .NET Client for Reqres API

This is a small .NET 8 project that connects to the [Reqres API] to fetch user information.

It includes:

- `ReqClientAPI/`: A reusable class library to handle API communication
- `APIConsole/`: A small console application to test the library
- `ReqClientAPI.Tests/`: A unit test project to validate functionality

---

## Project Structure

```
/assignment
│
├── ReqClientAPI/         # Main logic to call the Reqres API
├── APIConsole/           # Console app to run and test the client
└── ReqClientAPI.Tests/   # Unit tests to verify the logic
```

---

## How to Build and Run

### Step 1: Build the solution

```bash
dotnet build
```

### Step 2: Run the Console App

```bash
dotnet run --project APIConsole
```

You will see output like:

```
Getting all users...
1: George Bluth
2: Janet Weaver

Getting single user...
1: George Bluth
```

---

## How to Run Tests

Make sure you're in the root folder where the `.sln` or all projects are located.

```bash
dotnet test ReqClientAPI.Tests
```

This will run all unit tests and show the result in the terminal.






