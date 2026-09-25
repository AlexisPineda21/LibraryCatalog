USE LibraryCatalog;
GO

SELECT 'Autores' AS Tabla, COUNT(*) AS Registros FROM dbo.Autores
UNION ALL SELECT 'Categorias', COUNT(*) FROM dbo.Categorias
UNION ALL SELECT 'Libros', COUNT(*) FROM dbo.Libros;
GO

SELECT
    l.Id,
    l.Titulo,
    l.ISBN,
    l.AnioPublicacion,
    a.Nombre AS Autor,
    c.Nombre AS Categoria
FROM dbo.Libros AS l
INNER JOIN dbo.Autores AS a ON a.Id = l.AutorId
INNER JOIN dbo.Categorias AS c ON c.Id = l.CategoriaId
ORDER BY l.Titulo;
GO


SELECT
    c.Nombre AS Categoria,
    COUNT(l.Id) AS TotalLibros
FROM dbo.Categorias AS c
LEFT JOIN dbo.Libros AS l ON l.CategoriaId = c.Id
GROUP BY c.Nombre
ORDER BY TotalLibros DESC, c.Nombre;
GO


SELECT
    fk.name AS ClaveForanea,
    OBJECT_NAME(fk.parent_object_id) AS TablaOrigen,
    OBJECT_NAME(fk.referenced_object_id) AS TablaDestino,
    fk.delete_referential_action_desc AS AccionAlEliminar
FROM sys.foreign_keys AS fk
WHERE OBJECT_NAME(fk.parent_object_id) = 'Libros';

SELECT
    OBJECT_NAME(i.object_id) AS Tabla,
    i.name AS Indice,
    i.is_unique AS EsUnico
FROM sys.indexes AS i
WHERE OBJECT_NAME(i.object_id) IN ('Libros', 'Autores', 'Categorias')
  AND i.name IS NOT NULL
ORDER BY Tabla, Indice;
GO


SELECT COUNT(*) AS LibrosHuerfanos
FROM dbo.Libros AS l
WHERE NOT EXISTS (SELECT 1 FROM dbo.Autores AS a WHERE a.Id = l.AutorId)
   OR NOT EXISTS (SELECT 1 FROM dbo.Categorias AS c WHERE c.Id = l.CategoriaId);
GO
