# Pokémon Full‑Stack Web App

This is a full‑stack Pokémon web application built with:

- **Backend:** ASP.NET Core (.NET 8), Entity Framework (code‑first), SQL Server, JWT authentication
- **Frontend:** Vue.js (Vite)

The app allows users to register, log in, view a list of Pokémon, and search Pokémon by name or ID.  
Protected routes are only accessible to authenticated users.

---

## 📦 Project structure

```plaintext
/backend         ← .NET 8 API with clean architecture
/frontend        ← Vue.js frontend
````

---

## 🛠 Backend (.NET 8 API)

* Runs on: https://localhost:44306/
* Built with:

  * ASP.NET Core (.NET 8)
  * Entity Framework Core (code‑first)
  * SQL Server (local db)
  * JWT authentication
* On first run, it will automatically create a SQL Server database named:

  ```
  PokemonWebAppDb
  ```

### 📋 APIs overview

| Endpoint                                          | Method | Description                                   |
| ------------------------------------------------- | :----: | --------------------------------------------- |
| `/api/Account/register`                           |  POST  | Register a new user                           |
| `/api/Account/login`                              |  POST  | Login and get JWT token                       |
| `/api/Pokemon/list-with-images?limit=10&offset=0` |   GET  | Get paginated list of Pokémon (protected)     |
| `/api/Pokemon/{nameOrId}`                         |   GET  | Get Pokémon details by name or ID (protected) |

> Pokémon routes require JWT Bearer token.

---

## ▶️ **How to run backend**

1. Open `/backend` folder in Visual Studio
2. Select **IIS Express** profile (runs on https://localhost:44306/ )
3. Build & run

On startup, the database `PokemonWebAppDb` will be created automatically by EF migrations.

---

## 🖥 Frontend (Vue.js)

* Runs on: [http://localhost:5173/](http://localhost:5173/)
* Built with:

  * Vue.js 3
  * Vite
  * Vue Router
  * Axios

### ▶️ **How to run frontend**

```bash
cd frontend
npm install
npm run dev
```

The app will be hosted at:
[http://localhost:5173/](http://localhost:5173/)

---

## 🔒 **Authentication flow**

* Users register & log in
* JWT token is stored locally and sent in `Authorization: Bearer` header to access Pokémon APIs

---

## ✏ **Notes**

* Backend will run on HTTPS (`https://localhost:44306/`)
* Frontend calls backend APIs securely from `http://localhost:5173/`

---

## ✅ **Status**

* Backend & frontend both working
* All modules tested: register, login, logout, Pokémon list, search
* Responsive UI with clean modern design

