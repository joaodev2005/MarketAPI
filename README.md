# MarketAPI 🛒

[![CI/CD - GitHub Actions](https://github.com/joaodev2005/MarketAPI/actions/workflows/ci.yml/badge.svg)](https://github.com/joaodev2005/MarketAPI/actions)

Apresentando o **MarketAPI** - uma solução robusta, distribuída e de alta performance desenvolvida em **.NET 10** para gerenciamento de mercado. O projeto foi construído aplicando conceitos avançados de arquitetura, sistemas de mensageria assíncrona e processamento em segundo plano para suportar cenários reais, resilientes e escaláveis.

Esta API permite o gerenciamento completo de um ecossistema de mercado (como produtos, categorias e usuários). Ela conta com um fluxo seguro de cadastro e autenticação, além de processamento otimizado de tarefas pesadas por meio de mensageria e uma camada eficiente de cache para garantir respostas em milissegundos.

Seguindo os princípios de **Domain-Driven Design (DDD)** e **SOLID**, a arquitetura do projeto busca manter um design modular, limpo e sustentável. A validação dos dados é realizada utilizando **FluentValidation**, assegurando que todas as entradas de dados atendam aos critérios estabelecidos antes de chegar ao domínio.

---

## 🚀 Features

* **Gerenciamento do Mercado:** CRUD completo e otimizado com filtros avançados para as entidades do sistema. 📦🔍
* **Segurança Avançada:** Mecanismo de autenticação robusto utilizando **Tokens JWT**. 🔑🛡️
* **Processamento Assíncrono (Workers):** Background Services dedicados a escutar filas do **RabbitMQ** para processar tarefas demoradas fora da requisição principal HTTP. 📩🗂️
* **Cache de Alta Performance:** Camada de aceleração com **Redis** para evitar consultas repetitivas ao banco de dados em endpoints de leitura frequente. ⚡🚀
* **Tratamento de Erros Resiliente:** Camadas dedicadas de `Exception` para capturar falhas globalmente e mapear respostas HTTP padronizadas e amigáveis. 🩹❌
* **Estratégia de Contratos Limpos:** Camada de `Communication` isolada para garantir que as requisições (Requests) e respostas (Responses) sigam um padrão rigoroso. 📄↔️

---

## 🏗️ Arquitetura do Projeto

O projeto adota os princípios da **Clean Architecture** alinhados ao **DDD (Domain-Driven Design)**, garantindo uma separação clara de responsabilidades e desacoplamento de infraestrutura:

* **Domain:** Core do sistema contendo Entidades, Objetos de Valor (Value Objects), agregados, interfaces de repositórios e regras de negócio puras.
* **Application:** Casos de uso (Use Cases), validadores (FluentValidation), serviços de aplicação e publicação de eventos assíncronos.
* **Infrastructure:** Implementação do contexto do banco de dados, repositórios (EF Core), configurações de conexão do Redis e publishers do RabbitMQ.
* **Worker:** Serviços de segundo plano (Background Services) dedicados a consumir as filas e executar processamentos assíncronos.
* **Communication:** Camada compartilhada responsável por padronizar as entradas e saídas da API.
* **Exception:** Centralização e padronização do tratamento de erros com exceções de negócio customizadas.
* **API:** Controladores (Controllers), endpoints expostos, middlewares globais e configuração de injeção de dependência.

---

## 🧪 Estratégia de Testes & Integração Contínua (CI)

Para garantir a qualidade do código e evitar regressões, o projeto conta com uma esteira automatizada integrada ao **GitHub Actions**:
* **Testes de Integração Confiáveis:** Utilizamos o **Testcontainers** para subir temporariamente um container Docker oficial do **SQL Server** durante a execução da suíte de testes (via xUnit e Shouldly). Isso garante que os testes rodem contra um banco de dados real, idêntico ao de produção, eliminando falsos positivos comuns em bancos em memória (In-Memory).
* **Pipeline de CI:** A cada `git push` nas branches monitoradas, a pipeline compila o projeto e executa toda a suíte de testes automaticamente na nuvem, garantindo a integridade do ecossistema.

---

## 🛠️ Construído com

* **.NET 10** & **C#**
* **SQL Server** (com Entity Framework Core)
* **RabbitMQ** (Mensageria e Filas)
* **Redis** (Cache Distribuído)
* **xUnit** & **Shouldly** (Testes automatizados)
* **Testcontainers** (Ambientes reais em Docker para testes)
* **Docker** & **Docker Compose**
* **GitHub Actions** (CI/CD)

---

## 🏁 Getting Started

Para obter uma cópia local funcionando, siga estes passos simples.

### Pré-requisitos
* [.NET 10 SDK](https://dotnet.microsoft.com/download)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado e rodando

### Passo a Passo

1. Clone o repositório:
   ```bash
   git clone [https://github.com/joaodev2005/MarketAPI.git](https://github.com/joaodev2005/MarketAPI.git)
   cd MarketAPI
   ```
2. Suba a infraestrutura necessária (SQL Server, RabbitMQ e Redis) via Docker Compose:
   ```bash
   docker compose up -d
   ```
3. Rode a aplicação principal:
   ```bash
   dotnet run --project src/MarketAPI.API
   ```
4. Para rodar a suíte de testes localmente:
   ```bash
   dotnet test
   ```
