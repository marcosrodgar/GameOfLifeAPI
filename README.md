# Conway's Game of Life API

This is a minimal, production-ready Web API for simulating [Conway's Game of Life](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life), built with .NET 7, SQLite, and Entity Framework Core.

---

## 🚀 Features

- Upload a new board state (2D grid of 0s and 1s)
- Get the next generation for a board
- Get N generations ahead
- Get the final state (loop or all cells dead)
- Persist state between app restarts using SQLite
- Input validation for correct board structures

---

## 🧠 Conway’s Game of Life Rules

1. A live cell with fewer than 2 live neighbors dies.
2. A live cell with 2–3 live neighbors survives.
3. A live cell with more than 3 live neighbors dies.
4. A dead cell with exactly 3 live neighbors becomes alive.

---

## 📦 Requirements

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [SQLite](https://www.sqlite.org/) (included via EF Core)

---

## 🛠️ Running the API

### 1. Clone the repo

```bash
git clone https://github.com/your-username/GameOfLifeApi.git
cd GameOfLifeApi
```

### 2. Restore packages

```bash
dotnet restore
```

### 3. Create the database

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 4. Run the API

```bash
dotnet run
```

---

## 🔗 API Endpoints

### `POST /board`
**Upload a new board**

#### Request Body (2D array of 0s and 1s):

```json
[
  [0, 1, 0],
  [1, 1, 1],
  [0, 1, 0]
]
```

#### Response:

```json
{
  "boardId": "<GUID>"
}
```

---

### `GET /board/{boardId}/next`
**Returns the next generation for a board.**

---

### `GET /board/{boardId}/next/{steps}`
**Returns the board state after N steps.**

---

### `GET /board/{boardId}/final?maxAttempts=10`
**Simulates up to `maxAttempts` steps and returns the final stable state or dead board.**

Returns `400 Bad Request` if no stable state is reached.

---
