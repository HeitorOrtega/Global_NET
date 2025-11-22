
# GsNet API - Global Solution .NET

API RESTful desenvolvida em .NET 9.0 para gerenciamento de usuários, localizações de trabalho, mensagens e autenticação. Sistema voltado para monitoramento de condições de trabalho e análise de níveis de estresse.

## Tecnologias

- **.NET 9.0** - Framework principal
- **Entity Framework Core 9.0** - ORM
- **Oracle Database** - Banco de dados
- **Swagger/OpenAPI** - Documentação da API
- **API Versioning** - Versionamento de endpoints
- **HealthChecks** - Monitoramento de saúde da aplicação
- **OpenTelemetry** - Observabilidade e tracing
- **Serilog** - Logging estruturado
- **xUnit** - Testes unitários

## Funcionalidades

- CRUD completo de Usuários
- CRUD completo de Localizações de Trabalho
- CRUD completo de Mensagens
- Sistema de Login
- Paginação de resultados
- HATEOAS (Hypermedia as the Engine of Application State)
- Versionamento de API (v1 e v2)
- Health Checks
- Logging estruturado
- Documentação Swagger

## Documentação da API

### Usuários

#### Listar todos os usuários

```http
GET /api/v1/Usuario?page=1&pageSize=10
```

| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `page` | `int` | Número da página (padrão: 1) |
| `pageSize` | `int` | Itens por página (padrão: 10) |

#### Buscar usuário por ID

```http
GET /api/v1/Usuario/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `long` | **Obrigatório**. ID do usuário |

#### Criar usuário

```http
POST /api/v1/Usuario
```

**Body:**
```json
{
  "nome": "string",
  "cpf": "string",
  "email": "string",
  "senha": "string",
  "localizacaoTrabalhoId": 0
}
```

#### Atualizar usuário

```http
PUT /api/v1/Usuario/{id}
```

#### Deletar usuário

```http
DELETE /api/v1/Usuario/{id}
```

### Localização de Trabalho

#### Listar todas as localizações

```http
GET /api/v1/LocalizacaoTrabalho?page=1&pageSize=10
```

#### Buscar localização por ID

```http
GET /api/v1/LocalizacaoTrabalho/{id}
```

#### Criar localização

```http
POST /api/v1/LocalizacaoTrabalho
```

**Body:**
```json
{
  "tipo": "string",
  "grausCelcius": 0,
  "nivelUmidade": 0
}
```

### Mensagens

#### Listar todas as mensagens

```http
GET /api/v1/Mensagem?page=1&pageSize=10
```

#### Buscar mensagem por ID

```http
GET /api/v1/Mensagem/{id}
```

#### Criar mensagem

```http
POST /api/v1/Mensagem
```

**Body:**
```json
{
  "textoMensagem": "string",
  "nivelEstresse": 0,
  "usuarioId": 0
}
```

### Login

#### Listar todos os logins

```http
GET /api/v1/Login?page=1&pageSize=10
```

#### Criar login

```http
POST /api/v1/Login
```

**Body:**
```json
{
  "email": "string",
  "senha": "string"
}
```

## Configuração e Instalação

### Pré-requisitos

- .NET 9.0 SDK

### Configuração do Banco de Dados

1. Configure a connection string no arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=seu_usuario;Password=sua_senha;Data Source=oracle.fiap.com.br:1521/ORCL;"
  }
}
```

2. Execute as migrations:

```bash
dotnet ef database update
```

### Executando o Projeto

```bash
cd GsNet
dotnet restore
dotnet run
```

A API estará disponível em:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`
- Swagger: `https://localhost:5001/swagger`

## Rodando os Testes

```bash
cd GsNet.Tests
dotnet test
```

## Monitoramento

### Health Checks

A aplicação oferece endpoints de monitoramento:

- `/health` - Status geral da aplicação
- `/health/ready` - Verifica se a aplicação está pronta
- `/health/live` - Verifica se a aplicação está ativa
- `/health-ui` - Interface visual dos health checks

## Autores

- **Heitor Ortega** - RM557825
- **Pedro Saraiva**
- **Marcos Lourenço**