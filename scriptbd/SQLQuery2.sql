-- ============================================================
-- ELIMINAR PRODUTOS DUPLICADOS (MANTER SÓ 1 DE CADA)
-- ============================================================

USE mercadono;
GO

-- ============================================================
-- 1. VER OS PRODUTOS DUPLICADOS
-- ============================================================
SELECT * FROM ProdutoTbl ORDER BY nomepd, idproduto;
GO

-- ============================================================
-- 2. APAGAR PRODUTOS DUPLICADOS (MANTER O MAIS ANTIGO - ID MENOR)
-- ============================================================
-- Apagar produtos com ID > 100 (os duplicados)
DELETE FROM ProdutoTbl WHERE idproduto > 100;
GO

-- ============================================================
-- 3. APAGAR TAMBÉM DO ESTOQUE
-- ============================================================
DELETE FROM estoqueTbl WHERE idproduto > 100;
GO

-- ============================================================
-- 4. VERIFICAR RESULTADO
-- ============================================================
SELECT '=== PRODUTOS (SEM DUPLICADOS) ===' AS '';
SELECT * FROM ProdutoTbl ORDER BY idproduto;
GO

SELECT '=== ESTOQUE (SEM DUPLICADOS) ===' AS '';
SELECT * FROM estoqueTbl ORDER BY idproduto;
GO

-- ============================================================
-- 5. CONTAR QUANTOS PRODUTOS FICARAM
-- ============================================================
SELECT COUNT(*) AS 'Total de Produtos' FROM ProdutoTbl;
GO

-- ============================================================
-- 6. RESETAR O IDENTITY
-- ============================================================
DBCC CHECKIDENT ('ProdutoTbl', RESEED, 10);
GO

-- ============================================================
-- 7. RESULTADO FINAL
-- ============================================================
PRINT ' ';
PRINT '═══════════════════════════════════════════════════════════';
PRINT '✅ PRODUTOS DUPLICADOS REMOVIDOS!';
PRINT '═══════════════════════════════════════════════════════════';
PRINT ' ';
PRINT '📋 PRODUTOS RESTANTES:';
PRINT ' 1 - Arroz 5kg';
PRINT ' 2 - Feijão 1kg';
PRINT ' 3 - Açúcar 1kg';
PRINT ' 4 - Óleo 900ml';
PRINT ' 5 - Macarrão 500g';
PRINT ' 6 - Leite 1L';
PRINT ' 7 - Café 500g';
PRINT ' 8 - Farinha 1kg';
PRINT ' 9 - Sabão em pó 1kg';
PRINT '10 - Detergente 500ml';
PRINT ' ';
PRINT '✅ AGORA SÃO 10 PRODUTOS, SEM DUPLICADOS!';
GO





USE mercadono;
GO

-- ============================================================
-- VER OS DADOS DUPLICADOS NO ESTOQUE
-- ============================================================
SELECT * FROM estoqueTbl ORDER BY idproduto;
GO

-- ============================================================
-- APAGAR REGISTOS DUPLICADOS DO ESTOQUE (MANTER SÓ 1 POR PRODUTO)
-- ============================================================

-- Apagar todos os registos do estoque que tenham idproduto > 100
DELETE FROM estoqueTbl WHERE idproduto > 100;
GO

-- ============================================================
-- RECRIAR O ESTOQUE APENAS COM OS PRODUTOS ORIGINAIS
-- ============================================================

-- Ver os produtos originais (ID 1 a 10)
SELECT * FROM ProdutoTbl WHERE idproduto <= 10;
GO

-- Se o estoque estiver vazio, recriar
-- Inserir estoque para produtos com ID <= 10
INSERT INTO estoqueTbl (idproduto, nomeEt, preco_do_produtoEt, quantidade_estoque)
SELECT 
    idproduto, 
    nomepd, 
    precopd, 
    quantidadepd 
FROM ProdutoTbl
WHERE idproduto <= 10;
GO

-- ============================================================
-- VERIFICAR O RESULTADO
-- ============================================================
SELECT * FROM estoqueTbl ORDER BY idproduto;
GO

-- Ver produtos e estoque juntos
SELECT 
    p.idproduto,
    p.nomepd AS Produto,
    p.quantidadepd AS Quantidade_Produto,
    e.quantidade_estoque AS Quantidade_Estoque
FROM ProdutoTbl p
INNER JOIN estoqueTbl e ON p.idproduto = e.idproduto
WHERE p.idproduto <= 10
ORDER BY p.idproduto;
GO
USE mercadono;
GO

-- ============================================================
-- VER OS REGISTOS DUPLICADOS
-- ============================================================
SELECT * FROM estoqueTbl ORDER BY idproduto;
GO

-- ============================================================
-- APAGAR TUDO E RECRIAR DO ZERO
-- ============================================================

-- 1. Apagar todos os registos do estoque
DELETE FROM estoqueTbl;
GO

-- 2. Inserir apenas 1 registo por produto (os primeiros 10)
INSERT INTO estoqueTbl (idproduto, nomeEt, preco_do_produtoEt, quantidade_estoque)
SELECT 
    idproduto, 
    nomepd, 
    precopd, 
    quantidadepd 
FROM ProdutoTbl
WHERE idproduto <= 10;
GO

-- 3. Ver resultado (só devem aparecer 10 registos)
SELECT * FROM estoqueTbl ORDER BY idproduto;
GO

-- 4. Ver os produtos e estoque juntos
SELECT 
    p.idproduto,
    p.nomepd AS Produto,
    e.quantidade_estoque AS Estoque
FROM ProdutoTbl p
INNER JOIN estoqueTbl e ON p.idproduto = e.idproduto
WHERE p.idproduto <= 10
ORDER BY p.idproduto;
GO
