# 🧠 QuizGenerator – AI-Powered Quiz Platform

<p align="center">
  <img src="https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/Blazor-WebAssembly-512BD4?style=for-the-badge&logo=blazor&logoColor=white" />
  <img src="https://img.shields.io/badge/Architecture-Microservices-blue?style=for-the-badge" />
  <img src="https://img.shields.io/badge/AI-LLM%20Powered-orange?style=for-the-badge&logo=openai&logoColor=white" />
  <img src="https://img.shields.io/badge/License-MIT-green?style=for-the-badge" />
</p>

> **Projekt inżynierski** – Platforma webowa do automatycznego generowania interaktywnych quizów z wykorzystaniem sztucznej inteligencji (LLM) i architektury mikroserwisowej w technologii .NET 9.

---

## 📋 Spis treści

- [O projekcie](#-o-projekcie)
- [Kluczowe funkcjonalności](#-kluczowe-funkcjonalności)
- [Architektura systemu](#-architektura-systemu)
- [Stos technologiczny](#-stos-technologiczny)
- [Uruchomienie projektu](#-uruchomienie-projektu)
- [Pipeline generowania quizu](#-pipeline-generowania-quizu)
- [API Endpoints](#-api-endpoints)
- [Wzorce projektowe](#-wzorce-projektowe)
- [Praca dyplomowa](#-praca-dyplomowa)
- [Licencja](#-licencja)
- [Autor](#-autor)
---

## 🎯 O projekcie

**QuizGenerator** to platforma internetowa umożliwiająca użytkownikom tworzenie interaktywnych quizów w oparciu o przesłane materiały edukacyjne. System wykorzystuje modele językowe (LLM) do automatycznego generowania pytań i odpowiedzi na podstawie kontekstu wydobytego z dokumentów — dzięki technice **RAG (Retrieval-Augmented Generation)**.

Aplikacja została zaprojektowana w architekturze **mikroserwisowej** z wykorzystaniem **.NET Aspire** jako orkiestratora, co zapewnia skalowalność, modularność i łatwość utrzymania.

---

## ✨ Kluczowe funkcjonalności


| Funkcjonalność | Opis | Status |
|---|---|---|
| 📄 **Upload dokumentów** | Przesyłanie plików PDF, automatyczna konwersja do tekstu i przechowywanie w Azure Blob Storage | ✅ Zrealizowane |
| 🔪 **Chunking tekstu** | Inteligentne dzielenie tekstu na fragmenty z zachowaniem kontekstu (Semantic Kernel TextChunker) | ✅ Zrealizowane |
| 🧬 **Embedding & Vector Store** | Generowanie embeddingów i zapisywanie w bazie wektorowej Qdrant | ✅ Zrealizowane |
| 🤖 **Generowanie quizów przez AI** | Tworzenie pytań i odpowiedzi za pomocą LLM z walidacją i mechanizmem retry | ✅ Zrealizowane |
| 📝 **Zarządzanie quizami** | Tworzenie, przeglądanie i rozwiązywanie quizów | ✅ Zrealizowane |
| 🏷️ **System tagów** | Kategoryzowanie quizów za pomocą tagów | 🗺️ W planach |
| 👤 **Zarządzanie użytkownikami** | Profile użytkowników z uwierzytelnianiem przez Keycloak | 🛠️ W trakcie |
| 🔐 **Bezpieczeństwo** | JWT authentication (Keycloak), autoryzacja, komunikacja client-to-service i service-to-service | ✅ Zrealizowane |
| ⚡ **Asynchroniczne przetwarzanie** | Kolejka zadań w Redis z background service | ✅ Zrealizowane |
| 🌐 **Pełny routing przez YARP** | Centralny ruch przychodzący do usług przez API Gateway (YARP) | 🛠️ W trakcie |
| 🚪 **Ruch wychodzący przez Envoy** | Obsługa egress (ruch wychodzący) i polityk sieciowych przez Envoy | 🛠️ W trakcie |
| 🐳 **Docker Compose** | Plik `docker-compose.yml` do uruchamiania całego stacku (Keycloak/Redis/Qdrant/SQL/itp.) | 🛠️ W trakcie |
| 🚀 **Pełne wsparcie .NET Aspire** | Spójna orkiestracja, observability i uruchamianie całego systemu przez Aspire AppHost | 🛠️ W trakcie|
| 🐇 **Message Broker (RabbitMQ)** | Skalowalne przetwarzanie zdarzeń i zadań w tle | 🗺️ W planach |
| 🎨 **Frontend (Blazor)** | Interfejs webowy aplikacji | 🗺️ W planach |
| 🧪 **Testy jednostkowe** | Testy logiki biznesowej i usług (unit tests) | 🛠️ W trakcie |

---

## 🏗 Architektura systemu
<p align="center"><img width="1100" height="809" alt="image" src="https://github.com/user-attachments/assets/4da6d00f-eb0d-4da4-887d-36e94cbccd0d" /></p>



System składa się z **4 mikroserwisów** zarządzanych przez **.NET Aspire AppHost**:

| Serwis | Odpowiedzialność |
|---|---|
| **ContextBuilderService** | Upload dokumentów PDF, konwersja do tekstu, chunking, cache w Redis |
| **LLMService** | Generowanie embeddingów, wyszukiwanie wektorowe (RAG), generowanie quizów przez LLM, kolejka zadań |
| **QuizService** | CRUD quizów, zarządzanie pytaniami/odpowiedziami, podejścia do quizu (attempts), scoring |
| **UserService** | Zarządzanie profilami użytkowników, uprawnienia |
| **YarpGateway** | API Gateway (reverse proxy) routujący requesty do odpowiednich mikroserwisów |

---

## 🛠 Stos technologiczny

### Backend
- **.NET 9** – framework
- **.NET Aspire** – orkiestracja mikroserwisów
- **YARP** – reverse proxy / API Gateway
- **MediatR** – wzorzec CQRS (Command Query Responsibility Segregation)
- **Carter** – Minimal API endpoints
- **FluentValidation** – walidacja requestów
- **Mapster** – mapowanie obiektów
- **Entity Framework Core** – ORM (SQL Server)

### AI & Data Processing
- **Microsoft.Extensions.AI** – integracja z modelami LLM
- **Qdrant** – baza wektorowa do wyszukiwania semantycznego
- **Semantic Kernel TextChunker** – chunking dokumentów
- **PdfPig** – parsowanie plików PDF

### Infrastruktura
- **Keycloak** – uwierzytelnianie i autoryzacja (OAuth2 / OpenID Connect)
- **Redis** – cache, kolejka zadań (job queue), przechowywanie chunków
- **Azure Blob Storage** – przechowywanie dokumentów
- **SQL Server** – bazy danych serwisów
- **Docker** – konteneryzacja

### Frontend
- **Blazor** – WebAssembly + Interactive Server Rendering
- **HTML / CSS**

---


## 🚀 Uruchomienie projektu

> **Adnotacja:** W opracowaniu — aktualnie tworzony jest plik `docker-compose.yaml`.  
> **Adnotacja:** W opracowaniu - jest pełna migracja z ręcznej konfiguracji Docker Compose do **.NET Aspire**.

---
## 🔄 Pipeline generowania quizu

```
1. 📄 Upload PDF
   └─► ContextBuilderService: PDF → TXT → Azure Blob Storage

2. 🔪 Chunking
   └─► ContextBuilderService: Pobranie z Blob → TextChunker → Redis Cache

3. 🧬 Embedding
   └─► LLMService: Pobranie chunków z Redis → Generowanie embeddingów → Zapis do Qdrant

4. 🤖 Generowanie quizu (asynchroniczne)
   ├─► Użytkownik wysyła request → Job trafia do kolejki Redis
   ├─► Background Service pobiera job z kolejki
   ├─► Wyszukiwanie wektorowe (Top-K chunks) w Qdrant (RAG)
   ├─► Generowanie quizu przez LLM z walidacją + retry
   └─► Zapis wygenerowanego quizu w QuizService (SQL Server)

5. 📝 Rozwiązywanie quizu
   └─► QuizService: Podejście do quizu → Odpowiedzi → Scoring
```

---

## 📡 API Endpoints

### ContextBuilderService
| Metoda | Endpoint | Opis |
|--------|----------|------|
| `POST` | `/upload` | Upload pliku (PDF) jako `multipart/form-data` (`file`) |
| `GET`  | `/chunking/{documentId}` | Pobranie danych dokumentu i wykonanie chunkingu (na podstawie GUID dokumentu) |

### LLMService
| Metoda | Endpoint | Opis |
|--------|----------|------|
| `POST` | `/generate` | Enqueue generowania quizu (asynchronicznie). Body: `k`, `countQuestion`, `question`, `documentIds[]` |
| `GET`  | `/test/job/{jobId}` | Podgląd statusu/wyniku joba (endpoint developerski) |
| `POST` | `/embedding/{documentId}` | Utworzenie embeddingów dla dokumentu (na podstawie chunków) |

### QuizService
| Metoda | Endpoint | Opis |
|--------|----------|------|
| `POST` | `/quiz` | Utworzenie quizu (np. zapis quizu wygenerowanego przez LLM) |
| `GET`  | `/quiz` | Pobranie listy quizów |
| `GET`  | `/quiz/{quizId}` | Pobranie quizu po ID |
| `POST` | `/quiz/attempt/{quizId}` | Utworzenie podejścia (attempt) do quizu |
| `GET`  | `/quiz/attempt/{attemptId}` | Pobranie podejścia (attempt) po ID |
| `POST` | `/quiz/submit` | Wysłanie odpowiedzi i zakończenie podejścia (submit attempt) |

### UserService
| Metoda | Endpoint | Opis |
|--------|----------|------|
| `POST`  | `/create` | Utworzenie profilu użytkownika |
| `GET`   | `/profile` | Pobranie profilu aktualnego użytkownika |
| `GET`   | `/permission` | Sprawdzenie uprawnień (permissions) |
| `PATCH` | `/edit` | Edycja profilu (np. zmiana `privilegeUserDomain`) |

📬 Kolekcja Postman dostępna tutaj:  
> [QuizApi.postman_collection.json](https://github.com/Rumcajs121/Engineering_Project_QuizGeneratorWebAPP/blob/master/PostmanCollection/QuizApi.postman_collection.json)
---

## 📐 Wzorce projektowe

- **Microservices Architecture** – niezależne, wyspecjalizowane serwisy  
- **CQRS** (Command Query Responsibility Segregation) – rozdzielenie odczytu i zapisu  
- **DDD** (Domain-Driven Design) – model domenowy w QuizService  
- **Clean Architecture** – warstwowy podział odpowiedzialności (API → Application → Domain → Infrastructure), izolacja logiki biznesowej od frameworków i dostawców danych  
- **Service Pattern (Business Services)** – wydzielenie logiki biznesowej do serwisów aplikacyjnych/domenowych (np. `IQuizService`, `IGenerateQuizService`), aby endpointy były „cienkie”, a logika testowalna i reużywalna  
- **RAG** (Retrieval-Augmented Generation) – generowanie z kontekstem  
- **API Gateway** – centralny punkt wejścia (YARP)  
- **Repository Pattern** – abstrakcja dostępu do danych  
- **Mediator Pattern** – MediatR jako mediator komend i zapytań  
- **Vertical Slice Architecture** – organizacja kodu wg funkcjonalności (Features)  
---
## 🎓 Praca dyplomowa

- 📄 **Link do pracy dyplomowej:** _(zostanie dodany po obronie)_  

> **Adnotacja:** Dokumentacja pracy dyplomowej (PDF) zostanie opublikowana i podlinkowana **dopiero po obronie**.

## 📄 Licencja

Ten projekt jest udostępniany na licencji **MIT** – szczegóły w pliku [LICENSE](LICENSE).

---

## 👨‍💻 Autor

**Zbigniew Miara** – Projekt inżynierski

[![GitHub](https://img.shields.io/badge/GitHub-Rumcajs121-181717?style=flat-square&logo=github)](https://github.com/Rumcajs121)

---
