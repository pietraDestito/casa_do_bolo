# Casa do Bolo

Sistema de gerenciamento interno de uma loja de bolos (TCC). Controle de pedidos, clientes, sabores e tamanhos, com histórico de vendas. O acesso é restrito à equipe da loja: não existe cadastro público, as contas são criadas pelo administrador.

## Tecnologias

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core 8
- ASP.NET Core Identity
- SQL Server

## Como rodar

### Pré-requisitos

- .NET 8 SDK
- SQL Server (a edição Express serve) com a instância `localhost\SQLEXPRESS`
- Visual Studio 2022 (carga de trabalho *"ASP.NET e desenvolvimento para a Web"*) ou a linha de comando

### Passos

1. Clone o repositório e abra `casaDoBolo.sln`.
2. Se o seu SQL Server tiver outro nome, ajuste `ConnectionStrings:DBBolo` em `casaDoBolo/appsettings.json`.
3. Execute o projeto (`F5`, ou `dotnet run --project casaDoBolo`).
4. Não é preciso rodar `Update-Database`: ao iniciar, o sistema aplica as migrations, cria o banco `Bolo`, os perfis e o primeiro administrador.

## Primeiro acesso

| E-mail | Senha |
|---|---|
| `admin@casadobolo.com` | `Admin@123` |

Troque a senha no primeiro acesso (clique no seu e-mail no topo da página, em **Manage → Password**). Os dados do administrador inicial ficam em `appsettings.json`, na seção `AdminInicial`, e só são usados quando ainda não existe nenhum administrador. Para não deixar a senha no arquivo, use variáveis de ambiente (por exemplo `AdminInicial__Senha`) ou `dotnet user-secrets`.

## Perfis de acesso

| Perfil | Acessa |
|---|---|
| **Administrador(a)** | Tudo: sabores, tamanhos, perfis (roles), criação de usuários, além do que o funcionário acessa |
| **Funcionário(a)** | Pedidos, clientes e vendas |

O administrador cria novas contas em **Novo Usuário** (menu lateral), escolhendo o perfil de cada pessoa.

## Funcionalidades (CRUD)

O sistema cobre as operações de cadastro, consulta, edição e remoção (CRUD) de acordo com o perfil de acesso:

- **Pedidos:** registro, consulta, atualização e acompanhamento de pedidos da loja.
- **Clientes:** cadastro, consulta, edição e gerenciamento dos dados de clientes.
- **Sabores e tamanhos (Administrador(a)):** CRUD de sabores (incluindo upload de imagem salvo no banco) e tamanhos disponíveis.
- **Histórico de vendas:** visualização e acompanhamento das vendas registradas no sistema.
- **Usuários e perfis (Administrador(a)):** criação de contas internas e definição de perfis (`Administrador(a)` e `Funcionário(a)`).

## Observações

- As fotos dos sabores são salvas dentro do banco de dados. Um banco novo começa sem sabores: cadastre-os pela tela **Sabor** (como administrador) e escolha as imagens da pasta `docs/imagens-sabores`, que traz as fotos de exemplo para o upload.
- A senha precisa ter no mínimo 6 caracteres, com letra maiúscula, minúscula, número e símbolo.
