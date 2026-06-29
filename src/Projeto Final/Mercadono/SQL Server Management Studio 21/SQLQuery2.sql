-- Usar o banco de dados
USE mercadono;
GO

-- ============================================
-- 1. TABELA PRODUTO
-- ============================================
CREATE TABLE ProdutoTbl (
    idproduto INT PRIMARY KEY IDENTITY(1,1),
    nomepd VARCHAR(100) NOT NULL,
    quantidadepd INT NOT NULL DEFAULT 0,
    precopd DECIMAL(10,2) NOT NULL,
    descontopd DECIMAL(5,2) DEFAULT 0.00
);
GO

-- ============================================
-- 2. TABELA ESTOQUE
-- ============================================
CREATE TABLE estoqueTbl (
    idestoque INT PRIMARY KEY IDENTITY(1,1),
    idproduto INT NOT NULL,
    nomeEt VARCHAR(100) NOT NULL,
    preco_do_produtoEt DECIMAL(10,2) NOT NULL,
    quantidade_estoque INT NOT NULL DEFAULT 0,
    ultima_atualizacao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (idproduto) REFERENCES ProdutoTbl(idproduto) ON DELETE CASCADE
);
GO

-- ============================================
-- 3. TABELA AJUDA AO CLIENTE
-- ============================================
CREATE TABLE ajudaAoClienteTbl (
    idajuda INT IDENTITY(1,1),
    gmail VARCHAR(100) NOT NULL,
    reclamacao TEXT NOT NULL,
    resposta TEXT,
    data_abertura DATETIME DEFAULT GETDATE(),
    data_resposta DATETIME NULL,
    status VARCHAR(20) DEFAULT 'Pendente',
    PRIMARY KEY (idajuda, gmail),
    FOREIGN KEY (gmail) REFERENCES utilizadorTbl(gmail)
);
GO

-- ============================================
-- 4. TABELA COMPRA
-- ============================================
CREATE TABLE compraTbl (
    idcompra INT PRIMARY KEY IDENTITY(1,1),
    idcliente INT NOT NULL,
    id_produto INT NOT NULL,
    quantidade INT NOT NULL,
    valorfinal DECIMAL(10,2) NOT NULL,
    data_compra DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (idcliente) REFERENCES utilizadorTbl(id_cliente),
    FOREIGN KEY (id_produto) REFERENCES ProdutoTbl(idproduto)
);
GO

-- ============================================
-- 5. INSERIR DADOS DE EXEMPLO
-- ============================================

-- Inserir produtos
INSERT INTO ProdutoTbl (nomepd, quantidadepd, precopd, descontopd) VALUES
('Arroz 5kg', 100, 25.90, 0),
('Feijão 1kg', 150, 8.50, 5),
('Açúcar 1kg', 200, 4.99, 0),
('Óleo 900ml', 80, 9.90, 10),
('Macarrão 500g', 120, 4.50, 0),
('Leite 1L', 100, 5.79, 0),
('Café 500g', 60, 12.90, 15),
('Farinha 1kg', 90, 3.99, 0),
('Sabão em pó 1kg', 70, 8.50, 5),
('Detergente 500ml', 150, 2.50, 0);
GO

-- Inserir estoque baseado nos produtos
INSERT INTO estoqueTbl (idproduto, nomeEt, preco_do_produtoEt, quantidade_estoque)
SELECT 
    idproduto, 
    nomepd, 
    precopd, 
    quantidadepd 
FROM ProdutoTbl;
GO

-- Inserir algumas compras de exemplo (assumindo que existem usuários com id 2 e 3)
INSERT INTO compraTbl (idcliente, id_produto, quantidade, valorfinal) VALUES
(2, 1, 2, 51.80),
(2, 3, 3, 14.97),
(3, 5, 1, 4.50),
(3, 7, 2, 25.80);
GO

-- Inserir algumas reclamações de exemplo
INSERT INTO ajudaAoClienteTbl (gmail, reclamacao, status) VALUES
('joao@email.com', 'Produto chegou com defeito', 'Pendente'),
('maria@email.com', 'Atraso na entrega', 'Em andamento');
GO

-- ============================================
-- 6. VERIFICAR DADOS
-- ============================================

-- Ver todos os produtos
SELECT * FROM ProdutoTbl;
GO

-- Ver estoque
SELECT * FROM estoqueTbl;
GO

-- Ver compras com informações detalhadas
SELECT 
    c.idcompra,
    u.nome AS Cliente,
    p.nomepd AS Produto,
    c.quantidade,
    c.valorfinal,
    c.data_compra
FROM compraTbl c
INNER JOIN utilizadorTbl u ON c.idcliente = u.id_cliente
INNER JOIN ProdutoTbl p ON c.id_produto = p.idproduto;
GO

-- Ver reclamações
SELECT * FROM ajudaAoClienteTbl;
GO

-- ============================================
-- 7. CRIAR ÍNDICES PARA PERFORMANCE
-- ============================================

CREATE INDEX idx_compra_cliente ON compraTbl(idcliente);
CREATE INDEX idx_compra_produto ON compraTbl(id_produto);
CREATE INDEX idx_estoque_produto ON estoqueTbl(idproduto);
CREATE INDEX idx_ajuda_cliente ON ajudaAoClienteTbl(gmail);
CREATE INDEX idx_ajuda_status ON ajudaAoClienteTbl(status);
CREATE INDEX idx_produto_nome ON ProdutoTbl(nomepd);
GO

-- ============================================
-- 8. VIEWS PARA CONSULTAS COMUNS
-- ============================================

-- View de compras completa
CREATE VIEW vw_compras_completas AS
SELECT 
    c.idcompra,
    u.nome AS cliente_nome,
    u.gmail AS cliente_email,
    p.nomepd AS produto_nome,
    c.quantidade,
    c.valorfinal,
    c.data_compra
FROM compraTbl c
INNER JOIN utilizadorTbl u ON c.idcliente = u.id_cliente
INNER JOIN ProdutoTbl p ON c.id_produto = p.idproduto;
GO

-- View de estoque atual
CREATE VIEW vw_estoque_atual AS
SELECT 
    e.idestoque,
    e.nomeEt AS produto,
    e.preco_do_produtoEt AS preco,
    e.quantidade_estoque AS quantidade,
    e.ultima_atualizacao,
    p.descontopd
FROM estoqueTbl e
INNER JOIN ProdutoTbl p ON e.idproduto = p.idproduto;
GO

-- View de reclamações pendentes
CREATE VIEW vw_reclamacoes_pendentes AS
SELECT 
    idajuda,
    gmail,
    reclamacao,
    data_abertura,
    status
FROM ajudaAoClienteTbl
WHERE status = 'Pendente'
ORDER BY data_abertura ASC;
GO

-- ============================================
-- 9. PROCEDURES PARA OPERAÇÕES COMUNS
-- ============================================

-- Procedure para adicionar produto ao estoque
CREATE PROCEDURE sp_AdicionarEstoque
    @idproduto INT,
    @quantidade INT
AS
BEGIN
    UPDATE estoqueTbl 
    SET quantidade_estoque = quantidade_estoque + @quantidade,
        ultima_atualizacao = GETDATE()
    WHERE idproduto = @idproduto;
    
    UPDATE ProdutoTbl 
    SET quantidadepd = quantidadepd + @quantidade
    WHERE idproduto = @idproduto;
END
GO

-- Procedure para realizar uma compra
CREATE PROCEDURE sp_RealizarCompra
    @idcliente INT,
    @id_produto INT,
    @quantidade INT
AS
BEGIN
    DECLARE @preco DECIMAL(10,2);
    DECLARE @desconto DECIMAL(5,2);
    DECLARE @valorfinal DECIMAL(10,2);
    
    SELECT @preco = precopd, @desconto = descontopd 
    FROM ProdutoTbl 
    WHERE idproduto = @id_produto;
    
    SET @valorfinal = @preco * @quantidade * (1 - @desconto/100);
    
    INSERT INTO compraTbl (idcliente, id_produto, quantidade, valorfinal)
    VALUES (@idcliente, @id_produto, @quantidade, @valorfinal);
    
    UPDATE estoqueTbl 
    SET quantidade_estoque = quantidade_estoque - @quantidade,
        ultima_atualizacao = GETDATE()
    WHERE idproduto = @id_produto;
    
    UPDATE ProdutoTbl 
    SET quantidadepd = quantidadepd - @quantidade
    WHERE idproduto = @id_produto;
END
GO

-- ============================================
-- 10. VERIFICAR TUDO
-- ============================================

SELECT 'Tabelas criadas com sucesso!' AS Status;
GO

SELECT 
    TABLE_NAME AS 'Tabelas Criadas'
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;
GO