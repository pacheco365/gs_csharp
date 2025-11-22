# 🧘‍♂ Equilibrium API

> *Plataforma de Bem-Estar Corporativo e Gamificação*

O *Equilibrium* é uma solução projetada para combater o burnout e aumentar o engajamento de colaboradores através do incentivo ao autocuidado. O sistema permite que funcionários registrem seu humor diário, ganhem *EQ Points* e troquem esses pontos por recompensas reais, criando um ciclo positivo de feedback e saúde mental.

---

## 📋 Índice

* [Visão Geral](#-visão-geral)
* [Arquitetura do Projeto](#-arquitetura-do-projeto)
* [Tecnologias Utilizadas](#-tecnologias-utilizadas)
* [Funcionalidades](#-funcionalidades)
* [Versionamento da API](#-versionamento-da-api)
* [Configuração e Execução](#-configuração-e-execução)
* [Documentação da API](#-documentação-da-api)
---

## Vídeo
https://youtu.be/XfVN4zOh4GY
---

## 🔭 Visão Geral

O sistema funciona como o "banco central" de pontos de bem-estar. Ele recebe dados de inputs (App Mobile ou Dispositivos IoT), processa as regras de negócio de gamificação e persiste as transações financeiras (pontos) de forma segura e auditável.

*O ciclo do usuário:*
1.  *Cadastro:* Funcionário entra na plataforma.
2.  *Check-in:* Registra como está se sentindo (Ganho de Pontos).
3.  *Acúmulo:* Pontos são somados ao saldo e registrados no histórico.

---

## 🏗 Arquitetura do Projeto

O projeto foi desenvolvido seguindo uma arquitetura em camadas *(N-Layer)* para garantir a separação de responsabilidades, testabilidade e manutenção, alinhado aos princípios de Clean Architecture.

O fluxo de dados segue a ordem: Client -> API -> Application -> Business -> Data -> Database.

### Estrutura da Solução:

| Projeto | Responsabilidade |
| :--- | :--- |
| *EquilibriumAPI* | Camada de Apresentação. Contém os *Controllers*, Configuração de Swagger e Injeção de Dependência. É o ponto de entrada. |
| *EquilibriumApplication* | Interfaces de Serviços e *DTOs* (Data Transfer Objects). Define os contratos do sistema. |
| *EquilibriumBusiness* | *Lógica de Negócio*. Implementa as regras (ex: validação de saldo, cálculo de pontos, estorno). |
| *EquilibriumData* | Infraestrutura de dados. Contém o *DbContext* do Entity Framework e configurações de mapeamento. |
| *EquilibriumModel* | Entidades de Domínio e Modelos de Banco de Dados. |

---

## 🚀 Tecnologias Utilizadas

* *Runtime:* .NET 9
* *Framework Web:* ASP.NET Core Web API
* *ORM:* Entity Framework Core 9
* *Banco de Dados:* Oracle Database (19c/21c)
* *Documentação:* Swagger UI (Swashbuckle)
* *Boas Práticas:* Injeção de Dependência, Repository Pattern (via Service), Migrations.

---

## ✨ Funcionalidades

### 1. Gestão de Usuários
* Cadastro de novos colaboradores.
* Consulta de perfil e *Saldo de EQ Points* em tempo real.
* Atualização de dados cadastrais.

### 2. Gamificação (Check-in)
* Registro diário de nível de humor (1 a 5).
* Atribuição automática de pontos (Regra atual: +10 pontos por check-in).
* Possibilidade de correção (Update) e remoção (Delete) de check-ins.
---

## 📌 Versionamento da API

Esta API utiliza *Versionamento por URL* para garantir a evolução do contrato sem quebrar clientes existentes.

* *Versão Atual (Estável):* /api/v1/

Exemplo de chamada:
POST https://localhost:7124/api/v1/CheckIn

---

## ⚙ Configuração e Execução

### Pré-requisitos
* [.NET 9 SDK](https://dotnet.microsoft.com/download) instalado.
* Acesso a uma instância do Oracle Database.

### Passo a Passo

1.  *Clone o repositório:*
    bash
    git clone [https://github.com/seu-usuario/equilibrium.git](https://github.com/seu-usuario/equilibrium.git)
    

2.  *Configure a Connection String:*
    No arquivo EquilibriumAPI/appsettings.json, ajuste a conexão para o seu banco Oracle:
    json
    "ConnectionStrings": {
      "DefaultConnection": "User Id=SEU_USER;Password=SUA_SENHA;Data Source=localhost:1521/xe;Pooling=false;"
    }
    

3.  *Execute as Migrations (Criar Banco):*
    Na raiz da solução, execute o comando para criar as tabelas:
    bash
    dotnet ef database update --project EquilibriumData --startup-project EquilibriumAPI
    

4.  *Rode a Aplicação:*
    bash
    dotnet run --project EquilibriumAPI
    

---

## 📚 Documentação da API

A API possui documentação interativa via *Swagger*.
Após rodar o projeto, acesse no navegador:

👉 **https://localhost:7124/swagger/index.html**

### Resumo dos Endpoints

#### 👤 Usuários
* GET /api/v1/Usuarios - Lista todos os usuários.
* GET /api/v1/Usuarios/{id} - Detalhes e Saldo do usuário.
* POST /api/v1/Usuarios - Cria novo usuário.
* PUT /api/v1/Usuarios/{id} - Atualiza usuário.
* DELETE /api/v1/Usuarios/{id} - Remove usuário.

#### 😄 Check-In (Ganhos)
* POST /api/v1/CheckIn - Registra humor (+10 Pontos).
* GET /api/v1/CheckIn/usuario/{id} - Histórico de check-ins.
* PUT /api/v1/CheckIn/{id} - Edita um check-in.
* DELETE /api/v1/CheckIn/{id} - Remove check-in (Estorna pontos).

---

*Equilibrium* - Transformando autocuidado em valor real.
