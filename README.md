# Escola de Cursos

## Sistema de Gerenciamento Escolar

---

# README

## Descrição

O **Escola de Cursos** é um sistema web desenvolvido utilizando a plataforma **ASP.NET Core**, com o objetivo de gerenciar todas as atividades relacionadas ao funcionamento de uma escola de cursos.

O sistema permite o cadastro e gerenciamento de alunos, professores, cursos, turmas, categorias, matrículas e usuários, oferecendo uma estrutura organizada para administração acadêmica.

A aplicação foi construída utilizando uma arquitetura em camadas, promovendo baixo acoplamento entre os componentes e facilitando futuras manutenções e expansões do sistema.

---

# Objetivo

O principal objetivo do projeto é centralizar as informações da instituição em um único sistema, permitindo que administradores realizem o controle das atividades acadêmicas de forma simples, segura e organizada.

Entre os principais objetivos estão:

* Gerenciar cursos disponíveis;
* Controlar turmas;
* Cadastrar professores;
* Cadastrar alunos;
* Realizar matrículas;
* Organizar categorias de cursos;
* Gerenciar usuários do sistema;
* Armazenar informações de forma estruturada em banco de dados.

---

# Tecnologias Utilizadas

O projeto foi desenvolvido utilizando as seguintes tecnologias:

* C#
* ASP.NET Core
* Entity Framework Core
* SQL Server (via Entity Framework)
* Razor Pages / MVC
* HTML5
* CSS3
* JavaScript
* Bootstrap
* Visual Studio

---

# Arquitetura do Projeto

O sistema segue uma arquitetura em camadas, dividindo as responsabilidades entre diferentes módulos da aplicação.

Essa organização facilita:

* manutenção do código;
* reutilização de componentes;
* separação das regras de negócio;
* escalabilidade do sistema.

De maneira geral, a arquitetura é composta pelas seguintes camadas:

## Camada de Apresentação

Responsável pela interface com o usuário.

Nesta camada encontram-se:

* Controllers
* Views
* Arquivos estáticos
* Interface gráfica

Sua responsabilidade é receber as requisições dos usuários e apresentar as informações retornadas pelas demais camadas.

---

## Camada de Aplicação

Responsável por coordenar as operações realizadas pelo sistema.

Essa camada recebe as solicitações dos Controllers, executa as regras necessárias e encaminha os resultados para a camada de apresentação.

---

## Camada de Domínio

É considerada o núcleo do sistema.

Nela encontram-se:

* Entidades
* Regras de negócio
* Validações
* Objetos do domínio

Toda a lógica principal da aplicação está concentrada nesta camada.

---

## Camada de Infraestrutura

Responsável pelo acesso aos dados.

Nesta camada encontram-se:

* Contexto do Entity Framework;
* Repositórios;
* Configurações do banco de dados;
* Migrations;
* Persistência das informações.

É a camada responsável pela comunicação entre o sistema e o banco de dados.

---

# Estrutura Geral do Projeto

O projeto encontra-se organizado em diversos diretórios, cada um possuindo uma responsabilidade específica.

Exemplo simplificado:

```text
EscolaDeCursos
│
├── Controllers
├── Models
├── Data
├── Services
├── Repositories
├── Views
├── Migrations
├── wwwroot
├── Program.cs
└── appsettings.json
```

Essa organização segue boas práticas de desenvolvimento utilizadas em aplicações ASP.NET Core.

---

# Características do Sistema

O sistema apresenta diversas características importantes, como:

* Cadastro de informações acadêmicas;
* Organização em módulos;
* Persistência de dados através do Entity Framework Core;
* Interface web;
* Separação entre regras de negócio e acesso aos dados;
* Facilidade de manutenção;
* Código modular;
* Arquitetura escalável.

Essas características tornam o projeto adequado para futuras implementações e expansão de funcionalidades.

# Principais Entidades do Sistema

O sistema foi desenvolvido para controlar os principais processos de uma escola de cursos. Para isso, utiliza diversas entidades responsáveis por representar os dados armazenados no banco de dados.

Cada entidade possui uma responsabilidade específica dentro da aplicação.

---

# Aluno

A entidade **Aluno** representa os estudantes cadastrados na escola.

Ela armazena as informações necessárias para identificação e gerenciamento dos alunos, permitindo sua participação em cursos e turmas.

Entre suas principais responsabilidades estão:

* Cadastro de alunos;
* Atualização dos dados pessoais;
* Consulta de informações;
* Associação com matrículas;
* Acompanhamento da vida acadêmica.

---

# Professor

A entidade **Professor** representa os docentes responsáveis por ministrar os cursos oferecidos pela instituição.

Suas principais funções incluem:

* Cadastro de professores;
* Atualização de informações;
* Associação às turmas;
* Controle dos cursos ministrados.

Essa entidade permite que um mesmo professor possa estar vinculado a diferentes cursos ou turmas, dependendo da regra implementada pela aplicação.

---

# Curso

A entidade **Curso** representa cada curso disponibilizado pela escola.

Nela ficam armazenadas informações como nome, descrição, carga horária e demais características do curso.

Entre suas responsabilidades estão:

* Cadastro de novos cursos;
* Alteração das informações;
* Exclusão de cursos;
* Associação com categorias;
* Associação com turmas.

O curso é uma das principais entidades do sistema, pois organiza toda a estrutura acadêmica.

---

# Categoria

As categorias são utilizadas para organizar os cursos em grupos.

Essa classificação facilita consultas e melhora a organização administrativa do sistema.

Exemplos de categorias:

* Informática
* Idiomas
* Administração
* Marketing
* Design

A utilização de categorias torna o gerenciamento dos cursos mais eficiente.

---

# Turma

A entidade **Turma** representa uma oferta específica de um curso.

Uma turma normalmente possui informações como:

* Curso associado;
* Professor responsável;
* Data de início;
* Data de término;
* Horários;
* Quantidade de vagas.

Essa separação permite que um mesmo curso seja oferecido várias vezes ao longo do tempo.

---

# Matrícula

A matrícula representa o vínculo entre um aluno e uma turma.

Por meio dela, o sistema controla quais alunos estão participando de determinado curso.

Suas principais responsabilidades são:

* Registrar novas matrículas;
* Consultar alunos matriculados;
* Cancelar matrículas;
* Controlar o histórico acadêmico.

Essa entidade é fundamental para o funcionamento do sistema.

---

# Usuários

O sistema também possui uma entidade destinada ao gerenciamento dos usuários responsáveis por acessar a aplicação.

Dependendo da implementação, esses usuários podem possuir diferentes níveis de acesso, permitindo maior segurança no gerenciamento das informações.

Entre suas responsabilidades estão:

* Autenticação;
* Controle de acesso;
* Gerenciamento de permissões;
* Administração do sistema.

---

# Relacionamento entre as Entidades

As entidades trabalham de forma integrada para representar o funcionamento da escola.

De forma simplificada, o relacionamento pode ser descrito da seguinte maneira:

```text id="g4k8s1"
Categoria
     │
     ▼
Curso
     │
     ▼
Turma
     │
     ▼
Matrícula
     ▲
     │
Aluno

Professor ─────────────► Turma

Usuário ───────────────► Sistema
```

Esse relacionamento demonstra como cada módulo depende dos demais para realizar o gerenciamento completo da instituição.

---

# Funcionalidades do Sistema

O sistema oferece diversas funcionalidades administrativas.

Entre as principais destacam-se:

* Cadastro de alunos;
* Cadastro de professores;
* Cadastro de cursos;
* Cadastro de categorias;
* Cadastro de turmas;
* Realização de matrículas;
* Consulta de registros;
* Atualização de informações;
* Exclusão de registros;
* Controle de usuários;
* Persistência dos dados em banco de dados.

Essas funcionalidades atendem às necessidades básicas de gerenciamento de uma escola de cursos.

---

# Organização do Banco de Dados

A persistência das informações é realizada utilizando o **Entity Framework Core**, que faz o mapeamento entre as classes da aplicação e as tabelas do banco de dados.

Essa abordagem oferece vantagens como:

* Redução da escrita manual de SQL;
* Facilidade na manutenção;
* Controle de versões do banco através de *Migrations*;
* Maior integração com o código da aplicação.

Com isso, alterações na estrutura do sistema podem ser propagadas ao banco de dados de maneira organizada e controlada.

# Estrutura do Código

O projeto **Escola de Cursos** foi desenvolvido seguindo uma organização modular, separando responsabilidades entre diferentes componentes da aplicação.

Essa divisão facilita a manutenção do sistema, permitindo que alterações em uma determinada parte do projeto não afetem diretamente os demais módulos.

A estrutura segue o padrão utilizado em aplicações ASP.NET Core, utilizando Controllers para receber requisições, entidades para representar os dados, serviços para regras de negócio e repositórios para comunicação com o banco de dados.

---

# Controllers

Os **Controllers** são responsáveis por receber as requisições realizadas pelo usuário através da interface web.

Eles funcionam como uma ponte entre a camada de apresentação e as demais camadas da aplicação.

Suas principais responsabilidades são:

* Receber requisições HTTP;
* Validar informações recebidas;
* Chamar serviços ou repositórios necessários;
* Retornar páginas ou resultados ao usuário.

Exemplos de operações realizadas pelos Controllers:

* Listagem de registros;
* Criação de novos dados;
* Edição de informações existentes;
* Exclusão de registros.

Um fluxo comum dentro da aplicação ocorre da seguinte forma:

```text
Usuário
   |
   ▼
Controller
   |
   ▼
Service
   |
   ▼
Repository
   |
   ▼
Banco de Dados
```

---

# Models / Entidades

Os **Models** representam as estruturas de dados utilizadas pelo sistema.

Cada classe de modelo corresponde a uma entidade do domínio da aplicação.

Exemplos:

* Aluno;
* Professor;
* Curso;
* Turma;
* Categoria;
* Matrícula.

Essas classes possuem:

* Propriedades;
* Relacionamentos;
* Regras de validação;
* Configurações utilizadas pelo Entity Framework Core.

Exemplo simplificado:

```csharp
public class Curso
{
    public int Id { get; set; }

    public string Nome { get; set; }

    public string Descricao { get; set; }

    public int CategoriaId { get; set; }

    public Categoria Categoria { get; set; }
}
```

Nesse exemplo, a classe representa um curso e mantém uma relação com a entidade Categoria.

---

# Services

A camada de **Services** é responsável por concentrar as regras de negócio do sistema.

Essa camada evita que os Controllers fiquem sobrecarregados com lógica de processamento.

Suas responsabilidades incluem:

* Validar regras do sistema;
* Processar informações;
* Coordenar operações;
* Aplicar regras específicas antes de salvar os dados.

Exemplo de responsabilidade de um serviço:

Ao realizar uma matrícula, o sistema pode verificar:

* Se o aluno já está matriculado;
* Se existem vagas disponíveis;
* Se a turma está ativa;
* Se os dados são válidos.

Essas verificações devem ser realizadas nessa camada.

---

# Repositories

Os **Repositories** são responsáveis pelo acesso aos dados.

Eles fazem a comunicação entre a aplicação e o banco de dados através do Entity Framework Core.

Suas principais funções são:

* Buscar registros;
* Inserir dados;
* Atualizar informações;
* Remover registros.

A utilização de repositórios permite separar a lógica de negócio da lógica de persistência.

Exemplo:

```text
Service
   |
   |
Repository
   |
   |
Entity Framework Core
   |
   |
Banco de Dados
```

Dessa forma, caso o banco de dados seja alterado no futuro, o impacto no restante da aplicação é reduzido.

---

# Data / Contexto do Banco de Dados

O projeto utiliza o **Entity Framework Core** para realizar o gerenciamento da persistência dos dados.

O principal componente dessa comunicação é o contexto do banco de dados, geralmente representado por uma classe derivada de:

```csharp
DbContext
```

Essa classe possui:

* Configuração das tabelas;
* Relacionamentos entre entidades;
* Acesso aos registros;
* Controle das operações no banco.

Exemplo:

```csharp
public class ApplicationDbContext : DbContext
{
    public DbSet<Aluno> Alunos { get; set; }

    public DbSet<Curso> Cursos { get; set; }

    public DbSet<Turma> Turmas { get; set; }
}
```

---

# Migrations

As **Migrations** são utilizadas pelo Entity Framework Core para controlar alterações na estrutura do banco de dados.

Elas permitem:

* Criar tabelas automaticamente;
* Alterar estruturas existentes;
* Adicionar novos campos;
* Controlar versões do banco.

Exemplo de fluxo:

```text
Alteração na Entidade
          |
          ▼
Criar Migration
          |
          ▼
Atualizar Banco de Dados
```

Isso facilita a evolução do sistema durante o desenvolvimento.

---

# Views

As **Views** são responsáveis pela interface visual apresentada ao usuário.

Elas utilizam normalmente:

* HTML;
* CSS;
* JavaScript;
* Razor Pages.

As Views recebem os dados enviados pelos Controllers e apresentam as informações de forma organizada.

Exemplos:

* Tela de cadastro de alunos;
* Lista de cursos;
* Formulário de matrícula;
* Página de gerenciamento de turmas.

---

# Arquivos Principais

## Program.cs

O arquivo `Program.cs` é responsável pela inicialização da aplicação.

Nele são configurados:

* Serviços utilizados pelo sistema;
* Injeção de dependência;
* Banco de dados;
* Middleware;
* Rotas da aplicação.

Ele define como o sistema será executado.

---

## appsettings.json

O arquivo `appsettings.json` armazena configurações da aplicação.

Normalmente contém informações como:

* String de conexão do banco;
* Configurações de ambiente;
* Parâmetros gerais do sistema.

Exemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "string_de_conexao"
  }
}
```

---

# Injeção de Dependência

O projeto utiliza o mecanismo de **Dependency Injection** disponibilizado pelo ASP.NET Core.

Esse recurso permite que as dependências das classes sejam fornecidas automaticamente pelo framework.

Benefícios:

* Menor acoplamento;
* Código mais organizado;
* Facilidade de testes;
* Melhor manutenção.

Exemplo:

```csharp
public class CursoController : Controller
{
    private readonly ICursoService _service;

    public CursoController(ICursoService service)
    {
        _service = service;
    }
}
```

Nesse exemplo, o Controller recebe automaticamente o serviço necessário para executar suas operações.

---

# Fluxo Geral da Aplicação

O funcionamento geral do sistema segue o seguinte fluxo:

```text
1. Usuário acessa uma página do sistema

            ↓

2. Controller recebe a solicitação

            ↓

3. Service executa as regras de negócio

            ↓

4. Repository realiza o acesso aos dados

            ↓

5. Entity Framework comunica-se com o banco

            ↓

6. Resultado retorna para a interface

            ↓

7. Usuário visualiza a resposta
```

Esse modelo garante uma aplicação organizada, seguindo boas práticas de desenvolvimento de software.

# Instalação e Execução do Projeto

Para executar o sistema **Escola de Cursos**, é necessário possuir o ambiente de desenvolvimento configurado com as ferramentas necessárias para aplicações ASP.NET Core.

---

# Pré-requisitos

Antes de executar o projeto, é necessário instalar:

* .NET SDK compatível com a versão utilizada no projeto;
* Visual Studio ou outra IDE compatível com desenvolvimento em C#;
* SQL Server ou outro banco de dados configurado;
* Entity Framework Core Tools.

Para verificar a instalação do .NET SDK, pode ser utilizado o comando:

```bash
dotnet --version
```

---

# Clonando o Projeto

Primeiramente, o projeto deve ser obtido através do repositório ou arquivo disponibilizado.

Exemplo utilizando Git:

```bash
git clone URL_DO_REPOSITORIO
```

Após o download, acessar o diretório principal do projeto:

```bash
cd EscolaDeCursos
```

---

# Restauração das Dependências

O projeto utiliza pacotes do ecossistema .NET que precisam ser restaurados antes da execução.

O comando responsável por realizar essa etapa é:

```bash
dotnet restore
```

Esse processo instala todas as dependências definidas no arquivo do projeto.

---

# Configuração do Banco de Dados

A aplicação utiliza o **Entity Framework Core** para realizar a comunicação com o banco de dados.

A configuração da conexão normalmente está localizada no arquivo:

```text
appsettings.json
```

Exemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EscolaCursos;Trusted_Connection=True;"
  }
}
```

Essa configuração define:

* Servidor do banco de dados;
* Nome da base utilizada;
* Método de autenticação;
* Parâmetros de conexão.

Antes de iniciar a aplicação, é necessário garantir que o banco de dados esteja disponível.

---

# Criação e Atualização do Banco de Dados

Como o projeto utiliza Entity Framework Core, a estrutura do banco pode ser criada através das migrations.

Para aplicar as alterações existentes:

```bash
dotnet ef database update
```

Esse comando:

* Cria o banco caso ele ainda não exista;
* Cria as tabelas;
* Aplica alterações registradas nas migrations.

---

# Comandos do Entity Framework Core

Durante o desenvolvimento, alguns comandos importantes podem ser utilizados.

## Criar uma nova Migration

Quando uma entidade é alterada ou uma nova tabela é adicionada:

```bash
dotnet ef migrations add NomeDaAlteracao
```

Exemplo:

```bash
dotnet ef migrations add CriarTabelaCursos
```

---

## Atualizar o Banco de Dados

Após criar uma migration:

```bash
dotnet ef database update
```

---

## Remover a última Migration

Caso seja necessário desfazer uma alteração:

```bash
dotnet ef migrations remove
```

---

# Executando a Aplicação

Após configurar o ambiente e o banco de dados, a aplicação pode ser iniciada utilizando:

```bash
dotnet run
```

Ou através da IDE:

1. Abrir o projeto no Visual Studio;
2. Restaurar os pacotes;
3. Configurar o banco de dados;
4. Executar utilizando o botão **Start/Run**.

Após iniciar, o sistema ficará disponível através do endereço informado no console da aplicação.

Exemplo:

```text
https://localhost:5001
```

---

# Exemplo de Utilização do Sistema

O fluxo básico de utilização da aplicação ocorre da seguinte maneira:

## 1. Acesso ao Sistema

O usuário acessa a aplicação através do navegador.

---

## 2. Gerenciamento de Cursos

O administrador pode:

* Visualizar cursos cadastrados;
* Adicionar novos cursos;
* Editar informações;
* Remover registros.

---

## 3. Cadastro de Professores

O sistema permite cadastrar professores responsáveis pelas disciplinas oferecidas.

Informações como nome e dados de contato podem ser armazenadas.

---

## 4. Cadastro de Alunos

Os alunos podem ser registrados no sistema para posteriormente participarem das turmas disponíveis.

---

## 5. Criação de Turmas

Uma turma relaciona:

* Um curso;
* Um professor;
* Informações de período e funcionamento.

---

## 6. Realização de Matrículas

Após a criação das turmas, os alunos podem ser associados através do processo de matrícula.

Esse relacionamento permite controlar quais estudantes participam de cada curso.

---

# Segurança e Organização do Sistema

A arquitetura utilizada proporciona diversos benefícios relacionados à segurança e manutenção.

Entre eles:

* Separação entre interface e regras de negócio;
* Controle centralizado das operações;
* Menor exposição direta ao banco de dados;
* Facilidade para implementação de autenticação e autorização;
* Organização do código em módulos independentes.

---

# Possíveis Melhorias Futuras

Apesar de possuir uma estrutura organizada, algumas melhorias podem ser implementadas futuramente.

## Autenticação e Autorização Avançada

Adicionar:

* Login com Identity;
* Controle de permissões;
* Diferentes níveis de usuário.

Exemplo:

* Administrador;
* Professor;
* Aluno.

---

## API REST

Criar uma API separada permitiria:

* Integração com aplicativos móveis;
* Consumo por sistemas externos;
* Maior flexibilidade na comunicação.

---

## Dashboard Administrativo

Adicionar uma área com indicadores como:

* Quantidade de alunos cadastrados;
* Cursos ativos;
* Número de matrículas;
* Desempenho dos estudantes.

---

## Melhorias na Interface

Possíveis melhorias:

* Design responsivo;
* Melhor experiência de usuário;
* Componentes visuais mais modernos;
* Melhor organização das telas.

---

## Testes Automatizados

A implementação de testes unitários e testes de integração aumentaria a confiabilidade do sistema.

Exemplos:

* Testes dos serviços;
* Testes dos repositórios;
* Validação das regras de negócio.

---

# Conclusão

O projeto **Escola de Cursos** apresenta uma solução organizada para gerenciamento de uma instituição de ensino, utilizando tecnologias modernas do ecossistema .NET.

A utilização de uma arquitetura baseada em camadas permite uma melhor separação das responsabilidades, tornando o sistema mais fácil de compreender, manter e expandir.

A aplicação demonstra conceitos importantes do desenvolvimento de software, como:

* Programação orientada a objetos;
* Separação de responsabilidades;
* Entity Framework Core;
* Persistência de dados;
* Injeção de dependência;
* Organização modular.

Com futuras melhorias, o sistema pode evoluir para uma plataforma completa de gerenciamento acadêmico, atendendo diferentes perfis de usuários e possibilitando novas integrações.

---

# Autor

**Gustavo Tessaro e Alec Luí**

Projeto desenvolvido como prática de desenvolvimento de aplicações web utilizando **ASP.NET Core MVC**, aplicando conceitos de arquitetura em camadas, boas práticas de programação e padrões modernos de desenvolvimento.

Caso tenha gostado do projeto, deixe uma ⭐ no repositório.

---

# Licença

Este projeto foi desenvolvido para fins acadêmicos e de estudo.

Sinta-se à vontade para utilizá-lo como referência, respeitando os créditos ao autor.

---

<div align="center">

## ⭐ Se este projeto foi útil para você, considere deixar uma estrela no repositório!

</div>
