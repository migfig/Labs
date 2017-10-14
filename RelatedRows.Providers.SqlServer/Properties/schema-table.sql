CREATE TABLE #ccu (FROM_TABLE_SCHEMA VARCHAR(255), FROM_TABLE_NAME VARCHAR(255), FROM_COLUMN_NAME VARCHAR(255)
	,TO_TABLE_SCHEMA VARCHAR(255), TO_TABLE_NAME VARCHAR(255), TO_COLUMN_NAME VARCHAR(255), PK_CONSTRAINT_NAME VARCHAR(255)
	,FK_CONSTRAINT_NAME VARCHAR(255) NULL)

INSERT INTO #ccu
	SELECT 
		ccufk.TABLE_SCHEMA AS FROM_TABLE_SCHEMA
		,ccufk.TABLE_NAME AS FROM_TABLE_NAME
		,ccufk.COLUMN_NAME AS FROM_COLUMN_NAME
		,ccu.TABLE_SCHEMA AS TO_TABLE_SCHEMA
		,ccu.TABLE_NAME AS TO_TABLE_NAME
		,ccu.COLUMN_NAME AS TO_COLUMN_NAME
		,ccu.CONSTRAINT_NAME AS PK_CONSTRAINT_NAME
		,rc.CONSTRAINT_NAME AS FK_CONSTRAINT_NAME
	FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
		INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc ON rc.UNIQUE_CONSTRAINT_NAME = ccu.CONSTRAINT_NAME
		INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccufk ON rc.CONSTRAINT_NAME = ccufk.CONSTRAINT_NAME
	WHERE
		ccu.CONSTRAINT_NAME IN
		(
			SELECT rc.UNIQUE_CONSTRAINT_NAME
			FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc
			WHERE rc.CONSTRAINT_NAME IN
			(
				SELECT ccufk.CONSTRAINT_NAME
				FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccufk
				WHERE left(ccufk.CONSTRAINT_NAME, 2) = 'FK'
			)
		)
	ORDER BY 1, 2, 3

SELECT  '[' + [Table].TABLE_CATALOG + ']' AS catalog, '[' + [Table].TABLE_SCHEMA + ']' AS schemaName, '[' + [Table].TABLE_NAME + ']' AS name 
	,rows = (SELECT TOP 1 pa.rows 
			FROM sys.partitions pa
			WHERE pa.object_id in (SELECT ta.object_id FROM sys.tables ta WHERE ta.type = 'U' AND ta.name = [Table].TABLE_NAME)
			),
(
        SELECT 
			'[' + [Column].COLUMN_NAME + ']' AS name, 
			[Column].ORDINAL_POSITION as ordinal,
			[Column].DATA_TYPE AS DbType, 
			[Column].CHARACTER_MAXIMUM_LENGTH AS maxLen, 
			ISNULL([Column].NUMERIC_PRECISION, [Column].DATETIME_PRECISION) AS [precision], 
			CASE [Column].IS_NULLABLE WHEN 'YES' THEN 1 ELSE 0 END AS isNullable, 
			[Column].COLUMN_DEFAULT as defaultValue,
			'isPrimaryKey' =
			(
				select case when count(*) >= 1 then 1 else 0 end
				from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
				where ccu.TABLE_NAME = [Table].TABLE_NAME and ccu.COLUMN_NAME = [Column].COLUMN_NAME and left(ccu.CONSTRAINT_NAME, 2) = 'PK'
	        ),
		    'isForeignKey' =
			(
				select case when count(*) >= 1 then 1 else 0 end
				from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
				where ccu.TABLE_NAME = [Table].TABLE_NAME and ccu.COLUMN_NAME = [Column].COLUMN_NAME and left(ccu.CONSTRAINT_NAME, 2) = 'FK'
	        ),
			'isIdentity' = 
			(
				select sc.is_identity
				from sys.tables st 
					inner join sys.columns sc on sc.object_id = st.object_id 
				where st.type = 'U' and st.name = [Table].TABLE_NAME and sc.name = [Column].COLUMN_NAME
			)			
			,(
				SELECT DISTINCT  '[' + [Relationship].TO_TABLE_SCHEMA + ']' as toSchemaName
								, '[' + [Relationship].TO_TABLE_NAME + ']' as toTable
								, '[' + [Relationship].TO_COLUMN_NAME + ']' as toColumn
				FROM #ccu AS [Relationship]					
				WHERE [Relationship].FROM_TABLE_NAME = [Table].TABLE_NAME AND [Relationship].FROM_COLUMN_NAME = [Column].COLUMN_NAME
				FOR XML AUTO, TYPE
			)        
        FROM information_schema.columns AS [Column]
        WHERE [Column].TABLE_NAME = [Table].TABLE_NAME
        ORDER BY [Column].ORDINAL_POSITION
        FOR XML AUTO, TYPE
)
FROM information_schema.TABLES AS [Table]
WHERE 
	TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME = '@tableName'
ORDER BY 1, 2, 3
FOR XML AUTO, TYPE, ROOT('schema')

DROP TABLE #ccu	