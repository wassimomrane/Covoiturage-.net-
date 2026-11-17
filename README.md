#  Covoiturage Express — API .NET 10 & PostgreSQL

Plateforme moderne de covoiturage développée sous **.NET 10** en respectant les principes de la **Clean Architecture**, du **Repository Pattern** et du pattern **Unit of Work**.

---

##  Architecture du Projet

Le projet est découpé en 4 couches distinctes :
* **`Covoiturage.Domain`** : Entités métiers (`ApplicationUser`, `Trajet`, `Reservation`, `Vehicule`, `Ville`) et énumérations d'états.
* **`Covoiturage.Application`** : DTOs, interfaces de services, abstractions de dépôts (`IRepository`, `IUnitOfWork`) et pagination générique (`PagedResult<T>`).
* **`Covoiturage.Infrastructure`** : Contexte EF Core (`ApplicationDbContext`), migrations, implémentation des dépôts et service d'authentification JWT avec vérification OTP.
* **`Covoiturage.API`** : Contrôleurs REST, middlewares globaux, configuration Swagger/OpenAPI et interface Web SPA (`wwwroot`).

---

##  Prérequis

* [.NET 10 SDK](https://dotnet.microsoft.com/)
* [PostgreSQL](https://www.postgresql.org/) (port par défaut : `5432`)
* Outil EF Core CLI : `dotnet tool install --global dotnet-ef`

---

##  Installation & Démarrage Rapide

### 1. Configuration de la base de données
Vérifiez ou adaptez la chaîne de connexion dans `Covoiturage.API/appsettings.json` :
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=covoiturage_db;Username=postgres;Password=postgres"
}