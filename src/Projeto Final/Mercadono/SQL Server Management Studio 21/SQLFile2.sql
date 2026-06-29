USE Mercadono;
GO

-- Apagar todos os dados da tabela
DELETE FROM utilizadorTbl;
GO

-- Resetar o contador de ID
DBCC CHECKIDENT ('utilizadorTbl', RESEED, 0);
GO

-- Inserir administrador
INSERT INTO utilizadorTbl (nome, senha, gmail, dinheiro, is_admin) 
VALUES ('Administrador', 'admin123', 'admin@admin.com', 0, 1);
GO

-- Inserir usuários de teste
INSERT INTO utilizadorTbl (nome, senha, gmail, dinheiro, is_admin) VALUES
('João Silva', '123456', 'joao@email.com', 150.50, 0),
('Maria Santos', '123456', 'maria@email.com', 250.00, 0);
GO

-- Verificar
SELECT * FROM utilizadorTbl;
GO