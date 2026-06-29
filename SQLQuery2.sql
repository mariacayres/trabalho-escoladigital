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