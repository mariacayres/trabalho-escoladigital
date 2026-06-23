/*
   Autor: maria cayres
  */
IF DB_ID('mercearia_expandida') IS NOT NULL
BEGIN
    ALTER DATABASE mercearia_expandida SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE mercearia_expandida;
END
GO
CREATE DATABASE mercearia_expandida;
GO

USE mercearia_expandida;
GO


IF OBJECT_ID('Detalhes_venda', 'U') IS NOT NULL DROP TABLE Detalhes_venda;
IF OBJECT_ID('Vendas', 'U') IS NOT NULL DROP TABLE Vendas;
IF OBJECT_ID('Produtos', 'U') IS NOT NULL DROP TABLE Produtos;
IF OBJECT_ID('Tipos_produtos', 'U') IS NOT NULL DROP TABLE Tipos_produtos;
GO

CREATE TABLE Tipos_produtos(
    id_tipo_produto INT IDENTITY(1,1) PRIMARY KEY,
    descricao NVARCHAR(100) NOT NULL, 
    IVA TINYINT NOT NULL              
);

CREATE TABLE Produtos(
    id_produto INT IDENTITY(1,1) PRIMARY KEY,
    nome_produto NVARCHAR(120) NOT NULL,  
    preco DECIMAL(10,2) NOT NULL,          
    id_tipo_produto INT NOT NULL
        CONSTRAINT FK_Produtos_Tipo REFERENCES Tipos_produtos(id_tipo_produto)
);

CREATE TABLE Vendas(
    id_venda INT IDENTITY(1,1) PRIMARY KEY,
    data DATE NOT NULL                    
);


CREATE TABLE Detalhes_venda(
    id_venda INT NOT NULL,               
    id_produto INT NOT NULL,              
    quantidade INT NOT NULL,              
    CONSTRAINT PK_Detalhes PRIMARY KEY (id_venda, id_produto),
    CONSTRAINT FK_Detalhes_Vendas FOREIGN KEY (id_venda)
        REFERENCES Vendas(id_venda) ON DELETE CASCADE,
    CONSTRAINT FK_Detalhes_Produtos FOREIGN KEY (id_produto)
        REFERENCES Produtos(id_produto)
);
GO


INSERT INTO Tipos_produtos (descricao, IVA) VALUES
('Alimentar', 6),               -- 6% para bens alimentares essenciais
('Bebida não alcoólica', 13),   -- 13% para sumos, refrigerantes, etc.
('Bebida alcoólica', 23),       -- 23% para vinhos, cervejas, licores
('Outros', 23);                 -- 23% para detergentes, papel, etc.


DECLARE @base TABLE(
    nome_base NVARCHAR(80),
    categoria NVARCHAR(10),     -- ALIM / BNA / BALC / OUT
    preco_base DECIMAL(10,2)
);

INSERT INTO @base (nome_base,categoria,preco_base) VALUES
('Arroz Agulha', 'ALIM', 1.20),
('Arroz Carolino', 'ALIM', 1.35),
('Massa Esparguete', 'ALIM', 0.95),
('Massa Penne', 'ALIM', 1.05),
('Azeite Virgem Extra', 'ALIM', 5.20),
('Óleo de Girassol', 'ALIM', 2.10),
('Atum em Azeite', 'ALIM', 1.10),
('Feijão Encarnado', 'ALIM', 0.90),
('Grão-de-bico', 'ALIM', 0.85),
('Leite Meio-gordo', 'ALIM', 0.79),
('Iogurte Natural', 'ALIM', 0.50),
('Pão de Forma', 'ALIM', 1.40),
('Bolacha Maria', 'ALIM', 1.15),
('Açúcar Branco', 'ALIM', 1.00),
('Sal Fino', 'ALIM', 0.60),
('Farinha Trigo T55', 'ALIM', 0.80),
('Café Moído', 'ALIM', 3.80),
('Chá Preto', 'ALIM', 1.90),
('Chocolate de Leite', 'ALIM', 1.20),
('Manteiga', 'ALIM', 1.50),
('Queijo Flamengo', 'ALIM', 2.40),
('Fiambre da Perna', 'ALIM', 2.20),

('Água Mineral', 'BNA', 0.30),
('Refrigerante Cola', 'BNA', 1.20),
('Refrigerante Laranja', 'BNA', 1.20),
('Sumo Laranja', 'BNA', 1.10),
('Ice Tea Pêssego', 'BNA', 1.05),

('Cerveja Lager', 'BALC', 0.90),
('Cerveja IPA', 'BALC', 1.30),
('Vinho Tinto', 'BALC', 3.50),
('Vinho Branco', 'BALC', 3.20),
('Vinho Verde', 'BALC', 2.80),
('Licor Beirão', 'BALC', 12.00),

('Detergente Loiça', 'OUT', 1.80),
('Detergente Roupa', 'OUT', 6.50),
('Amaciador Roupa', 'OUT', 3.80),
('Papel Higiénico', 'OUT', 3.90),
('Guardanapos', 'OUT', 1.10),
('Toalhitas', 'OUT', 1.70);

DECLARE @tamanhos TABLE(sufixo NVARCHAR(40), fator DECIMAL(10,2));
INSERT INTO @tamanhos VALUES
('250 g', 0.6), ('500 g', 1.0), ('1 kg', 1.9),
('0,5 L', 0.6), ('1 L', 1.0), ('1,5 L', 1.4);

DECLARE @sizesBALC TABLE(sufixo NVARCHAR(40), fator DECIMAL(10,2));
INSERT INTO @sizesBALC VALUES
('0,33 L', 0.7), ('0,5 L', 1.0), ('0,75 L', 1.5);

WITH Mapa AS (
    SELECT 'ALIM' AS cat, id_tipo_produto FROM Tipos_produtos WHERE descricao='Alimentar'
    UNION ALL SELECT 'BNA', id_tipo_produto FROM Tipos_produtos WHERE descricao='Bebida não alcoólica'
    UNION ALL SELECT 'BALC', id_tipo_produto FROM Tipos_produtos WHERE descricao='Bebida alcoólica'
    UNION ALL SELECT 'OUT', id_tipo_produto FROM Tipos_produtos WHERE descricao='Outros'
),
Gerados AS (
    SELECT TOP (140)
        b.nome_base + ' ' + t.sufixo AS nome_produto,
        ROUND(b.preco_base * t.fator, 2) AS preco,
        m.id_tipo_produto
    FROM @base b
    JOIN Mapa m ON m.cat = b.categoria
    CROSS JOIN (
        SELECT * FROM @tamanhos
        UNION ALL
        SELECT * FROM @sizesBALC
    ) t
    WHERE (b.categoria <> 'BALC' AND t.sufixo IN (SELECT sufixo FROM @tamanhos))
       OR (b.categoria = 'BALC' AND t.sufixo IN (SELECT sufixo FROM @sizesBALC))
)
INSERT INTO Produtos (nome_produto, preco, id_tipo_produto)
SELECT nome_produto, preco, id_tipo_produto
FROM Gerados;

SELECT COUNT(*) AS total_produtos FROM Produtos;


DECLARE @n INT=1, @dataInicial DATE='2025-09-01';

WHILE @n <= 50
BEGIN
    INSERT INTO Vendas (data) VALUES (DATEADD(DAY, @n-1, @dataInicial));
    DECLARE @idVenda INT = SCOPE_IDENTITY();

    DECLARE @linhas INT = (ABS(CHECKSUM(NEWID())) % 5) + 2;

    ;WITH rnd AS (
        SELECT TOP (@linhas)
               id_produto,
               (ABS(CHECKSUM(NEWID())) % 5) + 1 AS quantidade
        FROM Produtos
        ORDER BY NEWID()
    )
    INSERT INTO Detalhes_venda (id_venda, id_produto, quantidade)
    SELECT @idVenda, id_produto, quantidade FROM rnd;

    SET @n += 1;
END;
GO

IF OBJECT_ID('vw_TotalPorVenda','V') IS NOT NULL DROP VIEW vw_TotalPorVenda;
GO
CREATE VIEW vw_TotalPorVenda AS
SELECT 
    v.id_venda,
    v.data,
    CAST(ROUND(SUM(d.quantidade * p.preco), 2) AS DECIMAL(10,2)) AS Subtotal,
    CAST(ROUND(SUM(d.quantidade * p.preco * (1 + tp.IVA/100.0)), 2) AS DECIMAL(10,2)) AS Total
FROM Vendas v
JOIN Detalhes_venda d ON d.id_venda = v.id_venda
JOIN Produtos p ON p.id_produto = d.id_produto
JOIN Tipos_produtos tp ON tp.id_tipo_produto = p.id_tipo_produto
GROUP BY v.id_venda, v.data;
GO

SELECT TOP 10 * FROM Produtos ORDER BY id_produto;
SELECT TOP 10 * FROM Vendas ORDER BY id_venda;
SELECT TOP 10 * FROM Detalhes_venda ORDER BY id_venda;
SELECT TOP 10 * FROM vw_TotalPorVenda ORDER BY id_venda;
GO


PRINT '=== 1. TOTAL DE VENDAS POR DIA ==='
SELECT 
    data,
    COUNT(*) AS numero_vendas,
    CAST(ROUND(SUM(Total), 2) AS DECIMAL(10,2)) AS total_dia,
    CAST(ROUND(AVG(Total), 2) AS DECIMAL(10,2)) AS media_por_venda,
    CAST(ROUND(MAX(Total), 2) AS DECIMAL(10,2)) AS venda_mais_alta,
    CAST(ROUND(MIN(Total), 2) AS DECIMAL(10,2)) AS venda_mais_baixa
FROM vw_TotalPorVenda
GROUP BY data
ORDER BY data;
GO


PRINT '=== 2. VALOR MÉDIO, MÁXIMO E MÍNIMO DAS VENDAS ==='
SELECT 
    CAST(ROUND(AVG(Total), 2) AS DECIMAL(10,2)) AS valor_medio_vendas,
    CAST(ROUND(MAX(Total), 2) AS DECIMAL(10,2)) AS valor_maximo_venda,
    CAST(ROUND(MIN(Total), 2) AS DECIMAL(10,2)) AS valor_minimo_venda
FROM vw_TotalPorVenda;
GO


PRINT '=== 3. PRODUTOS DIFERENTES POR VENDA ==='
SELECT 
    v.id_venda,
    v.data,
    COUNT(DISTINCT dv.id_produto) AS produtos_diferentes,
    tv.Total AS total_venda
FROM Vendas v
JOIN Detalhes_venda dv ON v.id_venda = dv.id_venda
JOIN vw_TotalPorVenda tv ON v.id_venda = tv.id_venda
GROUP BY v.id_venda, v.data, tv.Total
ORDER BY produtos_diferentes DESC, v.id_venda;
GO


PRINT '=== 4. DIAS COM TOTAL DE VENDAS SUPERIOR A 20€ ==='
SELECT 
    data,
    COUNT(*) AS numero_vendas,
    CAST(ROUND(SUM(Total), 2) AS DECIMAL(10,2)) AS total_dia
FROM vw_TotalPorVenda
GROUP BY data
HAVING SUM(Total) > 20
ORDER BY total_dia DESC;
GO


PRINT '=== 5. ESTATÍSTICAS COMPLETAS DAS VENDAS ==='
SELECT 
    COUNT(*) AS numero_vendas,
    CAST(ROUND(SUM(Total), 2) AS DECIMAL(10,2)) AS total_global,
    CAST(ROUND(AVG(Total), 2) AS DECIMAL(10,2)) AS media_por_venda,
    CAST(ROUND(MAX(Total), 2) AS DECIMAL(10,2)) AS venda_mais_alta,
    CAST(ROUND(MIN(Total), 2) AS DECIMAL(10,2)) AS venda_mais_baixa,
    SUM(quantidade_total) AS total_produtos_vendidos,
    CAST(ROUND(AVG(produtos_diferentes), 2) AS DECIMAL(5,2)) AS media_produtos_diferentes_por_venda
FROM (
    SELECT 
        tv.id_venda,
        tv.Total,
        (SELECT SUM(quantidade) FROM Detalhes_venda WHERE id_venda = tv.id_venda) AS quantidade_total,
        (SELECT COUNT(DISTINCT id_produto) FROM Detalhes_venda WHERE id_venda = tv.id_venda) AS produtos_diferentes
    FROM vw_TotalPorVenda tv
) AS detalhes;
GO


PRINT '=== 6. VENDAS POR TIPO DE PRODUTO ==='
SELECT 
    tp.descricao AS tipo_produto,
    tp.IVA,
    COUNT(DISTINCT dv.id_venda) AS vendas_com_este_tipo,
    SUM(dv.quantidade) AS quantidade_vendida,
    CAST(ROUND(SUM(dv.quantidade * p.preco), 2) AS DECIMAL(10,2)) AS subtotal,
    CAST(ROUND(SUM(dv.quantidade * p.preco * (1 + tp.IVA/100.0)), 2) AS DECIMAL(10,2)) AS total_com_IVA
FROM Detalhes_venda dv
JOIN Produtos p ON dv.id_produto = p.id_produto
JOIN Tipos_produtos tp ON p.id_tipo_produto = tp.id_tipo_produto
GROUP BY tp.descricao, tp.IVA
ORDER BY total_com_IVA DESC;
GO