# RepairIT

Application de gestion d'un prestataire de **vente et de réparation d'ordinateurs**, développée en microservices avec ASP.NET Core, Blazor WebAssembly et Entity Framework Core (projet ALTN71 – EFREI 2026-2027).

> **Version actuelle : V1 – Catalogue informatique**
> Elle contient le squelette de l'application et les premiers microservices : CRUD des ordinateurs, création des utilisateurs, EF Core avec SQLite, initialisation de la base et tests unitaires.

---

## Sommaire
1. [Architecture](#architecture)
2. [Lancement](#lancement)
3. [Contenu de la V1](#contenu-de-la-v1)
4. [API REST](#api-rest)
5. [Modèles de base de données](#modèles-de-base-de-données)
6. [Choix techniques](#choix-techniques)
7. [Tests et couverture](#tests-et-couverture)
8. [Feuille de route](#feuille-de-route)

---

## Architecture

```
RepairIT/
├── RepairIT.sln
├── start-all.bat                         Lance tous les projets en une fois
├── run-tests.bat                         Tests + couverture de code
├── src/
│   ├── RepairIT.Shared/                  Bibliothèque partagée : entités, énumérations, DTO
│   ├── RepairIT.CatalogService/          Web API – catalogue d'ordinateurs (SQLite catalog.db)
│   ├── RepairIT.UserService/             Web API – utilisateurs            (SQLite users.db)
│   ├── RepairIT.RepairService/           Web API – réparations     (squelette, V2)
│   ├── RepairIT.SalesService/            Web API – ventes          (squelette, V3)
│   ├── RepairIT.AuthentificationService/ Web API – Keycloak        (squelette, V5)
│   ├── RepairIT.Gateway/                 Passerelle                (squelette, V4)
│   └── RepairIT.Client/                  Front Blazor WebAssembly
└── tests/
    ├── RepairIT.CatalogService.Tests/    xUnit + Moq + EF Core InMemory
    └── RepairIT.UserService.Tests/       xUnit + Moq + EF Core InMemory
```

| Projet | Type | Port | Rôle en V1 |
|---|---|---|---|
| RepairIT.Client | Blazor WASM | **5000** | Squelette de l'interface : thème, menu, pages |
| RepairIT.Gateway | Web | 5100 | Squelette (configuration prévue en V4) |
| RepairIT.CatalogService | Web API | 5101 | CRUD des ordinateurs |
| RepairIT.UserService | Web API | 5102 | Création et consultation des utilisateurs |
| RepairIT.RepairService | Web API | 5103 | Squelette (`GET /api/health`) |
| RepairIT.SalesService | Web API | 5104 | Squelette (`GET /api/health`) |
| RepairIT.AuthentificationService | Web API | 5105 | Squelette (`GET /api/health`) |
| RepairIT.Shared | classlib | – | Modèles communs |

## Lancement

Prérequis : **SDK .NET 10**.

```bat
start-all.bat
```

Le script compile la solution, puis ouvre une fenêtre par projet. Ensuite :

- Interface : http://localhost:5000 (administration : http://localhost:5000/admin)
- Swagger des APIs : http://localhost:5101/swagger à http://localhost:5105/swagger

Pour lancer un seul projet : `dotnet run --project src/RepairIT.CatalogService`.

Les bases SQLite sont **créées automatiquement au démarrage** (migrations EF Core) puis remplies avec des données de démonstration : 8 ordinateurs, 1 administrateur et 4 gestionnaires. Pour repartir de zéro, il suffit de supprimer `catalog.db` ou `users.db`.

## Contenu de la V1

| Attendu (énoncé V1) | Réalisation |
|---|---|
| Création de la solution | `RepairIT.sln` |
| Création des différents projets (front et APIs) | Client Blazor, 5 Web APIs, Gateway, Shared, 2 projets de tests |
| CRUD Ordinateurs | `RepairIT.CatalogService` – `/api/ordinateurs` |
| Gestion des utilisateurs (création, valeur par défaut pour Keycloak) | `RepairIT.UserService` – `/api/utilisateurs`, `UserKeycloak = "non-configure"` |
| Configuration d'EF Core et création de la base (SQLite) | `CatalogDbContext`, `UserDbContext`, migrations `InitialCreate` |
| Initialisation de la base | `CatalogDbInitializer`, `UserDbInitializer` |
| Quelques tests unitaires | 33 tests xUnit / Moq |
| Injection de dépendances, Logging, Configuration | Voir [Choix techniques](#choix-techniques) |
| Script de lancement | `start-all.bat` |

Le front est livré **en squelette** : thème personnalisé, menu complet (Accueil, Ordinateurs, Réparations, Ventes, Tableau de bord, Administration, Connexion) et pages indiquant leur version de livraison. Le branchement aux APIs, via `HttpClientFactory` et la Gateway, est prévu en V4. En attendant, les APIs se testent avec Swagger.

## API REST

### CatalogService – `/api/ordinateurs`
| Méthode | Route | Description | Réponses |
|---|---|---|---|
| GET | `/api/ordinateurs` | Liste des ordinateurs | 200 |
| GET | `/api/ordinateurs/{id}` | Détail | 200 / 404 |
| POST | `/api/ordinateurs` | Création | 201 / 400 |
| PUT | `/api/ordinateurs/{id}` | Modification | 200 / 400 / 404 |
| DELETE | `/api/ordinateurs/{id}` | Suppression | 204 / 404 |

Exemple de corps (POST/PUT) :
```json
{
  "marque": "Dell", "modele": "XPS 13", "processeur": "Intel Core i7",
  "ramGo": 16, "stockageGo": 512, "type": "Portable", "etat": "Neuf",
  "prix": 1299.00, "quantiteStock": 5
}
```

### UserService – `/api/utilisateurs`
| Méthode | Route | Description | Réponses |
|---|---|---|---|
| GET | `/api/utilisateurs` | Liste des utilisateurs | 200 |
| GET | `/api/utilisateurs/{id}` | Détail | 200 / 404 |
| POST | `/api/utilisateurs` | Création | 201 / 400 / 409 (email déjà utilisé) |

Exemple de corps (POST) :
```json
{ "nom": "Dupont", "prenom": "Jean", "email": "jean.dupont@repairit.fr", "role": "Gestionnaire" }
```

### Services squelettes
`GET /api/health` sur RepairService (5103), SalesService (5104) et AuthentificationService (5105).

## Modèles de base de données

Chaque microservice possède **sa propre base**.

### `catalog.db` – table `Ordinateurs`
| Colonne | Type | Contraintes |
|---|---|---|
| Id | INTEGER | PK, auto-incrément |
| Marque | TEXT(50) | obligatoire |
| Modele | TEXT(100) | obligatoire |
| Processeur | TEXT(100) | obligatoire |
| RamGo | INTEGER | 1 – 1024 |
| StockageGo | INTEGER | 1 – 100000 |
| Type | TEXT(20) | `Portable`, `Fixe`, `AllInOne`, `Serveur` |
| Etat | TEXT(20) | `Neuf`, `Reconditionne`, `Occasion` |
| Prix | DECIMAL(10,2) | ≥ 0 |
| QuantiteStock | INTEGER | ≥ 0 |
| DateAjout | DATETIME | UTC, renseignée par le service |

### `users.db` – table `Utilisateurs`
| Colonne | Type | Contraintes |
|---|---|---|
| Id | INTEGER | PK, identifiant technique |
| UserKeycloak | TEXT(100) | `non-configure` par défaut (lien Keycloak en V5) |
| Nom | TEXT(50) | obligatoire |
| Prenom | TEXT(50) | obligatoire |
| Email | TEXT(150) | obligatoire, **unique** |
| Role | TEXT(20) | `Gestionnaire`, `Administrateur` |
| EstActif | BOOLEAN | `true` à la création |
| DateCreation | DATETIME | UTC |

Aucun mot de passe n'est stocké : l'authentification sera entièrement déléguée à Keycloak (V5).

## Choix techniques

- **Une base par microservice** : chaque service est autonome. Ils ne partagent que les modèles de `RepairIT.Shared`.
- **SQLite en V1**, comme demandé. La chaîne de connexion est dans `appsettings.json` (`ConnectionStrings`), ce qui permettra de passer à PostgreSQL en V2 sans toucher au code métier.
- **Migrations EF Core** (`Data/Migrations`) appliquées au démarrage par les initialiseurs. Le jeu de démonstration n'est inséré que si la table est vide, et seulement si l'option `Database:SeedOnStartup` est activée.
- **Injection de dépendances** :
  - services métier derrière des interfaces (`IOrdinateurService`, `IUtilisateurService`) ;
  - `DbContext` et initialiseurs enregistrés dans `Program.cs`.
- **Logging** : `ILogger<T>` dans les contrôleurs, les services et les initialiseurs (création, modification, suppression, identifiant introuvable, email en double). Les niveaux se règlent dans `appsettings.json`.
- **Configuration** : chaînes de connexion, options de base de données (`DatabaseOptions`), informations des services squelettes (`ServiceInfoOptions`). Toutes sont lues via le pattern Options.
- **DTO de requête** (`OrdinateurRequest`, `UtilisateurRequest`) : le client ne fournit ni l'identifiant ni les dates. La validation par DataAnnotations renvoie automatiquement 400.
- **Énumérations en texte**, en JSON comme en base, pour des données lisibles.
- **Pas d'authentification en V1** : elle est hors périmètre et sera ajoutée en V5 (Keycloak, `[Authorize(Roles = "...")]`).

## Tests et couverture

```bat
run-tests.bat
```

- **33 tests** xUnit.
- Les services sont testés avec **EF Core InMemory** : CRUD des ordinateurs, création d'utilisateur, email en double, initialisation de la base.
- Les contrôleurs sont testés avec des services simulés par **Moq** : codes 200, 201, 204, 404 et 409.

Couverture de lignes mesurée par coverlet, sans compter le code généré (migrations, `Program.cs`) :

| Projet | Lignes | Branches |
|---|---|---|
| RepairIT.CatalogService | 96,7 % | 83,3 % |
| RepairIT.UserService | 95,9 % | 83,3 % |
| RepairIT.Shared | 100 % | 100 % |

Le front n'est pas testé, conformément à la stratégie de tests du projet.

## Feuille de route

| Version | Contenu | Date de rendu |
|---|---|---|
| **V1** | Squelette, CRUD ordinateurs, création des utilisateurs, SQLite | 20/11/2026 |
| V2 | Clients, réparations, PostgreSQL | 06/12/2026 |
| V3 | Ventes, tableau de bord commercial | 20/12/2026 |
| V4 | Intégration du front, Gateway, tableau de bord | 10/01/2027 |
| V5 | Keycloak, protection des endpoints, Docker | 24/01/2027 |
