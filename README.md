#  MERCADONO - DOCUMENTAÇÃO DO CÓDIGO

##  VISÃO GERAL

O projeto **Mercadono** é um sistema de gestão de supermercado desenvolvido em **C# Windows Forms** com **SQL Server LocalDB**. Permite a gestão de produtos, compras, estoque e utilizadores.

---

##  ESTRUTURA DO PROJETO

```
Mercadono/
├── Program.cs                    # Ponto de entrada da aplicação
├── Session.cs                    # Gestão de sessão do utilizador
│
├── 📱 FORMS PRINCIPAIS
│   ├── Form1.cs                  # Registo de novos utilizadores
│   ├── Login.cs                  # Autenticação de utilizadores
│   ├── interface_principal.cs    # Interface principal do utilizador
│   └── Form2.cs                  # Painel do Administrador (Gestão de Produtos)
│
├── 📊 FORMS DE GESTÃO
│   ├── compras.cs                # Gestão de Compras
│   ├── estoque.cs                # Gestão de Estoque
│   └── utilizadores.cs           # Gestão de Utilizadores
│
└── 🗄️ BANCO DE DADOS (SQL Server LocalDB)
    ├── utilizadorTbl             # Utilizadores (clientes e admin)
    ├── ProdutoTbl                # Produtos
    ├── estoqueTbl                # Estoque
    └── compraTbl                 # Compras
```

---

## 🔐 . SESSION.CS

**Classe estática para armazenar os dados do utilizador logado.**

```csharp
public static class Session
{
    public static int LoggedUserId { get; set; }
    public static string LoggedUserName { get; set; }
    public static string LoggedUserEmail { get; set; }
    public static bool IsAdmin { get; set; }
}
```

**Funcionalidade:** Mantém os dados do utilizador durante toda a sessão.

---

##  2. LOGIN.CS

**Formulário de autenticação de utilizadores.**

### Funcionalidades:
- ✅ Login com email e senha
- ✅ Verificação de administrador (`admin@admin.com` / `admin123`)
- ✅ Redirecionamento para:
  - `Form2` (se for administrador)
  - `interface_principal` (se for utilizador normal)
- ✅ Link para registo (`Form1`)

### Principais Métodos:
| Método | Descrição |
|--------|-----------|
| `button2_Click_Login()` | Valida as credenciais e redireciona |
| `button1_Click_OpenRegister()` | Abre o formulário de registo |

---

##  3. FORM1.CS

**Formulário de registo de novos utilizadores.**

### Funcionalidades:
- ✅ Validação de campos (nome, email, senha)
- ✅ Verificação de email duplicado
- ✅ Criação de conta na base de dados
- ✅ Redirecionamento para a interface principal após registo
- ✅ Verificação de administrador (se for `admin@admin.com`)

### Validações:
| Campo | Regra |
|-------|-------|
| Nome | Mínimo 3 caracteres |
| Email | Deve conter "@" |
| Senha | Mínimo 6 caracteres |

---

##  4. INTERFACE_PRINCIPAL.CS

**Interface principal do utilizador comum.**

### Funcionalidades:
- ✅ Lista de produtos disponíveis com CheckBox
- ✅ Cálculo automático do total
- ✅ Compra de produtos selecionados
- ✅ Atualização automática do estoque
- ✅ Navegação entre forms

### Botões de Navegação:
| Botão | Destino |
|-------|---------|
| **button1** | Form2 (Produtos) |
| **button2** | compras.cs |
| **button3** | estoque.cs |
| **button4** | utilizadores.cs |
| **button5** | Ignorado |

---

##  5. FORM2.CS - GESTÃO DE PRODUTOS

**Painel do Administrador para gestão de produtos.**

### Funcionalidades:
- ✅ Lista de produtos (ID, Nome, Preço, Desconto, Quantidade)
- ✅ Visualização de detalhes ao clicar
- ✅ Atualização de produtos (button1)
- ✅ Navegação entre forms

### Base de Dados:
| Tabela | Colunas |
|--------|---------|
| **ProdutoTbl** | idproduto, nomepd, quantidadepd, precopd, descontopd |

---

##  6. COMPRAS.CS

**Gestão de compras realizadas.**

### Funcionalidades:
- ✅ Lista de compras (ID, Cliente, Produto, Qtd, Valor Total, Data)
- ✅ Visualização de detalhes ao clicar
- ✅ Atualização da lista (button2)
- ✅ Navegação entre forms

### Base de Dados:
| Tabelas | Relação |
|---------|---------|
| **compraTbl** | idcompra, idcliente, id_produto, quantidade, valorfinal, data_compra |
| **utilizadorTbl** | INNER JOIN para obter nome do cliente |
| **ProdutoTbl** | INNER JOIN para obter nome do produto |

---

##  7. ESTOQUE.CS

**Gestão do estoque de produtos.**

### Funcionalidades:
- ✅ Lista de estoque (ID, Produto, Quantidade)
- ✅ Visualização de detalhes ao clicar
- ✅ Atualização do estoque (button3)
- ✅ Navegação entre forms

### Base de Dados:
| Tabelas | Relação |
|---------|---------|
| **estoqueTbl** | idestoque, idproduto, quantidade_estoque |
| **ProdutoTbl** | INNER JOIN para obter nome do produto |

---

##  8. UTILIZADORES.CS

**Gestão de utilizadores (pendente de implementação).**

### Funcionalidades planeadas:
- [ ] Lista de utilizadores
- [ ] Edição de utilizadores
- [ ] Eliminação de utilizadores
- [ ] Promoção a administrador

---

## 🗄️ 9. BASE DE DADOS

### Estrutura das Tabelas:

#### utilizadorTbl
| Coluna | Tipo | Descrição |
|--------|------|-----------|
| id_cliente | INT (PK) | ID do utilizador |
| nome | VARCHAR(100) | Nome do utilizador |
| senha | VARCHAR(255) | Senha do utilizador |
| gmail | VARCHAR(100) (UNIQUE) | Email do utilizador |
| dinheiro | DECIMAL(10,2) | Saldo do utilizador |
| is_admin | INT | 1=Admin, 0=Utilizador |

#### ProdutoTbl
| Coluna | Tipo | Descrição |
|--------|------|-----------|
| idproduto | INT (PK) | ID do produto |
| nomepd | VARCHAR(100) | Nome do produto |
| quantidadepd | INT | Quantidade em stock |
| precopd | DECIMAL(10,2) | Preço do produto |
| descontopd | DECIMAL(5,2) | Desconto em % |

#### estoqueTbl
| Coluna | Tipo | Descrição |
|--------|------|-----------|
| idestoque | INT (PK) | ID do registo de estoque |
| idproduto | INT (FK) | ID do produto |
| quantidade_estoque | INT | Quantidade em estoque |
| preco_do_produtoEt | DECIMAL(10,2) | Preço no estoque |
| ultima_atualizacao | DATETIME | Data da última atualização |

#### compraTbl
| Coluna | Tipo | Descrição |
|--------|------|-----------|
| idcompra | INT (PK) | ID da compra |
| idcliente | INT (FK) | ID do cliente |
| id_produto | INT (FK) | ID do produto |
| quantidade | INT | Quantidade comprada |
| valorfinal | DECIMAL(10,2) | Valor total da compra |
| data_compra | DATETIME | Data da compra |

### Relações entre Tabelas:

```
utilizadorTbl (1) ────── (N) compraTbl
       │                      │
       │                      │
       ▼                      ▼
  (1:N)                 ProdutoTbl (1) ────── (N) compraTbl
       │                      │
       │                      │
       ▼                      ▼
  (1:1)                 estoqueTbl (1) ────── (1) ProdutoTbl
```

---

## 10. CONFIGURAÇÃO

### Connection String:
```csharp
@"Server=(localdb)\MSSQLLocalDB;Database=mercadono;Integrated Security=True;"
```

### Credenciais de Teste:
| Tipo | Email | Senha |
|------|-------|-------|
| **Administrador** | `admin@admin.com` | `admin123` |
| **Utilizador** | `joao@email.com` | `123456` |

---

##  11. RESUMO DOS BOTÕES

| Form | button1 | button2 | button3 | button4 | button5 |
|------|---------|---------|---------|---------|---------|
| **interface_principal** | Produtos (Form2) | Compras | Estoque | Utilizadores | Ignorado |
| **Form2** | Atualizar Produtos | Compras | Estoque | Utilizadores | Ignorado |
| **compras** | Produtos | Atualizar Compras | Estoque | Utilizadores | Ignorado |
| **estoque** | Produtos | Compras | Atualizar Estoque | Utilizadores | Ignorado |
| **utilizadores** | Produtos | Compras | Estoque | Atualizar Utilizadores | Ignorado |

---

##  12. FLUXO DA APLICAÇÃO

```
1. Login / Registo
   ↓
2. Interface Principal (utilizador) OU Painel Admin (Form2)
   ↓
3. Navegação entre forms de gestão
   ↓
4. Operações CRUD nas tabelas
```

---

##  13. ESTADO DO PROJETO

| Componente | Estado |
|------------|--------|
| Login | ✅ Completo |
| Registo | ✅ Completo |
| Interface Principal | ✅ Completo |
| Gestão de Produtos | ✅ Completo |
| Gestão de Compras | ✅ Completo |
| Gestão de Estoque | ✅ Completo |
| Gestão de Utilizadores | ⚠️ Pendente |
| Base de Dados | ✅ Completa |
